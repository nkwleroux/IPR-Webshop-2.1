using ClientApplication.AccountScreen;
using ClientApplication.CategoryProductScreen;
using ClientApplication.PreviousOrderScreen;
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
        private PreviousOrderScreenUC previousOrderScreenUC;
        private ShoppingCartUC shoppingCartUC;
        private ProductDetailScreenUC productDetailScreenUC;
        private PurchaseCheckoutUC purchaseCheckoutUC;
        private Client client;

        private bool HasAccount { get; set; }

        private List<InCartProduct> InCartProducts { get; set; }

        public void SetInCartProducts(List<InCartProduct> InCartProducts) { this.InCartProducts = InCartProducts; }

        public List<InCartProduct> GetInCartProducts() { return this.InCartProducts; }

        public string SelectedCategory { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            client = new Client(this);

            Init();


        }

        public List<ListViewSelectProduct> ListViewProducts { get; set; }

        public void SetCategory()
        {
            ListViewProducts = new List<ListViewSelectProduct>();
            foreach (Product p in client.Products)
            {
                foreach (ListViewSelectProduct LVSP in ListViewProducts)
                {
                    //If cateogry doesnt exist, add category
                    if (!LVSP.SelectId.Equals(p.Category))
                    {
                        ListViewProducts.Add(
                            new ListViewSelectProduct(
                                p.Category, new List<Product>() { p }));

                    }
                    //If category exist, add product to category
                    else
                    {
                        LVSP.Products.Add(p);
                    }
                }
            }
        }

        private void Init()
        {
            //Init products
            SetInCartProducts(new List<InCartProduct>()
                {
                    new InCartProduct("Aardappel, groente, fruit",2,50.0,"/Assets/images/aardappelen_groente_fruit.png"),
                    new InCartProduct("Salades, pizza, maaltijden",3,20.0,"/Assets/images/salades-pizza-maaltijden.png"),
                    new InCartProduct("Vlees, kip, vis, vega",1,2.0,"/Assets/images/vlees-kip-vis-vega.png"),
                    new InCartProduct("Kaas, vleeswaren, tapas",1,3.0,"/Assets/images/kaas-vleeswaren-tapas.png"),
                    new InCartProduct("Zuivel, plantaardig en eiren",5,20.0,"/Assets/images/boter-eieren-zuivel.png")
                });

            /*List<InCartProduct> InCartP = new List<InCartProduct>();

            foreach (Product p in products)
            {
                string productName = p.Name;
                int productAmount = p.Amount;
                double productPrice = p.Price;
                //string ProductImage = productImage;
                InCartProduct ICProdcut = new InCartProduct(productName, productAmount, productPrice, null);
                InCartP.Add(ICProdcut);
            }

            SetInCartProducts(InCartP);*/



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
            previousOrderScreenUC = new PreviousOrderScreenUC(this);
            LayoutControl.Children.Add(previousOrderScreenUC);
            shoppingCartUC = new ShoppingCartUC(this);
            LayoutControl.Children.Add(shoppingCartUC);
            productDetailScreenUC = new ProductDetailScreenUC(this);
            LayoutControl.Children.Add(productDetailScreenUC);
            purchaseCheckoutUC = new PurchaseCheckoutUC(this);
            LayoutControl.Children.Add(purchaseCheckoutUC);

            ChangeView("MainProduct");
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
            previousOrderScreenUC.Visibility = Visibility.Hidden;
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
                    if (!HasAccount)
                    {
                        HasAccount = true;
                    }
                    break;
                case "CategoryProduct":
                    categoryDetailScreenUC.SetListView(SelectedCategory);
                    categoryDetailScreenUC.Visibility = Visibility.Visible;
                    break;
                case "PreviousOrder":
                    previousOrderScreenUC.SetProductsListSource();
                    previousOrderScreenUC.Visibility = Visibility.Visible;
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
            System.Windows.Application.Current.Shutdown();
        }

        #endregion
    }
}

