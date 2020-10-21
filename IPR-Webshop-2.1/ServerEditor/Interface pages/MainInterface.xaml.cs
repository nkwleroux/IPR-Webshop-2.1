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
        public MainInterface(Window window)
        {
            TcpClient tcpClient = new TcpClient();
            tcpClient.Connect("localhost", 2000);
            InitializeComponent();

            this.productEditor = new Page_ProductEditor(tcpClient.GetStream());
            this.userEditor = new Page_UserEditor();

            this.ViewPort_ProductEditor.Navigate(this.productEditor);
            this.ViewPort_UserEditor.Navigate(this.userEditor);
            window.Closed += Window_Closed;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            this.productEditor.disconnectMessage();
        }
    }
}
