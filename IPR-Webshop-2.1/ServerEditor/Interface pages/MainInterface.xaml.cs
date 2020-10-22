using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for MainInterface.xaml
    /// </summary>
    public partial class MainInterface : Page
    {
        private Page_ProductEditor productEditor;
        private Page_UserEditor userEditor;
        private Crypto crypto;

        public MainInterface(Window window, Crypto crypto)
        {
            InitializeComponent();
            this.crypto = crypto;
            this.crypto.handleMethod = this.HandleData;

            this.productEditor = new Page_ProductEditor(this.crypto);
            this.userEditor = new Page_UserEditor(this.crypto);

            this.ViewPort_ProductEditor.Navigate(this.productEditor);
            this.ViewPort_UserEditor.Navigate(this.userEditor);
            window.Closed += Window_Closed;
        }

        private void HandleData(string receivedText)
        {
            JObject receivedMessage = (JObject)JsonConvert.DeserializeObject(receivedText);
            // Type of message received.
            string type = (string)receivedMessage["type"];
            JObject receivedData = (JObject)receivedMessage["data"];

            switch (type)
            {
                case "server/productListResponse":
                    productEditor.handleProductList(receivedData);
                    break;
                case "server/userListResponse":
                    userEditor.handleUserList(receivedData);
                    break;
                default:
                    // TODO: when message is not undestood.
                    Debug.WriteLine($"received type: {type}");
                    return;
            }
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            this.disconnectMessage();
        }
        public void disconnectMessage()
        {
            this.crypto.WriteTextMessage(DataProtocol.getJsonMessage("client/disconnect", new { }));
        }
    }
}
