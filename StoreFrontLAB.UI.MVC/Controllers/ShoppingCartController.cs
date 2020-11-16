using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StoreFrontLAB.UI.MVC.Models;

namespace StoreFrontLAB.UI.MVC.Controllers
{
    public class ShoppingCartController : Controller
    {
        // GET: ShoppingCart
        public ActionResult Index()
        {
            var shoppingCart = (Dictionary<int, ShoppingCartViewModel>)Session["cart"];

            if (shoppingCart == null || shoppingCart.Count == 0)
            {
                //empty cart to avoid "object reference not set" exception (NullReference)
                shoppingCart = new Dictionary<int, ShoppingCartViewModel>();

                ViewBag.Message = "Your cart is empty.";
            }

            else
            {
                ViewBag.Message = null;
            }

            return View(shoppingCart);
        }

        public ActionResult RemoveFromCart(int id)
        {
            //session cart -> local cart
            Dictionary<int, ShoppingCartViewModel> shoppingCart = (Dictionary<int, ShoppingCartViewModel>)Session["cart"];

            shoppingCart.Remove(id);

            if (shoppingCart.Count == 0)
            {
                Session["cart"] = null;
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UpdateCart(int productID, int qty)
        {
            //account for 0 qty in cart
            if (qty <= 0)
            {
                RemoveFromCart(productID);
                return RedirectToAction("Index");
            }

            //session cart -> local cart
            Dictionary<int, ShoppingCartViewModel> shoppingCart = (Dictionary<int, ShoppingCartViewModel>)Session["cart"];

            //update the qty in local storage 
            shoppingCart[productID].Qty = qty;

            //local cart -> session cart
            Session["cart"] = shoppingCart;

            if (shoppingCart.Count <= 0)
            {
                ViewBag.Message = "Your cart is empty";
            }

            return RedirectToAction("Index");
        }
    }
}