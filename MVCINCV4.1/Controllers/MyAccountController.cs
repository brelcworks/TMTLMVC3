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
        public ActionResult login(user1 l, string ReturnUrl = "")
        {
            using (DB1Entities dc = new DB1Entities())
            {
                var user = dc.user1.Where(a => a.uid.Equals(l.uid) && a.pass.Equals(l.pass)).FirstOrDefault();
                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(user.uid, true);
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
    }
}
