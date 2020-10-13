using System;
using System.Collections.Generic;
using System.Text;

namespace ClientApplication
{
    class Client
    {
        private Database databaseConnection;
        private List<Product> products { get; set; }
        private User currentUser;

        public void setDatabaseConnection(Database databaseConnection) { this.databaseConnection = databaseConnection; }
        private void setCurrentUser(User currentUser) { this.currentUser = currentUser; }

        public Client(Database databaseConnection, List<Product> products, User currentUser)
        {
            this.databaseConnection = databaseConnection;
            this.products = products;
            this.currentUser = currentUser;
        }
    }
}
