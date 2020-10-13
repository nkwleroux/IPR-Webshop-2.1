using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Diagnostics;

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

        private void OnConnect(string iPAddress, int port)
        {
            try
            {
                tcpClient.Connect(IPAddress, port);
                networkStream = tcpClient.GetStream();

                if (tcpClient.Connected)
                {
                    OnConnected();
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
                if (!tcpClient.Connected)
                {
                    OnConnect(IPAddress, port);
                }
                else
                {
                    OnDisconnect();
                }
            }
        }

        private void OnConnected()
        {
            networkStream.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(OnRead), null);
        }

        private void OnDisconnect()
        {
            networkStream.Dispose();
            tcpClient.Close();
        }

        public void sendLogin(string username, string password)
        {
            byte[] bytes = PackageWrapper.SerializeData("client/login", new { name = name, password = password });

            networkStream.Write(bytes, 0, bytes.Length);
        }

        public void sendRegistrationRequest(string username, string password)
        {
            throw new NotImplementedException();
        }

        public void sendProductQuery(string query)
        {
            throw new NotImplementedException();
        }

        public void sendStockRequest(string query)
        {
            throw new NotImplementedException();
        }

        private void OnRead(IAsyncResult ar)
        {
            try
            {
                int receivedBytes = networkStream.EndRead(ar);
                string receivedText = System.Text.Encoding.ASCII.GetString(buffer, 0, receivedBytes);
                dynamic receivedData = JsonConvert.DeserializeObject(receivedText);
                HandleData(receivedData);
                networkStream.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(OnRead), null);
            }
            catch (IOException)
            {
                OnDisconnect();
                return;
            }
        }

        public void HandleData(dynamic receivedData)
        {
            JObject data = receivedData as JObject;
            string type = data["tag"].ToObject<string>();

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
        }

        public List<Product> HandleProductData(JObject json)
        {
            return new List<Product>();
        }

        public User HandleUserData(JObject json)
        {
            return new User(null, null, null, null, 0.00);
        }

        public bool HandleStockRequestData(JObject json)
        {
            return false;
        }
    
    }
}
