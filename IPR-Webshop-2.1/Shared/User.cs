using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared
{
    public class User
    {
        public User()
        {
            this.cart = new List<Product>();
        }
        public string FullName { get { return (this.FirstName + " " + this.LastName); } }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public double Credits { get; set; }
        public string ShippingDetails { get; set; }
        public string BillingDetails { get; set; }
        public List<Product> cart { get; set; }
        public int Id { get; set; }
        public bool IsEditor { get; set; }

        public static explicit operator User(JToken v)
        {
            throw new NotImplementedException();
        }
    }
}
