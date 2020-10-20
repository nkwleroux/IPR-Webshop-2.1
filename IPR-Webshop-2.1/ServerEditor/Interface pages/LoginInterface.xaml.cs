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

namespace ServerEditor.Interface_pages
{
    /// <summary>
    /// Interaction logic for LoginInterface.xaml
    /// </summary>
    public partial class LoginInterface : Page
    {
        public LoginInterface()
        {
            InitializeComponent();
        }
        private void Button_LoginButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new MainInterface());
        }
    }
}
