﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ServerApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Server server;
        public MainWindow()
        {
            InitializeComponent();
            // wrapper for a richtextfield
            LogField log = new LogField(this.Log, this.Dispatcher);
            // wrapper for a status label.
            ServerStatusLabel statusLabel = new ServerStatusLabel(this.Label_Status, this.Indicator);
            // wrapperfor server buttons. 
            ServerButtons serverButtons = new ServerButtons(this.Button_Start, this.Button_Stop);
            this.server = new Server(log, statusLabel, serverButtons);
        }

        private void OnStart_Click(object sender, RoutedEventArgs e)
        {
            server.StartServer();
        }
        private void OnStop_Click(object sender, RoutedEventArgs e)
        {
            server.StopServer();
            
        }

        private void Button_Save_Click(object sender, RoutedEventArgs e)
        {
            server.save();
        }
    }
}
