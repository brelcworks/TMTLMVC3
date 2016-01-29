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
        public ActionResult login(USER L, string ReturnUrl = "")
        {
            using (DB1Entities dc = new DB1Entities())
            using (MVCINCDBENTITY DC1 = new MVCINCDBENTITY())

            {
                var user = DC1.USERs.Where(a => a.UNAME.Equals(L.UNAME) && a.PASS.Equals(L.PASS)).FirstOrDefault();
                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(user.FNAME, false);
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
