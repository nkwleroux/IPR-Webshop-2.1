﻿using System;
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

namespace Shared
{
    /// <summary>
    /// Interaction logic for MainProductScreenUC.xaml
    /// </summary>
    public partial class MainProductScreenUC : UserControl
    {
        private MainWindow mainWindow;

        /// <summary>
        /// The constructor of MainProductScreenUC
        /// </summary>
        /// <param name="mainWindow">
        /// MainWindow is used to change the view of the application.
        /// </param>
        public MainProductScreenUC(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;

            InitializeComponent();

            var categories = GetCategories();
            ListViewCategories.ItemsSource = categories;
        }

        /// <summary>
        /// List which contains the different categories used in the webshop.
        /// </summary>
        /// <returns>
        /// The list of categories.
        /// </returns>
        private List<Category> GetCategories()
        {
            return new List<Category>()
            {
                new Category("Aardappel, groente, fruit","/Assets/images/aardappelen_groente_fruit.png"),
                new Category("Salades, pizza, maaltijden","/Assets/images/salades-pizza-maaltijden.png"),
                new Category("Vlees, kip, vis, vega","/Assets/images/vlees-kip-vis-vega.png"),
                new Category("Kaas, vleeswaren, tapas","/Assets/images/kaas-vleeswaren-tapas.png"),
                new Category("Zuivel, plantaardig en eiren","/Assets/images/boter-eieren-zuivel.png"),
                new Category("Bakkerij en banket","/Assets/images/brood-gebak.png"),
                new Category("Ontbijtgranen, beleg, tussendoor","/Assets/images/ontbijtgranen-beleg-tussendoor.png"),
                new Category("Frisdrank, sappen, koffie, thee","/Assets/images/frisdrank-koffie-thee-sappen.png"),
                new Category("Wijn en bubbels","/Assets/images/wijn.png"),
                new Category("Bier, sterke drank, aperitieven","/Assets/images/bier-sterke-drank.png"),
                new Category("Pasta, rijst en wereldkeuken","/Assets/images/pasta-rijst-wereldkeuken.png"),
                new Category("Soepen, sauzen, kruiden, olie","/Assets/images/soepen-sauzen-kruiden-olie.png"),
                new Category("Snoep, koek, chips en chocolade","/Assets/images/chips-koek-snoep-chocolade.png"),
                new Category("Diepvries","/Assets/images/diepvries.png"),
                new Category("Baby, verzorging en hygiene","/Assets/images/baby-drogisterij.png"),
                new Category("Bewuste voeding","/Assets/images/bewuste-voeding.png"),
                new Category("Huishouden, huisdier","/Assets/images/huishouden-huisdier.png"),
                new Category("Koken, tafelen, vrije tijd","/Assets/images/koken-tafelen-nonfood.png"),
                };
        }

        /// <summary>
        /// The onClick method. Used to change the view to the selected item.
        /// </summary>
        /// <param name="sender">
        /// The data of the object.
        /// </param>
        /// <param name="e">
        /// The button event argument.
        /// </param>
        private void Button_Category(object sender, RoutedEventArgs e)
        {
            string cat = ((Button)sender).Tag.ToString();
            foreach (Category item in GetCategories())
            {
                if (cat.Equals(item.Name))
                {
                    mainWindow.SelectedCategory = item.Name;
                    mainWindow.ChangeView("CategoryProduct");
                    return;
                }
            }
        }
    }
}
