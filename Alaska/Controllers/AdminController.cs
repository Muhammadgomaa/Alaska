using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Alaska.Models;

namespace Alaska.Controllers
{
    public class AdminController : Controller
    {
        DB_Context dB = new DB_Context();


        //Check Category Before Create New 
        public ActionResult CheckCategory(string Cat_Name, int? Cat_ID)
        {
            //Create Case
            if (Cat_ID == null)
            {
                Category category = dB.Categories.Where(n => n.Cat_Name== Cat_Name).FirstOrDefault();

                if (category == null)
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }

            }

            //Update Case
            else
            {
                Category category = dB.Categories.Where(n => n.Cat_Name == Cat_Name && n.Cat_ID != Cat_ID).FirstOrDefault();

                if (category == null)
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }
        }

        //Check Product Before Create New 
        public ActionResult CheckProduct(string Prod_Name, int? Prod_ID)
        {
            //Create Case
            if (Prod_ID == null)
            {
                Product product = dB.Products.Where(n => n.Prod_Name == Prod_Name).FirstOrDefault();

                if (product == null)
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }

            }

            //Update Case
            else
            {
                Product product = dB.Products.Where(n => n.Prod_Name == Prod_Name && n.Prod_ID != Prod_ID).FirstOrDefault();

                if (product == null)
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }
        }

        public ActionResult Dashboard()
        {
            int id = int.Parse(Session["userid"].ToString());
            User user = dB.Users.Where(n => n.User_ID == id).SingleOrDefault();

            double TotalSales = 0 , TotalRefund = 0;

            List<double> Sales = dB.Shipping_Details.Select(n => n.AmountPaid).ToList();

            for(int i=0; i < Sales.Count; i++)
            {
                if(Sales.Count == 0)
                {
                    TotalSales = 0;
                    ViewBag.Sales = TotalSales;
                }
                else
                {
                    TotalSales += Sales[i];
                    ViewBag.Sales = TotalSales;
                }
            }

            List<double> Refund = dB.Refund_Details.Select(n => n.AmountPaid).ToList();

            for (int i = 0; i < Refund.Count; i++)
            {
                if (Refund.Count == 0)
                {
                    TotalRefund = 0;
                    ViewBag.Refund = TotalRefund;
                }
                else
                {
                    TotalRefund += Refund[i];
                    ViewBag.Refund = TotalRefund;
                }
            }

            List<Shipping_Details> Orders = dB.Shipping_Details.ToList();
            ViewBag.Orders = Orders.Count();

            List<Refund_Details> NumRefund = dB.Refund_Details.ToList();
            ViewBag.RefundNum = NumRefund.Count();

            List<User> Customer= dB.Users.Where(n=>n.User_Role=="User").ToList();
            ViewBag.Customers = Customer.Count();

            List<Product> products  = dB.Products.ToList();
            ViewBag.Products = products.Count();

            List<Category> category  = dB.Categories.ToList();
            ViewBag.Category = category.Count();

            return View(user);
        }

        public ActionResult Category()
        {
            ViewBag.Catg = dB.Categories.ToList();
            return View();
        }

        public ActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddCategory(Category category , HttpPostedFileBase photo , string submit)
        {
            //Upload File to Webserver

            photo.SaveAs(Server.MapPath($"~/Category_Images/{photo.FileName}"));

            category.Cat_Image = photo.FileName;

            dB.Categories.Add(category);
            dB.SaveChanges();

            ViewBag.Message = "The Add Category Process is Completed";


            return RedirectToAction("Category", "Admin");
        }

        public ActionResult DeleteCategory(int id)
        {
            Category category = dB.Categories.Where(n => n.Cat_ID == id).FirstOrDefault();

            System.IO.File.Delete(Server.MapPath($"~/Category_Images/{category.Cat_Image}"));

            dB.Categories.Remove(category);
            dB.SaveChanges();

            return RedirectToAction("Category", "Admin");
        }

        public ActionResult UpdateCategory(int id)
        {
            Category category = dB.Categories.Where(n => n.Cat_ID == id).FirstOrDefault();

            return View(category);
        }

        [HttpPost]
        public ActionResult UpdateCategory(Category category, string submit)
        {
            Category category1 = dB.Categories.Where(n => n.Cat_ID == category.Cat_ID).SingleOrDefault();

            category1.Cat_Name = category.Cat_Name;
            category1.Cat_Image = category.Cat_Image;

            ViewBag.Message = "The Update Category Process is Completed";


            dB.SaveChanges();

            return RedirectToAction("Category", "Admin");
        }

        public ActionResult Product()
        {
            ViewBag.Prod = dB.Products.ToList();
            return View();
        }

        public ActionResult AddProduct()
        {
            ViewBag.Catg = new SelectList(dB.Categories.ToList(), "Cat_ID", "Cat_Name");
            return View();
        }

        [HttpPost]
        public ActionResult AddProduct(Product product, HttpPostedFileBase photo, string submit)
        {
            //Upload File to Webserver

            photo.SaveAs(Server.MapPath($"~/Product_Images/{photo.FileName}"));

            product.Prod_Img= photo.FileName;

            dB.Products.Add(product);
            dB.SaveChanges();

            ViewBag.Message = "The Add Product Process is Completed";


            return RedirectToAction("Product", "Admin");
        }

        public ActionResult DeleteProduct(int id)
        {
            Product product = dB.Products.Where(n => n.Prod_ID == id).FirstOrDefault();

            System.IO.File.Delete(Server.MapPath($"~/Product_Images/{product.Prod_Img}"));

            dB.Products.Remove(product);
            dB.SaveChanges();

            return RedirectToAction("Product", "Admin");
        }

        public ActionResult UpdateProduct(int id)
        {
            Product product = dB.Products.Where(n => n.Prod_ID == id).FirstOrDefault();
            ViewBag.Catg = new SelectList(dB.Categories.ToList(), "Cat_ID", "Cat_Name",product.Cat_ID);

            return View(product);
        }

        [HttpPost]
        public ActionResult UpdateProduct(Product product , string submit)
        {
            Product product1 = dB.Products.Where(n => n.Prod_ID == product.Prod_ID).SingleOrDefault();

            product1.Prod_Name = product.Prod_Name;
            product1.Price = product.Price;
            product1.Quantity = product.Quantity;
            product1.Prod_Img = product.Prod_Img;
            product1.Cat_ID = product.Cat_ID;

            ViewBag.Message = "The Update Product Process is Completed";

            dB.SaveChanges();

            return RedirectToAction("Product", "Admin");
        }

        public ActionResult Account()
        {
            ViewBag.Acc = dB.Users.ToList();
            return View();
        }

        public ActionResult DeleteAccount(int id)
        {
            User user = dB.Users.Where(n => n.User_ID == id).FirstOrDefault();

            dB.Users.Remove(user);
            dB.SaveChanges();

            return RedirectToAction("Account", "Admin");
        }

        public ActionResult Orders()
        {
            ViewBag.Orders = dB.Shipping_Details.ToList();
            return View();
        }

        public ActionResult Refund()
        {
            ViewBag.Refund = dB.Refund_Details.ToList();
            return View();
        }

        public ActionResult AcceptRefund(int id)
        {
            Shipping_Details shipping = dB.Shipping_Details.Where(n => n.Shipping_ID == id).SingleOrDefault();
            
            dB.Shipping_Details.Remove(shipping);
            dB.SaveChanges();

            return RedirectToAction("Orders");
        }

        public ActionResult RefuseRefund(int id)
        {
            Refund_Details refund = dB.Refund_Details.Where(n => n.Refund_ID == id).SingleOrDefault();

            dB.Refund_Details.Remove(refund);
            dB.SaveChanges();

            return RedirectToAction("Refund");
        }

        public ActionResult Message()
        {
            ViewBag.Mess = dB.Messages.ToList();
            return View();
        }

    }
}