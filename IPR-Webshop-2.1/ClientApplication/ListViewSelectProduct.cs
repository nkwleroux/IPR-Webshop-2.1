using Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Shared
{
    public class ListViewSelectProduct
    {
        public string SelectId { get; set; }


        public List<Product> Products { get; set; }

        /// <summary>
        /// The constructor of the class ListViewSelectProduct.
        /// </summary>
        public ListViewSelectProduct(){}

        /// <summary>
        /// Overloaded constructor of the class ListViewSelectProduct.
        /// </summary>
        /// <param name="selectId">
        /// The id of the list of products.
        /// </param>
        /// <param name="products">
        /// The list of products.
        /// </param>
        public ListViewSelectProduct(string selectId, List<Product> products)
        {
            this.SelectId = selectId;
            this.Products = products;
        }


    }
}
