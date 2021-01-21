using Shared;
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
    /// Interaction logic for AccountOverviewUC.xaml
    /// </summary>
    public partial class AccountOverviewUC : UserControl
    {
        private MainWindow mainWindow;
        private User currentUser;

        /// <summary>
        /// The constructor of AccountOverviewUC.
        /// </summary>
        /// <param name="mainWindow">
        /// Mainwindow is used to send data to the server.
        /// </param>
        public AccountOverviewUC(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            this.currentUser = new User();
            InitializeComponent();
        }

        /// <summary>
        /// Used to change the text of the textboxes of the application.
        /// </summary>
        /// <param name="currentUser">
        /// The user which will be displayed.
        /// </param>
        public void SetUserData(User currentUser)
        {
            this.Dispatcher.Invoke(() =>
            {
                this.currentUser = currentUser;
                TextBox_Firstname.Text = currentUser.FirstName;
                TextBox_Lastname.Text = currentUser.LastName;
                TextBox_BillingAddress.Text = currentUser.BillingDetails;
                TextBox_ShippingAddress.Text = currentUser.ShippingDetails;
                TextBlock_CreditAmount.Text = (currentUser.Credits).ToString();
            });
        }

        /// <summary>
        /// Used to save the changes made to user.
        /// </summary>
        /// <param name="sender">
        /// The data of the object.
        /// </param>
        /// <param name="e">
        /// The button event argument.
        /// </param>
        private void Button_SaveChanges(object sender, RoutedEventArgs e)
        {
            currentUser.FirstName = TextBox_Firstname.Text;
            currentUser.LastName = TextBox_Lastname.Text;
            currentUser.BillingDetails = TextBox_BillingAddress.Text;
            currentUser.ShippingDetails = TextBox_ShippingAddress.Text;

            mainWindow.EditUser(this.currentUser);
        }
    }
}
