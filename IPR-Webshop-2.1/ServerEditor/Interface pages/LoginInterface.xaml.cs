using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shared;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

            TcpClient tcpClient = new TcpClient();
            tcpClient.Connect("localhost", 2000);
            this.crypto = new Crypto(tcpClient, HandleData);
        }
        public void HandleData(string receivedText)
        {
            JObject receivedMessage = (JObject)JsonConvert.DeserializeObject(receivedText);
            // Type of message received.
            string type = (string)receivedMessage["type"];
            JObject receivedData = (JObject)receivedMessage["data"];
            if (type == "server/loginResponse")
            {
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

            this.crypto.WriteTextMessage(
                DataProtocol.getJsonMessage("client/login",
                DataProtocol.getCredentialDynamic(username, password, true)));
        }
    }
}
