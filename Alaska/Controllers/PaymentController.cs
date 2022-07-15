﻿using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Alaska.Models;

namespace Alaska.Controllers
{
    public class PaymentController : Controller
    {
        public ActionResult PaymentWithPapal()
        {
            APIContext aPIContext = PaypalConfiguration.GetAPIContext();

            try
            {
                string PayerId = Request.Params["PayerID"];
                if (string.IsNullOrEmpty(PayerId) && PayerId != null)
                {
                    string baseURi = Request.Url.Scheme + "://" + Request.Url.Authority +
                                     "PaymentWithPapal/PaymentWithPapal?";

                    var Guid = Convert.ToString((new Random()).Next(100000000));
                    var createPayment = this.CreatePayment(aPIContext, baseURi + "guid=" + Guid);

                    var links = createPayment.links.GetEnumerator();
                    string paypalRedirectURL = null;

                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;

                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            paypalRedirectURL = lnk.href;
                        }

                    }
                }
                else
                {
                    var guid = Request.Params["guid"];
                    var executedPaymnt = ExecutePayment(aPIContext, PayerId, Session[guid] as string);


                    if (executedPaymnt.ToString().ToLower() != "approved")
                    {
                        return View("FailureView");
                    }

                }
            }
            catch (Exception)
            {
                return View("FailureView");
                //throw;
            }
            return View("SuccessView");
        }

        private object ExecutePayment(APIContext aPIContext, string payerId, string PaymentId)
        {
            var paymentExecution = new PaymentExecution() { payer_id = payerId };
            this.payment = new Payment() { id = PaymentId };
            return this.payment.Execute(aPIContext, paymentExecution);
        }

        private PayPal.Api.Payment payment;

        private Payment CreatePayment(object apicontext, string redirectURl)
        {
            var ItemLIst = new ItemList() { items = new List<Item>() };

            if (Session["cart"] != "")
            {
                List<Items> cart = (List<Items>)(Session["cart"]);
                foreach (var item in cart)
                {
                    ItemLIst.items.Add(new Item()
                    {
                        name = item.Product.Prod_Name,
                        currency = "USD",
                        price = item.Product.Price.ToString(),
                        quantity = item.Product.Quantity.ToString(),
                        sku = "sku"
                    });


                }


                var payer = new Payer() { payment_method = "paypal" };

                var redirUrl = new RedirectUrls()
                {
                    cancel_url = redirectURl + "&Cancel=true",
                    return_url = redirectURl
                };

                var details = new Details()
                {
                    tax = "1",
                    shipping = "1",
                    subtotal = "1"
                };

                var amount = new Amount()
                {
                    currency = "USD",

                    total = Session["SesTotal"].ToString(),
                    details = details
                };

                var transactionList = new List<Transaction>();
                transactionList.Add(new Transaction()
                {
                    description = "Transaction Description",
                    invoice_number = "#100000",
                    amount = amount,
                    item_list = ItemLIst

                });

                this.payment = new Payment()
                {
                    intent = "sale",
                    payer = payer,
                    transactions = transactionList,
                    redirect_urls = redirUrl
                };
            }
             
            return this.payment.Create((APIContext)apicontext);
        }
    }
}