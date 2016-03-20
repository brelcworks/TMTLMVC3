using MVCINCV4._1.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        public JsonResult List_BILL(string bno)
        {
            var dbResult = dc.BILL1.Where(a => a.BNO.Equals(bno)).ToList();
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

        public ActionResult Save()
        {
            return View();
        }  

        [HttpPost]
        public ActionResult Save(BILL bill)
        {
            string message = "";
            if (ModelState.IsValid)
            {
                try
                {
                    using (DBCTX aid = new DBCTX())
                    {
                        aid.BILL.Add(bill);
                        message = "Successfully Saved!";
                    }
                }
                catch (Exception ex) { message = "Error! Please try again."; }
            }
            else
            {
                message = "Please provide required fields value.";
            }
            if (Request.IsAjaxRequest())
            {
                return new JsonResult { Data = message, JsonRequestBehavior =JsonRequestBehavior.AllowGet};
            }
            else
            {
                ViewBag.Message = message;
                return View(bill);
            }
        }  
    }
}
