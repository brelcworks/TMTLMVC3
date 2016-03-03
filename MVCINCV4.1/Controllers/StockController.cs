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
            itms = dc.SHEET1.Where(x => x.PARTI.StartsWith(term))
                .Select(y => y.PARTI).ToList();
            return Json(itms, JsonRequestBehavior.AllowGet);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public JsonResult GetPtno(string term)
        {
            List<string> itms;
            itms = dc.SHEET1.Where(x => x.PART_NO.StartsWith(term))
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
            List<SHEET1> STLIST = new List<SHEET1>();
            using (DBCTX FC = new DBCTX())
            {
                STLIST = FC.SHEET1.Where(A => A.PARTI.Equals(aData)).ToList();
            }
            return new JsonResult { Data = STLIST, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult gdata2(string aData)
        {
            List<SHEET1> STLIST = new List<SHEET1>();
            using (DBCTX FC = new DBCTX())
            {
                STLIST = FC.SHEET1.Where(A => A.PART_NO.Equals(aData)).ToList();
            }
            return new JsonResult { Data = STLIST, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [Authorize]
        public ActionResult Add_item()
        {
            return PartialView("Add_item");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Add_item(SHEET1 e)
        {
            using (dc)
            {
                dc.SHEET1.Add(e);
                dc.SaveChanges();
            }
            return RedirectToAction("Create");
        }

        public JsonResult List_Pr(int page, int rows, string sidx, string sord)
        {
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;

            var dbResult = dc.SHEET1.Select(
                a => new
                {
                    a.RID,
                    a.PART_NO,
                    a.PARTI,
                    a.MRP,
                    a.GROP,
                    a.CATE,
                    a.TRATE,
                    a.unit
                });
            int totalRecords = dbResult.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);
            if (sord.ToUpper() == "DESC")
            {
                dbResult = dbResult.OrderByDescending(s => s.PARTI);
                dbResult = dbResult.Skip(pageIndex * pageSize).Take(pageSize);
            }
            else
            {
                dbResult = dbResult.OrderBy(s => s.PARTI);
                dbResult = dbResult.Skip(pageIndex * pageSize).Take(pageSize);
            }
            var JsonData = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = (from a in dc.SHEET1.ToList()select new
                  {
                      id = a.RID,
                   cell = new string[]{ a.PART_NO,
                   a.PARTI,
                    a.MRP,
                    a.GROP,
                   a.CATE,
                    a.TRATE,
                    a.unit 
            }
                  }).ToArray()
            };  
            return Json(JsonData, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public ActionResult List_Pr1()
        {
            return View(dc.SHEET1.ToList());
        }
        [HttpPost]
        public string crt([Bind(Exclude = "RID")] SHEET1 objPMR)
        {
            string msg;
            try
            {
                if (ModelState.IsValid)
                {
                    dc.SHEET1.Add(objPMR);
                    dc.SaveChanges();
                    msg = "Saved Successfully";
                }
                else
                {
                    msg = "Validation Failed";
                }
            }
            catch(Exception ex)
            {
                msg = ex.Message;
            }
            return msg;
        }
    }
}
