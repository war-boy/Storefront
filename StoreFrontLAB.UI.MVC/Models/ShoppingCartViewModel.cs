using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StoreFront.DATA.EF;

namespace StoreFrontLAB.UI.MVC.Models
{
    public class ShoppingCartViewModel
    {
        public int Qty { get; set; }

        public Product Product { get; set; }

        public ShoppingCartViewModel(int qty, Product product)
        {
            Qty = qty;
            Product = product;
        }
    }
}