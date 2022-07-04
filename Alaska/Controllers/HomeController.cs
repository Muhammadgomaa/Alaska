using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Alaska.Models;

namespace Alaska.Controllers
{
    public class HomeController : Controller
    {
        DB_Context dB = new DB_Context();

        //Check User Before Create New 
        public ActionResult CheckUser(string User_Email , string User_Phone , int? User_ID)
        {
            //Create Case
            if (User_ID == null)
            {
                User user = dB.Users.Where(n => n.User_Email== User_Email || n.User_Phone == User_Phone).FirstOrDefault();

                if (user == null)
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
                User user = dB.Users.Where(n => ( n.User_Email == User_Email || n.User_Phone == User_Phone) && n.User_ID != User_ID ).FirstOrDefault();

                if (user == null)
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }
        }

        public ActionResult Login()
        {
            //if coockies founded in pc
            if (Request.Cookies["coockie"] != null)
            {
                Session["userid"] = Request.Cookies["coockie"].Values["id"];
                return RedirectToAction("Home");
            }

            return View();
        }

        [HttpPost]
        public ActionResult Login(User user, string reuserme)
        {
            User user1 = dB.Users.Where(n => n.User_Email== user.User_Email && n.User_Password== user.User_Password).SingleOrDefault();

            if (user1 != null)
            {
                Session.Add("userid", user1.User_ID);

                //cookie 
                //if checkbox is checked
                if (reuserme == "true")
                {
                    HttpCookie cookie = new HttpCookie("coockie"); //create file
                    cookie.Values.Add("id", user1.User_ID.ToString()); //save data
                    cookie.Expires = DateTime.Now.AddDays(90); //expire date
                    Response.Cookies.Add(cookie);
                }

                return RedirectToAction("Home");
            }
            else
            {
                ViewBag.status = "Invalid Username or Password";
                return View();
            }
        }

        public ActionResult Logout()
        {
            Session["userid"] = null;
            HttpCookie cookie = new HttpCookie("coockie"); //create file
            cookie.Expires = DateTime.Now.AddDays(-15); //expire date (to delete coockie)
            Response.Cookies.Add(cookie);
            return RedirectToAction("Login");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(User user,string submit)
        {
            dB.Users.Add(user);
            dB.SaveChanges();

            ViewBag.Message = "The Register Process is Completed";

            return RedirectToAction("Login");
        }

        public ActionResult Home()
        {
            ViewBag.Catg = dB.Categories.ToList();
            return View();
        }
        
        public ActionResult Shop()
        {
            ViewBag.Catg = dB.Categories.ToList();
            ViewBag.Prod = dB.Products.ToList();

            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            int id = int.Parse(Session["userid"].ToString());
            User user = dB.Users.Where(n => n.User_ID == id).SingleOrDefault();

            return View(user);
        }

        [HttpPost]
        public ActionResult Contact(Message message)
        {
            dB.Messages.Add(message);
            dB.SaveChanges();

            return RedirectToAction("Home");
        }

        public ActionResult AddAccount()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddAccount(User user, string submit)
        {
            dB.Users.Add(user);
            dB.SaveChanges();

            ViewBag.Message = "The Add Admin Account Process is Completed";


            return RedirectToAction("Account", "Admin");
        }

        public ActionResult UpdateAccount(int id)
        {
            User user = dB.Users.Where(n => n.User_ID == id).FirstOrDefault();

            return View(user);
        }

        [HttpPost]
        public ActionResult UpdateAccount(User user, string submit)
        {
            User user1 = dB.Users.Where(n => n.User_ID == user.User_ID).SingleOrDefault();

            user1.User_Email = user.User_Email;
            user1.First_Name = user.First_Name;
            user1.Last_Name = user.Last_Name;
            user1.User_Password = user.User_Password;
            user1.User_Phone = user.User_Phone;
            user1.User_Role = user.User_Role;

            ViewBag.Message = "The Update Admin Account Process is Completed";

            dB.SaveChanges();

            return RedirectToAction("Account", "Admin");
        }

        public ActionResult AddCart(int id)
        {
            //First Item Added
            if (Session["cart"] == null)
            {
                List<Items> Cart = new List<Items>();

                Product product = dB.Products.Where(n => n.Prod_ID == id).SingleOrDefault();

                Cart.Add(new Items()
                {
                    Product = product,
                    Quantity = 1
                });

                Session["cart"] = Cart;
            }

            else
            {
                List<Items> Cart = (List<Items>)Session["cart"];

                //Avoid Duplicate
                List<Items> Cart1 = Cart.Distinct().ToList();

                Product product = dB.Products.Where(n => n.Prod_ID == id).SingleOrDefault();

                foreach (var items in Cart1)
                {
                    if (items.Product.Prod_ID == id)
                    {
                        Cart1.Remove(items);
                        Cart1.Add(new Items()
                        {
                            Product = product,
                            Quantity = 1
                        });
                        break;
                    }
                    else
                    {
                        Cart1.Add(new Items()
                        {
                            Product = product,
                            Quantity = 1
                        });
                        break;
                    }
                }

                Session["cart"] = Cart1;
            }

            return RedirectToAction("Shop");
        }

        public ActionResult RemoveCart(int id)
        {
            List<Items> Cart = (List<Items>)Session["cart"];

            for (int i = 0; i < Cart.Count; i++)
            {
                if (Cart[i].Product.Prod_ID == id)
                {
                    Cart.Remove(Cart[i]);
                    break;
                }
            }
            if (Cart.Count == 0)
            {
                Session["cart"] = null;
            }
            else
            {
                Session["cart"] = Cart;
            }

            return RedirectToAction("Shop");
        }

        public ActionResult Checkout()
        {
            return View();
        }

        public ActionResult IncreaseQuantity(int id)
        {
            if (Session["cart"] != null)
            {
                List<Items> Cart = (List<Items>)Session["cart"];

                Product product = dB.Products.Where(n => n.Prod_ID == id).SingleOrDefault();

                foreach (var items in Cart)
                {
                    if (items.Product.Prod_ID == id)
                    {
                        int prevQty = items.Quantity;
                        Cart.Remove(items);
                        Cart.Add(new Items()
                        {
                            Product = product,
                            Quantity = prevQty + 1
                        });
                        break;
                    }
                }

                Session["cart"] = Cart;
            }
            return RedirectToAction("Checkout");
        }

        public ActionResult DecreaseQuantity(int id)
        {
            if (Session["cart"] != null)
            {
                List<Items> Cart = (List<Items>)Session["cart"];

                Product product = dB.Products.Where(n => n.Prod_ID == id).SingleOrDefault();

                foreach (var items in Cart)
                {
                    if (items.Product.Prod_ID == id)
                    {
                        int prevQty = items.Quantity;
                        Cart.Remove(items);
                        Cart.Add(new Items()
                        {
                            Product = product,
                            Quantity = prevQty - 1
                        });
                        break;
                    }
                }

                Session["cart"] = Cart;
            }
            return RedirectToAction("Checkout");
        }

        public ActionResult CheckoutDetails()
        {
            return View();
        }

        public ActionResult ShippingDetails(Shipping_Details shipping , string submit)
        {

            dB.Shipping_Details.Add(shipping);
            dB.SaveChanges();

            Session["cart"] = null;

            ViewBag.Message = "The Payment Process is Completed Check Your Profile Page";

            return RedirectToAction("Home");
        }

        public ActionResult Account()
        {
            return View();
        }

        public ActionResult Orders()
        {
            return View();
        }

        public ActionResult Refund(int id)
        {
            Shipping_Details order = dB.Shipping_Details.Where(n => n.Shipping_ID == id).SingleOrDefault();

            return View(order);
        }

        [HttpPost]
        public ActionResult Refund(Refund_Details refund , string submit)
        {
            dB.Refund_Details.Add(refund);
            dB.SaveChanges();

            ViewBag.Message = "The Refund Process is Completed";

            return RedirectToAction("Home");
        }

        public ActionResult UpdateInformation()
        {
            int id = int.Parse(Session["userid"].ToString());
            User user = dB.Users.Where(n => n.User_ID == id).FirstOrDefault();

            return View(user);
        }

        [HttpPost]
        public ActionResult UpdateInformation(User user)
        {
            User user1 = dB.Users.Where(n => n.User_ID == user.User_ID).SingleOrDefault();

            user1.First_Name = user.First_Name;
            user1.Last_Name = user.Last_Name;
            user1.User_Password = user.User_Password;
            user1.User_Email= user.User_Email;
            user1.User_Role = user.User_Role;
            user1.User_Phone = user.User_Phone;

            dB.SaveChanges();

            return RedirectToAction("Account");
        }

        public ActionResult UpdatePassword()
        {
            int id = int.Parse(Session["userid"].ToString());
            User user = dB.Users.Where(n => n.User_ID == id).FirstOrDefault();

            return View(user);
        }

        [HttpPost]
        public ActionResult UpdatePassword(User user)
        {
            User user1 = dB.Users.Where(n => n.User_ID == user.User_ID).SingleOrDefault();

            user1.First_Name = user.First_Name;
            user1.Last_Name = user.Last_Name;
            user1.User_Password = user.User_Password;
            user1.User_Email = user.User_Email;
            user1.User_Role = user.User_Role;
            user1.User_Phone = user.User_Phone;

            dB.SaveChanges();

            return RedirectToAction("Account");
        }
    }
}