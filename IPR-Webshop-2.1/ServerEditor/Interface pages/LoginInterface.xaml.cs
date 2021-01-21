using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shared;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace ServerEditor.Interface_pages
{
    /// <summary>
    /// Interaction logic for LoginInterface.xaml
    /// </summary>
    public partial class LoginInterface : Page
    {
        private Window window;
        private Crypto crypto;
        public LoginInterface(Window window)
        {
            this.window = window;
            InitializeComponent();

            // tcp client will be our low level tcp packets handler.
            TcpClient tcpClient = new TcpClient();
            tcpClient.Connect("localhost", 2000);
            this.crypto = new Crypto(tcpClient, HandleData, null);
        }
        /// <summary>
        /// This method is our initial handle data method for our received messages.
        /// </summary>
        /// <param name="receivedText"> only login status </param>
        public void HandleData(string receivedText)
        {
            JObject receivedMessage = (JObject)JsonConvert.DeserializeObject(receivedText);
            // Type of message received.
            string type = (string)receivedMessage["type"];
            JObject receivedData = (JObject)receivedMessage["data"];
            if (type == "server/loginResponse")
            {
                // when messages status lable is true, our application will continue.
                if ((bool)receivedData["status"])
                {
                    this.Dispatcher.Invoke(() => this.NavigationService.Navigate(
                        new MainInterface(this.window, this.crypto)));
                }
            }
        }
        private void Button_LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = this.TextBlock_LoginUsername.Text;
            string password = this.TextBlock_LoginPassword.Text;
            if (username.Length <= 0 && password.Length <= 0)
                return;
            // sends our login attempt to the server.
            this.crypto.WriteTextMessage(
                DataProtocol.getJsonMessage("client/login",
                DataProtocol.getCredentialDynamic(username, password, true)));
        }
    }
}
