using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace ServerApplication
{
    public class ServerClient
    {
        // The accepted client from out tcp listener
        private TcpClient client;
        // The stream to send and receive tcp data
        private NetworkStream stream;
        public ServerClient(TcpClient client)
        {
            this.client = client;
            this.stream = client.GetStream();

            this.onReadBuffer = new byte[BUFFERSIZE_DEFAULT];
            this.stream.BeginRead(onReadBuffer, 0, onReadBuffer.Length, new AsyncCallback(OnRead), null);
        }

        // Buffer which is used to buffer our incomming data, size:1024
        private static readonly int BUFFERSIZE_DEFAULT = 1024;
        private byte[] onReadBuffer;
        
        // Async callback when the stream receives new data
        private void OnRead(IAsyncResult ar)
        {
            try
            {
                // Amount of received bytes to index in our buffer
                int receivedBytes = stream.EndRead(ar);
                // Received bytes converted to a string (text)
                string receivedText = Encoding.ASCII.GetString(onReadBuffer, 0, receivedBytes);
                // The received string converted to a JObject to extract the values
                JObject receivedData = (JObject)JsonConvert.DeserializeObject(receivedText);
                HandleData(receivedData);
            }
            catch (IOException)
            {
                // TODO: catching or handling.
            }
        }

        /*
         * When the received message is wrapped by a JObject, the message ends in the 
         * handledata method, which filters the next steps by switching with the type value
         * in the message.
        */
        private void HandleData(JObject receivedData)
        {
            // Type of message received.
            string type = (string)receivedData["type"];

            switch (type)
            {
                case "":
                    break;
                default:
                    // TODO: when message is not understood.
                    Debug.WriteLine($"Messagetype not recognised: {type}");
                    break;
            }
        }
        public void Disconect()
        {
            this.stream.Dispose();
            this.client.Dispose();
        }
    }
}
