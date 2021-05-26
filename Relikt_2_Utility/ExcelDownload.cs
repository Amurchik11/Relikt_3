using ClosedXML.Excel;
using Relikt_2_Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Relikt_2_Utility
{
    public class OrderExportCommand {
        public IList<Product> ProductList { get; set; }
        public OrderHeader OrderHeader { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public DateTime OrderDate { get; set; }

        public double Discount { get; set; }

        public double Total => ProductList.Sum(s => s.Price * s.TempCount);
    }

    public interface IExcelDownload
    {
        Task<Stream> GenerateAsync(OrderExportCommand ProductUserVM);

    }

    public class ExcelDownload : IExcelDownload
    {
        public Task<Stream> GenerateAsync(OrderExportCommand command)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.AddWorksheet();
            var products = command.ProductList;


            WriteClientInfo(worksheet, command.OrderDate, command.ApplicationUser);

            WriteInvoiceNumberHeader(worksheet, command.OrderHeader);
            WriteInvoiceDateHeader(worksheet, command.OrderDate);


            WriteTableHeader(worksheet);

            
            //цикл
            int startingRow = 11;
            WriteTableData(worksheet, startingRow, command);

            int rowsToSkip = 5;
            
            var lastRowUsed = worksheet.RangeUsed().LastRowUsed();
            var commentRow = lastRowUsed.RowBelow();
            if (!string.IsNullOrWhiteSpace(command.OrderHeader.Comments))
            {
                commentRow.Cell(4).Value = "Коментарии:";
                commentRow.Cell(5).Value = command.OrderHeader.Comments;
            }

            var signatureRow = lastRowUsed;
            for (int i = 0; i < rowsToSkip; i++)
            {
                signatureRow = signatureRow.RowBelow();
            }

            signatureRow.Cell(3).Value = "Виписал(а):";
            signatureRow.Cell(4).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            signatureRow.Cell(5).Style.Border.BottomBorder = XLBorderStyleValues.Thin;

            signatureRow.Cell(7).Value = "Подпись:";
            signatureRow.Cell(8).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            signatureRow.Cell(9).Style.Border.BottomBorder = XLBorderStyleValues.Thin;

            foreach (var xlColumn in worksheet.ColumnsUsed())
            {
                xlColumn.AdjustToContents();
            }

            MemoryStream memoryStream = new MemoryStream();
            workbook.SaveAs(memoryStream);

            // ensure that stream is at the beginning of the file
            memoryStream.Seek(0, SeekOrigin.Begin);

            return Task.FromResult<Stream>(memoryStream);
        }

        private static void WriteClientInfo(IXLWorksheet worksheet, DateTime currentDate, ApplicationUser user)
        {
            worksheet.Cell(1, 2).SetValue("Поставщик");
            worksheet.Cell(1, 2).Style.Font.Underline = XLFontUnderlineValues.Single;
            worksheet.Cell(1, 2).Style.Font.Bold = true;

            worksheet.Cell(1, 4).SetValue("ТОВ \"Реликт Арте\"");

            worksheet.Cell(4, 2).SetValue("Получатель");
            worksheet.Cell(4, 2).Style.Font.Underline = XLFontUnderlineValues.Single;
            worksheet.Cell(4, 2).Style.Font.Bold = true;

            worksheet.Cell(4, 4).SetValue($"Клиент:  {user.FullName}");
            worksheet.Cell(5, 4).SetValue($"Телефон:  {user.PhoneNumber}");

        }

        private static void WriteInvoiceNumberHeader(IXLWorksheet worksheet, OrderHeader orderHeader)
        {
            var orderNumberHeader = worksheet.Range(worksheet.Cell(7, 1).Address, worksheet.Cell(7, 10).Address);
            orderNumberHeader.Cell(1, 1).Value = $"Счет №  {orderHeader.Id}";
            orderNumberHeader.Cell(1, 1).Style.Font.Bold = true;
            orderNumberHeader.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            orderNumberHeader.Merge();
        }

        private static void WriteInvoiceDateHeader(IXLWorksheet worksheet, DateTime currentDate)
        {
            var orderDate = worksheet.Range(worksheet.Cell(8, 1).Address, worksheet.Cell(8, 10).Address);
            orderDate.Cell(1, 1).Value = $"от {currentDate:dd.MM.yyyy  HH:mm}";
            orderDate.Cell(1, 1).Style.Font.Bold = true;
            orderDate.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            orderDate.Merge();
        }

        private static void WriteTableHeader(IXLWorksheet worksheet)
        {
            var tableHeader = worksheet.Range(worksheet.Cell(10, 1).Address, worksheet.Cell(10, 10).Address);
            tableHeader.Style.Font.Bold = true;
            tableHeader.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            tableHeader.Style.Fill.BackgroundColor = XLColor.LightGray;
            tableHeader.Style.Border.BottomBorder = XLBorderStyleValues.Medium;
            tableHeader.Style.Border.TopBorder = XLBorderStyleValues.Medium;
            tableHeader.Style.Border.LeftBorder = XLBorderStyleValues.Medium;
            tableHeader.Style.Border.RightBorder = XLBorderStyleValues.Medium;


            tableHeader.Cell(1, 1).Value = "№";
            tableHeader.Cell(1, 2).Value = "Название";
            tableHeader.Cell(1, 3).Value = "Цвет";
            tableHeader.Cell(1, 4).Value = "Тип Товара";
            tableHeader.Cell(1, 5).Value = "Размер";
            tableHeader.Cell(1, 6).Value = "Открывание";
            tableHeader.Cell(1, 7).Value = "Цвет стекла";
            tableHeader.Cell(1, 8).Value = "Кол-во, шт.";
            tableHeader.Cell(1, 9).Value = "Цена, шт. грн.";
            tableHeader.Cell(1, 10).Value = "Сумма, грн.";
            
        }

        private void WriteTableData(IXLWorksheet worksheet, int startingRow, OrderExportCommand command)
        {
            int currentRow = startingRow;
            int rowNumber = 1;
            foreach (var product in command.ProductList)
            {
                var range = worksheet.Range(worksheet.Cell(currentRow, 1).Address, worksheet.Cell(currentRow, 10).Address);
                range.Cell(1, 1).Value = rowNumber.ToString();
                range.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                range.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                range.Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                range.Style.Border.RightBorder = XLBorderStyleValues.Medium;
                range.Style.Border.InsideBorder = XLBorderStyleValues.Medium;

                range.Cell(1, 2).Value = product.Name;
                range.Cell(1, 3).Value = product.Category?.Name ?? " - ";
                range.Cell(1, 4).Value = product.ApplicationType?.Name ?? " - ";
                range.Cell(1, 5).Value = product.SizeType?.Name ?? " - ";
                range.Cell(1, 6).Value = product.RightLeft?.Name ?? " - ";
                range.Cell(1, 7).Value = product.ColourOfGlass?.Name ?? " - ";
                range.Cell(1, 8).Value = product.TempCount;
                range.Cell(1, 9).Value = product.Price;
                range.Cell(1, 10).Value = product.TempCount * product.Price;

                currentRow++;
                rowNumber++;
            }
            worksheet.LastRowUsed().CellsUsed().Style.Border.BottomBorder = XLBorderStyleValues.Medium;

            var emptyTextCell = worksheet.Cell(currentRow, 9);
            var emptyValueCell = worksheet.Cell(currentRow, 10);
            BorderedCell(emptyTextCell, XLBorderStyleValues.Thin);
            BorderedCell(emptyValueCell, XLBorderStyleValues.Thin);

            var totalTextCell = worksheet.Cell(currentRow + 1, 9);
            var totalValueCell = worksheet.Cell(currentRow + 1, 10);
            totalTextCell.Value = "Итого:";
            totalTextCell.Style.Font.Bold = true;
            var total = command.Total;
            totalValueCell.Value = total;
            BorderedCell(totalTextCell, XLBorderStyleValues.Medium);
            BorderedCell(totalValueCell, XLBorderStyleValues.Medium);

            var discountTextCell = worksheet.Cell(currentRow + 2, 9);
            var discountValueCell = worksheet.Cell(currentRow + 2, 10);

            discountTextCell.Value = "Скидка составила:";
            discountTextCell.Style.Font.Bold = true;
            //discountValueCell.Value = $"{total * coupon.Discount / 100}";
            var discount = command.Discount;
            discountValueCell.Value = discount;
            BorderedCell(discountTextCell, XLBorderStyleValues.Medium);
            BorderedCell(discountValueCell, XLBorderStyleValues.Medium);

            var netTotalTextCell = worksheet.Cell(currentRow + 3, 9);
            var netTotalValueCell = worksheet.Cell(currentRow + 3, 10);
            netTotalTextCell.Value = "Итого к оплате:";
            netTotalTextCell.Style.Font.Bold = true;
            netTotalValueCell.Value = total - discount;
            BorderedCell(netTotalTextCell, XLBorderStyleValues.Medium);
            BorderedCell(netTotalValueCell, XLBorderStyleValues.Medium);

            
        }

        private void BorderedCell(IXLCell cell, XLBorderStyleValues border)
        {
            cell.Style.Border.BottomBorder = border;
            cell.Style.Border.TopBorder = border;
            cell.Style.Border.LeftBorder = border;
            cell.Style.Border.RightBorder = border;



        }
    }

}
