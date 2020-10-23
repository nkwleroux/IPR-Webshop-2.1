using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shared;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace ClientApplication
{
    public class Client
    {
        public List<Product> Products { get; set; }
        private User currentUser;
        private Crypto crypto;
        private readonly string IPAddress = "127.0.0.1";
        private readonly int port = 2000;
        private TcpClient tcpClient;

        private int totalTries = 0;
        private readonly int MAXRECONTRIES = 3;

        private void setCurrentUser(User currentUser) { this.currentUser = currentUser; }

        private MainWindow mainWindow;

        public Client(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            this.Products = new List<Product>();
            this.tcpClient = new TcpClient();
            OnConnect(IPAddress, port);
        }

        //Method used to make the initial connection to the server. Upon failure this method automatically retries until it reaches the given MAXRECONTIRES before stopping.
        private void OnConnect(string iPAddress, int port)
        {
            try
            {
                tcpClient.Connect(IPAddress, port);
                if (tcpClient.Connected)
                {
                    OnConnected();
                }
            }
            catch (System.Exception ex)
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

        //Method used upon connection to the server. It is used to start the OnRead method and read incoming data.
        private void OnConnected()
        {
            this.crypto = new Crypto(tcpClient, HandleData);

            this.crypto.WriteTextMessage(DataProtocol.getJsonMessage("client/productListRequest",
    DataProtocol.getProductListRequest("")));
        }

        //Method used to safely close all connections to the server.
        private void OnDisconnect()
        {
            tcpClient.Close();
        }

        public void HandleData(string receivedText)
        {
            JObject receivedMessage = (JObject)JsonConvert.DeserializeObject(receivedText);
            // Type of message received.
            string type = (string)receivedMessage["type"];
            JObject receivedData = (JObject)receivedMessage["data"];

            switch (type)
            {
                case "server/productListResponse":

                    HandleProductList(receivedData);

                    mainWindow.SetCategory();

                    break;

                case "server/userListRequest":
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

        //Adds products to prodcut list
        public void HandleProductList(JObject receivedData)
        {
            Products.Clear();
            JArray productList = (JArray)receivedData["productList"];
            foreach (JToken Jproduct in productList)
            {
                Product product = JsonConvert.DeserializeObject<Product>(Jproduct.ToString());
                Products.Add(product);
            }
        }

        public bool HandleCredentialResponse(JObject receivedData)
        {
            return (bool)receivedData["status"];
        }

        public void SendCredentials(string username, string password)
        {
            this.crypto.WriteTextMessage(DataProtocol.getJsonMessage("client/register", DataProtocol.getRegisterDynamic(username, password)));
        }
    }
}
