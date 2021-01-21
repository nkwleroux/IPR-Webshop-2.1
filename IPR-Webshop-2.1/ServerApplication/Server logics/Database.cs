using Newtonsoft.Json;
using Shared;
using System;
using System.Collections.Generic;
using System.IO;
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
                // default admin credentials
                Username = "admin",
                Password = "admin",
                IsEditor = true
            });
        }

        #region Json constructors
        public string[] GetAllProductsJson()
        {
            string[] jsons = new string[this.Products.Count];

            for (int i = 0; i < this.Products.Count; i++)
            {
                jsons[i] = JsonConvert.SerializeObject(Products[i]);
            }

            return jsons;
        }

        public string[] GetAllUsersJson()
        {
            string[] jsons = new string[this.Users.Count];

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
            foreach (User userI in Users)
            {
                if (userI.Username == user.Username)
                {
                    selected = userI;
                }
            }

            if (selected != null)
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
        /// <summary>
        /// returns a product list based on category.
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public List<Product> GetProductListFromCategory(string category)
        {
            List<Product> returningList = new List<Product>();
            foreach (Product product in this.Products)
            {
                if (product.Category == category)
                {
                    returningList.Add(product);
                }
            }
            return returningList;
        }
        /// <summary>
        /// This method will indentify a attempt of login.
        /// </summary>
        /// <param name="username"> attempt </param>
        /// <param name="password"> attempt </param>
        /// <param name="isEditor"> attempt </param>
        /// <returns></returns>
        public User CheckUserLogin(string username, string password, bool isEditor)
        {
            foreach (User user in this.Users)
            {
                if (user.Username == username && user.Password == password)
                {
                    return user;
                }
            }
            return null;
        }
        /// <summary>
        /// This method will register a new user to our database.
        /// </summary>
        /// <param name="username"> username must be unique </param>
        /// <param name="password"> password of user </param>
        /// <returns></returns>
        internal User RegisterUser(string username, string password)
        {
            foreach (User u in this.Users)
            {
                if (u.Username == username)
                {
                    return null;
                }
            }
            User user = new User()
            {
                Username = username,
                Password = password
            };
            this.Users.Add(user);
            return user;
        }
        /// <summary>
        /// This method will save our current database to a file.
        /// </summary>
        /// <param name="dirName"></param>
        public void Save(string dirName)
        {
            string location = Environment.CurrentDirectory + @"\" + dirName;
            string productsFile = location + @"\products.txt";
            string usersFile = location + @"\users.txt";
            if (!Directory.Exists(location))
            {
                Directory.CreateDirectory(location);
            }
            if (!File.Exists(productsFile))
            {
                File.Create(productsFile);
            }
            if (!File.Exists(usersFile))
            {
                File.Create(usersFile);
            }

            using (StreamWriter streamWriter = new StreamWriter(productsFile, false))
            {
                string products = JsonConvert.SerializeObject(this.Products);
                streamWriter.Write(products);
                streamWriter.Flush();
            }
            using (StreamWriter streamWriter = new StreamWriter(usersFile, false))
            {
                string users = JsonConvert.SerializeObject(this.Users);
                streamWriter.Write(users);
                streamWriter.Flush();
            }
        }
        /// <summary>
        /// This method will load our latest data from a file.
        /// </summary>
        /// <param name="dirName"></param>
        public void Load(string dirName)
        {
            string location = Environment.CurrentDirectory + @"\" + dirName;
            string productsFile = location + @"\products.txt";
            string usersFile = location + @"\users.txt";
            if (Directory.Exists(location))
            {
                if (File.Exists(productsFile))
                {
                    using(StreamReader streamReader = new StreamReader(productsFile))
                    {
                        string productsJson = streamReader.ReadToEnd();
                        List<Product> loaded = JsonConvert.DeserializeObject<List<Product>>(productsJson);
                        if (loaded != null)
                            this.Products = loaded;
                        streamReader.Close();
                    }
                }
                if (File.Exists(usersFile))
                {
                    using (StreamReader streamReader = new StreamReader(usersFile))
                    {
                        string userJson = streamReader.ReadToEnd();
                        List<User> loaded = JsonConvert.DeserializeObject<List<User>>(userJson);
                        if (loaded != null)
                            this.Users = loaded;
                        streamReader.Close();
                    }
                }
            }
        }
        /// <summary>
        /// Updates stock numbers when a product is bougt.
        /// </summary>
        /// <param name="product"> which product to change stock </param>
        /// <param name="add"> add or remove a stock </param>
        /// <returns></returns>
        public bool CheckStockAndUpdate(Product product, bool add)
        {
            int amount = product.Amount;
            if (add)
            {
                foreach (Product p in this.Products)
                {
                    if (p.Id == product.Id)
                    {
                        if (p.Amount >= product.Amount)
                        {
                            p.Amount -= product.Amount;
                            return true;
                        }
                    }
                }
                return false;
            }
            else
            {
                foreach (Product p in this.Products)
                {
                    if (p.Id == product.Id)
                    {
                        p.Amount += product.Amount;
                    }
                }
                return false;
            }
        }
    }
}
