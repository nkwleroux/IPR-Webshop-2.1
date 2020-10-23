using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Diagnostics;
using Util;

namespace ClientApplication
{
    class Database
    {
        private TcpClient tcpClient;
        private NetworkStream networkStream;
        private TcpListener tcpListener;
        private String IPAddress = "127.0.0.1";
        private int port = 1330;
        private byte[] buffer = new byte[1024];
        private int totalTries = 0;
        private readonly int MAXRECONTRIES = 3;

        public Database()
        {
            this.tcpClient = new TcpClient();

            OnConnect(IPAddress, port);
        }

        public Database(string IPAddress, int port)
        {
            this.IPAddress = IPAddress;
            this.port = port;

            OnConnect(IPAddress, port);
        }

        //Method used to make the initial connection to the server. Upon failure this method automatically retries until it reaches the given MAXRECONTIRES before stopping.
        private void OnConnect(string iPAddress, int port)
        {
            try
            {
                tcpClient.Connect(IPAddress, port);
                networkStream = tcpClient.GetStream();

                if (tcpClient.Connected)
                {
                 /*   OnConnected();*/
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
/*        private void OnConnected()
        {
            networkStream.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(OnRead), null);
        }
*/
        //Method used to safely close all connections to the server.
        private void OnDisconnect()
        {
            networkStream.Dispose();
            tcpClient.Close();
        }

        //Base method to send data to the server.
        private void MessageToServer(string type, dynamic jsonObject)
        {
            byte[] bytes = Parse.SerializeData(type, jsonObject);

            networkStream.Write(bytes, 0, bytes.Length);
        }

        //Method used to send login credentials to the server. 
        public void sendLogin(string username, string password)
        {
            MessageToServer("client/login", new { name = username, password = password });
        }

        //Method used to register a new user account to the server.
        public void sendRegistrationRequest(string username, string password)
        {
            MessageToServer("client/register", new { name = username, password = password });
        }

        //Method used to send product queries to the server.
        public void sendProductQuery(string query)
        {
            MessageToServer("client/product/query", new { query = query });
        }

        //Method used to send the user's shopping cart to the server upon checkout.
        //MABYE REMOVE
 /*       public void sendOrderList(List<Product> products)
        {
            MessageToServer("client/product/orderList", new { products = products });
        }

        //Method used to send stock requests to the server.
        public void sendStockRequest(Product product)
        {
            MessageToServer("client/product/stockRequest", new { product = product });
        }*/

        //Used to read all incoming data and convert it to a readable json format.
/*        private void OnRead(IAsyncResult ar)
        {
            try
            {
                int receivedBytes = networkStream.EndRead(ar);
                string receivedText = System.Text.Encoding.ASCII.GetString(buffer, 0, receivedBytes);
                JObject receivedData = (JObject)JsonConvert.DeserializeObject(receivedText);
                HandleData(receivedData);
                networkStream.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(OnRead), null);
            }
            catch (IOException)
            {
                OnDisconnect(); // Disconnects when there is no more incoming messages.
                return;
            }
        }*/

        //Method used to handle all incoming data from the server. Used to separate on the different data types.
/*        public void HandleData(dynamic receivedData)
        {
            JObject data = receivedData as JObject;
            string type = (string)data["type"];

            switch (type)
            {
                case "productList":
                    //returns product list
                    List<Product> products = HandleProductData(data);
                    break;
                case "userData":
                    //returns new user data
                    User currentUser = HandleUserData(data);
                    break;
                case "stockRequest":
                    //returns true or false
                    HandleStockRequestData(data);
                    break;
                default:
                    Debug.WriteLine("Type not valid");
                    break;

            }
        }*/

        //Handles all product data. Returns a list of requested products which will be shown.
/*        public List<Product> HandleProductData(JObject json)
        {
            return new List<Product>();
        }*/

        //Handles current user data. Receives the new user data from the server and sets it locally. Returns a User object.
        public User HandleUserData(JObject json)
        {
            return new User(null, null, null, null, 0.00);
        }

        //Handles stock request data. Returns a boolean which implies whether the stock request was successful or not.
        public bool HandleStockRequestData(JObject json)
        {
            return false;
        }

    }
}
