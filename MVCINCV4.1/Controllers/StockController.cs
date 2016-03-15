using ClosedXML.Excel;
using MVCINCV4._1.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Services;
using System.Web.Security;
using System.Web.Services;
using System.Web.UI.WebControls;

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
            return PartialView("Create");
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
            return PartialView("Edit", dc.STOCK.Find(id));
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
                rows = dbResult
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
        public string edt(SHEET1 objPMR)
        {
            string msg;
            try
            {
                dc.Entry(objPMR).State = EntityState.Modified;
                dc.SaveChanges();
                msg = "Saved Successfully";
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return msg;
        }

        public string dlt(int id)
        {
            SHEET1 dl = dc.SHEET1.Find(id);
            dc.SHEET1.Remove(dl);
            dc.SaveChanges();
            return "Deleted Successfully!";
        }

        public JsonResult List_SC()
        {
            var dbResult = dc.STOCK.ToList();
            return Json(dbResult, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public ActionResult ExportData()
        {
            GridView gv = new GridView();
            gv.DataSource = dc.STOCK.ToList();
            gv.DataBind();
            var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("STOCK");
            ws.Cell(1, 1).Value = "SL. NO.";
            ws.Cell(1, 2).Value = "PART NAME";
            ws.Cell(1, 3).Value = "PART NO";
            ws.Cell(1, 4).Value = "QTY";
            ws.Cell(1, 5).Value = "MRP";
            ws.Cell(1, 6).Value = "SELL PRICE";
            ws.Cell(1, 7).Value = "UNIT";
            ws.Cell(1, 8).Value = "RACK NO";
            ws.Cell(1, 9).Value = "MRP TOTAL";
            ws.Cell(1, 10).Value = "SELL PRICE TOTAL";
            for (int I = 0; I < gv.Rows.Count; I++)
            {
                ws.Cell(I + 2, 1).Value = Convert.ToInt32(I.ToString()) + 1;
                ws.Cell(I + 2, 2).Value = gv.Rows[I].Cells[4].Text;
                ws.Cell(I + 2, 3).Value = gv.Rows[I].Cells[3].Text;
                ws.Cell(I + 2, 4).Value = gv.Rows[I].Cells[6].Text;
                ws.Cell(I + 2, 5).Value = gv.Rows[I].Cells[5].Text;
                ws.Cell(I + 2, 6).Value = gv.Rows[I].Cells[14].Text;
                ws.Cell(I + 2, 7).Value = gv.Rows[I].Cells[13].Text;
                ws.Cell(I + 2, 8).Value = gv.Rows[I].Cells[8].Text;
                ws.Cell(I + 2, 9).Value = gv.Rows[I].Cells[7].Text;
                ws.Cell(I + 2, 10).Value = gv.Rows[I].Cells[11].Text;
            }
            ws.Range("I2", "J" + ws.RangeUsed().RowCount()).Style.NumberFormat.SetFormat("#.##");
            ws.Range("e2", "f" + ws.RangeUsed().RowCount()).Style.NumberFormat.SetFormat("#.##");
            ws.Columns().AdjustToContents();
            ws.SheetView.FreezeRows(1);
            ClosedXML.Excel.IXLRange range = ws.RangeUsed();
            string RCNT = "J" + range.RowCount();
            ws.Range("a1", RCNT).Style.Border.TopBorder = XLBorderStyleValues.Thin;
            ws.Range("a1", RCNT).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            ws.Range("a1", RCNT).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            ws.Range("a1", RCNT).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            ws.Range("a1", "J1").Style.Fill.BackgroundColor = XLColor.Turquoise;
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=STOCK.xlsx");
            using (MemoryStream MyMemoryStream = new MemoryStream())
            {
                wb.SaveAs(MyMemoryStream);
                MyMemoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
            return RedirectToAction("List");
        }
    }
}
