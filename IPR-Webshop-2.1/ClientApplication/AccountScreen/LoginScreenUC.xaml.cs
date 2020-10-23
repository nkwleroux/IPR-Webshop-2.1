using System;
using System.Collections.Generic;
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

namespace ClientApplication.AccountScreen
{
    /// <summary>
    /// Interaction logic for LoginScreenUC.xaml
    /// </summary>
    public partial class LoginScreenUC : UserControl
    {
        private MainWindow mainWindow;
        public LoginScreenUC(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            InitializeComponent();

            
        }

        private void Button_Login(object sender, RoutedEventArgs e)
        {
            string username = Username.Text;
            string password = Password.Password;

            if (username.Length >= 5 && password.Length >= 5)
            {
                mainWindow.SendCredentials(username, password);
            }
        }

        private void Button_Register(object sender, RoutedEventArgs e)
        {
            mainWindow.ChangeView("Register");
        }
    }
}
