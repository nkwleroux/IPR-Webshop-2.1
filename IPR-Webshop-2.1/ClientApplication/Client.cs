using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shared;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Shared
{
    public class Client
    {
        private KeepAliveService keepAliveServie;
        public List<Product> Products { get; set; }
        public User currentUser;
        private Crypto crypto;
        private readonly string IPAddress = "127.0.0.1";
        private readonly int port = 2000;
        private TcpClient tcpClient;

        private int totalTries = 0;
        private readonly int MAXRECONTRIES = 3;

        public void SetCurrentUser(User currentUser) { this.currentUser = currentUser; }

        public TcpClient GetClient() { return this.tcpClient; }

        private MainWindow mainWindow;

        /// <summary>
        /// The constructor of Client
        /// </summary>
        /// <param name="mainWindow">
        /// MainWindow is used to call methods in MainWindow which changes the view or data of user in the application.
        /// </param>
        public Client(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            this.Products = new List<Product>();
            this.tcpClient = new TcpClient();
            OnConnect(IPAddress, port);
        }

        /// <summary>
        /// Method used to make the initial connection to the server. Upon failure this method automatically retries until it reaches the given MAXRECONTIRES before stopping.
        /// </summary>
        /// <param name="IPAddress">
        /// The IP address of the server to which the client is connecting to.
        /// </param>
        /// <param name="port">
        /// The port of the server. 
        /// </param>
        private void OnConnect(string IPAddress, int port)
        {
            try
            {
                tcpClient.Connect(IPAddress, port);
                if (tcpClient.Connected)
                {
                    OnConnected();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                if (!tcpClient.Connected && totalTries < MAXRECONTRIES)
                {
                    totalTries++;
                    OnConnect(IPAddress, port);
                }
                else
                {
                    OnDisconnect();
                }
            }
        }

        /// <summary>
        /// Method is used to reconnect to the server upon client disconnect.
        /// </summary>
        public void Reconnect()
        {
            this.tcpClient = new TcpClient();
            totalTries = 0;
            OnConnect(IPAddress, port);
        }

        /// <summary>
        /// Method used upon connection to the server. It is used to start the OnRead method and read incoming data.
        /// </summary>
        private void OnConnected()
        {
            this.crypto = new Crypto(tcpClient, HandleData, this.OnDisconnectNoMessage);

            this.keepAliveServie = new KeepAliveService(this.crypto);
            this.keepAliveServie.Run();

            this.crypto.WriteTextMessage(DataProtocol.getJsonMessage("client/productListRequest",
            DataProtocol.getProductListRequest("")));
        }

        /// <summary>
        /// Method used to safely close all connections to the server.
        /// </summary>
        public void OnDisconnect()
        {
            if (this.crypto != null)
                this.crypto.WriteTextMessage(DataProtocol.getJsonMessage("client/disconnect", new { }));
            this.OnDisconnectNoMessage();
        }

        /// <summary>
        /// Method used to close the connection to the server by closing the TCP client if there is no incomming message.
        /// </summary>
        public void OnDisconnectNoMessage()
        {
            if (this.keepAliveServie != null)
                this.keepAliveServie.Stop();
            tcpClient.Close();
        }

        /// <summary>
        /// Method used to parse data
        /// </summary>
        /// <param name="receivedString">
        /// The data to be converted to a jsonObject
        /// </param>
        /// <param name="type">
        /// The type of object.
        /// </param>
        /// <param name="receivedData">
        /// The converted JObject
        /// </param>
        private void ParseRecievedString(string receivedString, out string type, out JObject receivedData)
        {
            JObject receivedMessage = (JObject)JsonConvert.DeserializeObject(receivedString);
            // Type of message received.
            type = (string)receivedMessage["type"];
            receivedData = (JObject)receivedMessage["data"];
        }

        /// <summary>
        /// Used to handel the different server responses.
        /// </summary>
        /// <param name="receivedText">
        /// The response from the server.
        /// </param>
        private void HandleData(string receivedText)
        {
            ParseRecievedString(receivedText, out string type, out JObject receivedData);

            switch (type)
            {
                case "server/productListResponse":

                    HandleProductList(receivedData);

                    break;
                case "server/userResponse":

                    HandleUserResponse(receivedData);

                    break;
                case "server/registerResponse":

                    mainWindow.IsLoggedIn(HandleCredentialResponse(receivedData));

                    break;
                case "server/loginResponse":

                    mainWindow.IsLoggedIn(HandleCredentialResponse(receivedData));

                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Adds products to prodcut list
        /// </summary>
        /// <param name="receivedData">
        /// JObject containg the productList.
        /// </param>
        private void HandleProductList(JObject receivedData)
        {
            Products.Clear();
            JArray productList = (JArray)receivedData["productList"];
            foreach (JToken Jproduct in productList)
            {
                Product product = JsonConvert.DeserializeObject<Product>(Jproduct.ToString());
                Products.Add(product);
            }

            mainWindow.SetCategories();
        }

        /// <summary>
        /// Takes the status and user object out of the JObject.
        /// </summary>
        /// <param name="receivedData">
        /// The data object recieved from the server.
        /// </param>
        /// <returns>
        /// The converted JObject.
        /// </returns>
        public (bool, User) HandleCredentialResponse(JObject receivedData)
        {
            (bool status, User user) response;
            response.status = (bool)receivedData["status"];
            User user = JsonConvert.DeserializeObject<User>(receivedData["user"].ToString());
            response.user = user;

            return response;
        }

        /// <summary>
        /// Converts the JObject to a user object.
        /// </summary>
        /// <param name="receivedData">
        /// The data object recieved from the server.
        /// </param>
        public void HandleUserResponse(JObject receivedData)
        {
            this.currentUser = JsonConvert.DeserializeObject<User>(receivedData["user"].ToString());
            mainWindow.SetUser(this.currentUser);

            mainWindow.UpdateCart(this.currentUser.cart);
        }

        /// <summary>
        /// Sends a message to the server containing the user credentials.
        /// </summary>
        /// <param name="tag">
        /// The type of credentials.
        /// </param>
        /// <param name="username">
        /// The username of the user
        /// </param>
        /// <param name="password">
        /// The password of the user.
        /// </param>
        public void MessageSendCredentials(string tag, string username, string password)
        {
            this.crypto.WriteTextMessage(DataProtocol.getJsonMessage(tag, DataProtocol.getCredentialDynamic(username, password, false)));
        }

        /// <summary>
        /// Used to send the edited user data to the server.
        /// </summary>
        /// <param name="newUser">
        /// The user object with the edited data of the user.
        /// </param>
        public void MessageSendNewUser(User newUser)
        {
            this.crypto.WriteTextMessage(DataProtocol.getJsonMessage("client/userEditRequest", DataProtocol.getUserChangeDynamic(newUser)));

        }

        /// <summary>
        /// Method used to add a product to the users cart.
        /// </summary>
        /// <param name="product">
        /// The product which is to be sent to the server.
        /// </param>
        public void MessageSendToCart(Product product)
        {
            this.crypto.WriteTextMessage(DataProtocol.getJsonMessage("client/cartChangeProduct", DataProtocol.getCartChangedDynamic("add", product)));

        }

        /// <summary>
        /// Method used to remove a product from the users cart.
        /// </summary>
        /// <param name="product">
        /// The product which is to be sent to the server.
        /// </param>
        public void MessageRemoveFromCart(Product product)
        {
            this.crypto.WriteTextMessage(DataProtocol.getJsonMessage("client/cartChangeProduct", DataProtocol.getCartChangedDynamic("remove", product)));

        }

    }
}
