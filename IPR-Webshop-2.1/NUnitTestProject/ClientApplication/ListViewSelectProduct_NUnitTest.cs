using ClientApplication;
using NUnit.Framework;
using Shared;
using System.Collections.Generic;
using System.Threading;

namespace NUnitTestProject.ClientApplication
{

    class ListViewSelectProduct_NUnitTest
    {

        private ListViewSelectProduct listViewSelectProduct;

        [SetUp]
        public void Setup()
        {
            listViewSelectProduct = new ListViewSelectProduct();
        }

        [TearDown]
        public void TearDown()
        {
        }

        [Test]
        public void SelectedID()
        {
            //Arrange
            string selectID = "ID";

            //Act
            listViewSelectProduct.SelectId = selectID;

            //Assert
            Assert.AreEqual(selectID, listViewSelectProduct.SelectId);
        }

        [Test]
        public void Products()
        {
            //Arrange
            List<Product> products = new List<Product>();

            //Act
            listViewSelectProduct.Products = products;

            //Assert
            Assert.AreEqual(products, listViewSelectProduct.Products);
        }

    }
}
