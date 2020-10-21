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
    /// Interaction logic for RegisterScreenUC.xaml
    /// </summary>
    public partial class RegisterScreenUC : UserControl
    {
        private MainWindow mainWindow;

        public RegisterScreenUC(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            InitializeComponent();
        }

        private void Button_Register(object sender, RoutedEventArgs e)
        {
            mainWindow.ChangeView("AccountOverview");
        }

        private void Button_Login(object sender, RoutedEventArgs e)
        {
            mainWindow.ChangeView("Login");
        }
    }
}
