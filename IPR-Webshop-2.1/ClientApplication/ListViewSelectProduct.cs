using Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace ClientApplication
{   
    public class ListViewSelectProduct
    {
        public string SelectId { get; set; }

        public List<Category> Categories { get; set; }

        public List<Product> Products { get; set;}

        public ListViewSelectProduct(string selectId, List<Category> categories)
        {
            this.SelectId = selectId;
            this.Categories = categories;
        }

        public ListViewSelectProduct(string selectId,List<Product> products)
        {
            this.SelectId = selectId;
            this.Products = products;
        }

    }
}
