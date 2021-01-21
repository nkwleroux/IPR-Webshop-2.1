using System;
using System.Collections.Generic;
using System.Text;
using ClientApplication;
using NUnit.Framework;
using Shared;
using System.Threading;

namespace NUnitTestProject.ClientApplication
{
    [TestFixture, Apartment(ApartmentState.STA)]
    class MainWindow_NUnitTest
    {
        MainWindow window;

        [SetUp]
        public void Setup()
        {
            window = new MainWindow();
        }

        [TearDown]
        public void TearDown()
        {
        }

        [Test]
        public void Init()
        {
            //Arrange
            Client client = window.client;

            //Act
           
            //Assert
            Assert.NotNull(client);

        }

        [Test]
        public void SetCategories()
        {
            //Arrange
            Product p1 = new Product()
            {
                Category = "Bewuste voeding"
            };
            Product p2 = new Product()
            {
                Category = "Bewuste voeding"
            };
            List<Product> Products = new List<Product>
            {
                p1,
                p2
            };
            window.client.Products = Products;
            List<ListViewSelectProduct> ListViewProducts = new List<ListViewSelectProduct>();


            //Act
            window.SetCategories();
            foreach (ListViewSelectProduct LVSP in window.ListViewProducts)
            {
                ListViewProducts.Add(LVSP);
            }

            //Assert
            Assert.AreEqual(ListViewProducts, window.ListViewProducts);

        }


    }
}
