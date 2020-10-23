using Newtonsoft.Json;
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
            this.Image = new byte[0];
        }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Amount { get; set; }
        public string Category { get; set; }
        public int Id { get; set; }
        public byte[] Image { get; set; }

        [JsonIgnore]
        public BitmapImage bitmapImage { get { return BitmapConverter.LoadImage(Image); } }

    }
}
