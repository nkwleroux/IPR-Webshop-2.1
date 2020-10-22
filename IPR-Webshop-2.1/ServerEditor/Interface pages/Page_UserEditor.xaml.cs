using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for Page_UserEditor.xaml
    /// </summary>
    public partial class Page_UserEditor : Page
    {
        private TextBox[] textBoxes;
        private Crypto crypto;
        public Page_UserEditor(Crypto crypto)
        {
            InitializeComponent();
            this.crypto = crypto;
            this.crypto.WriteTextMessage(DataProtocol.getJsonMessage("client/userListRequest",
                DataProtocol.getUserListRequest()));

            this.textBoxes = new TextBox[] { 
                this.TextBox_Username, 
                this.TextBox_Password, 
                this.TextBox_FirstName, 
                this.TextBox_LastName, 
                this.TextBox_BillingDetails, 
                this.TextBox_ShippingAddress, 
                this.TextBox_Credits};

        }
        private void Button_Clear_Click(object sender, RoutedEventArgs e)
        {
            foreach (TextBox box in textBoxes)
            {
                box.Clear();
            }
        }
        private void Button_RemoveUser_Click(object sender, RoutedEventArgs e)
        {
            User user = (User)this.ListView_UserList.SelectedItem;
            this.crypto.WriteTextMessage(DataProtocol.getJsonMessage(
                                            "client/userListChangeRequest",
                                            DataProtocol.getUserChangeDynamic("remove", user)));
        }

        private void Button_EditUser_Click(object sender, RoutedEventArgs e)
        {
            User selectedUser = (User)this.ListView_UserList.SelectedItem;
            if(selectedUser != null)
            {
                foreach (TextBox box in textBoxes)
                {
                    if (box.Text.Length <= 0)
                        return;
                }

                double credits;
                if (!double.TryParse(this.TextBox_Credits.Text, out credits))
                    return;

                selectedUser.FirstName = this.TextBox_FirstName.Text;
                selectedUser.LastName = this.TextBox_LastName.Text;
                selectedUser.Username = this.TextBox_Username.Text;
                selectedUser.Password = this.TextBox_Password.Text;
                selectedUser.Credits = credits;
                selectedUser.ShippingDetails = this.TextBox_ShippingAddress.Text;
                selectedUser.BillingDetails = this.TextBox_BillingDetails.Text;

                this.crypto.WriteTextMessage(DataProtocol.getJsonMessage(
                                            "client/userListChangeRequest",
                                            DataProtocol.getUserChangeDynamic("edit", selectedUser)));
            }
        }

        private void Button_AddUser_Click(object sender, RoutedEventArgs e)
        {
            foreach(TextBox box in textBoxes)
            {
                if (box.Text.Length <= 0)
                    return;
            }

            double credits;
            if (!double.TryParse(this.TextBox_Credits.Text, out credits))
                return;
            
            User user = new User()
            {
                FirstName = this.TextBox_FirstName.Text,
                LastName = this.TextBox_LastName.Text,
                Username = this.TextBox_Username.Text,
                Password = this.TextBox_Password.Text,
                Credits = credits,
                ShippingDetails = this.TextBox_ShippingAddress.Text,
                BillingDetails = this.TextBox_BillingDetails.Text
            };

            this.crypto.WriteTextMessage(DataProtocol.getJsonMessage(
                                            "client/userListChangeRequest",
                                            DataProtocol.getUserChangeDynamic("add", user)));
        }
        public void handleUserList(JObject receivedData)
        {
            this.Dispatcher.Invoke(() => this.ListView_UserList.Items.Clear());
            JArray userList = (JArray)receivedData["userList"];
            foreach (JToken Jproduct in userList)
            {
                User user = JsonConvert.DeserializeObject<User>(Jproduct.ToString());
                this.Dispatcher.Invoke(() => this.ListView_UserList.Items.Add(user));
            }
        }
        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ListViewItem item = (ListViewItem)sender;
            User selectedObject = (User)item.DataContext;

            if (selectedObject != null)
            {
                this.TextBox_FirstName.Text = selectedObject.FirstName;
                this.TextBox_LastName.Text = selectedObject.LastName;
                this.TextBox_Username.Text = selectedObject.Username;
                this.TextBox_Password.Text = selectedObject.Password;
                this.TextBox_ShippingAddress.Text = selectedObject.ShippingDetails;
                this.TextBox_BillingDetails.Text = selectedObject.BillingDetails;
                this.TextBox_Credits.Text = selectedObject.Credits.ToString();
            }
        }
    }
}
