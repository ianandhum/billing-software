using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Provider.Data;

namespace Kait.ViewModel
{
    public class GSTReportViewModel
    {
        public GSTReportViewModel()
        {
            App.disableLogOutToFile();
            TestOpenXml();
        }
        private FileInfo TmpFileInfo;
        private static decimal[] ColTotals;

        private void TestOpenXml()
        {
            TmpFileInfo = null;
            try
            {
                TmpFileInfo = new FileInfo("C:\\Users\\Anoo PR\\Documents\\getch.xlsx");
                Console.WriteLine(CreateSalesReport(TmpFileInfo));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        public static string CreateSalesReport(FileInfo file)
        {
            using (ExcelPackage xlPackage = new ExcelPackage(file))
            {

                ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.Add("Sales Report");
                if (worksheet != null)
                {

                    worksheet.DefaultColWidth = 10;
                    worksheet.Cells["A1"].Value = "Sales Report of " + DateTime.Today.ToString("dd-MM-yyyy") + " to " + DateTime.Today.ToString("dd-MM-yyyy");
                    int rowCount=GSTReportViewModel.AddHeader(worksheet,2);
                    int nextRow=GSTReportViewModel.AddInvoices(worksheet, 3,DateTime.Parse("2018-06-20"),DateTime.Today,rowCount);
                    GSTReportViewModel.AddTotalRow(worksheet, nextRow);

                }
                
                xlPackage.Workbook.Properties.Title = "Sales Report for Kait Electricals";
                xlPackage.Workbook.Properties.Author = "Kait Electrical";
                xlPackage.Workbook.Properties.Company = "Kait Elecctrical";
                xlPackage.Save();
            }

            return file.FullName;
        }

        private static int AddHeader(ExcelWorksheet worksheet,int row)
        {
            string[] Columns = new string[]
            {
                "",
                "Date",
                "Bill No",
                "Particulars",
                "GSTIN",
                "GST 0%",
                "GST 0%-Tax",
                "GST 5%",
                "GST 5%-Tax",
                "GST 12%",
                "GST 12%-Tax",
                "GST 18%",
                "GST 18%-Tax",
                "GST 24%",
                "GST 24%-Tax",
                "CESS SP",
                "CESS",
                "GST Total",
                "Roff",
                "Other Exp",
                "Bill Amount"
            };

            using (ExcelRange r = worksheet.Cells[1,1,1,Columns.Length])
            {
                r.Merge = true;
                r.Style.Font.SetFromFont(new Font("Sans Serif", 15, FontStyle.Bold));
                r.Style.Font.Color.SetColor(Color.Black);
                r.Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
            }

            for (int col = 0; col < Columns.Length; col++)
            {
                worksheet.Cells[row, col+1].Value = Columns[col];
            }
            Console.WriteLine("AddHeader-Count Colummns:{0}", Columns.Length);
            return Columns.Length;
        }

        private static int  AddInvoices(ExcelWorksheet worksheet,int startRow,DateTime startDate,DateTime endDate,int rowLength=0)
        {
            int row = startRow;

            // +1 is added because of indexoutofrange exception
            // BUG: proper debug iis required
            ColTotals = new decimal[rowLength + 1];
            foreach (var invoice in App.DataProvider.Invoices.ToList())
            {

                if (invoice.IssueDate >= startDate && invoice.IssueDate <= endDate)
                {
                    int col = 1;
                    worksheet.Cells[row, col++].Value = "TAX INVOICE";
                    worksheet.Cells[row, col++].Value = invoice.IssueDate.ToString("dd.MM.yyyy");
                    worksheet.Cells[row, col++].Value = invoice.InvoiceId;
                    worksheet.Cells[row, col++].Value = "CASH";
                    worksheet.Cells[row, col++].Value = invoice.Client.GSTIN;

                    ColTotals[col]=GetTotalAmountInGSTCategory(invoice, 0);
                    worksheet.Cells[row, col++].Value = GetTotalAmountInGSTCategory(invoice, 0);

                    ColTotals[col] += GetTotalAmountInGSTCategory(invoice, 0,false);
                    worksheet.Cells[row, col++].Value = GetTotalAmountInGSTCategory(invoice, 0, false);

                    ColTotals[col] += GetTotalAmountInGSTCategory(invoice, 5);
                    worksheet.Cells[row, col++].Value = GetTotalAmountInGSTCategory(invoice, 5);

                    ColTotals[col] += GetTotalAmountInGSTCategory(invoice, 5,false);
                    worksheet.Cells[row, col++].Value = GetTotalAmountInGSTCategory(invoice, 5, false);

                    ColTotals[col] += GetTotalAmountInGSTCategory(invoice, 12);
                    worksheet.Cells[row, col++].Value = GetTotalAmountInGSTCategory(invoice, 12);

                    ColTotals[col] += GetTotalAmountInGSTCategory(invoice, 12,false);
                    worksheet.Cells[row, col++].Value = GetTotalAmountInGSTCategory(invoice, 12, false);

                    ColTotals[col] += GetTotalAmountInGSTCategory(invoice, 18);
                    worksheet.Cells[row, col++].Value = GetTotalAmountInGSTCategory(invoice, 18);

                    ColTotals[col] += GetTotalAmountInGSTCategory(invoice, 18,false);
                    worksheet.Cells[row, col++].Value = GetTotalAmountInGSTCategory(invoice, 18, false);

                    ColTotals[col] += GetTotalAmountInGSTCategory(invoice, 24);
                    worksheet.Cells[row, col++].Value = GetTotalAmountInGSTCategory(invoice, 24);

                    ColTotals[col] += GetTotalAmountInGSTCategory(invoice, 24,false);
                    worksheet.Cells[row, col++].Value = GetTotalAmountInGSTCategory(invoice, 24, false);

                    worksheet.Cells[row, col++].Value = 0;
                    worksheet.Cells[row, col++].Value = 0;

                    ColTotals[col] += invoice.TotalTax;
                    worksheet.Cells[row, col++].Value = invoice.TotalTax;

                    worksheet.Cells[row, col++].Value = 0;

                    ColTotals[col] += invoice.ShippingCharge;
                    worksheet.Cells[row, col++].Value = invoice.ShippingCharge;

                    ColTotals[col] += invoice.Total;
                    worksheet.Cells[row, col++].Value = invoice.Total;
                    row++;
                }
            }
            return row;
        }

        private static void AddTotalRow(ExcelWorksheet worksheet, int nextRow)
        {
            for (int i = 1; i < ColTotals.Length; i++)
            {
                worksheet.Cells[nextRow, i].Value = ColTotals[i];
                worksheet.Cells[nextRow, i].Style.Font.Bold = true;
            }
            worksheet.Cells[nextRow, 2].Value = "Total";
        }

        private static decimal GetTotalAmountInGSTCategory( Invoice invoice, int taxPercent,bool totalTaxOrTaxableAmt = true)
        {
            decimal taxAmt = 0;
            foreach (var product in invoice.Products)
            {
                if(product.Tax.Rate == taxPercent)
                    taxAmt += (totalTaxOrTaxableAmt)?product.TotalNoTax : product.TotalTax;
            }
            return Decimal.Round(taxAmt , 2);
        }

    }
}
