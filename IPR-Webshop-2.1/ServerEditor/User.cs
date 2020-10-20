using System;
using System.Collections.Generic;
using System.Text;

namespace ServerEditor
{
    public class User
    {
        public string FullName { get { return (this.FirstName + " " + this.LastName); } }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public double Credits { get; set; }
        public string ShippingDetails { get; set; }
        public string BillingDetails { get; set; }
    }
}
