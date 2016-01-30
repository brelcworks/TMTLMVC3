using MVCINCV4._1.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Services;
using System.Web.Security;
using System.Web.Services;

namespace MVCINCV4._1.Controllers
{
    public class StockController : Controller
    {
        DBCTX dc = new DBCTX();
        [Authorize]
        public ActionResult List()
        {
            return View(dc.STOCK.ToList());
        }
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Create(TABLE2 e)
        {
            using (dc)
            {
                dc.STOCK.Add(e);
                dc.SaveChanges();
            }
            return RedirectToAction("List");
        }

        public JsonResult GetParti(string term)
        {
            List<string> itms;
            itms = dc.STOCK.Where(x => x.PARTI.StartsWith(term))
                .Select(y => y.PARTI).ToList();
            return Json(itms, JsonRequestBehavior.AllowGet);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public JsonResult GetPtno(string term)
        {
            List<string> itms;
            itms = dc.STOCK.Where(x => x.PART_NO.StartsWith(term))
                .Select(y => y.PART_NO).ToList();
            return Json(itms, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult Details(int id = 0)
        {
            return View(dc.STOCK.Find(id));
        }

        [Authorize]
        public ActionResult Edit(int id = 0)
        {
            return View(dc.STOCK.Find(id));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(TABLE2 e)
        {
            dc.Entry(e).State = EntityState.Modified;
            dc.SaveChanges();
            return RedirectToAction("List");
        }

        [Authorize]
        public ActionResult Delete(int id = 0)
        {
            return View(dc.STOCK.Find(id));
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult delete_conf(int id)
        {
            TABLE2 tc = dc.STOCK.Find(id);
            dc.STOCK.Remove(tc);
            dc.SaveChanges();
            return RedirectToAction("List");
        }

        public JsonResult gdata1(string aData)
        {
            List<MVCINCV4._1.Models.SHEET1> STLIST = new List<MVCINCV4._1.Models.SHEET1>();
            using (DBCTX FC = new DBCTX())
            {
                STLIST = FC.SHEET1.Where(A => A.PARTI.Equals(aData)).ToList();
            }
            return new JsonResult { Data = STLIST, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult gdata2(string aData)
        {
            List<MVCINCV4._1.Models.SHEET1> STLIST = new List<MVCINCV4._1.Models.SHEET1>();
            using (DBCTX FC = new DBCTX())
            {
                STLIST = FC.SHEET1.Where(A => A.PART_NO.Equals(aData)).ToList();
            }
            return new JsonResult { Data = STLIST, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}
