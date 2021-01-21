using System;
using System.Collections.Generic;
using System.Text;

namespace Shared
{
    public class Category
    {
        public string Name { get; set; }

        public string CategoryImage { get; set; }

        /// <summary>
        /// The constructor of the class Category.
        /// </summary>
        public Category() {}

        /// <summary>
        /// Overloaded constructor of the class Category.
        /// </summary>
        /// <param name="name">
        /// The name of the category.
        /// </param>
        public Category(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Overloaded constructor of the class Category.
        /// </summary>
        /// <param name="name">
        /// The name of the category.
        /// </param>
        /// <param name="categoryImage">
        /// The image of the category.
        /// </param>
        public Category(string name, string categoryImage) : this(name)
        {
            this.CategoryImage = categoryImage;
        }
    }
}
