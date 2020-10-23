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
    /// Interaction logic for AccountOverviewUC.xaml
    /// </summary>
    public partial class AccountOverviewUC : UserControl
    {
        private MainWindow mainWindow;

        public AccountOverviewUC(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            InitializeComponent();

        }

        private void Button_ChangePassword(object sender, RoutedEventArgs e)
        {
            Username.Clear();
            Password.Clear();
            ConfirmPassword.Clear();
        }

        #region //Unused code
        /*var prevOrders = GetPreviousOrders();
        if (prevOrders.Count > 0)
            PreviousOrdersListView.ItemsSource = prevOrders;
    }

    private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        var item = sender as ListViewItem;
        if (item != null && item.IsSelected)
        {
            mainWindow.SetInCartProducts(((ListViewSelectProduct)item.DataContext).ICProducts);
            mainWindow.ChangeView("PreviousOrder");
        }
    }

    private List<ListViewSelectProduct> GetPreviousOrders()
    {

        return new List<ListViewSelectProduct>()
        {
            new ListViewSelectProduct("Order 1", new List<InCartProduct>()
            {
                new InCartProduct("Aardappel, groente, fruit",2,50.0,"/Assets/images/aardappelen_groente_fruit.png"),
                new InCartProduct("Salades, pizza, maaltijden",3,20.0,"/Assets/images/salades-pizza-maaltijden.png"),
                new InCartProduct("Vlees, kip, vis, vega",1,2.0,"/Assets/images/vlees-kip-vis-vega.png"),
                new InCartProduct("Kaas, vleeswaren, tapas",1,3.0,"/Assets/images/kaas-vleeswaren-tapas.png"),
                new InCartProduct("Zuivel, plantaardig en eiren",5,20.0,"/Assets/images/boter-eieren-zuivel.png")
            }),

            new ListViewSelectProduct("Order 2", new List<InCartProduct>()
                {
                new InCartProduct("Groente, fruit",2,50.0,"/Assets/images/aardappelen_groente_fruit.png"),
                new InCartProduct("Salades, maaltijden",3,20.0,"/Assets/images/salades-pizza-maaltijden.png"),
                new InCartProduct("Aardappel",2,50.0,"/Assets/images/aardappelen_groente_fruit.png"),
                new InCartProduct("Pizza",3,20.0,"/Assets/images/salades-pizza-maaltijden.png")
            })
       };
    }*/

        #endregion


    }

}
