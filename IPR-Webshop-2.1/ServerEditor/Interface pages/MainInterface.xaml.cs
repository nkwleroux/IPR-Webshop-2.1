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

namespace ServerEditor.Interface_pages
{
    /// <summary>
    /// Interaction logic for MainInterface.xaml
    /// </summary>
    public partial class MainInterface : Page
    {
        public MainInterface()
        {
            InitializeComponent();
            this.ViewPort_ProductEditor.Navigate(new Page_ProductEditor());
            this.ViewPort_UserEditor.Navigate(new Page_UserEditor());
        }
    }
}
