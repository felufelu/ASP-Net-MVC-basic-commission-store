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
    public class SellersController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            var sellers = from u in db.Users
                          where u.Roles.Any(r => r.RoleId == "2")
                          select u;
            return View(sellers.ToList());
        }

    }
}
