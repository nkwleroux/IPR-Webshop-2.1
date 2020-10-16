using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace ClientApplication
{
    class Product
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public string ProductImage { get; set; } //Not sure what type (bitmap or image or string)

        public Product(String name, double price, string description)
        {
            this.Name = name;
            this.Price = price;
            this.Description = description;
            this.ProductImage = "";
        }

        public override string ToString()
        {
            return "Product : " + Name + "\n"
                                + Price + " $\n"
                                + Description + "\n";
        }
    }
}
