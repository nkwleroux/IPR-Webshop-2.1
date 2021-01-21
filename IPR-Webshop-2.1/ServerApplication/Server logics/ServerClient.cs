using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ServerApplication.Server_logics;
using Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Documents;

namespace ServerApplication
{
    public class ServerClient
    {
        private static readonly string SOURCE_LABEL = "ServerClient";
        private KeepAliveReceiver keepAliveReceiver;
        // The accepted client from out tcp listener
        private TcpClient client;
        // The stream to send and receive tcp data
        private NetworkStream stream;
        // Database
        private Database database;
        // Object for sending and receiving data
        private Crypto crypto;
        // Log object
        private LogField log;
        // server
        private Server server;
        // the logged in user
        private User currentUser;
        public ServerClient(TcpClient client, Server server)
        {
            this.loginStatus = false;
            this.server = server;
            this.log = server.Log;
            this.log.PrintLine(SOURCE_LABEL, $"client connected: {client.Client.RemoteEndPoint}");
            this.client = client;
            this.database = server.Database;
            this.stream = client.GetStream();
            this.crypto = new Crypto(client, HandleData, Disconect);
            this.currentUser = new User();
            this.keepAliveReceiver = new KeepAliveReceiver(this.Disconect);
            this.keepAliveReceiver.Run();
        }
        /// <summary>
        /// When the received message is wrapped by a JObject, the message ends in the 
        /// handledata method, which filters the next steps by switching with the type value
        /// in the message.
        /// </summary>
        /// <param name="receivedText"> received json typed message </param>
        private void HandleData(string receivedText)
        {
            JObject receivedMessage = (JObject)JsonConvert.DeserializeObject(receivedText);
            // Type of message received.
            string type = (string)receivedMessage["type"];
            JObject receivedData = (JObject)receivedMessage["data"];

            switch (type)
            {
                case "client/productListRequest":
                    SendProductList(receivedData);
                    break;
                case "client/userListRequest":
                    SendUserList();
                    break;
                case "client/cartUpdateRequest":
                    SendCurrentCart();
                    break;
                case "client/cartChangeProduct":
                    changeProductInCart(receivedData);
                    break;
                case "client/disconnect":
                    Disconect();
                    break;
                case "client/userEditRequest":
                    EditUser(receivedData);
                    break;
                case "client/login":
                    handleClientLogin(receivedData);
                    break;
                case "client/register":
                    handleClientRegister(receivedData);
                    break;
                case "client/alive":
                    int interval;
                    int.TryParse(receivedData["nextAliveTime"].ToString(), out interval);
                    this.keepAliveReceiver.received(interval);
                    break;
                default:
                    if (!currentUser.IsEditor)
                    {
                        log.PrintLine(SOURCE_LABEL, $"Unsupported message type: {type}");
                        return;
                    }
                    break;
            };
            if (currentUser.IsEditor)
            {
                switch (type)
                {
                    case "client/productListChangeRequest":
                        handleProductListChangeRequest(receivedData);
                        break;
                    case "client/userListChangeRequest":
                        handleUserListChangeRequest(receivedData);
                        break;
                }
            }
        }
        /// <summary>
        /// Method will be called if the message type equals that of a register request.
        /// </summary>
        /// <param name="receivedData"> json typed message </param>
        private void handleClientRegister(JObject receivedData)
        {
            string username = receivedData["username"].ToString();
            string password = receivedData["password"].ToString();
            User registeredUser = this.database.RegisterUser(username, password);
            bool registerSucceeded = (registeredUser != null);
            if (registerSucceeded)
            {
                this.currentUser = registeredUser;
            }
            dynamic data = new
            {
                status = registerSucceeded,
                user = this.currentUser
            };
            
            this.crypto.WriteTextMessage(DataProtocol.getJsonMessage("server/registerResponse", data));
        }
        // our currect login status
        private bool loginStatus;
        /// <summary>
        /// This method will handle login messages to trigger our loginstatus and to premise to more data.
        /// </summary>
        /// <param name="receivedData"> json typed message </param>
        private void handleClientLogin(JObject receivedData)
        {
            string username = receivedData["username"].ToString();
            string password = receivedData["password"].ToString();
            bool isEditor = (bool)receivedData["isEditor"];

            User user = this.database.CheckUserLogin(username, password, isEditor);

            if (user != null)
            {
                this.loginStatus = true;
                this.currentUser = user;
                if (this.currentUser.IsEditor)
                    log.PrintLine(SOURCE_LABEL, $"editor logged on: {this.currentUser.Username}");
            }

            dynamic data = new
            {
                status = loginStatus,
                user = this.currentUser
            };

            this.crypto.WriteTextMessage(DataProtocol.getJsonMessage("server/loginResponse", data));
        }
        /// <summary>
        /// When a list change is send from servereditor.
        /// </summary>
        /// <param name="receivedData"> json typed message </param>
        private void handleUserListChangeRequest(JObject receivedData)
        {
            User user = JsonConvert.DeserializeObject<User>(receivedData["user"].ToString());
            string typeOfChange = (string)receivedData["typeOfChange"];
            switch (typeOfChange)
            {
                case "edit":
                    database.EditUser(user);
                    log.PrintLine(SOURCE_LABEL, $"Edit user: {user.FullName}");
                    break;
                case "remove":
                    database.RemoveUser(user);
                    log.PrintLine(SOURCE_LABEL, $"Removed user: {user.FullName}");
                    break;
                case "add":
                    database.AddUser(user);
                    log.PrintLine(SOURCE_LABEL, $"Added user: {user.FullName}");
                    break;
            }
            this.server.SendUpdateUserList();
        }
        /// <summary>
        /// When a list change is send from servereditor.
        /// </summary>
        /// <param name="receivedData"> json typed message </param>
        private void handleProductListChangeRequest(JObject receivedData)
        {
            string typeOfChange = (string)receivedData["typeOfChange"];
            Product product = JsonConvert.DeserializeObject<Product>(receivedData["product"].ToString());

            switch (typeOfChange)
            {
                case "edit":
                    database.EditProduct(product);
                    log.PrintLine(SOURCE_LABEL, $"Edit product: {product.Name}");
                    break;
                case "remove":
                    database.RemoveProduct(product);
                    log.PrintLine(SOURCE_LABEL, $"Remove product: {product.Name}");
                    break;
                case "add":
                    database.AddProduct(product);
                    log.PrintLine(SOURCE_LABEL, $"Added product: {product.Name}");
                    break;
            }

            this.server.sendUpdateProductList();
        }
        /// <summary>
        /// Sends a list of user's current cart.
        /// </summary>
        private void SendCurrentCart()
        {
            dynamic data = new
            {
                this.currentUser.cart
            };
            this.crypto.WriteTextMessage(DataProtocol.getJsonMessage("server/cartUpdateResponse", data));
        }
        /// <summary>
        /// This method is called when our client sends a change in cart.
        /// </summary>
        /// <param name="receivedData"> json typed message </param>
        private void changeProductInCart(JObject receivedData)
        {
            string typeOfChange = receivedData["typeOfChange"].ToString();
            Product product = JsonConvert.DeserializeObject<Product>(receivedData["product"].ToString());
            if(typeOfChange == "add")
            {
                bool status = this.database.CheckStockAndUpdate(product, true);
                if (status)
                {
                    this.currentUser.cart.Add(product);
                }
            } else if(typeOfChange == "remove")
            {
                this.database.CheckStockAndUpdate(product, false);
                foreach(Product p in this.currentUser.cart)
                {
                    if(p.Id == product.Id)
                    {
                        this.currentUser.cart.Remove(p);
                    }
                }
            }
            
            this.SendCurrentUser();
        }
        /// <summary>
        /// This method will send our client its current user.
        /// </summary>
        private void SendCurrentUser()
        {
            dynamic data = new
            {
                user = this.currentUser
            };
            this.crypto.WriteTextMessage(DataProtocol.getJsonMessage("server/userResponse", data));
        }
        private void EditUser(JObject receivedData)
        {
            User user = JsonConvert.DeserializeObject<User>(receivedData["user"].ToString());
            database.EditUser(user);
            this.currentUser = user;
            SendCurrentUser();
        }
        /// <summary>
        /// This method will send our client a list with products based on category.
        /// </summary>
        /// <param name="json"></param>
        public void SendProductList(JObject json)
        {
            string category = "";
            if(json != null)
            {
                category = (string)json["category"];
            }
            List<Product> productList;
            if (category.Length > 0)
            {
                productList = database.GetProductListFromCategory(category);
            }
            else
            {
                productList = database.Products;
            }
            dynamic data = new
            {
                productList
            };
            this.crypto.WriteTextMessage(DataProtocol.getJsonMessage("server/productListResponse", data));
        }
        /// <summary>
        /// This method wil send a list of users (only to an sertified editor)
        /// </summary>
        public void SendUserList()
        {
            List<User> userList = database.Users;
            dynamic data = new
            {
                userList
            };
            this.crypto.WriteTextMessage(DataProtocol.getJsonMessage("server/userListResponse", data));
        }
        /// <summary>
        /// This method will disconnect our client end dispose itself.
        /// </summary>
        public void Disconect()
        {
            this.log.PrintLine(SOURCE_LABEL, $"client disconnect: {this.client.Client.RemoteEndPoint}");
            this.stream.Dispose();
            this.client.Dispose();            
            this.server.OnDisposeServerClient(this);
            this.keepAliveReceiver.Stop();
        }
    }
}
