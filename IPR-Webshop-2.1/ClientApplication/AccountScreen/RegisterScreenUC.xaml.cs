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

namespace Shared.AccountScreen
{
    /// <summary>
    /// Interaction logic for RegisterScreenUC.xaml
    /// </summary>
    public partial class RegisterScreenUC : UserControl
    {
        private MainWindow mainWindow;

        /// <summary>
        /// The constructor of LoginScreenUC.
        /// </summary>
        /// <param name="mainWindow">
        /// MainWindow is used to send data to the server and change the view of the application.
        /// </param>
        public RegisterScreenUC(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            InitializeComponent();
        }

        /// <summary>
        /// Used to register a new user on the server.
        /// </summary>
        /// <param name="sender">
        /// The data of the object.
        /// </param>
        /// <param name="e">
        /// The button event argument.
        /// </param>
        private void Button_Register(object sender, RoutedEventArgs e)
        {
            string username = Username.Text;
            string password = Password.Password;
            string confirmPassword = ConfirmPassword.Password;

            if (!password.Equals(confirmPassword))
            {
                Password.Clear();
                ConfirmPassword.Clear();
                return;
            }

            if (username.Length >= 5 && password.Length >= 5)
            {
                mainWindow.SendCredentials("client/register", username, password);
            }
        }

        /// <summary>
        /// Used to change the current view.
        /// </summary>
        /// <param name="sender">
        /// The data of the object.
        /// </param>
        /// <param name="e">
        /// The button event argument.
        /// </param>
        private void Button_Login(object sender, RoutedEventArgs e)
        {
            mainWindow.ChangeView("Login");
        }
    }
}
