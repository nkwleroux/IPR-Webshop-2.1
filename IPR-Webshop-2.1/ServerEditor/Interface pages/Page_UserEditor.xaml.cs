using Shared;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ServerEditor.Interface_pages
{
    /// <summary>
    /// Interaction logic for Page_UserEditor.xaml
    /// </summary>
    public partial class Page_UserEditor : Page
    {
        public TextBox[] textBoxes;
        public UserEditor UserEditor;
        public Page_UserEditor(Crypto crypto)
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

            this.UserEditor = new UserEditor(crypto, this);
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
            this.UserEditor.OnRemove();
        }

        private void Button_EditUser_Click(object sender, RoutedEventArgs e)
        {
            this.UserEditor.OnEdit();    
        }

        private void Button_AddUser_Click(object sender, RoutedEventArgs e)
        {
            this.UserEditor.OnAdd();
        }
        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ListViewItem item = (ListViewItem)sender;
            this.UserEditor.OnItemSelect(item);
        }
    }
}
