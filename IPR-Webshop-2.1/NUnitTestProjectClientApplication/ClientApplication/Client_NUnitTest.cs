using ClientApplication;
using NUnit.Framework;
using System.Threading;

namespace NUnitTestProject.ClientApplication
{
    [TestFixture, Apartment(ApartmentState.STA)]
    public class Client_NUnitTest
    {
        private Client client;

        [SetUp]
        public void Setup()
        {
            var window = new MainWindow();
            client = window.client;
        }

        [TearDown]
        public void TearDown()
        {
        }

        [Test]
        public void IsConnected_ServerOn()
        {
            //Arrange

            //Act

            //Assert
            Assert.IsTrue(client.GetClient().Connected);

            client.OnDisconnect();
        }

        [Test]
        public void IsConnected_ServerOff()
        {
            //Arrange

            //Act

            //Assert
            Assert.IsFalse(client.GetClient().Connected);

            client.OnDisconnect();
        }

        [Test]
        public void SetCurrestUser()
        {
            //Arrange
            var currentUser = new Shared.User();

            //Act
            client.SetCurrentUser(currentUser);

            //Assert
            Assert.AreEqual(currentUser, client.currentUser);

            client.OnDisconnect();
        }

        [Test]
        public void OnDisconnect_Success()
        {
            //Arrange

            //Act
            this.client.OnDisconnect();

            //Assert
            Assert.IsFalse(client.GetClient().Connected);
        }

    }
}