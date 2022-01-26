using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace WebApplication1.Controllers
{
    public class OrdersController : Controller
    {
        protected string CurrentUserID
        {
            get
            {
                return System.Web.HttpContext.Current.GetOwinContext()
                      .GetUserManager<ApplicationUserManager>()
                      .FindById(System.Web.HttpContext.Current.User.Identity.GetUserId()).Id;
            }
        }
        private ApplicationDbContext db = new ApplicationDbContext();


        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View(db.Orders.ToList());
        }

        [Authorize(Roles = "Admin,Seller,Buyer")]
        public ActionResult MyOrders()
        {
            return View(db.Orders.Where(o => o.Buyer.Id == CurrentUserID).ToList());
        }

        [Authorize(Roles = "Admin,Seller")]
        public ActionResult MyCommissions()
        {
            return View(db.Orders.Where(o => o.Gig.Seller.Id == CurrentUserID).ToList());
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order.Buyer.Id != CurrentUserID)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        [Authorize(Roles = "Admin,Seller,Buyer")]
        public ActionResult Create()
        {
            return View();
        }

    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Amount")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                order.Buyer = db.Users.SingleOrDefault(e => e.Id == CurrentUserID);
                order.OrderDate = DateTime.Now;
                order.Status = 1;
                db.SaveChanges();
                return RedirectToAction("MyOrders");
            }

            return View(order);
        }

        [Authorize(Roles = "Admin,Seller")]
        // GET: Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
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
