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
    }
}
