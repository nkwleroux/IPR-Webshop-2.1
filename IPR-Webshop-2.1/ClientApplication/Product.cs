using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace ClientApplication
{
    class Product
    {
        public string name { get; set; }
        public double price { get; set; }
        public string description { get; set; }
        public Image image { get; set; } //Not sure what type (bitmap or image)

        public Product(String name, double price, string description)
        {
            this.name = name;
            this.price = price;
            this.description = description;
            this.image = null;
        }

        public override string ToString()
        {
            return "Product : " + name + "\n"
                                + price + " $\n"
                                + description + "\n";
        }
    }
}
