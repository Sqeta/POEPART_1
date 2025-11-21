using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using POEPART_1.Models;
using System;
using System.Globalization;

namespace POEPART_1.Documents
{
    public class InvoiceDocument : IDocument
    {
        public Invoice InvoiceData { get; }

        private readonly CultureInfo saCurrency = new CultureInfo("en-ZA"); // South African Rands

        public InvoiceDocument(Invoice invoice)
        {
            InvoiceData = invoice;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Margin(30);
                page.Size(PageSizes.A4);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(12));

                page.Header()
                    .Text($"Invoice - {InvoiceData.LecturerName}")
                    .SemiBold().FontSize(18).FontColor(Colors.Blue.Medium);

                page.Content().PaddingVertical(10).Column(column =>
                {
                    column.Item().Row(row =>
                    {
                        row.RelativeItem().Column(c =>
                        {
                            c.Item().Text($"Lecturer: {InvoiceData.LecturerName}").Bold();
                            c.Item().Text($"Period: {InvoiceData.MonthYear}");
                        });

                        row.ConstantItem(200).Column(c =>
                        {
                            c.Item().Text($"Invoice date: {DateTime.Now:dd MMM yyyy}");
                            c.Item().Text($"Total: {InvoiceData.TotalAmount.ToString("C", saCurrency)}").SemiBold();
                        });
                    });

                    column.Item().PaddingTop(10).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(60);   // Claim ID
                            columns.RelativeColumn();    // Module
                            columns.ConstantColumn(60);  // Sessions
                            columns.ConstantColumn(90);  // Hourly
                            columns.ConstantColumn(90);  // Total
                        });

                        // header
                        table.Header(header =>
                        {
                            header.Cell().Element(CellStyle).Text("ID").SemiBold();
                            header.Cell().Element(CellStyle).Text("Module").SemiBold();
                            header.Cell().Element(CellStyle).AlignCenter().Text("Sess").SemiBold();
                            header.Cell().Element(CellStyle).AlignRight().Text("Hourly").SemiBold();
                            header.Cell().Element(CellStyle).AlignRight().Text("Amount").SemiBold();
                        });

                        // rows
                        foreach (var claim in InvoiceData.Claims)
                        {
                            table.Cell().Element(CellStyle).Text(claim.Claim_Id.ToString());
                            table.Cell().Element(CellStyle).Text(claim.Module_Name);
                            table.Cell().Element(CellStyle).AlignCenter().Text(claim.Number_ofSessions.ToString());
                            table.Cell().Element(CellStyle).AlignRight().Text(claim.Hourly_rate.ToString("C", saCurrency));
                            table.Cell().Element(CellStyle).AlignRight().Text(claim.Total_Amount.ToString("C", saCurrency));
                        }

                        // footer (total)
                        table.Footer(footer =>
                        {
                            footer.Cell().ColumnSpan(4).AlignRight().Text("Grand Total").SemiBold();
                            footer.Cell().AlignRight().Text(InvoiceData.TotalAmount.ToString("C", saCurrency)).SemiBold();
                        });

                        static IContainer CellStyle(IContainer c)
                            => c.Padding(5).BorderBottom(1).BorderColor(Colors.Grey.Lighten3);
                    });
                });

                page.Footer()
                    .AlignCenter()
                    .Text(text =>
                    {
                        text.Span("Generated: ").SemiBold();
                        text.Span(DateTime.Now.ToString("dd MMM yyyy"));
                    });
            });
        }
    }
}
