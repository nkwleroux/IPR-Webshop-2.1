using Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientApplication
{   
    public class ListViewSelectProduct
    {
        public string SelectId { get; set; }

        //temp
        public List<Category> Categories { get; set; }

        public List<InCartProduct> ICProducts { get; set; }

        public List<Product> Products { get; set;}

        public ListViewSelectProduct(string selectId, List<Category> categories)
        {
            this.SelectId = selectId;
            this.Categories = categories;
            this.ICProducts = null;
        }

        public ListViewSelectProduct(string selectId, List<InCartProduct> products)
        {
            this.SelectId = selectId;
            this.Categories = null;
            this.ICProducts = products;
        }

        public ListViewSelectProduct(string selectId,List<Product> products)
        {
            this.SelectId = SelectId;
            this.Products = products;
        }
    }
}
