using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace BulletinBoard.Controllers
{
    public class HomeController : Controller
    {
        Models.MessagesEntities db = new Models.MessagesEntities();
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection collection)
        {
            //find the user record
            string username = collection["username"];
            Models.User theUser = db.Users.SingleOrDefault(u => u.username.Equals(username));
            if(theUser != null &&
                Crypto.VerifyHashedPassword(theUser.password_hash, collection["password_hash"]))
            {
                Session["user_id"] = theUser.user_id;
                return RedirectToAction("Index", "Post");
            }
            else
            {
                ViewBag.error = "Wrong Username/Password combination!";
                return View();
            } 
                
        }


        // GET: Home/Details/5
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index");
        }

        // GET: Home/Create
        public ActionResult Register()
        {
            return View();
        }

        // POST: Home/Create
        [HttpPost]
        public ActionResult Register(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here
                string username = collection["username"];
                Models.User theUser = db.Users.SingleOrDefault(u => u.username.Equals(username));
                if (theUser != null)
                    return RedirectToAction("Register");//todo:provide feedback

                Models.User newUser = new Models.User()
                {
                    username = collection["username"],
                    password_hash = Crypto.HashPassword(collection["password_hash"])
                };
                db.Users.Add(newUser);
                db.SaveChanges();
                
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

      
    }
}
