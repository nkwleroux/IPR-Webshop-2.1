using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Shared
{
    public class Product
    {
        public Product()
        {

        }
        public Product(ProductSerializable product)
        {
            this.Name = product.Name;
            this.Price = product.Price;
            this.Amount = product.Amount;
            this.Id = product.Id;
            this.Image = BitmapConvertor.LoadImage(product.Image);
        }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Amount { get; set; }
        public int Id { get; set; }
        public BitmapImage Image { get; set; }
    }
}
