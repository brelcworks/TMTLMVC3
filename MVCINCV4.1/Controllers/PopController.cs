using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Services;
using System.Web.Security;
using System.Web.Services;
using System.Data.Entity;

namespace MVCINCV4._1.Controllers
{
    public class PopController : Controller
    {
        DB1Entities dc = new DB1Entities();
        [Authorize]
        public ActionResult List()
        {
            return View(dc.MAINPOPU.ToList());
        }

        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Create(MAINPOPU e)
        {
            using (dc)
            {
                dc.MAINPOPU.Add(e);
                dc.SaveChanges();
            }
            return RedirectToAction("List");
        }
    }
}
