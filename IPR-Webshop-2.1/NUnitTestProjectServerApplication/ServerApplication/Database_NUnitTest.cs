using Shared;
using NUnit.Framework;
using ServerApplication;
using System.Collections.Generic;
using System.Threading;
using ServerApplication.Server_logics;

namespace NUnitTestProject.ServerApplication
{
    [TestFixture]
    class Database_NUnitTest
    {

        Database database;

        [SetUp]
        public void Setup()
        {
            database = new Database();
        }

        [TearDown]
        public void TearDown()
        {
        }

        [Test]
        public void Users_ContainsAdmin()
        {
            //Arrange
            User expected = new User
            {
                Username = "admin",
                Password = "admin",
                IsEditor = true
            };
            User actual = null;

            //Act
            foreach (User u in database.Users)
            {
                if (u.Username.Equals(expected.Username)
                    && u.Password.Equals(expected.Password)
                    && u.IsEditor == expected.IsEditor)
                {
                    actual = u;
                    return;
                }

            }
            //Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetAllProductsJson()
        {
            //Arrange
            List<Product> list = new List<Product>(){
                new Product(){
                Name = "Apple",
                Price = 16.0,
                Amount = 5,
                Category = "Fruit"
                }
            };

            database.Products = list;

            string expected = "{" + "\"Name\":\"Apple\",\"Price\":16.0,\"Amount\":5,\"Category\":\"Fruit\",\"Id\":0,\"Image\":\"\"}";

            //Act
            string[] databaseOutput = database.GetAllProductsJson();
            string actual = databaseOutput[0].ToString();

            //Assert
            Assert.AreEqual(expected, actual);

        }

        [Test]
        public void GetAllUsersJson()
        {
            //Arrange
            List<User> list = new List<User>(){
                new User(){
                Username = "admin",
                Password = "admin",
                IsEditor = true
                }
            };

            database.Users = list;

            string expected = "{" + "\"FullName\":\" \",\"FirstName\":null,\"LastName\":null,\"Username\":\"admin\",\"Password\":\"admin\",\"Credits\":0.0,\"ShippingDetails\":null,\"BillingDetails\":null,\"cart\":[],\"Id\":0,\"IsEditor\":true}";

            //Act
            string[] databaseOutput = database.GetAllUsersJson();
            string actual = databaseOutput[0].ToString();

            //Assert
            Assert.AreEqual(expected, actual);

        }

        [Test]
        public void AddUser()
        {
            //Arrange
            User user = new User()
            {
                Username = "admin2",
                Password = "admin2",
                IsEditor = true
            };

            //Act
            database.AddUser(user);

            //Assert
            Assert.IsTrue(database.Users.Contains(user));
        }

        [Test]
        public void RemoveUser()
        {
            //Arrange
            User user = new User()
            {
                Username = "admin2",
                Password = "admin2",
                IsEditor = true
            };

            //Act
            database.AddUser(user);
            database.RemoveUser(user);

            //Assert
            Assert.IsFalse(database.Users.Contains(user));
        }

        [Test]
        public void EditUser()
        {
            //Arrange
            User before = new User()
            {
                Username = "admin2",
                Password = "admin2",
                IsEditor = true
            };
            User after = before;
            after.IsEditor = false;

            //Act
            database.AddUser(before);
            database.EditUser(after);

            //Assert
            Assert.IsFalse(database.Users[1].IsEditor);
        }


        [Test]
        public void AddProduct()
        {
            //Arrange
            Product product = new Product()
            {
                Name = "Apple",
                Price = 16.0,
                Amount = 5,
                Category = "Fruit"
            };

            //Act
            database.AddProduct(product);

            //Assert
            Assert.IsTrue(database.Products.Contains(product));
        }

        [Test]
        public void RemoveProduct()
        {
            //Arrange
            Product product = new Product()
            {
                Name = "Apple",
                Price = 16.0,
                Amount = 5,
                Category = "Fruit"
            };

            //Act
            database.AddProduct(product);
            database.RemoveProduct(product);

            //Assert
            Assert.IsFalse(database.Products.Contains(product));
        }

        [Test]
        public void EditProduct()
        {
            //Arrange
            Product before = new Product()
            {
                Name = "Apple",
                Price = 16.0,
                Amount = 5,
                Category = "Fruit"
            };
            Product after = before;
            after.Price = 20.0;

            //Act
            database.AddProduct(before);
            database.EditProduct(after);

            //Assert
            Assert.AreEqual(20.0, database.Products[0].Price);
        }

        [Test]
        public void GetCategoryList()
        {
            //Arrange
            List<Product> list = new List<Product>(){
                new Product(){
                Name = "Apple",
                Price = 4.0,
                Amount = 5,
                Category = "Fruit"
                },
                 new Product(){
                Name = "Banana",
                Price = 16.0,
                Amount = 4,
                Category = "Fruit"
                },
                new Product(){
                Name = "Fish",
                Price = 16.0,
                Amount = 2,
                Category = "Meat"
                },
                new Product(){
                Name = "Carrot",
                Price = 5.0,
                Amount = 25,
                Category = "Vegtable"
                }
            };
            database.Products = list;

            //Act
            List<Product> productList = database.GetProductListFromCategory("Fruit");

            //Assert
            Assert.AreEqual(2, productList.Count);
        }

        [Test]
        public void CheckUserLogin()
        {
            //Arrange
            User expected = database.Users[0];

            //Act
            User actual = database.CheckUserLogin(expected.Username, expected.Password, expected.IsEditor);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CheckStockAndUpdate_Add()
        {
            //Arrange

            Product Product = new Product()
            {
                Name = "Banana",
                Price = 16.0,
                Amount = 4,
                Category = "Fruit",
            };
            Product updateProduct = new Product()
            {
                Name = "Banana",
                Price = 16.0,
                Amount = 10,
                Category = "Fruit",
            };

            //Act
            database.AddProduct(Product);
            bool update = database.CheckStockAndUpdate(updateProduct, false);

            //Assert
            Assert.AreEqual(14, database.Products[0].Amount);
        }

        [Test]
        public void CheckStockAndUpdate_Subtract()
        {
            //Arrange

            Product Product = new Product()
            {
                Name = "Banana",
                Price = 16.0,
                Amount = 10,
                Category = "Fruit",
            };
            Product updateProduct = new Product()
            {
                Name = "Banana",
                Price = 16.0,
                Amount = 4,
                Category = "Fruit",
            };

            //Act
            database.AddProduct(Product);
            bool update = database.CheckStockAndUpdate(updateProduct, true);

            //Assert
            Assert.AreEqual(6, database.Products[0].Amount);
        }





        /*        [Test]
                public void TestTemplate()
                {
                    //Arrange

                    //Act

                    //Assert
                }*/

    }
}
