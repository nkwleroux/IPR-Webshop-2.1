using Shared;
using NUnit.Framework;
using Shared;
using System.Collections.Generic;
using System.Threading;

namespace NUnitTestProject.ClientApplication
{
    [TestFixture]
    class Category_NUnitTest
    {

        private Category category;

        [SetUp]
        public void Setup()
        {
            category = new Category();
        }

        [TearDown]
        public void TearDown()
        {
        }

        [Test]
        public void Name()
        {
            //Arrange
            string name = "Name";

            //Act
            category.Name = name;

            //Assert
            Assert.AreEqual(name, category.Name);
        }

        [Test]
        public void CategoryImage()
        {
            //Arrange
            string categoryImage = "CategoryImage";

            //Act
            category.CategoryImage = categoryImage;

            //Assert
            Assert.AreEqual(categoryImage, category.CategoryImage);
        }

    }
}
