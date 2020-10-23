using ClientApplication.AccountScreen;
using ClientApplication.CategoryProductScreen;
using ClientApplication.ProductDetailScreen;
using ClientApplication.PurchaseCheckoutScreen;
using ClientApplication.ShoppingCartScreen;
using Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace ClientApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainProductScreenUC mainProductScreenUC;
        private LoginScreenUC loginScreenUC;
        private RegisterScreenUC registerScreenUC;
        private AccountOverviewUC accountOverviewUC;
        private CategoryDetailScreenUC categoryDetailScreenUC;
        private ShoppingCartUC shoppingCartUC;
        private ProductDetailScreenUC productDetailScreenUC;
        private PurchaseCheckoutUC purchaseCheckoutUC;
        public Client client;

        private bool HasAccount { get; set; }
        public string SelectedCategory { get; set; }
        public List<ListViewSelectProduct> ListViewProducts { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            HasAccount = false;
            client = new Client(this);

            SetCategories();

            // Initialise the different pages.
            mainProductScreenUC = new MainProductScreenUC(this);
            LayoutControl.Children.Add(mainProductScreenUC);
            loginScreenUC = new LoginScreenUC(this);
            LayoutControl.Children.Add(loginScreenUC);
            registerScreenUC = new RegisterScreenUC(this);
            LayoutControl.Children.Add(registerScreenUC);
            accountOverviewUC = new AccountOverviewUC(this);
            LayoutControl.Children.Add(accountOverviewUC);
            categoryDetailScreenUC = new CategoryDetailScreenUC(this);
            LayoutControl.Children.Add(categoryDetailScreenUC);
            shoppingCartUC = new ShoppingCartUC(this);
            LayoutControl.Children.Add(shoppingCartUC);
            productDetailScreenUC = new ProductDetailScreenUC(this);
            LayoutControl.Children.Add(productDetailScreenUC);
            purchaseCheckoutUC = new PurchaseCheckoutUC(this);
            LayoutControl.Children.Add(purchaseCheckoutUC);

            ChangeView("MainProduct");
        }


        public void RemoveFromCart(Product product)
        {
            client.MessageRemoveFromCart(product);
        }

        public void SetCategories()
        {
            ListViewProducts = new List<ListViewSelectProduct>(){
                new ListViewSelectProduct("Aardappel, groente, fruit", new List<Product>() ),
                new ListViewSelectProduct("Salades, pizza, maaltijden", new List<Product>()),
                new ListViewSelectProduct("Vlees, kip, vis, vega", new List<Product>()),
                new ListViewSelectProduct("Kaas, vleeswaren, tapas", new List<Product>()),
                new ListViewSelectProduct("Zuivel, plantaardig en eiren", new List<Product>()),
                new ListViewSelectProduct("Bakkerij en banket", new List<Product>()),
                new ListViewSelectProduct("Ontbijtgranen, beleg, tussendoor", new List<Product>()),
                new ListViewSelectProduct("Frisdrank, sappen, koffie, thee", new List<Product>()),
                new ListViewSelectProduct("Wijn en bubbels", new List<Product>()),
                new ListViewSelectProduct("Bier, sterke drank, aperitieven", new List<Product>()),
                new ListViewSelectProduct("Pasta, rijst en wereldkeuken", new List<Product>()),
                new ListViewSelectProduct("Soepen, sauzen, kruiden, olie", new List<Product>()),
                new ListViewSelectProduct("Snoep, koek, chips en chocolade", new List<Product>()),
                new ListViewSelectProduct("Diepvries", new List<Product>()),
                new ListViewSelectProduct("Baby, verzorging en hygiene", new List<Product>()),
                new ListViewSelectProduct("Bewuste voeding", new List<Product>()),
                new ListViewSelectProduct("Huishouden, huisdier", new List<Product>()),
                new ListViewSelectProduct("Koken, tafelen, vrije tijd", new List<Product>()),
            };

            foreach (ListViewSelectProduct LVSP in ListViewProducts)
            {
                foreach (Product p in client.Products)
                {
                    if (!LVSP.Products.Contains(p) && LVSP.SelectId.Equals(p.Category))
                    {
                        LVSP.Products.Add(p);
                    }
                }
            }
        }


        public void SetProductDetail(Product p)
        {
            this.Dispatcher.Invoke(() =>
            {
                productDetailScreenUC.SetProductDetail(p);
            });
        }

        //Login and register 
        public void SendCredentials(string tag, string username, string password)
        {
            client.MessageSendCredentials(tag, username, password);
        }

        //Login and register - Sets user and login.
        public void IsLoggedIn((bool status, User user) response)
        {
            if (response.status)
                this.Dispatcher.Invoke(() =>
                {
                    client.setCurrentUser(response.user);
                    UpdateCart(response.user.cart);
                    HasAccount = true;
                    ChangeView("AccountOverview");
                });
        }

        //Account overview screen - button save changes
        public void UserEdit(User userEdit)
        {
            client.MessageSendNewUser(userEdit);
        }

        //Account overview screen
        public void SetUser(User user)
        {
            accountOverviewUC.SetUserData(user);
        }

        //Prodcut detail screen
        public void AddToCart(Product product)
        {
            this.Dispatcher.Invoke(() =>
            {
                client.MessageSendToCart(product);
            });
        }

        //Shopping cart screen
        public void UpdateCart(List<Product> cart)
        {
            this.Dispatcher.Invoke(() =>
            {
                shoppingCartUC.SetInCart(cart);
            });
        }

        /**
         * Used to change the middle section of the screen. 
         */
        public void ChangeView(String viewName)
        {
            mainProductScreenUC.Visibility = Visibility.Hidden;
            loginScreenUC.Visibility = Visibility.Hidden;
            registerScreenUC.Visibility = Visibility.Hidden;
            accountOverviewUC.Visibility = Visibility.Hidden;
            categoryDetailScreenUC.Visibility = Visibility.Hidden;
            shoppingCartUC.Visibility = Visibility.Hidden;
            productDetailScreenUC.Visibility = Visibility.Hidden;
            purchaseCheckoutUC.Visibility = Visibility.Hidden;

            //Sets default min heigh/width.
            mainscreen.MinHeight = 640;
            mainscreen.MinWidth = 960;

            switch (viewName)
            {
                case "MainProduct":
                    mainProductScreenUC.Visibility = Visibility.Visible;
                    break;
                case "Login":
                    if (HasAccount)
                    {
                        ChangeView("AccountOverview");
                    }
                    else
                    {
                        loginScreenUC.Visibility = Visibility.Visible;
                    }
                    break;
                case "Register":
                    if (HasAccount)
                    {
                        ChangeView("AccountOverview");
                    }
                    else
                    {
                        registerScreenUC.Visibility = Visibility.Visible;
                    }
                    break;
                case "AccountOverview":                   
                    accountOverviewUC.Visibility = Visibility.Visible;
                    accountOverviewUC.SetUserData(client.currentUser);
                    break;
                case "CategoryProduct":
                    categoryDetailScreenUC.SetListView(SelectedCategory);
                    categoryDetailScreenUC.Visibility = Visibility.Visible;
                    break;
                case "ShoppingCart":

                    shoppingCartUC.Visibility = Visibility.Visible;
                    break;
                case "ProductDetail":
                    productDetailScreenUC.Visibility = Visibility.Visible;
                    break;
                case "PurchaseCheckout":
                    purchaseCheckoutUC.Visibility = Visibility.Visible;
                    break;
                default:
                    break;
            }
        }

        #region //Button onClicks

        private void Button_AppName(object sender, RoutedEventArgs e)
        {
            ChangeView("MainProduct");
        }

        private void Button_MyAccount(object sender, RoutedEventArgs e)
        {
            ChangeView("Login");
        }

        private void Button_Cart(object sender, RoutedEventArgs e)
        {
            ChangeView("ShoppingCart");
        }

        private void Button_Shutdown(object sender, RoutedEventArgs e)
        {
            client.OnDisconnect();
            System.Windows.Application.Current.Shutdown();
        }

        #endregion
    }
}

