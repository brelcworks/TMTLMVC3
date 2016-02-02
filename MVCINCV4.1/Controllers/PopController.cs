using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Services;
using System.Web.Security;
using System.Web.Services;
using System.Data.Entity;
using MVCINCV4._1.Models;

namespace MVCINCV4._1.Controllers
{
    public class PopController : Controller
    {
        DBCTX dc = new DBCTX();
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

        [Authorize]
        public ActionResult Edit(int id = 0)
        {
            return View(dc.MAINPOPU.Find(id));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(MAINPOPU e)
        {
            dc.Entry(e).State = EntityState.Modified;
            dc.SaveChanges();
            return RedirectToAction("List");
        }

        [Authorize]
        public ActionResult Delete(int id)
        {
            MAINPOPU frnds = new MAINPOPU();
            frnds = dc.MAINPOPU.Find(id);
            return PartialView("Delete", frnds);  
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult delete_conf(int id)
        {
            MAINPOPU tc = dc.MAINPOPU.Find(id);
            dc.MAINPOPU.Remove(tc);
            dc.SaveChanges();
            return RedirectToAction("List");
        }
    }
}
