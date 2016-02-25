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
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
using ClosedXML.Excel;

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

        
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public JsonResult stype(string term)
        {
            List<string> itms;
            itms = dc.PMR.Where(x => x.STYPE.StartsWith(term))
                .Select(y => y.STYPE).ToList();
            return Json(itms, JsonRequestBehavior.AllowGet);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public JsonResult tech(string term)
        {
            List<string> itms;
            itms = dc.PMR.Where(x => x.Technician.StartsWith(term))
                .Select(y => y.Technician).ToList();
            return Json(itms, JsonRequestBehavior.AllowGet);
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
            return PartialView("View_PMR", STLIST);
        }

        public JsonResult lreco(string aData)
        {
            var lstrec = (from c in dc.PMR.Where(A => A.ENGINE_No.Equals(aData))
                          orderby c.RECID1 descending
                          select c).First();
            return Json(lstrec, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExportData()
        {
            GridView gv = new GridView();
            gv.DataSource = dc.MAINPOPU.ToList();
            gv.DataBind();
            var wb = new XLWorkbook();
            var ws = wb.Worksheets .Add ("Mainpop");
            ws.Cell(1, 1).Value = "SL NO";
            ws.Cell(1, 2).Value = "CUSTOMER";
            ws.Cell(1, 3).Value = "SITE ID";
            ws.Cell(1, 4).Value = "SITE NAME";
            ws.Cell(1, 5).Value = "ENGINE SR. NO";
            ws.Cell(1, 6).Value = "RATING";
            ws.Cell(1, 7).Value = "PHASE";
            ws.Cell(1, 8).Value = "DG SET NO";
            ws.Cell(1, 9).Value = "DT. OF COMMISSIONING";
            ws.Cell(1, 10).Value = "CONTACT PERSON";
            ws.Cell(1, 11).Value = "PH NO";
            ws.Cell(1, 12).Value = "ALT. MAKE";
            ws.Cell(1, 13).Value = "ALT SR. NO";
            ws.Cell(1, 14).Value = "BATTERY SR. NO";
            ws.Cell(1, 15).Value = "AMC STATUS";
            ws.Cell(1, 16).Value = "WARRANTY STATUS";
            ws.Cell(1, 17).Value = "PRESENT HMR";
            ws.Cell(1, 18).Value = "PRESENT HMR ON DATE";
            ws.Cell(1, 19).Value = "LAST SERVICE HMR";
            ws.Cell(1, 20).Value = "LAST SERVICE HMR ON DATE";
            ws.Cell(1, 21).Value = "NEXT SERVICE DATE";
            ws.Cell(1, 22).Value = "OEA";
            ws.Cell(1, 23).Value = "SITE STATUS";
            for (int I = 0; I < gv.Rows.Count; I++)
            {
                ws.Cell(I + 2, 1).Value = I.ToString() + 1;
                ws.Cell(I + 2, 2).Value = gv.Rows[I].Cells[3].Text;
                ws.Cell(I + 2, 3).Value = gv.Rows[I].Cells[2].Text;
                ws.Cell(I + 2, 4).Value = gv.Rows[I].Cells[4].Text;
                ws.Cell(I + 2, 5).Value = gv.Rows[I].Cells[5].Text;
                ws.Cell(I + 2, 6).Value = gv.Rows[I].Cells[7].Text;
                ws.Cell(I + 2, 7).Value = gv.Rows[I].Cells[8].Text;
                ws.Cell(I + 2, 8).Value = gv.Rows[I].Cells[23].Text;
                ws.Cell(I + 2, 9).Value = gv.Rows[I].Cells[14].Text;
                ws.Cell(I + 2, 10).Value = gv.Rows[I].Cells[11].Text;
                ws.Cell(I + 2, 11).Value = gv.Rows[I].Cells[12].Text;
                ws.Cell(I + 2, 12).Value = gv.Rows[I].Cells[27].Text;
                ws.Cell(I + 2, 13).Value = gv.Rows[I].Cells[6].Text;
                ws.Cell(I + 2, 14).Value = gv.Rows[I].Cells[10].Text;
                ws.Cell(I + 2, 15).Value = gv.Rows[I].Cells[16].Text;
                ws.Cell(I + 2, 16).Value = gv.Rows[I].Cells[28].Text;
                ws.Cell(I + 2, 17).Value = gv.Rows[I].Cells[17].Text;
                ws.Cell(I + 2, 18).Value = gv.Rows[I].Cells[18].Text;
                ws.Cell(I + 2, 19).Value = gv.Rows[I].Cells[19].Text;
                ws.Cell(I + 2, 20).Value = gv.Rows[I].Cells[20].Text;
                ws.Cell(I + 2, 21).Value = gv.Rows[I].Cells[21].Text;
                ws.Cell(I + 2, 22).Value = gv.Rows[I].Cells[29].Text;
                ws.Cell(I + 2, 23).Value = gv.Rows[I].Cells[43].Text;
            }

            ws.Range("i2", "i" + ws.RangeUsed().RowCount()).Style.NumberFormat.SetFormat("dd-MMM-yyyy hh:mm:ss");
            ws.Range("r2", "r" + ws.RangeUsed().RowCount()).Style.NumberFormat.SetFormat("dd-MMM-yyyy hh:mm:ss");
            ws.Range("t2", "u" + ws.RangeUsed().RowCount()).Style.NumberFormat.SetFormat("dd-MMM-yyyy hh:mm:ss");
            ws.Range("e2", "e" + ws.RangeUsed().RowCount()).Style.NumberFormat.SetFormat("#");
            ws.Columns().AdjustToContents();
            ws.SheetView.FreezeRows(1);
            ClosedXML.Excel.IXLRange range= ws.RangeUsed();
            string  RCNT = "w" + range.RowCount();
            ws.Range("a1", RCNT).Style.Border.TopBorder = XLBorderStyleValues.Thin;
            ws.Range("a1", RCNT).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            ws.Range("a1", RCNT).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            ws.Range("a1", RCNT).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            ws.Range("a1", "w1").Style.Fill.BackgroundColor = XLColor.Turquoise;
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=POP.xlsx");
            using (MemoryStream MyMemoryStream = new MemoryStream())
            {
                wb.SaveAs(MyMemoryStream);
                MyMemoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
            return RedirectToAction("List");
        }

        [Authorize]
        public ActionResult List_PMR()
        {
            return View(dc.PMR.ToList());
        }

        public JsonResult filrec(DateTime sdt, DateTime edt)
        {
            List<PMR> STLIST = new List<PMR>();
            using (DBCTX FC = new DBCTX())
            {
                STLIST = FC.PMR.Where(A => A.CDATI >= sdt && A.CDATI <= edt).ToList();
            }
            return new JsonResult { Data = STLIST, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult allrec()
        {
            List<PMR> STLIST = new List<PMR>();
            using (DBCTX FC = new DBCTX())
            {
                STLIST = FC.PMR.ToList();
            }
            return new JsonResult { Data = STLIST, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [Authorize]
        public ActionResult PMR_Dtls(int ens)
        {
            return PartialView("PMR_Dtls", dc.PMR.Find(ens));
        }

        public ActionResult fil_pmr(DateTime sdt, DateTime edt)
        {
            List<PMR> STLIST = new List<PMR>();
            using (DBCTX FC = new DBCTX())
            {
                STLIST = FC.PMR.Where(A => A.CDATI >= sdt && A.CDATI <= edt).ToList();
            }
            return View(STLIST);
        }
    }
}
