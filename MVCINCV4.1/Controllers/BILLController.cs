﻿using MVCINCV4._1.Models;
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
            return PartialView(dc.BILL1.ToList());
        }
        public JsonResult List_BILL()
        {
            var dbResult = dc.BILL1.ToList();
            return Json(dbResult, JsonRequestBehavior.AllowGet);
        }
        public JsonResult List_BILL_ITM(string bno)
        {
            var dbResult = dc.BILL.Where(a => a.BILL_NO.Equals(bno)).ToList();
            return Json(dbResult, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Create(BILL1 e)
        {
            using (dc)
            {
                dc.BILL1.Add(e);
                dc.SaveChanges();
            }
            return RedirectToAction("Create");
        }
        public JsonResult dtls(string id)
        {
            List<BILL> STLIST = new List<BILL>();
            using (DBCTX FC = new DBCTX())
            {
                STLIST = FC.BILL.Where(A => A.BID.Equals(id)).ToList();
            }
            return new JsonResult { Data = STLIST, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult dtls1(string id)
        {
            List<BILL1> STLIST = new List<BILL1>();
            using (DBCTX FC = new DBCTX())
            {
                STLIST = FC.BILL1.Where(A => A.BNO.Equals(id)).ToList();
            }
            return new JsonResult { Data = STLIST, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public ActionResult Save()
        {
            return View();
        }
        public JsonResult GetPtno(string term)
        {
            List<string> itms;
            itms = dc.STOCK.Where(x => x.PART_NO.StartsWith(term))
                .Select(y => y.PART_NO).ToList();
            return Json(itms, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetParti(string term)
        {
            List<string> itms;
            itms = dc.STOCK.Where(x => x.PARTI.StartsWith(term))
                .Select(y => y.PARTI).ToList();
            return Json(itms, JsonRequestBehavior.AllowGet);
        }
        public JsonResult cstat(string term)
        {
            List<string> itms;
            itms = dc.BILL1.Where(x => x.CUST.StartsWith(term))
                .Select(y => y.CUST).ToList();
            return Json(itms, JsonRequestBehavior.AllowGet);
        }
        public JsonResult snmat(string term)
        {
            List<string> itms;
            itms = dc.BILL1.Where(x => x.SNAME.StartsWith(term))
                .Select(y => y.SNAME).ToList();
            return Json(itms, JsonRequestBehavior.AllowGet);
        }
        public JsonResult gdata2(string aData)
        {
            List<TABLE2> STLIST = new List<TABLE2>();
            using (DBCTX FC = new DBCTX())
            {
                STLIST = FC.STOCK.Where(A => A.PART_NO.Equals(aData)).ToList();
            }
            return new JsonResult { Data = STLIST, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult gdata1(string aData)
        {
            List<TABLE2> STLIST = new List<TABLE2>();
            using (DBCTX FC = new DBCTX())
            {
                STLIST = FC.STOCK.Where(A => A.PARTI.Equals(aData)).ToList();
            }
            return new JsonResult { Data = STLIST, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult getct(string aData)
        {
            List<BILL1> STLIST = new List<BILL1>();
            using (DBCTX FC = new DBCTX())
            {
                STLIST = FC.BILL1.Where(A => A.CUST.Equals(aData)).ToList();
            }
            return new JsonResult { Data = STLIST, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult getlrec()
        {
            var max = dc.BILL1.OrderByDescending(p => p.BID).FirstOrDefault().BID;
            return new JsonResult { Data = max, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
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
                        aid.SaveChanges();
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
                return new JsonResult { Data = message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else
            {
                ViewBag.Message = message;
                return View(bill);
            }
        }

        [HttpPost]
        public ActionResult STUPD(TABLE2 TBL)
        {
            string message = "";
            if (ModelState.IsValid)
            {
                try
                {
                    dc.Entry(TBL).State = EntityState.Modified;
                    dc.SaveChanges();
                    message = "Successfully Saved!";
                }
                catch (Exception ex) { message = "Error! Please try again."; }
            }
            else
            {
                message = "Please provide required fields value.";
            }
            if (Request.IsAjaxRequest())
            {
                return new JsonResult { Data = message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else
            {
                ViewBag.Message = message;
                return View(TBL);
            }
        }

        [HttpPost]
        public ActionResult BILLUPD(BILL TBL)
        {
            string message = "";
            if (ModelState.IsValid)
            {
                try
                {
                    dc.Entry(TBL).State = EntityState.Modified;
                    dc.SaveChanges();
                    message = "Successfully Saved!";
                }
                catch (Exception ex) { message = "Error! Please try again."; }
            }
            else
            {
                message = "Please provide required fields value.";
            }
            if (Request.IsAjaxRequest())
            {
                return new JsonResult { Data = message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else
            {
                ViewBag.Message = message;
                return View(TBL);
            }
        }

        [HttpPost]
        public ActionResult INVUPD(BILL1 TBL)
        {
            string message = "";
            if (ModelState.IsValid)
            {
                try
                {
                    dc.Entry(TBL).State = EntityState.Modified;
                    dc.SaveChanges();
                    message = "Successfully Saved!";
                }
                catch (Exception ex) { message = "Error! Please try again."; }
            }
            else
            {
                message = "Please provide required fields value.";
            }
            if (Request.IsAjaxRequest())
            {
                return new JsonResult { Data = message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else
            {
                ViewBag.Message = message;
                return View(TBL);
            }
        }
    }
}
