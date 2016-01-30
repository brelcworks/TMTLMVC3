using MVCINCV4._1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MVCINCV4._1.Controllers
{
    public class MyAccountController : Controller
    {
        public ActionResult login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult login(user2 L, string ReturnUrl = "")
        {
            using (DBCTX DC1 = new DBCTX())

            {
                var user = DC1.user.Where(a => a.uid.Equals(L.uid) && a.pass.Equals(L.pass )).FirstOrDefault();
                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(user.fname, false);
                    if (Url.IsLocalUrl(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("AfterLogin", "Home");
                    }
                }
            }
            return View();
        }

        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Register(user2 e)
        {
            DBCTX DC = new DBCTX();
                DC.user.Add(e);
                DC.SaveChanges();
            return RedirectToAction("login");
        }
    }
}
