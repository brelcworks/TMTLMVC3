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
            return PartialView(dc.BILL1.ToList());
        }
        [Authorize]
        public JsonResult List_BILL()
        {
            var dbResult = dc.BILL1.ToList();
            return Json(dbResult, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
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
        [Authorize]
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
        [Authorize]
        public JsonResult dtls(string id)
        {
            List<BILL> STLIST = new List<BILL>();
            using (DBCTX FC = new DBCTX())
            {
                STLIST = FC.BILL.Where(A => A.BID.Equals(id)).ToList();
            }
            return new JsonResult { Data = STLIST, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [Authorize]
        public JsonResult dtls1(string id)
        {
            List<BILL1> STLIST = new List<BILL1>();
            using (DBCTX FC = new DBCTX())
            {
                STLIST = FC.BILL1.Where(A => A.BNO.Equals(id)).ToList();
            }
            return new JsonResult { Data = STLIST, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [Authorize]
        public ActionResult Save()
        {
            return View();
        }
        [Authorize]
        public JsonResult GetPtno(string term)
        {
            List<string> itms;
            itms = dc.STOCK.Where(x => x.PART_NO.StartsWith(term))
                .Select(y => y.PART_NO).ToList();
            return Json(itms, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult GetParti(string term)
        {
            List<string> itms;
            itms = dc.STOCK.Where(x => x.PARTI.StartsWith(term))
                .Select(y => y.PARTI).ToList();
            return Json(itms, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult cstat(string term)
        {
            List<string> itms;
            itms = dc.BILL1.Where(x => x.CUST.StartsWith(term))
                .Select(y => y.CUST).ToList();
            return Json(itms, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult snmat(string term)
        {
            List<string> itms;
            itms = dc.BILL1.Where(x => x.SNAME.StartsWith(term))
                .Select(y => y.SNAME).ToList();
            return Json(itms, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult gdata2(string aData)
        {
            List<TABLE2> STLIST = new List<TABLE2>();
            using (DBCTX FC = new DBCTX())
            {
                STLIST = FC.STOCK.Where(A => A.PART_NO.Equals(aData)).ToList();
            }
            return new JsonResult { Data = STLIST, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [Authorize]
        public JsonResult gdata1(string aData)
        {
            List<TABLE2> STLIST = new List<TABLE2>();
            using (DBCTX FC = new DBCTX())
            {
                STLIST = FC.STOCK.Where(A => A.PARTI.Equals(aData)).ToList();
            }
            return new JsonResult { Data = STLIST, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [Authorize]
        public JsonResult getct(string aData)
        {
            List<BILL1> STLIST = new List<BILL1>();
            using (DBCTX FC = new DBCTX())
            {
                STLIST = FC.BILL1.Where(A => A.CUST.Equals(aData)).ToList();
            }
            return new JsonResult { Data = STLIST, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [Authorize]
        public JsonResult gtbtot(string aData)
        {
            int tot = 0;
            int sum = (from d in dc.BILL1
                       where d.CUST == aData
                       select (int?)d.NTOT.Value).Sum() ?? 0;
            int sum1 = (from d in dc.BILL1
                       where d.CUST == aData
                       select (int?)d.PAYMENT.Value).Sum() ?? 0;
            tot = sum - sum1;
            return new JsonResult { Data = tot, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [Authorize]
        public JsonResult getlrec()
        {
            try
            {
                var max = dc.BILL1.OrderByDescending(p => p.BID).FirstOrDefault().BID;
                return new JsonResult { Data = max, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ex.ToString(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }
        [Authorize]
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
        [Authorize]
        [HttpPost]
        public ActionResult Save_ITM(TABLE2 bill)
        {
            string message = "";
            if (ModelState.IsValid)
            {
                try
                {
                    using (DBCTX aid = new DBCTX())
                    {
                        aid.STOCK.Add(bill);
                        aid.SaveChanges();
                        message = "Successfully Saved!";
                    }
                }
                catch (Exception ex) { ex.ToString(); }
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
        [Authorize]
        [HttpPost]
        public ActionResult Save_ITM1(SHEET1 bill)
        {
            string message = "";
            if (ModelState.IsValid)
            {
                try
                {
                    using (DBCTX aid = new DBCTX())
                    {
                        aid.SHEET1.Add(bill);
                        aid.SaveChanges();
                        message = "Successfully Saved!";
                    }
                }
                catch (Exception ex) { ex.ToString(); }
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
        [Authorize]
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
        [Authorize]
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
        [Authorize]
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
                catch (Exception ex) { message = ex.ToString(); }
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
        [Authorize]
        [HttpPost]
        public JsonResult DELITEM_BILL(int id)
        {
            string message = "";
                try
                {
                    BILL tc = dc.BILL.Find(id);
                    dc.BILL.Remove(tc);
                    dc.SaveChanges();
                    message = "Successfully Saved!";
                }
                catch (Exception ex)
                {
                    message = ex.ToString();
                }
                return new JsonResult { Data = message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };  
        }
    }
}
