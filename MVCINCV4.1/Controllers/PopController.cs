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

        [Authorize]
        public ActionResult Add_Pmr(int id = 0)
        {
            return View(new PMRMODEL {PMRs =new PMR(), MAINPOP =dc.MAINPOPU.Find (id)});
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Add_Pmr(PMRMODEL e)
        {
            using (dc)
            {
                dc.PMR.Add(e.PMRs);
                dc.SaveChanges();
                dc.Entry(e.MAINPOP).State = EntityState.Modified;
                dc.SaveChanges();
            }
            return RedirectToAction("List");
        }

        public JsonResult gdata2(string aData)
        {
            List<MAINPOPU> STLIST = new List<MAINPOPU>();
            using (DBCTX FC = new DBCTX())
            {
                STLIST = FC.MAINPOPU.Where(A => A.ENS.Equals(aData)).ToList();
            }
            return new JsonResult { Data = STLIST, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [Authorize]
        public ActionResult Details(int id = 0)
        {
            return View(dc.MAINPOPU.Find(id));
        }

        [Authorize]
        public ActionResult View_PMR(string id)
        {
            List<PMR> STLIST = new List<PMR>();
            STLIST = dc.PMR.Where(A => A.ENGINE_No.Equals(id)).ToList();
            return View(STLIST);
        }
    }
}
