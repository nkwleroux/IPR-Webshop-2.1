using System;
using System.Collections.Generic;
using System.Text;

namespace ClientApplication
{
    class User
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string shippingAddress { get; set; }
        public string billingAddress { get; set; }
        public double credit { get; set; }

        public User(string firstName, string lastName, string shippingAddress, string billingAddress, double credit)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.shippingAddress = shippingAddress;
            this.billingAddress = billingAddress;
            this.credit = credit;
        }
    }
}
