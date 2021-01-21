using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shared;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace ServerEditor.Interface_pages
{
    /// <summary>
    /// Interaction logic for MainInterface.xaml
    /// </summary>
    public partial class MainInterface : Page
    {
        private KeepAliveService keepAliveService;
        private Page_ProductEditor productEditorPage;
        private Page_UserEditor userEditorPage;
        private Crypto crypto;

        public MainInterface(Window window, Crypto crypto)
        {
            InitializeComponent();
            this.crypto = crypto;
            this.crypto.handleMethod = this.HandleData;
            this.crypto.onDisconnect = this.OnDisconnect;

            this.keepAliveService = new KeepAliveService(this.crypto);
            this.keepAliveService.Run();

            this.productEditorPage = new Page_ProductEditor(this.crypto);
            this.userEditorPage = new Page_UserEditor(this.crypto);

            this.ViewPort_ProductEditor.Navigate(this.productEditorPage);
            this.ViewPort_UserEditor.Navigate(this.userEditorPage);
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
                    productEditorPage.ProductEditor.OnProductListReceived(receivedData);
                    break;
                case "server/userListResponse":
                    userEditorPage.UserEditor.OnUserListReceived(receivedData);
                    break;
                default:
                    // TODO: when message is not undestood.
                    Debug.WriteLine($"received type: {type}");
                    return;
            }
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            this.DisconnectMessage();
        }
        public void DisconnectMessage()
        {
            this.crypto.WriteTextMessage(DataProtocol.getJsonMessage("client/disconnect", new { }));
            this.OnDisconnect();
        }

        public void OnDisconnect()
        {
            this.keepAliveService.Stop();
        }
    }
}
