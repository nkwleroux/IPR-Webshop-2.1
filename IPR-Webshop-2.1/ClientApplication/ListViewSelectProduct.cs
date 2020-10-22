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

        public List<InCartProduct> Products { get; set; }

        public ListViewSelectProduct(string selectId, List<Category> categories)
        {
            this.SelectId = selectId;
            this.Categories = categories;
            this.Products = null;
        }

        public ListViewSelectProduct(string selectId, List<InCartProduct> products)
        {
            this.SelectId = selectId;
            this.Categories = null;
            this.Products = products;
                    }
    }
}
