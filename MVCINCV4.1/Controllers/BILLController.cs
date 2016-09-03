using MVCINCV4._1.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClosedXML.Excel;
using System.IO;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;

using Xdr = DocumentFormat.OpenXml.Drawing.Spreadsheet;


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

        private static Column CreateColumnData(UInt32 StartColumnIndex, UInt32 EndColumnIndex, double ColumnWidth)
        {
            Column column;
            column = new Column();
            column.Min = StartColumnIndex;
            column.Max = EndColumnIndex;
            column.Width = ColumnWidth;
            column.CustomWidth = true;
            return column;
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
                worksheetPart.Worksheet = new Worksheet();
                WorkbookStylesPart stylePart = workbookPart.AddNewPart<WorkbookStylesPart>();
                stylePart.Stylesheet = CreateStylesheet();
                stylePart.Stylesheet.Save();
                Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());
                Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Test Sheet" };
                sheets.Append(sheet);
                workbookPart.Workbook.Save();

                Columns columns = new Columns();
                columns.Append(CreateColumnData(1, 1, 5.43));
                columns.Append(CreateColumnData(2, 2, 6));
                columns.Append(CreateColumnData(3, 3, 6.29));
                columns.Append(CreateColumnData(4, 4, 8.43));
                columns.Append(CreateColumnData(5, 5, 8.43));
                columns.Append(CreateColumnData(6, 6, 17.57));
                columns.Append(CreateColumnData(7, 7, 13));
                columns.Append(CreateColumnData(8, 8, 7.29));
                columns.Append(CreateColumnData(9, 9, 9.5));
                columns.Append(CreateColumnData(10, 10, 7.43));
                columns.Append(CreateColumnData(11, 11, 10.86));
                worksheetPart.Worksheet.AppendChild(columns);
                SheetData sheetData = worksheetPart.Worksheet.AppendChild(new SheetData());
                
                Row row = new Row();
                Row row1 = new Row();
                Row row2 = new Row();
                Row row3 = new Row();
                Row row4 = new Row();
                Row row5 = new Row();
                Row row6 = new Row();
                Row row7 = new Row();
                Row row8 = new Row();
                Row row9 = new Row();
                sheetData.AppendChild(row);
                sheetData.AppendChild(row1);
                sheetData.AppendChild(row2);
                sheetData.AppendChild(row3);
                sheetData.AppendChild(row4);
                sheetData.AppendChild(row5);
                sheetData.AppendChild(row6);
                sheetData.AppendChild(row7);
                sheetData.AppendChild(row8);
                sheetData.AppendChild(row9);
                row.Append(ConstructCell("INVOICE", CellValues.String, 6), ConstructCell("", CellValues.String, 5), ConstructCell("", CellValues.String, 5), ConstructCell("", CellValues.String, 5), ConstructCell("", CellValues.String, 5), ConstructCell("", CellValues.String, 5), ConstructCell("", CellValues.String, 5), ConstructCell("", CellValues.String, 5), ConstructCell("", CellValues.String, 5), ConstructCell("", CellValues.String, 5), ConstructCell("", CellValues.String, 7));
                row1.Append(ConstructCell("B & R ELECTRICAL WORKS", CellValues.String, 3), ConstructCell("", CellValues.String, 0), ConstructCell("", CellValues.String, 0), ConstructCell("", CellValues.String, 0), ConstructCell("", CellValues.String, 0), ConstructCell("", CellValues.String, 0), ConstructCell("", CellValues.String, 0), ConstructCell("", CellValues.String, 0), ConstructCell("", CellValues.String, 0), ConstructCell("", CellValues.String, 0), ConstructCell("", CellValues.String, 8));
                row2.Append(ConstructCell("STALL NO. T/542, NEAR DOOARS BUS STAND, BIDHAN MARKET, SILIGURI - 734001", CellValues.String, 2), ConstructCell("", CellValues.String, 0), ConstructCell("", CellValues.String, 0), ConstructCell("", CellValues.String, 0), ConstructCell("", CellValues.String, 0), ConstructCell("", CellValues.String, 0), ConstructCell("", CellValues.String, 0), ConstructCell("", CellValues.String, 0), ConstructCell("", CellValues.String, 0), ConstructCell("", CellValues.String, 0), ConstructCell("", CellValues.String, 8));
                row3.Append(ConstructCell("0353 - 2433269, 2526285", CellValues.String, 2), ConstructCell("", CellValues.String, 0), ConstructCell("", CellValues.String, 0), ConstructCell("", CellValues.String, 0), ConstructCell("", CellValues.String, 0), ConstructCell("", CellValues.String, 0), ConstructCell("", CellValues.String, 0), ConstructCell("", CellValues.String, 0), ConstructCell("", CellValues.String, 0), ConstructCell("", CellValues.String, 0), ConstructCell("", CellValues.String, 8));
                row4.Append(ConstructCell("VAT NO: 19896290025, CST NO: 19896290219, PAN NO: AACFB7969H, S.TAX NO: AACFB7969HST001", CellValues.String, 2), ConstructCell("", CellValues.String, 0), ConstructCell("", CellValues.String, 0), ConstructCell("", CellValues.String, 0), ConstructCell("", CellValues.String, 0), ConstructCell("", CellValues.String, 0), ConstructCell("", CellValues.String, 0), ConstructCell("", CellValues.String, 0), ConstructCell("", CellValues.String, 0), ConstructCell("", CellValues.String, 0), ConstructCell("", CellValues.String, 8));
                row5.Height = 3;
                row5.CustomHeight = true;
                for (int i1 = 1; i1 < 12; i1++)
                {
                    row5.Append(
                     ConstructCell("", CellValues.String, 5));
                }
                Cell cell = new Cell(){ CellReference = "A7", DataType = CellValues.String, CellValue = new CellValue("INVOICE NO:"), StyleIndex=9 };
                Cell bcl1 = new Cell() { CellReference = "B7", DataType = CellValues.String, CellValue = new CellValue(""), StyleIndex = 16 };
                Cell cell1 = new Cell() { CellReference = "C7", DataType = CellValues.String, CellValue = new CellValue(BNO), StyleIndex= 16 };
                Cell bcl2 = new Cell() { CellReference = "D7", DataType = CellValues.String, CellValue = new CellValue(""), StyleIndex = 5 };
                Cell bcl3 = new Cell() { CellReference = "E7", DataType = CellValues.String, CellValue = new CellValue(""), StyleIndex = 5 };
                Cell bcl4 = new Cell() { CellReference = "F7", DataType = CellValues.String, CellValue = new CellValue(""), StyleIndex = 5 };
                Cell bcl5 = new Cell() { CellReference = "G7", DataType = CellValues.String, CellValue = new CellValue(""), StyleIndex = 5 };
                Cell bcl6 = new Cell() { CellReference = "H7", DataType = CellValues.String, CellValue = new CellValue(""), StyleIndex = 5 };
                Cell bcl7 = new Cell() { CellReference = "I7", DataType = CellValues.String, CellValue = new CellValue(""), StyleIndex = 5 };
                Cell cell2 = new Cell() { CellReference = "J7", DataType = CellValues.String, CellValue = new CellValue("DATE:"), StyleIndex = 16 };
                Cell cell3 = new Cell() { CellReference = "K7", DataType = CellValues.String, CellValue = new CellValue(BDATE), StyleIndex = 7 };
                
                row6.Append(cell,bcl1,cell1,bcl2,bcl3,bcl4,bcl5,bcl6,bcl7,cell2,cell3);
                Cell cell4 = new Cell() { CellReference = "A8", DataType = CellValues.String, CellValue = new CellValue("CUSTOMER:"), StyleIndex= 10};
                Cell cell5 = new Cell() { CellReference = "C8", DataType = CellValues.String, CellValue = new CellValue(CUST) };
                Cell bcl8 = new Cell() { CellReference = "K8", DataType = CellValues.String, CellValue = new CellValue(""), StyleIndex=8 };
                row7.Append(cell4,cell5,bcl8);
                Cell cell6 = new Cell() { CellReference = "A9", DataType = CellValues.String, CellValue = new CellValue("ADDRESS:"), StyleIndex=10 };
                Cell cell7 = new Cell() { CellReference = "C9", DataType = CellValues.String, CellValue = new CellValue(ADDR) };
                Cell bcl9 = new Cell() { CellReference = "K9", DataType = CellValues.String, CellValue = new CellValue(""), StyleIndex = 8 };
                row8.Append(cell6,cell7,bcl9);
                Cell cell8 = new Cell() { CellReference = "A10", DataType = CellValues.String, CellValue = new CellValue("VAT NO:"), StyleIndex=10 };
                Cell cell9 = new Cell() { CellReference = "C10", DataType = CellValues.String, CellValue = new CellValue(VNO) };
                Cell bcl10 = new Cell() { CellReference = "K10", DataType = CellValues.String, CellValue = new CellValue(""), StyleIndex = 8 };
                row9.Append(cell8,cell9,bcl10);
                row = new Row();
                row.Height = 3;
                row.CustomHeight = true;
                for (int i1 = 1; i1 < 12; i1++)
                {
                    row.Append(
                     ConstructCell("", CellValues.String, 5));
                }
                sheetData.AppendChild(row);
                
                List <BILL> employees = dc.BILL.Where(A => A.BILL_NO.Equals(BNO)).ToList();
                 row = new Row();
                row.Append(
                    ConstructCell("SL NO", CellValues.String,12),
                    ConstructCell("PART NO", CellValues.String,12),
                    ConstructCell("", CellValues.String,14),
                    ConstructCell("PART NAME", CellValues.String,12),
                     ConstructCell("", CellValues.String, 14),
                     ConstructCell("", CellValues.String, 14),
                     ConstructCell("", CellValues.String, 14),
                    ConstructCell("QTY", CellValues.String,12),
                ConstructCell("RATE", CellValues.String, 12),
                ConstructCell("PER", CellValues.String, 12),
                ConstructCell("AMOUNT", CellValues.String, 15));

                // Insert the header row to the Sheet Data
                sheetData.AppendChild(row);
                int i = 1;
                foreach (var employee in employees)
                {
                    row = new Row();
                    row.Append(
                    ConstructCell(i.ToString(), CellValues.String,10),
                    ConstructCell(employee.PART_NO, CellValues.String,10),
                    ConstructCell("", CellValues.String),
                    ConstructCell(employee.PARTI, CellValues.String,10),
                     ConstructCell("", CellValues.String),
                      ConstructCell("", CellValues.String),
                       ConstructCell("", CellValues.String),
                    ConstructCell(employee.QTY, CellValues.Number,10),
                    ConstructCell(employee.SPRICE, CellValues.Number, 10),
                    ConstructCell(employee.UNIT, CellValues.String, 10),
                    ConstructCell(employee.STOT, CellValues.Number, 4));

                    sheetData.AppendChild(row);
                    i = i + 1;
                }
                for (int i1 = i; i1 < 37; i1++)
                {
                    row = new Row();
                    row.Append(
                     ConstructCell(i1.ToString(), CellValues.String, 10), ConstructCell("", CellValues.String, 10), ConstructCell("", CellValues.String), ConstructCell("", CellValues.String,10), ConstructCell("", CellValues.String), ConstructCell("", CellValues.String), ConstructCell("", CellValues.String), ConstructCell("", CellValues.String, 10), ConstructCell("", CellValues.String, 10), ConstructCell("", CellValues.String, 10), ConstructCell("", CellValues.String, 4));
                     sheetData.AppendChild(row);
                }
                row = new Row();
                for (int i1 = 1; i1 < 9; i1++)
                {
                    row.Append(
                     ConstructCell("", CellValues.String, 5));
                }
                row.Append(ConstructCell("GRAND TOTAL", CellValues.String, 16));
                row.Append(ConstructCell("", CellValues.String, 16));
                row.Append(ConstructCell("", CellValues.String, 16));
                sheetData.AppendChild(row);
                row = new Row();
                for (int i1 = 1; i1 < 9; i1++)
                {
                    row.Append(
                     ConstructCell("", CellValues.String));
                }
                row.Append(ConstructCell("VAT @ 14.5%", CellValues.String));
                row.Append(ConstructCell("", CellValues.String));
                row.Append(ConstructCell("", CellValues.String));
                sheetData.AppendChild(row);
                row = new Row();
                for (int i1 = 1; i1 < 9; i1++)
                {
                    row.Append(
                     ConstructCell("", CellValues.String));
                }
                row.Append(ConstructCell("ROUND OFF ( +/- )", CellValues.String));
                row.Append(ConstructCell("", CellValues.String));
                row.Append(ConstructCell("", CellValues.String));
                sheetData.AppendChild(row);
                row = new Row();
                for (int i1 = 1; i1 < 9; i1++)
                {
                    row.Append(
                     ConstructCell("", CellValues.String));
                }
                row.Append(ConstructCell("NET TOTAL", CellValues.String));
                row.Append(ConstructCell("", CellValues.String));
                row.Append(ConstructCell("", CellValues.String));
                sheetData.AppendChild(row);
                MergeCells mergeCells = new MergeCells();
                mergeCells.Append(new MergeCell() { Reference = new StringValue("A1:K1") });
                mergeCells.Append(new MergeCell() { Reference = new StringValue("A2:K2") });
                mergeCells.Append(new MergeCell() { Reference = new StringValue("A3:K3") });
                mergeCells.Append(new MergeCell() { Reference = new StringValue("A4:K4") });
                mergeCells.Append(new MergeCell() { Reference = new StringValue("A5:K5") });
                for (int i1 = 13; i1 < 49; i1++)
                {
                    mergeCells.Append(new MergeCell() { Reference = new StringValue("B" + i1 + ":C" + i1 ) });
                    mergeCells.Append(new MergeCell() { Reference = new StringValue("D" + i1 + ":G" + i1) });
                }
                worksheetPart.Worksheet.InsertAfter(mergeCells, worksheetPart.Worksheet.Elements<SheetData>().First());
                var drawingsPart = worksheetPart.AddNewPart<DrawingsPart>();

                if (!worksheetPart.Worksheet.ChildElements.OfType<Drawing>().Any())
                {
                    worksheetPart.Worksheet.Append(new Drawing { Id = worksheetPart.GetIdOfPart(drawingsPart) });
                }

                if (drawingsPart.WorksheetDrawing == null)
                {
                    drawingsPart.WorksheetDrawing = new Xdr.WorksheetDrawing();
                }

                var worksheetDrawing = drawingsPart.WorksheetDrawing;

                var imagePart = drawingsPart.AddImagePart(ImagePartType.Jpeg);
                string imageFileName = Server.MapPath("~/App_Data/favicon.ico");
                using (var stream = new FileStream(imageFileName, FileMode.Open))
                {
                    imagePart.FeedData(stream);
                }

               System.Drawing.Bitmap bm = new System.Drawing.Bitmap(imageFileName);
                DocumentFormat.OpenXml.Drawing.Extents extents = new DocumentFormat.OpenXml.Drawing.Extents();
                var extentsCx = (long)bm.Width * (long)((float)914400 / bm.HorizontalResolution);
                var extentsCy = (long)bm.Height * (long)((float)914400 / bm.VerticalResolution);
                bm.Dispose();

                var colOffset = 0;
                var rowOffset = 0;
                int colNumber = 1;
                int rowNumber = 2;

                var nvps = worksheetDrawing.Descendants<Xdr.NonVisualDrawingProperties>();
                var nvpId = nvps.Count() > 0 ?
                    (UInt32Value)worksheetDrawing.Descendants<Xdr.NonVisualDrawingProperties>().Max(p => p.Id.Value) + 1 :
                    1U;

                var oneCellAnchor = new Xdr.OneCellAnchor(
                    new Xdr.FromMarker
                    {
                        ColumnId = new Xdr.ColumnId((colNumber - 1).ToString()),
                        RowId = new Xdr.RowId((rowNumber - 1).ToString()),
                        ColumnOffset = new Xdr.ColumnOffset(colOffset.ToString()),
                        RowOffset = new Xdr.RowOffset(rowOffset.ToString())
                    },
                    new Xdr.Extent { Cx = extentsCx, Cy = extentsCy },
                    new Xdr.Picture(
                        new Xdr.NonVisualPictureProperties(
                            new Xdr.NonVisualDrawingProperties { Id = nvpId, Name = "Picture " + nvpId, Description = imageFileName },
                            new Xdr.NonVisualPictureDrawingProperties(new DocumentFormat.OpenXml.Drawing.PictureLocks { NoChangeAspect = true })
                        ),
                        new Xdr.BlipFill(
                            new DocumentFormat.OpenXml.Drawing.Blip { Embed = drawingsPart.GetIdOfPart(imagePart), CompressionState = DocumentFormat.OpenXml.Drawing.BlipCompressionValues.Print },
                            new DocumentFormat.OpenXml.Drawing.Stretch(new DocumentFormat.OpenXml.Drawing.FillRectangle())
                        ),
                        new Xdr.ShapeProperties(
                            new DocumentFormat.OpenXml.Drawing.Transform2D(
                                new DocumentFormat.OpenXml.Drawing.Offset { X = 0, Y = 0 },
                                new DocumentFormat.OpenXml.Drawing.Extents { Cx = extentsCx, Cy = extentsCy }
                            ),
                            new DocumentFormat.OpenXml.Drawing.PresetGeometry { Preset = DocumentFormat.OpenXml.Drawing.ShapeTypeValues.Rectangle }
                        )
                    ),
                    new Xdr.ClientData()
                );

                worksheetDrawing.Append(oneCellAnchor);
      
                worksheetPart.Worksheet.Save();
            }
            MS.WriteTo(Response.OutputStream);
            Response.Flush();
            Response.End();
            return Content("OK");
        }
        private Cell ConstructCell(string value, CellValues dataType, uint styleIndex = 0)
        {
            return new Cell()
            {
                CellValue = new CellValue(value),
                DataType = new EnumValue<CellValues>(dataType),
                StyleIndex = styleIndex
            };
        }

        private static Stylesheet CreateStylesheet()
        {
            Stylesheet ss = new Stylesheet();

            Font font0 = new Font();
            FontSize fnt1 = new FontSize() { Val = 10 };// Default font
            font0.Append(fnt1);

            Font font1 = new Font();         // Bold font
            Bold bold = new Bold();
            font1.Append(bold);

            Font font2 = new Font();         // Bold font
            Bold bold1 = new Bold();
            FontSize fnt = new FontSize() { Val = 24 };
            FontName fnme = new FontName() { Val = "Book Antiqua" };
            font2.Append(bold1);
            font2.Append(fnt);
            font2.Append(fnme);
            

            Fonts fonts = new Fonts();      // <APENDING Fonts>
            fonts.Append(font0);
            fonts.Append(font1);
            fonts.Append(font2);

            // <Fills>
            Fill fill0 = new Fill();        // Default fill

            Fills fills = new Fills();      // <APENDING Fills>
            fills.Append(fill0);

            // <Borders>
            Border border0 = new Border();     // Defualt border

            Borders borders = new Borders(new Border(new LeftBorder(), new RightBorder(), new TopBorder(), new BottomBorder(), new DiagonalBorder()),
            new Border(new LeftBorder(new Color() { Auto = true }){ Style = BorderStyleValues.Thin }, new RightBorder(new Color() { Auto = true }){ Style = BorderStyleValues.Thin }, new TopBorder(new Color() { Auto = true }){ Style = BorderStyleValues.None },new BottomBorder(new Color() { Auto = true }){ Style = BorderStyleValues.None },new DiagonalBorder()), //left right 1
            new Border(new LeftBorder(new Color() { Auto = true }){ Style = BorderStyleValues.None },new RightBorder(new Color() { Auto = true }){ Style = BorderStyleValues.None },new TopBorder(new Color() { Auto = true }){ Style = BorderStyleValues.Thin },new BottomBorder(new Color() { Auto = true }){ Style = BorderStyleValues.None },new DiagonalBorder()), //top 2
            new Border(new LeftBorder(new Color() { Auto = true }){ Style = BorderStyleValues.Thin },new RightBorder( new Color() { Auto = true }){ Style = BorderStyleValues.None },new TopBorder(new Color() { Auto = true }){ Style = BorderStyleValues.Thin },new BottomBorder(new Color() { Auto = true }){ Style = BorderStyleValues.None },new DiagonalBorder()), //left top 3
            new Border(new LeftBorder(new Color() { Auto = true }){ Style = BorderStyleValues.None },new RightBorder(new Color() { Auto = true }){ Style = BorderStyleValues.Thin },new TopBorder(new Color() { Auto = true }){ Style = BorderStyleValues.Thin },new BottomBorder(new Color() { Auto = true }){ Style = BorderStyleValues.None },new DiagonalBorder()), //right top 4
            new Border(new LeftBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin }, new RightBorder(new Color() { Auto = true }) { Style = BorderStyleValues.None }, new TopBorder(new Color() { Auto = true }) { Style = BorderStyleValues.None }, new BottomBorder(new Color() { Auto = true }) { Style = BorderStyleValues.None }, new DiagonalBorder()), //left border 5
            new Border(new LeftBorder(new Color() { Auto = true }) { Style = BorderStyleValues.None }, new RightBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin }, new TopBorder(new Color() { Auto = true }) { Style = BorderStyleValues.None }, new BottomBorder(new Color() { Auto = true }) { Style = BorderStyleValues.None }, new DiagonalBorder()), //right border 6
            new Border(new LeftBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin }, new RightBorder(new Color() { Auto = true }) { Style = BorderStyleValues.None }, new TopBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin }, new BottomBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin }, new DiagonalBorder()), //left top bottom 7
            new Border(new LeftBorder(new Color() { Auto = true }) { Style = BorderStyleValues.None }, new RightBorder(new Color() { Auto = true }) { Style = BorderStyleValues.None }, new TopBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin }, new BottomBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin }, new DiagonalBorder()), //bottom top bottom 8
            new Border(new LeftBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin }, new RightBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin }, new TopBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin }, new BottomBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin }, new DiagonalBorder()) //all border 9
            );    // <APENDING Borders>
            borders.Append(border0);

            CellFormat cellformat0 = new CellFormat() { FontId = 0, FillId = 0, BorderId = 0 }; // Default style : Mandatory | Style ID =0

            CellFormat cellformat1 = new CellFormat() { FontId = 1 };
            CellFormat cellformat2 = new CellFormat(new Alignment() { Horizontal = HorizontalAlignmentValues.Center }) { FontId = 0, BorderId= 5 };
            CellFormat cellformat3 = new CellFormat(new Alignment() { Horizontal = HorizontalAlignmentValues.Center }) { FontId = 2, BorderId= 5 };
            CellFormat cellformat4 = new CellFormat(new Alignment()) { BorderId = 1 };
            CellFormat cellformat5 = new CellFormat(new Alignment() { Horizontal = HorizontalAlignmentValues.Center }) { BorderId = 2 };
            CellFormat cellformat6 = new CellFormat(new Alignment() { Horizontal = HorizontalAlignmentValues.Center }) { BorderId = 3 };
            CellFormat cellformat7 = new CellFormat() { BorderId = 4 };
            CellFormat rigtborder = new CellFormat() { BorderId = 6 };
            CellFormat lefttopbroder = new CellFormat() { BorderId = 3 };
            CellFormat leftborder = new CellFormat() { BorderId = 5 };
            CellFormat topborderbld = new CellFormat() {FontId= 1, BorderId = 2 };
            CellFormat LEFTtopbottomborderbld = new CellFormat() { FontId = 1, BorderId = 7 };
            CellFormat RIGHTtopborderbld = new CellFormat() { FontId = 1, BorderId = 4 };
            CellFormat topbottomborder = new CellFormat() { BorderId = 8 };
            CellFormat allborderbld = new CellFormat() {FontId=1, BorderId = 9 };
            CellFormat topborder = new CellFormat() {  BorderId = 2 };
            CellFormats cellformats = new CellFormats();
            cellformats.Append(cellformat0); // 0
            cellformats.Append(cellformat1); // 1
            cellformats.Append(cellformat2); // 2
            cellformats.Append(cellformat3); // 3
            cellformats.Append(cellformat4); // 4
            cellformats.Append(cellformat5); // 5
            cellformats.Append(cellformat6); //6
            cellformats.Append(cellformat7); // 7
            cellformats.Append(rigtborder); // 8
            cellformats.Append(lefttopbroder); // 9
            cellformats.Append(leftborder); // 10
            cellformats.Append(topborderbld); // 11
            cellformats.Append(LEFTtopbottomborderbld); // 12
            cellformats.Append(RIGHTtopborderbld); // 13
            cellformats.Append(topbottomborder); // 14
            cellformats.Append(allborderbld); // 15
            cellformats.Append(topborder); // 16

            ss.Append(fonts);
            ss.Append(fills);
            ss.Append(borders);
            ss.Append(cellformats);


            return ss;
        }

        private Stylesheet GenerateStylesheet()
        {
            Stylesheet styleSheet = null;

            Fonts fonts = new Fonts(
                new Font( // Index 0 - default
                    new FontSize() { Val = 10 }

                ),
                new Font( // Index 1 - header
                    new FontSize() { Val = 10 },
                    new Bold(),
                    new Color() { Rgb = "FFFFFF" }
                    
                ));

            Fills fills = new Fills(
                    new Fill(new PatternFill() { PatternType = PatternValues.None }), // Index 0 - default
                    new Fill(new PatternFill() { PatternType = PatternValues.Gray125 }), // Index 1 - default
                    new Fill(new PatternFill(new ForegroundColor { Rgb = new HexBinaryValue() { Value = "66666666" } })
                    { PatternType = PatternValues.Solid }) // Index 2 - header
                );

            Borders borders = new Borders(
                    new Border(), // index 0 default
                    new Border( // index 1 black border
                        new LeftBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new RightBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new TopBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new BottomBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new DiagonalBorder())
                );

            CellFormats cellFormats = new CellFormats(
                    new CellFormat(), // default
                    new CellFormat { FontId = 0, FillId = 0, BorderId = 1, ApplyBorder = true }, // body
                    new CellFormat { FontId = 1, FillId = 2, BorderId = 1, ApplyFill = true } // header
                );

            styleSheet = new Stylesheet(fonts, fills, borders, cellFormats);

            return styleSheet;
        }



        [Authorize]
        public ActionResult PRINTCHALLAN(string BNO, string BDATE, string CUST, string SNAME, string ADDR, string VNO)
        {
            string message = "";
            try
            {
                System.Web.UI.WebControls.GridView gv = new System.Web.UI.WebControls.GridView();
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
                    string FL = "CHALLAN " + CUST + "- " + SNAME + "- " + Rlst.Value + ".xlsx";
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
                    range = ws.Range("a1", "k5");
                    range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws.Row(6).Height = 3;
                    ws.Cell(7, 1).Value = "CHALLAN NO:-";
                    ws.Cell(7, 10).Value = "DATE:-";
                    ws.Cell(7, 11).Value = BDATE;
                    ws.Range("K7").Style.NumberFormat.SetFormat("dd-MMM-yyyy");
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
                    ws.Range("i13", "i52").Style.NumberFormat.SetFormat("#.00");
                    ws.PageSetup.Margins.Left = 0.35;
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
                    ws.Cell(56, 1).Value = "Signature of Customer";
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
                    string FL = "CHALLAN " + CUST + "- " + SNAME + "- " + Rlst.Value + ".xlsx";
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
