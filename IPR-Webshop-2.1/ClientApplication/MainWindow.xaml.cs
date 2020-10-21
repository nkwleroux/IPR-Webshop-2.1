using ClientApplication.AccountScreen;
using ClientApplication.FAQScreen;
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
        private FAQScreenUC fAQUC;
        private LoginScreenUC loginScreenUC;
        private RegisterScreenUC registerScreenUC;
        private AccountOverviewUC accountOverviewUC;

        private bool HasAccount { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            init();
        }

        private void init()
        {
            // Initialise the different pages.

            mainProductScreenUC = new MainProductScreenUC(this);
            LayoutControl.Children.Add(mainProductScreenUC);
            fAQUC = new FAQScreenUC(this);
            LayoutControl.Children.Add(fAQUC);
            loginScreenUC = new LoginScreenUC(this);
            LayoutControl.Children.Add(loginScreenUC);
            registerScreenUC = new RegisterScreenUC(this);
            LayoutControl.Children.Add(registerScreenUC);
            accountOverviewUC = new AccountOverviewUC(this);
            LayoutControl.Children.Add(accountOverviewUC);

            ChangeView("MainProduct");
        }

        public void ChangeView(String viewName)
        {
            mainProductScreenUC.Visibility = Visibility.Hidden;
            fAQUC.Visibility = Visibility.Hidden;
            loginScreenUC.Visibility = Visibility.Hidden;
            registerScreenUC.Visibility = Visibility.Hidden;
            accountOverviewUC.Visibility = Visibility.Hidden;
            mainscreen.MinHeight = 480;
            mainscreen.MinWidth = 600;

            switch (viewName)
            {
                case "MainProduct":
                    mainProductScreenUC.Visibility = Visibility.Visible;
                    break;
                case "FAQ":
                    fAQUC.Visibility = Visibility.Visible;
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
                    mainscreen.MinHeight = 660;
                    mainscreen.MinWidth = 960;
                    if (!HasAccount)
                    {
                        HasAccount = true;
                    }

                    break;
                default:
                    break;
            }
        }

        #region //Button onClicks

        private void Button_FAQ(object sender, RoutedEventArgs e)
        {
            ChangeView("FAQ");
        }

        private void Button_AppName(object sender, RoutedEventArgs e)
        {
            ChangeView("MainProduct");
        }

        private void Button_MyAccount(object sender, RoutedEventArgs e)
        {
            ChangeView("Login");
        }
        #endregion
    }
}

