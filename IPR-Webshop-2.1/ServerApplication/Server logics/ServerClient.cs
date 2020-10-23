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
            this.crypto = new Crypto(client, HandleData);
            this.currentUser = new User();
        }
        /*
         * When the received message is wrapped by a JObject, the message ends in the 
         * handledata method, which filters the next steps by switching with the type value
         * in the message.
        */
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
        private void handleClientRegister(JObject receivedData)
        {
            string username = receivedData["username"].ToString();
            string password = receivedData["password"].ToString();
            bool registerSucceeded = this.database.RegisterUser(username, password);

            dynamic data = new
            {
                status = registerSucceeded,
                user = this.currentUser
            };
            
            this.crypto.WriteTextMessage(DataProtocol.getJsonMessage("server/registerResponse", data));
        }

        private bool loginStatus;
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
        private void SendCurrentCart()
        {
            dynamic data = new
            {
                this.currentUser.cart
            };
            this.crypto.WriteTextMessage(DataProtocol.getJsonMessage("server/cartUpdateResponse", data));
        }
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
            User user = JsonConvert.DeserializeObject<User>((string)receivedData["user"]);

        }
        public void SendProductList(JObject json)
        {
            //TODO categories
            string category = "";
            if(json != null)
            {
                category = (string)json["category"];
            }
            List<Product> productList;
            if (category.Length > 0)
            {
                productList = database.getCategoryList(category);
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
        public void SendUserList()
        {
            List<User> userList = database.Users;
            dynamic data = new
            {
                userList
            };
            this.crypto.WriteTextMessage(DataProtocol.getJsonMessage("server/userListResponse", data));
        }
        public void Disconect()
        {
            this.stream.Dispose();
            this.client.Dispose();
            this.log.PrintLine(SOURCE_LABEL, "Client disconnect");
            this.server.OnDisposeServerClient(this);
        }
    }
}
