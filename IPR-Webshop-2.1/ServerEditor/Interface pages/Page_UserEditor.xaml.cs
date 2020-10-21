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
        public Page_UserEditor()
        {
            InitializeComponent();
            this.textBoxes = new TextBox[] { 
                this.TextBox_Username, 
                this.TextBox_Password, 
                this.TextBox_FirstName, 
                this.TextBox_LastName, 
                this.TextBox_BillingDetails, 
                this.TextBox_ShippingAddress, 
                this.TextBox_Credits};

        }
        private void MyListView_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ListView list = (ListView)sender;
            if (list.Items.Count > 0)
            {
                User selectedObject = (User)list.SelectedItem;
                if(selectedObject != null)
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

        private void Button_Clear_Click(object sender, RoutedEventArgs e)
        {
            foreach (TextBox box in textBoxes)
            {
                box.Clear();
            }
        }

        private void Button_RemoveUser_Click(object sender, RoutedEventArgs e)
        {
            this.ListView_ProductList.Items.Remove(this.ListView_ProductList.SelectedItem);
        }

        private void Button_EditUser_Click(object sender, RoutedEventArgs e)
        {
            User selectedUser = (User)this.ListView_ProductList.SelectedItem;
            int index = this.ListView_ProductList.Items.IndexOf(selectedUser);
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
                this.ListView_ProductList.Items.Remove(selectedUser);
                this.ListView_ProductList.Items.Insert(index, selectedUser);
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
            
            this.ListView_ProductList.Items.Add(new User()
            {
                FirstName = this.TextBox_FirstName.Text,
                LastName = this.TextBox_LastName.Text,
                Username = this.TextBox_Username.Text,
                Password = this.TextBox_Password.Text,
                Credits = credits,
                ShippingDetails = this.TextBox_ShippingAddress.Text,
                BillingDetails = this.TextBox_BillingDetails.Text
            });
        }
    }
}
