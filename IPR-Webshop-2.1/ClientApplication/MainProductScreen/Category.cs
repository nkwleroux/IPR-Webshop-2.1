using System;
using System.Collections.Generic;
using System.Text;

namespace ClientApplication
{
    class Category
    {
        public string Name { get; set; }

        public string CategoryImage { get; set; }

        public Category(string name)
        {
            this.Name = name;
        }

        public Category(string name, string categoryImage) : this(name)
        {
            this.CategoryImage = categoryImage;
        }
    }
}
