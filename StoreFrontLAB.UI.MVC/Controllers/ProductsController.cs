using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using StoreFrontLAB.UI.MVC.Utilities;
using StoreFront.DATA.EF;
using PagedList;
using PagedList.Mvc;
using StoreFrontLAB.UI.MVC.Models;
using System.Data.Entity.Validation;

namespace StoreFrontLAB.UI.MVC.Controllers
{

    public class ProductsController : Controller
    {
        private StoreFrontEntities db = new StoreFrontEntities();

        [Authorize]
        public ActionResult AddToCart(int qty, int productID)
        {
            if (qty <= 0)
            {
                return RedirectToAction("Details", new { id = productID });
            }

            //Empty, local shopping cart
            Dictionary<int, ShoppingCartViewModel> localCart = null;

            if (Session["cart"] != null)
            {
                localCart = (Dictionary<int, ShoppingCartViewModel>)Session["cart"];
            }
            else
            {
                localCart = new Dictionary<int, ShoppingCartViewModel>();
            }

            Product product = db.Products.Where(p => p.ProductID == productID).FirstOrDefault();

            //prevent user bypassing logic
            if (product == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ShoppingCartViewModel item = new ShoppingCartViewModel(qty, product);

                //if cart already contains the item that is being added
                if (localCart.ContainsKey(product.ProductID))
                {
                    localCart[product.ProductID].Qty += qty;
                }
                else
                {
                    localCart.Add(product.ProductID, item);
                }


                Session["cart"] = localCart;
            }

            return RedirectToAction("Index", "ShoppingCart");


        }

        #region Index/Filters

        // GET: Products
        public ActionResult Index(string searchString, int page = 1)
        {
            int pageSize = 9;

            var products = db.Products.OrderBy(p => p.ProductName).ToList();

            if (!string.IsNullOrEmpty(searchString))
            {
                products = products.Where(p => p.ProductName.ToLower().Contains(searchString.ToLower()) || p.Category.CategoryName.ToLower().Contains(searchString.ToLower())).ToList();
            }

            ViewBag.NumOfResults = products.Count();
            return View(products.ToPagedList(page, pageSize));
        }
        #endregion

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                ViewBag.UnitsInStock = 0;
                ViewBag.IsInStock = false;
                return HttpNotFound();
            }

            ViewBag.UnitsInStock = product.UnitsInStock;
            ViewBag.IsInStock = true;
            return View(product);
        }

        // GET: Products/Create
        [Authorize(Roles = "Admin, Employee, User")]
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");

            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,ProductName,CategoryID,Size,Price,InStock,UnitsInStock,Condition,Source,SupplierID,ProductImage,Description")] Product product, HttpPostedFileBase productImg)
        {
            if (ModelState.IsValid)
            {
                #region FileUpload

                string imgName = "noImage.png";

                if (productImg != null)
                {
                    imgName = productImg.FileName;

                    string ext = imgName.Substring(imgName.LastIndexOf('.'));

                    string[] validExts = { ".jpeg", ".jpg", ".png" };

                    if (validExts.Contains(ext.ToLower()) && (productImg.ContentLength <= 4194304))//4mb
                    {
                        imgName = Guid.NewGuid() + ext.ToLower();

                        string savePath = Server.MapPath("~/Content/img/shop/");

                        Image convertedImage = Image.FromStream(productImg.InputStream);

                        int maxImgSize = 500;

                        int maxThumbSize = 255;

                        ImageService.ResizeImage(savePath, imgName, convertedImage, maxImgSize, maxThumbSize);
                    }
                    else
                    {
                        imgName = "noImage.png";
                    }

                    product.ProductImage = imgName;
                    #endregion
                }

                db.Products.Add(product);


                db.SaveChanges();



                return RedirectToAction("Index", "Home");
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);

            return View(product);
        }

        // GET: Products/Edit/5
        [Authorize(Roles = "Admin, Employee")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);

            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,ProductName,CategoryID,Size,Price,InStock,UnitsInStock,Condition,Source,SupplierID,ProductImage,Description")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);

            return View(product);
        }

        // GET: Products/Delete/5
        [Authorize(Roles = "Admin, Employee")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }



    }


}

