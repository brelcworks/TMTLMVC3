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
            return PartialView("Details_P", dc.MAINPOPU.Find(id));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Details(MAINPOPU e)
        {
            dc.Entry(e).State = EntityState.Modified;
            dc.SaveChanges();
            return RedirectToAction("List");
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
                ws.Cell(I + 2, 1).Value = Convert.ToInt32(I.ToString()) + 1;
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
        public ActionResult PMR_Dtls(int ens, string ens1)
        {
            return PartialView("PMR_Dtls", new PMRMODEL { PMRs = dc.PMR.Find(ens), MAINPOP = dc.MAINPOPU.Where(a=> a.ENS.Contains(ens1)).FirstOrDefault() });
        }
        [HttpPost]
        public ActionResult fil_pmr(string sdt, string edt)
        {
            List<PMR> STLIST = new List<PMR>();
            DateTime sdt1 = Convert.ToDateTime(sdt);
            DateTime edt1 = Convert.ToDateTime(edt);
            TimeSpan ts = new TimeSpan(23, 59, 59);
            edt1 = edt1.Add(ts);
            using (DBCTX FC = new DBCTX())
            {
                STLIST = FC.PMR.Where(A => A.CDATI >= sdt1 && A.CDATI <= edt1).ToList();
            }
            return View(STLIST);
        }
        

        public ActionResult exp_pmr(string sdt, string edt)
        {
            var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("PMR");
            ws.Cell(1, 1).Value = "Customer";
            ws.Cell(1, 2).Value = "Issue Type";
            ws.Cell(1, 3).Value = "Issue Open Date";
            ws.Cell(1, 4).Value = "Complaint / Service No";
            ws.Cell(1, 5).Value = "Site Id";
            ws.Cell(1, 6).Value = "Site Name";
            ws.Cell(1, 7).Value = "District";
            ws.Cell(1, 8).Value = "State";
            ws.Cell(1, 9).Value = "Issue Category";
            ws.Cell(1, 10).Value = "Engine No";
            ws.Cell(1, 11).Value = "Model";
            ws.Cell(1, 12).Value = "KVA";
            ws.Cell(1, 13).Value = "DOI";
            ws.Cell(1, 14).Value = "DG SET NO";
            ws.Cell(1, 15).Value = "Alt. Make";
            ws.Cell(1, 16).Value = "Alt. Sr. No";
            ws.Cell(1, 17).Value = "Battery Sr. No";
            ws.Cell(1, 18).Value = "HMR";
            ws.Cell(1, 19).Value = "Issue Nature";
            ws.Cell(1, 20).Value = "Serverity";
            ws.Cell(1, 21).Value = "Failure Reason";
            ws.Cell(1, 22).Value = "Issue Status";
            ws.Cell(1, 23).Value = "Issue Closed Date";
            ws.Cell(1, 24).Value = "Warranty Status";
            ws.Cell(1, 25).Value = "Action Taken";
            ws.Cell(1, 26).Value = "Material Used";
            ws.Cell(1, 27).Value = "Dealer Code";
            ws.Cell(1, 28).Value = "Technician Visited";
            ws.Cell(1, 29).Value = "OEA";
            ws.Cell(1, 30).Value = "AMC Status";
            ws.Cell(1, 31).Value = "TTR";
            ws.Cell(1, 32).Value = "SLA Status";
            ws.Cell(1, 33).Value = "Time Slot";
            ws.Cell(1, 34).Value = "Reason Of SLA";
            ws.Cell(1, 35).Value = "Remarks";
            if (string.IsNullOrEmpty(edt))
            {
                GridView gv = new GridView();
                gv.DataSource = dc.PMR.ToList();
                gv.DataBind();
                for (int I = 0; I < gv.Rows.Count; I++)
                {
                    ws.Cell(I + 2, 1).Value = gv.Rows[I].Cells[10].Text;
                    ws.Cell(I + 2, 2).Value = gv.Rows[I].Cells[5].Text;
                    ws.Cell(I + 2, 3).Value = gv.Rows[I].Cells[4].Text;
                    ws.Cell(I + 2, 4).Value = gv.Rows[I].Cells[11].Text;
                    ws.Cell(I + 2, 5).Value = gv.Rows[I].Cells[1].Text;
                    ws.Cell(I + 2, 6).Value = gv.Rows[I].Cells[2].Text;
                    ws.Cell(I + 2, 7).Value = gv.Rows[I].Cells[13].Text;
                    ws.Cell(I + 2, 8).Value = gv.Rows[I].Cells[14].Text;
                    ws.Cell(I + 2, 9).Value = gv.Rows[I].Cells[15].Text;
                    ws.Cell(I + 2, 10).Value = gv.Rows[I].Cells[3].Text;
                    ws.Cell(I + 2, 11).Value = gv.Rows[I].Cells[16].Text;
                    ws.Cell(I + 2, 12).Value = gv.Rows[I].Cells[34].Text;
                    ws.Cell(I + 2, 13).Value = gv.Rows[I].Cells[17].Text;
                    ws.Cell(I + 2, 14).Value = gv.Rows[I].Cells[18].Text;
                    ws.Cell(I + 2, 15).Value = gv.Rows[I].Cells[19].Text;
                    ws.Cell(I + 2, 16).Value = gv.Rows[I].Cells[20].Text;
                    ws.Cell(I + 2, 17).Value = gv.Rows[I].Cells[21].Text;
                    ws.Cell(I + 2, 18).Value = gv.Rows[I].Cells[6].Text;
                    ws.Cell(I + 2, 19).Value = gv.Rows[I].Cells[22].Text;
                    ws.Cell(I + 2, 20).Value = gv.Rows[I].Cells[23].Text;
                    ws.Cell(I + 2, 21).Value = gv.Rows[I].Cells[24].Text;
                    ws.Cell(I + 2, 22).Value = gv.Rows[I].Cells[25].Text;
                    ws.Cell(I + 2, 23).Value = gv.Rows[I].Cells[12].Text;
                    ws.Cell(I + 2, 24).Value = gv.Rows[I].Cells[26].Text;
                    ws.Cell(I + 2, 25).Value = gv.Rows[I].Cells[27].Text;
                    ws.Cell(I + 2, 26).Value = gv.Rows[I].Cells[9].Text;
                    ws.Cell(I + 2, 27).Value = gv.Rows[I].Cells[36].Text;
                    ws.Cell(I + 2, 28).Value = gv.Rows[I].Cells[8].Text;
                    ws.Cell(I + 2, 29).Value = gv.Rows[I].Cells[28].Text;
                    ws.Cell(I + 2, 30).Value = gv.Rows[I].Cells[29].Text;
                    ws.Cell(I + 2, 31).Value = gv.Rows[I].Cells[30].Text;
                    ws.Cell(I + 2, 32).Value = gv.Rows[I].Cells[31].Text;
                    ws.Cell(I + 2, 33).Value = gv.Rows[I].Cells[32].Text;
                    ws.Cell(I + 2, 34).Value = gv.Rows[I].Cells[33].Text;
                    ws.Cell(I + 2, 35).Value = gv.Rows[I].Cells[39].Text;
                }
            }
            else
            {
                List<PMR> STLIST = new List<PMR>();
                DateTime sdt1 = Convert.ToDateTime(sdt);
                DateTime edt1 = Convert.ToDateTime(edt);
                TimeSpan ts = new TimeSpan(23, 59, 59);
                edt1 = edt1.Add(ts);
                using (DBCTX FC = new DBCTX())
                {
                    STLIST = FC.PMR.Where(A => A.CDATI >= sdt1 && A.CDATI <= edt1).ToList();
                }
                GridView gv = new GridView();
                gv.DataSource = STLIST;
                gv.DataBind();
                for (int I = 0; I < gv.Rows.Count; I++)
                {
                    ws.Cell(I + 2, 1).Value = gv.Rows[I].Cells[10].Text;
                    ws.Cell(I + 2, 2).Value = gv.Rows[I].Cells[5].Text;
                    ws.Cell(I + 2, 3).Value = gv.Rows[I].Cells[4].Text;
                    ws.Cell(I + 2, 4).Value = gv.Rows[I].Cells[11].Text;
                    ws.Cell(I + 2, 5).Value = gv.Rows[I].Cells[1].Text;
                    ws.Cell(I + 2, 6).Value = gv.Rows[I].Cells[2].Text;
                    ws.Cell(I + 2, 7).Value = gv.Rows[I].Cells[13].Text;
                    ws.Cell(I + 2, 8).Value = gv.Rows[I].Cells[14].Text;
                    ws.Cell(I + 2, 9).Value = gv.Rows[I].Cells[15].Text;
                    ws.Cell(I + 2, 10).Value = gv.Rows[I].Cells[3].Text;
                    ws.Cell(I + 2, 11).Value = gv.Rows[I].Cells[16].Text;
                    ws.Cell(I + 2, 12).Value = gv.Rows[I].Cells[34].Text;
                    ws.Cell(I + 2, 13).Value = gv.Rows[I].Cells[17].Text;
                    ws.Cell(I + 2, 14).Value = gv.Rows[I].Cells[18].Text;
                    ws.Cell(I + 2, 15).Value = gv.Rows[I].Cells[19].Text;
                    ws.Cell(I + 2, 16).Value = gv.Rows[I].Cells[20].Text;
                    ws.Cell(I + 2, 17).Value = gv.Rows[I].Cells[21].Text;
                    ws.Cell(I + 2, 18).Value = gv.Rows[I].Cells[6].Text;
                    ws.Cell(I + 2, 19).Value = gv.Rows[I].Cells[22].Text;
                    ws.Cell(I + 2, 20).Value = gv.Rows[I].Cells[23].Text;
                    ws.Cell(I + 2, 21).Value = gv.Rows[I].Cells[24].Text;
                    ws.Cell(I + 2, 22).Value = gv.Rows[I].Cells[25].Text;
                    ws.Cell(I + 2, 23).Value = gv.Rows[I].Cells[12].Text;
                    ws.Cell(I + 2, 24).Value = gv.Rows[I].Cells[26].Text;
                    ws.Cell(I + 2, 25).Value = gv.Rows[I].Cells[27].Text;
                    ws.Cell(I + 2, 26).Value = gv.Rows[I].Cells[9].Text;
                    ws.Cell(I + 2, 27).Value = gv.Rows[I].Cells[36].Text;
                    ws.Cell(I + 2, 28).Value = gv.Rows[I].Cells[8].Text;
                    ws.Cell(I + 2, 29).Value = gv.Rows[I].Cells[28].Text;
                    ws.Cell(I + 2, 30).Value = gv.Rows[I].Cells[29].Text;
                    ws.Cell(I + 2, 31).Value = gv.Rows[I].Cells[30].Text;
                    ws.Cell(I + 2, 32).Value = gv.Rows[I].Cells[31].Text;
                    ws.Cell(I + 2, 33).Value = gv.Rows[I].Cells[32].Text;
                    ws.Cell(I + 2, 34).Value = gv.Rows[I].Cells[33].Text;
                    ws.Cell(I + 2, 35).Value = gv.Rows[I].Cells[39].Text;
                }
            }
            ws.Range("c2", "c" + ws.RangeUsed().RowCount()).Style.NumberFormat.SetFormat("dd-MMM-yyyy hh:mm:ss");
            ws.Range("m2", "m" + ws.RangeUsed().RowCount()).Style.NumberFormat.SetFormat("dd-MMM-yyyy hh:mm:ss");
            ws.Range("w2", "w" + ws.RangeUsed().RowCount()).Style.NumberFormat.SetFormat("dd-MMM-yyyy hh:mm:ss");
            ws.Range("j2", "j" + ws.RangeUsed().RowCount()).Style.NumberFormat.SetFormat("#");
            ws.Columns().AdjustToContents();
            ws.SheetView.FreezeRows(1);
            ClosedXML.Excel.IXLRange range = ws.RangeUsed();
            string RCNT = "ai" + range.RowCount();
            ws.Range("a1", RCNT).Style.Border.TopBorder = XLBorderStyleValues.Thin;
            ws.Range("a1", RCNT).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            ws.Range("a1", RCNT).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            ws.Range("a1", RCNT).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            ws.Range("a1", "ai1").Style.Fill.BackgroundColor = XLColor.Turquoise;
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=PMR.xlsx");
            using (MemoryStream MyMemoryStream = new MemoryStream())
            {
                wb.SaveAs(MyMemoryStream);
                MyMemoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
            return Content("Data added successfully");
        }

        [Authorize]
        public ActionResult PMR_Dtls1(int ens)
        {
            return PartialView("PMR_Dtls", dc.PMR.Find(ens));
        }

        public ActionResult exp1_pmr(string ens)
        {
            var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("PMR");
            ws.Cell(1, 1).Value = "Customer";
            ws.Cell(1, 2).Value = "Issue Type";
            ws.Cell(1, 3).Value = "Issue Open Date";
            ws.Cell(1, 4).Value = "Complaint / Service No";
            ws.Cell(1, 5).Value = "Site Id";
            ws.Cell(1, 6).Value = "Site Name";
            ws.Cell(1, 7).Value = "District";
            ws.Cell(1, 8).Value = "State";
            ws.Cell(1, 9).Value = "Issue Category";
            ws.Cell(1, 10).Value = "Engine No";
            ws.Cell(1, 11).Value = "Model";
            ws.Cell(1, 12).Value = "KVA";
            ws.Cell(1, 13).Value = "DOI";
            ws.Cell(1, 14).Value = "DG SET NO";
            ws.Cell(1, 15).Value = "Alt. Make";
            ws.Cell(1, 16).Value = "Alt. Sr. No";
            ws.Cell(1, 17).Value = "Battery Sr. No";
            ws.Cell(1, 18).Value = "HMR";
            ws.Cell(1, 19).Value = "Issue Nature";
            ws.Cell(1, 20).Value = "Serverity";
            ws.Cell(1, 21).Value = "Failure Reason";
            ws.Cell(1, 22).Value = "Issue Status";
            ws.Cell(1, 23).Value = "Issue Closed Date";
            ws.Cell(1, 24).Value = "Warranty Status";
            ws.Cell(1, 25).Value = "Action Taken";
            ws.Cell(1, 26).Value = "Material Used";
            ws.Cell(1, 27).Value = "Dealer Code";
            ws.Cell(1, 28).Value = "Technician Visited";
            ws.Cell(1, 29).Value = "OEA";
            ws.Cell(1, 30).Value = "AMC Status";
            ws.Cell(1, 31).Value = "TTR";
            ws.Cell(1, 32).Value = "SLA Status";
            ws.Cell(1, 33).Value = "Time Slot";
            ws.Cell(1, 34).Value = "Reason Of SLA";
            ws.Cell(1, 35).Value = "Remarks";
            List<PMR> STLIST = new List<PMR>();
            using (DBCTX FC = new DBCTX())
            {
                STLIST = FC.PMR.Where(A => A.ENGINE_No.Equals(ens)).ToList();
            }
            GridView gv = new GridView();
            gv.DataSource = STLIST;
            gv.DataBind();
            for (int I = 0; I < gv.Rows.Count; I++)
            {
                ws.Cell(I + 2, 1).Value = gv.Rows[I].Cells[10].Text;
                ws.Cell(I + 2, 2).Value = gv.Rows[I].Cells[5].Text;
                ws.Cell(I + 2, 3).Value = gv.Rows[I].Cells[4].Text;
                ws.Cell(I + 2, 4).Value = gv.Rows[I].Cells[11].Text;
                ws.Cell(I + 2, 5).Value = gv.Rows[I].Cells[1].Text;
                ws.Cell(I + 2, 6).Value = gv.Rows[I].Cells[2].Text;
                ws.Cell(I + 2, 7).Value = gv.Rows[I].Cells[13].Text;
                ws.Cell(I + 2, 8).Value = gv.Rows[I].Cells[14].Text;
                ws.Cell(I + 2, 9).Value = gv.Rows[I].Cells[15].Text;
                ws.Cell(I + 2, 10).Value = gv.Rows[I].Cells[3].Text;
                ws.Cell(I + 2, 11).Value = gv.Rows[I].Cells[16].Text;
                ws.Cell(I + 2, 12).Value = gv.Rows[I].Cells[34].Text;
                ws.Cell(I + 2, 13).Value = gv.Rows[I].Cells[17].Text;
                ws.Cell(I + 2, 14).Value = gv.Rows[I].Cells[18].Text;
                ws.Cell(I + 2, 15).Value = gv.Rows[I].Cells[19].Text;
                ws.Cell(I + 2, 16).Value = gv.Rows[I].Cells[20].Text;
                ws.Cell(I + 2, 17).Value = gv.Rows[I].Cells[21].Text;
                ws.Cell(I + 2, 18).Value = gv.Rows[I].Cells[6].Text;
                ws.Cell(I + 2, 19).Value = gv.Rows[I].Cells[22].Text;
                ws.Cell(I + 2, 20).Value = gv.Rows[I].Cells[23].Text;
                ws.Cell(I + 2, 21).Value = gv.Rows[I].Cells[24].Text;
                ws.Cell(I + 2, 22).Value = gv.Rows[I].Cells[25].Text;
                ws.Cell(I + 2, 23).Value = gv.Rows[I].Cells[12].Text;
                ws.Cell(I + 2, 24).Value = gv.Rows[I].Cells[26].Text;
                ws.Cell(I + 2, 25).Value = gv.Rows[I].Cells[27].Text;
                ws.Cell(I + 2, 26).Value = gv.Rows[I].Cells[9].Text;
                ws.Cell(I + 2, 27).Value = gv.Rows[I].Cells[36].Text;
                ws.Cell(I + 2, 28).Value = gv.Rows[I].Cells[8].Text;
                ws.Cell(I + 2, 29).Value = gv.Rows[I].Cells[28].Text;
                ws.Cell(I + 2, 30).Value = gv.Rows[I].Cells[29].Text;
                ws.Cell(I + 2, 31).Value = gv.Rows[I].Cells[30].Text;
                ws.Cell(I + 2, 32).Value = gv.Rows[I].Cells[31].Text;
                ws.Cell(I + 2, 33).Value = gv.Rows[I].Cells[32].Text;
                ws.Cell(I + 2, 34).Value = gv.Rows[I].Cells[33].Text;
                ws.Cell(I + 2, 35).Value = gv.Rows[I].Cells[39].Text;
            }
            ws.Range("c2", "c" + ws.RangeUsed().RowCount()).Style.NumberFormat.SetFormat("dd-MMM-yyyy hh:mm:ss");
            ws.Range("m2", "m" + ws.RangeUsed().RowCount()).Style.NumberFormat.SetFormat("dd-MMM-yyyy hh:mm:ss");
            ws.Range("w2", "w" + ws.RangeUsed().RowCount()).Style.NumberFormat.SetFormat("dd-MMM-yyyy hh:mm:ss");
            ws.Range("j2", "j" + ws.RangeUsed().RowCount()).Style.NumberFormat.SetFormat("#");
            ws.Columns().AdjustToContents();
            ws.SheetView.FreezeRows(1);
            ClosedXML.Excel.IXLRange range = ws.RangeUsed();
            string RCNT = "ai" + range.RowCount();
            ws.Range("a1", RCNT).Style.Border.TopBorder = XLBorderStyleValues.Thin;
            ws.Range("a1", RCNT).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            ws.Range("a1", RCNT).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            ws.Range("a1", RCNT).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            ws.Range("a1", "ai1").Style.Fill.BackgroundColor = XLColor.Turquoise;
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=PMR.xlsx");
            using (MemoryStream MyMemoryStream = new MemoryStream())
            {
                wb.SaveAs(MyMemoryStream);
                MyMemoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
            return Content("Data added successfully");
        }

        public JsonResult List_rm()
        {
            var dbResult = dc.PMR.ToList();
           
            return Json(dbResult, JsonRequestBehavior.AllowGet);
        }

        public ActionResult List_rm1()
        {
            return View(dc.PMR.ToList());
        }
        public JsonResult List_POP()
        {
            var dbResult = dc.MAINPOPU.ToList();

            return Json(dbResult, JsonRequestBehavior.AllowGet);
        }
    }
}
