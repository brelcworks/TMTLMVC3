using MVCINCV4._1.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClosedXML.Excel;
using System.Web.UI.WebControls;
using System.IO;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;



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
        public ActionResult Save1(BILL1 bill)
        {
            string message = "";
            if (ModelState.IsValid)
            {
                try
                {
                    using (DBCTX aid = new DBCTX())
                    {
                        aid.BILL1.Add(bill);
                        aid.SaveChanges();
                        message = "Successfully Saved!";
                    }
                }
                catch (Exception ex) { message = ex.ToString(); }
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
                catch (Exception ex) { message = ex.ToString(); }
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
        [Authorize]
        public ActionResult PRINTBILL(string BNO, string BDATE, string CUST, string SNAME, string ADDR, string VNO)
        {
            MemoryStream MS = new MemoryStream();
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            string FL = "CHALLAN.xlsx";
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=" + FL);
            using (SpreadsheetDocument document = SpreadsheetDocument.Create(MS, SpreadsheetDocumentType.Workbook))
            {
                WorkbookPart workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();
                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet(new SheetData());
                Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());
                Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Test Sheet" };
                sheets.Append(sheet);
                workbookPart.Workbook.Save();
            }
            MS.WriteTo(Response.OutputStream);
            Response.Flush();
            Response.End();
            return Content("OK");
        }
        [Authorize]
        public ActionResult PRINTCHALLAN(string BNO, string BDATE, string CUST, string SNAME, string ADDR, string VNO)
        {
            string message = "";
            try
            {
                GridView gv = new GridView();
                var totcon = dc.BILL.Where(A => A.BILL_NO.Equals(BNO)).ToList();
                gv.DataSource = totcon;
                gv.DataBind();
                var wb = new XLWorkbook();
                var ws = wb.Worksheets.Add("CHALLAN");
                if (totcon.Count <= 12)
                {
                    ws.Column(1).Width = 5.43;
                    ws.Column(2).Width = 6;
                    ws.Column(3).Width = 6.29;
                    ws.Column(4).Width = 8.43;
                    ws.Column(5).Width = 8.43;
                    ws.Column(6).Width = 17.57;
                    ws.Column(7).Width = 3;
                    ws.Column(8).Width = 7.29;
                    ws.Column(9).Width = 9.5;
                    ws.Column(10).Width = 7.43;
                    ws.Column(11).Width = 10.86;
                    ws.Range("a2", "k2").Merge();
                    ws.Range("a2").Value = "B. & R. ELECTRICAL WORKS";
                    ws.Range("a2").Style.Font.FontName = "Book Antiqua";
                    ws.Range("a2").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Range("a2").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    ws.Range("a2").Style.Font.FontSize = 24;
                    ws.Range("a2").Style.Font.Bold = true;
                    ClosedXML.Excel.IXLRange range = ws.Range("a1", "k5");
                    range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws.Row(6).Height = 3;
                    ws.Cell(7, 1).Value = "CHALLAN NO:-";
                    ws.Cell(7, 10).Value = "DATE:-";
                    ws.Cell(7, 11).Value = BDATE;
                    ws.Range("K7").Style.NumberFormat.SetFormat("dd-MMM-yyyy");
                    ws.Cell(8, 1).Value = "CUSTOMER:-";
                    ws.Cell(10, 1).Value = "VAT NO:-";
                    range = ws.Range("a7", "k10");
                    range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws.Row(11).Height = 3;
                    ws.Cell(12, 1).Value = "SL NO";
                    ws.Cell(12, 2).Value = "PART NO";
                    ws.Cell(12, 4).Value = "PARTICULARS";
                    ws.Cell(12, 9).Value = "MRP";
                    ws.Cell(12, 10).Value = "QTY";
                    ws.Cell(12, 11).Value = "PER";
                    range = ws.Range("a12", "k12");
                    range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    range = ws.Range("a12", "A24");
                    range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    range = ws.Range("d12", "H24");
                    range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    range = ws.Range("j12", "j24");
                    range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    range = ws.Range("k12", "k24");
                    range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws.Range("i12", "i24").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                    ws.Range("j12", "j24").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                    ws.Range("k12", "k24").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Range("a13", "k24").Style.Font.FontSize = 9.5;
                    ws.Range("a27", "k27").Style.Font.Bold = true;
                    ws.Range("i24", "k24").Style.Font.Bold = true;
                    ws.Range("i13", "i24").Style.NumberFormat.SetFormat("#.00");
                    ws.PageSetup.Margins.Left = 0.34;
                    ws.PageSetup.Margins.Right = 0.17;
                    ws.PageSetup.Margins.Top = 0.35;
                    ws.PageSetup.Margins.Bottom = 0.02;
                    ws.PageSetup.Margins.Header = 0.02;
                    ws.PageSetup.Margins.Footer = 0.02;
                    ws.PageSetup.PaperSize = XLPaperSize.A4Paper;
                    for (int i = 0; i < gv.Rows.Count; i++)
                    {

                        ws.Cell(i + 13, 1).Value = i + 1;
                        ws.Cell(i + 13, 2).Value = "'" + gv.Rows[i].Cells[6].Text;
                        ws.Cell(i + 13, 4).Value = gv.Rows[i].Cells[7].Text;
                        ws.Cell(i + 13, 9).Value = gv.Rows[i].Cells[9].Text;
                        ws.Cell(i + 13, 10).Value = gv.Rows[i].Cells[8].Text;
                        ws.Cell(i + 13, 11).Value = gv.Rows[i].Cells[16].Text;
                    }
                    System.Text.RegularExpressions.Match Rlst = System.Text.RegularExpressions.Regex.Match(BNO, "\\d+");
                    ws.Cell(7, 3).Value = Rlst.Value;
                    if (SNAME != null)
                    {
                        ws.Cell(8, 3).Value = CUST + "  [SITE: " + SNAME + "]";
                    }
                    else
                    {
                        ws.Cell(8, 3).Value = CUST;
                    }
                    ws.Cell(9, 1).Value = "ADDRESS:-";
                    ws.Cell(9, 3).Value = ADDR;
                    ws.Range("c10", "d10").Merge();
                    ws.Range("c10").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                    ws.Cell(10, 3).Value = VNO;
                    ws.Range("i27").Value = "For, B & R ELECTRICAL WORKS";
                    ws.Range("i27", "k27").Merge();
                    ws.Range("i27").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                    range = ws.Range("a25", "k27");
                    range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws.Range("a1", "k1").Merge();
                    ws.Range("a1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Range("a1").Value = "CHALLAN";
                    ws.Range("a1").Style.Font.Underline = XLFontUnderlineValues.Single;
                    ws.Row(32).Height = 3;
                    ws.Range("a3", "k3").Merge();
                    ws.Range("a3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Range("a3").Value = "STALL NO. T/542, NEAR DOOARS BUS STAND, BIDHAN MARKET, SILIGURI - 734001";
                    ws.Range("a4", "k4").Merge();
                    ws.Range("a4").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Range("a4").Value = "0353 - 2433269, 2526285";
                    ws.Range("a5", "k5").Merge();
                    ws.Range("a5").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Range("a5").Value = "VAT NO: " + "19896290025" + ", CST NO: " + "19896290219" + ", PAN NO: " + "AACFB7969H" + ", S.TAX NO: " + "AACFB7969HST001";
                    ws.Range("b13", "b24").Style.NumberFormat.SetFormat("@");
                    ws.Cell(27, 1).Value = "Signature of Customer";
                    ws.Range("a24").Style.Font.FontName = "Book Antiqua";
                    ws.Range("a24").Style.Font.Bold = true;
                    ws.Range("a13", "a24").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Range("a41", "a52").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Range("a24").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                    range = ws.Range("a1", "k27");
                    ws.Cell(30, 1).Value = range;
                    ws.Row(35).Height = 3;
                    ws.Row(40).Height = 3;
                    ws.Range("a28", "k28").Style.Border.BottomBorder = XLBorderStyleValues.SlantDashDot;
                    ws.Row(28).Height = 20;
                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    string FL = "CHALLAN" + Rlst.Value + ".xlsx";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=" + FL);
                    message = "Successfully Saved!";
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
                else
                {
                    ws.Column(1).Width = 5.43;
                    ws.Column(2).Width = 6;
                    ws.Column(3).Width = 6.29;
                    ws.Column(4).Width = 8.43;
                    ws.Column(5).Width = 8.43;
                    ws.Column(6).Width = 17.57;
                    ws.Column(7).Width = 3;
                    ws.Column(8).Width = 7.29;
                    ws.Column(9).Width = 9.5;
                    ws.Column(10).Width = 7.43;
                    ws.Column(11).Width = 10.86;
                    ws.Range("a2", "k2").Merge();
                    ws.Range("a2").Value = "B. & R. ELECTRICAL WORKS";
                    ws.Range("a2").Style.Font.FontName = "Book Antiqua";
                    ws.Range("a2").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Range("a2").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    ws.Range("a2").Style.Font.FontSize = 24;
                    ws.Range("a2").Style.Font.Bold = true;
                    ClosedXML.Excel.IXLRange range;
                    range = ws.Range("a1", "k1");
                    range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws.Row(6).Height = 3;
                    ws.Cell(7, 1).Value = "CHALLAN NO:-";
                    ws.Cell(7, 10).Value = "DATE:-";
                    ws.Cell(7, 11).Value = BDATE;
                    ws.Cell(8, 1).Value = "CUSTOMER:-";
                    ws.Cell(10, 1).Value = "VAT NO:-";
                    range = ws.Range("a1", "k5");
                    range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    range = ws.Range("a7", "k10");
                    range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws.Row(11).Height = 3;
                    ws.Cell(12, 1).Value = "SL NO";
                    ws.Cell(12, 2).Value = "PART NO";
                    ws.Cell(12, 4).Value = "PARTICULARS";
                    ws.Cell(12, 9).Value = "MRP";
                    ws.Cell(12, 10).Value = "QTY";
                    ws.Cell(12, 11).Value = "PER";
                    range = ws.Range("a12", "k12");
                    range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    range = ws.Range("a12", "A52");
                    range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    range = ws.Range("d12", "H52");
                    range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    range = ws.Range("j12", "j52");
                    range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    range = ws.Range("k12", "k52");
                    range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws.Range("i12", "i52").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                    ws.Range("j12", "j52").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                    ws.Range("k12", "k52").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Range("a13", "k52").Style.Font.FontSize = 9.5;
                    ws.Range("a56", "k56").Style.Font.Bold = true;
                    ws.PageSetup.Margins.Left = 0.25;
                    ws.PageSetup.Margins.Right = 0.17;
                    ws.PageSetup.Margins.Top = 0.15;
                    ws.PageSetup.Margins.Bottom = 0.02;
                    ws.PageSetup.Margins.Header = 0.02;
                    ws.PageSetup.Margins.Footer = 0.02;
                    for (int i = 0; i < gv.Rows.Count; i++)
                    {

                        ws.Cell(i + 13, 1).Value = i + 1;
                        ws.Cell(i + 13, 2).Value = "'" + gv.Rows[i].Cells[6].Text;
                        ws.Cell(i + 13, 4).Value = gv.Rows[i].Cells[7].Text;
                        ws.Cell(i + 13, 9).Value = gv.Rows[i].Cells[9].Text;
                        ws.Cell(i + 13, 10).Value = gv.Rows[i].Cells[8].Text;
                        ws.Cell(i + 13, 11).Value = gv.Rows[i].Cells[16].Text;
                    }
                    System.Text.RegularExpressions.Match Rlst = System.Text.RegularExpressions.Regex.Match(BNO, "\\d+");
                    ws.Cell(7, 3).Value = Rlst.Value;
                    if (SNAME != null)
                    {
                        ws.Cell(8, 3).Value = CUST + "  [SITE: " + SNAME + "]";
                    }
                    else
                    {
                        ws.Cell(8, 3).Value = CUST;
                    }
                    ws.Cell(9, 1).Value = "ADDRESS:-";
                    ws.Cell(9, 3).Value = ADDR;
                    ws.Range("c10", "d10").Merge();
                    ws.Range("c10").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                    ws.Cell(10, 3).Value = VNO;
                    ws.Range("i56").Value = "For, B & R ELECTRICAL WORKS";
                    ws.Range("i56", "k56").Merge();
                    ws.Range("i56").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                    range = ws.Range("a53", "k56");
                    range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws.Range("a1", "k1").Merge();
                    ws.Range("a1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Range("a1").Value = "CHALLAN";
                    ws.Range("a1").Style.Font.Underline = XLFontUnderlineValues.Single;
                    ws.Row(32).Height = 3;
                    ws.Range("a3", "k3").Merge();
                    ws.Range("a3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Range("a3").Value = "STALL NO. T/542, NEAR DOOARS BUS STAND, BIDHAN MARKET, SILIGURI - 734001";
                    ws.Range("a4", "k4").Merge();
                    ws.Range("a4").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Range("a4").Value = "0353 - 2433269, 2526285";
                    ws.Range("a5", "k5").Merge();
                    ws.Range("a5").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Range("a5").Value = "VAT NO: " + "19896290025" + ", CST NO: " + "19896290219" + ", PAN NO: " + "AACFB7969H" + ", S.TAX NO: " + "AACFB7969HST001";
                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=CHALLAN.xlsx");
                    message = "Successfully Saved!";
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);

                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
            }
            catch (Exception ex) { message = ex.Message; }
            return Content(message);
        }

        public JsonResult List_sls(int page, int rows, string sidx, string sord)
        {
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;

            var dbResult = dc.BILL.Select(
                a => new
                {
                    a.BILLID,
                    a.BILL_NO,
                    a.BDATE,
                    a.CUST,
                    a.DNAME,
                    a.PART_NO,
                    a.PARTI,
                    a.MRP,
                    a.SPRICE,
                    a.QTY,
                    a.UNIT,
                    a.TOTAL,
                    a.STOT,
                    a.TVAL
                });
            int totalRecords = dbResult.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);
            if (sord.ToUpper() == "DESC")
            {
                dbResult = dbResult.OrderByDescending(s => s.BILLID);
                dbResult = dbResult.Skip(pageIndex * pageSize).Take(pageSize);
            }
            else
            {
                dbResult = dbResult.OrderBy(s => s.BILLID);
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
        public ActionResult List_sls1()
        {
            return View(dc.BILL.ToList());
        }
    }
}
