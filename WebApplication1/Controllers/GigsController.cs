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
using System.Dynamic;

namespace WebApplication1.Controllers
{
    public class GigsController : Controller
    {
        public Gig cgig = new Gig();
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Gigs
        protected string CurrentUserID
        {
            get
            {
                return System.Web.HttpContext.Current.GetOwinContext()
                      .GetUserManager<ApplicationUserManager>()
                      .FindById(System.Web.HttpContext.Current.User.Identity.GetUserId()).Id;
            }
        }
        [Authorize(Roles = "Admin,Seller,Buyer")]
        public ActionResult Index(string id)
        {
            if (id != null)
            {
                return View(db.Gigs.Where(g => g.Seller.Id == id).ToList());
            }
            return View(db.Gigs.ToList());
        }

        [Authorize(Roles = "Admin,Seller")]
        public ActionResult MyGigs()
        {
            return View(db.Gigs.Where(g => g.Seller.Id == CurrentUserID).ToList());
        }

        //[Authorize(Roles = "Admin,Seller,Buyer")]
        public ActionResult Details(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gig gig = db.Gigs.Find(id);
            cgig = gig;
            ViewBag.GigID = id;
            if (gig == null)
            {
                return HttpNotFound();
            }

            return View(gig);
        }


        [Authorize(Roles = "Admin,Seller,Buyer")]
        public ActionResult Create()
        {
            return View();
        }

        //[Authorize(Roles = "Admin,Seller,Buyer")]
        public ActionResult PlaceAnOrder(int? id)
        {
            Order neworder = new Order();
            Gig cgig = db.Gigs.SingleOrDefault(g => g.GigID == 4);
            neworder.Buyer = db.Users.SingleOrDefault(e => e.Id == CurrentUserID);
            neworder.Gig = cgig;
            neworder.Amount = 1;
            neworder.OrderDate = DateTime.Now;
            neworder.Status = 1;
            db.Orders.Add(neworder);
            db.SaveChanges();
            return RedirectToAction("MyOrders", "Orders");
        }
        /*
        //[Authorize(Roles = "Admin,Seller,Buyer")]
        [HttpPost]
        public ActionResult PlaceAnOrder([Bind(Include = "OrderID,Amount,OrderDate,Status,Buyer,Gig")] Order order)
        {
            ViewBag.GigID = cgig.GigID;
            if (ModelState.IsValid)
            {
                order.Buyer = db.Users.SingleOrDefault(e => e.Id == CurrentUserID);
                order.Gig = cgig;
                order.OrderDate = DateTime.Now;
                order.Status = 1;
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("MyOrders","Orders");
            }

            return View(order);
        }
        */
        [Authorize(Roles = "Admin,Seller,Buyer")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "GigID,Name,Description,Price,Deliverytime,Seller")] Gig gig)
        {
            if (ModelState.IsValid)
            {
                gig.Seller = db.Users.SingleOrDefault(e => e.Id == CurrentUserID);
                System.Web.HttpContext.Current.GetOwinContext()
                      .GetUserManager<ApplicationUserManager>().AddToRole(CurrentUserID,"Seller");
                db.Gigs.Add(gig);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(gig);
        }
        // GET: Gigs/Edit/5
        [Authorize(Roles = "Admin,Seller")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gig gig = db.Gigs.Find(id);
            if (gig == null)
            {
                return HttpNotFound();
            }
            return View(gig);
        }

        [Authorize(Roles = "Admin,Seller")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GigID,Name,Description,Price,Deliverytime")] Gig gig)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gig).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(gig);
        }

        [Authorize(Roles = "Admin,Seller")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gig gig = db.Gigs.Find(id);
            if (gig == null)
            {
                return HttpNotFound();
            }
            return View(gig);
        }

        [Authorize(Roles = "Admin,Seller")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Gig gig = db.Gigs.Find(id);
            db.Gigs.Remove(gig);
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
