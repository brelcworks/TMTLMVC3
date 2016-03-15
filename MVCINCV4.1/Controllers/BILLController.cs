using MVCINCV4._1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCINCV4._1.Controllers
{
    public class BILLController : Controller
    {
        DBCTX dc = new DBCTX();
        [Authorize]
        public ActionResult Index()
        {
            return View(dc.BILL1.ToList());
        }
        [Authorize]
        public JsonResult List_BILL()
        {
            var dbResult = dc.BILL1.ToList();
            return Json(dbResult, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }
        [Authorize]
        public JsonResult List_BILL_ITM(string bno)
        {
            var dbResult = dc.BILL.Where(a=> a.BILL_NO.Equals(bno)).ToList();
            return Json(dbResult, JsonRequestBehavior.AllowGet);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Create(SALE e)
        {
            using (dc)
            {
                dc.BILL1.Add(e.BILL1);
                dc.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
