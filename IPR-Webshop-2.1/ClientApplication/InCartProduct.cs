using System;
using System.Collections.Generic;
using System.Text;

namespace ClientApplication
{
    public class InCartProduct
    {
        public string ProductName { get; set; }
        public int ProductAmount { get; set; }
        public double ProductPrice { get; set; }
        public string ProductImage { get; set; }

        public InCartProduct(string productName, int productAmount, double productPrice, string productImage)
        {
            this.ProductName = productName;
            this.ProductAmount = productAmount;
            this.ProductPrice = productPrice;
            this.ProductImage = productImage;
        }
    }
}
