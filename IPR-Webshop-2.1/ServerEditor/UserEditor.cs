using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ServerEditor.Interface_pages;
using Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace ServerEditor
{
    public class UserEditor
    {
        // our encrypted messaging logic.
        private Crypto crypto;
        // our view.
        private Page_UserEditor viewModel;
        public UserEditor(Crypto crypto, Page_UserEditor page)
        {
            this.viewModel = page;
            this.crypto = crypto;
            // initial data request to server.
            this.crypto.WriteTextMessage(DataProtocol.getJsonMessage("client/userListRequest",
                DataProtocol.getUserListRequest()));
        }
        /// <summary>
        /// When the add button is called.
        /// </summary>
        public void OnAdd()
        {
            foreach (TextBox box in viewModel.textBoxes)
            {
                if (box.Text.Length <= 0)
                    return;
            }

            double credits;
            if (!double.TryParse(viewModel.TextBox_Credits.Text, out credits))
                return;

            User user = new User()
            {
                FirstName = viewModel.TextBox_FirstName.Text,
                LastName = viewModel.TextBox_LastName.Text,
                Username = viewModel.TextBox_Username.Text,
                Password = viewModel.TextBox_Password.Text,
                Credits = credits,
                ShippingDetails = viewModel.TextBox_ShippingAddress.Text,
                BillingDetails = viewModel.TextBox_BillingDetails.Text
            };
            // Sends the server a request to add new user.
            this.crypto.WriteTextMessage(DataProtocol.getJsonMessage(
                                            "client/userListChangeRequest",
                                            DataProtocol.getUserChangeDynamic("add", user)));
        }
        /// <summary>
        /// Edit button callback.
        /// </summary>
        public void OnEdit()
        {
            User selectedUser = (User)viewModel.ListView_UserList.SelectedItem;
            if (selectedUser != null)
            {
                foreach (TextBox box in viewModel.textBoxes)
                {
                    if (box.Text.Length <= 0)
                        return;
                }

                double credits;
                if (!double.TryParse(viewModel.TextBox_Credits.Text, out credits))
                    return;

                selectedUser.FirstName = viewModel.TextBox_FirstName.Text;
                selectedUser.LastName = viewModel.TextBox_LastName.Text;
                selectedUser.Username = viewModel.TextBox_Username.Text;
                selectedUser.Password = viewModel.TextBox_Password.Text;
                selectedUser.Credits = credits;
                selectedUser.ShippingDetails = viewModel.TextBox_ShippingAddress.Text;
                selectedUser.BillingDetails = viewModel.TextBox_BillingDetails.Text;
                // sends a message to our server with edited user.
                this.crypto.WriteTextMessage(DataProtocol.getJsonMessage(
                                            "client/userListChangeRequest",
                                            DataProtocol.getUserChangeDynamic("edit", selectedUser)));
            }
        }
        /// <summary>
        /// Remove button callback.
        /// </summary>
        public void OnRemove()
        {
            User user = (User)viewModel.ListView_UserList.SelectedItem;
            // send a messgae to our server with the remove request.
            this.crypto.WriteTextMessage(DataProtocol.getJsonMessage(
                                            "client/userListChangeRequest",
                                            DataProtocol.getUserChangeDynamic("remove", user)));
        }
        /// <summary>
        /// When a item is selected.
        /// </summary>
        /// <param name="item"></param>
        public void OnItemSelect(ListViewItem item)
        {
            User selectedObject = (User)item.DataContext;

            if (selectedObject != null)
            {
                viewModel.TextBox_FirstName.Text = selectedObject.FirstName;
                viewModel.TextBox_LastName.Text = selectedObject.LastName;
                viewModel.TextBox_Username.Text = selectedObject.Username;
                viewModel.TextBox_Password.Text = selectedObject.Password;
                viewModel.TextBox_ShippingAddress.Text = selectedObject.ShippingDetails;
                viewModel.TextBox_BillingDetails.Text = selectedObject.BillingDetails;
                viewModel.TextBox_Credits.Text = selectedObject.Credits.ToString();
            }
        }
        /// <summary>
        /// When our server sends over our users.
        /// </summary>
        /// <param name="receivedData"> json with all users </param>
        public void OnUserListReceived(JObject receivedData)
        {
            viewModel.Dispatcher.Invoke(() => viewModel.ListView_UserList.Items.Clear());
            JArray userList = (JArray)receivedData["userList"];
            foreach (JToken Jproduct in userList)
            {
                User user = JsonConvert.DeserializeObject<User>(Jproduct.ToString());
                viewModel.Dispatcher.Invoke(() => viewModel.ListView_UserList.Items.Add(user));
            }
        }
    }
}
