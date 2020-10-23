using Newtonsoft.Json;
using Shared;
using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Security.Policy;
using System.Text;

namespace ServerApplication.Server_logics
{
    public class Database
    {
        private int IdCount;
        public List<Product> Products;
        public List<User> Users;
        public Database()
        {
            this.Products = new List<Product>();
            this.Users = new List<User>();
            this.Users.Add(new User
            {
                Username = "admin",
                Password = "admin",
                IsEditor = true
            });
        }

        #region Json constructors
        public string[] GetAllProductsJson()
        {
            string[] jsons = new string[this.Products.Count];

            for(int i = 0; i < this.Products.Count; i++)
            {
                jsons[i] = JsonConvert.SerializeObject(Products[i]);
            }

            return jsons;
        }
        public string[] GetAllUsersJson()
        {
            string[] jsons = new string[this.Products.Count];

            for (int i = 0; i < this.Users.Count; i++)
            {
                jsons[i] = JsonConvert.SerializeObject(Users[i]);
            }

            return jsons;
        }
        #endregion

        #region List editors
        public void RemoveUser(User user)
        {
            User selected = null;
            foreach(User userI in Users)
            {
                if(userI.Username == user.Username)
                {
                    selected = userI;
                }
            }

            if(selected != null)
            {
                this.Users.Remove(selected);
            }
        }
        public void AddUser(User user)
        {
            foreach (User userI in Users)
            {
                if (userI.Username == user.Username)
                {
                    return;
                }
            }
            user.Id = IdCount;
            IdCount++;
            this.Users.Add(user);
        }
        public void EditUser(User user)
        {
            int selected = -1;
            for (int i = 0; i < this.Users.Count; i++)
            {
                if (Users[i].Username == user.Username)
                {
                    selected = i;
                }
            }

            if (selected >= 0)
            {
                this.Users.RemoveAt(selected);
                this.Users.Insert(selected, user);
            }
        }
        public void RemoveProduct(Product product)
        {
            Product selected = null;
            foreach (Product productI in Products)
            {
                if (productI.Id == product.Id)
                {
                    selected = productI;
                }
            }

            if (selected != null)
            {
                this.Products.Remove(selected);
            }
        }
        public void AddProduct(Product product)
        {
            foreach (Product productI in Products)
            {
                if (productI.Name == product.Name)
                {
                    return;
                }
            }
            product.Id = IdCount;
            IdCount++;
            this.Products.Add(product);
        }
        public void EditProduct(Product product)
        {
            int selected = -1;
            for (int i = 0; i < this.Products.Count; i++)
            {
                if (Products[i].Id == product.Id)
                {
                    selected = i;
                }
            }

            if (selected >= 0)
            {
                this.Products.RemoveAt(selected);
                this.Products.Insert(selected, product);
            }
        }
        #endregion
        public List<Product> getCategoryList(string category)
        {
            List<Product> returningList = new List<Product>();
            foreach(Product product in this.Products)
            {
                if(product.Category == category)
                {
                    returningList.Add(product);
                }
            }
            return returningList;
        }
        public User CheckUserLogin(string username, string password, bool isEditor)
        {
            foreach(User user in this.Users)
            {
                if(user.Username == username && user.Password == password)
                {
                    return user;
                }
            }
            return null;
        }
    }
}
