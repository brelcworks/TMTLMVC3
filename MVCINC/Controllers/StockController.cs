using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Web.Services;
using System.Web.Script.Services;

namespace MVCINC.Controllers
{
    public class StockController : Controller
    {
        DB1Entities dc = new DB1Entities();
        [Authorize]
        public ActionResult List()
        {
            return View(dc.TABLE1 .ToList ());
        }
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost ,ValidateAntiForgeryToken ]
        public ActionResult Create(TABLE1 e)
        {
            using (dc)
            {
                dc.TABLE1.Add(e);
                dc.SaveChanges();
            }
            return RedirectToAction("List");
        }

        public JsonResult GetParti(string term)
        {
            List<string> itms;
            itms = dc.TABLE1.Where(x => x.PARTI.StartsWith(term))
                .Select(y => y.PARTI).ToList();
            return Json(itms, JsonRequestBehavior.AllowGet);
        }

        [WebMethod]
        [ScriptMethod (ResponseFormat= ResponseFormat.Json)]
        public JsonResult GetPtno(string term)
        {
            List<string> itms;
            itms = dc.TABLE1.Where(x => x.PART_NO.StartsWith(term))
                .Select(y => y.PART_NO).ToList();
            return Json(itms, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult Details(int id=0)
        {
            return View(dc.TABLE1.Find(id));
        }

        [Authorize]
        public ActionResult Edit(int id=0)
        {
            return View(dc.TABLE1.Find(id));
        }

        [HttpPost ,ValidateAntiForgeryToken ]
        public ActionResult Edit(TABLE1 e)
        {
            dc.Entry(e).State = EntityState.Modified;
            dc.SaveChanges();
            return RedirectToAction("List");
        }

        [Authorize]
        public ActionResult Delete(int id=0)
        {
            return View(dc.TABLE1.Find(id));
        }

        [HttpPost ,ActionName ("Delete")]
        public ActionResult delete_conf(int id)
        {
            TABLE1 tc = dc.TABLE1.Find(id);
            dc.TABLE1.Remove(tc);
            dc.SaveChanges();
            return RedirectToAction("List");
        }
        
        public JsonResult  gdata1(string aData)
        {
            List <SHEET1> STLIST= new List<SHEET1>();
            using (DB1Entities FC = new DB1Entities())
            {
                STLIST = FC.SHEET1.Where(A => A.PARTI.Equals(aData)).ToList();
            }
            return new JsonResult { Data = STLIST, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult gdata2(string aData)
        {
            List<SHEET1> STLIST = new List<SHEET1>();
            using (DB1Entities FC = new DB1Entities())
            {
                STLIST = FC.SHEET1.Where(A => A.PART_NO.Equals(aData)).ToList();
            }
            return new JsonResult { Data = STLIST, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}