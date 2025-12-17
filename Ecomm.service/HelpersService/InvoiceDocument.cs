using Ecomm.core.Entities.OrderEntities;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Ecomm.service.HelpersService
{
    public class InvoiceDocument : IDocument
    {
        private readonly Order _order;

        public InvoiceDocument(Order order)
        {
            _order = order;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(1, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(10).FontFamily(Fonts.Verdana));

                page.Header().Element(ComposeHeader);
                page.Content().Element(ComposeContent);
                page.Footer().Element(ComposeFooter);
            });
        }

        private void ComposeHeader(IContainer container)
        {
            var titleStyle = TextStyle.Default.FontSize(24).SemiBold().FontColor(Colors.Blue.Medium);

            container.Row(row =>
            {
                row.RelativeItem().Column(column =>
                {
                    column.Item().Text("Ventro Invoice").Style(titleStyle);

                    column.Item().Text(text =>
                    {
                        text.Span("Order ID: ").SemiBold();
                        text.Span($"#{_order.Id}");
                    });

                    column.Item().Text(text =>
                    {
                        text.Span("Date: ").SemiBold();
                        text.Span($"{_order.OrderDate:MMMM dd, yyyy}");
                    });
                });

                // تم إزالة مساحة الصورة من هنا ليكون النص ممتداً أو جهة اليسار فقط
            });
        }

        private void ComposeContent(IContainer container)
        {
            container.PaddingVertical(40).Column(column =>
            {
                column.Spacing(20);

                column.Item().Element(ComposeCustomerInfo);
                column.Item().Element(ComposeItemsTable);
                column.Item().Element(ComposeTotals);
            });
        }

        private void ComposeCustomerInfo(IContainer container)
        {
            container.Row(row =>
            {
                row.RelativeItem().Column(col =>
                {
                    col.Item().Text("Shipping Address").FontSize(12).SemiBold();
                    col.Item().PaddingTop(5).Column(innerCol =>
                    {
                        innerCol.Item().Text(_order.shippingAddress.FullName);
                        innerCol.Item().Text(_order.shippingAddress.Street);
                        innerCol.Item().Text($"{_order.shippingAddress.City}, {_order.shippingAddress.PostalCode}");
                        innerCol.Item().Text(_order.shippingAddress.Country);
                    });
                });

                row.RelativeItem().Column(col =>
                {
                    col.Item().Text("Delivery Method").FontSize(12).SemiBold();
                    var deliveryName = _order.DeliveryMethod?.Name ?? "Standard Shipping";
                    col.Item().PaddingTop(5).Text(deliveryName);
                });
            });
        }

        private void ComposeItemsTable(IContainer container)
        {
            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(25);   // #
                    columns.RelativeColumn(3);    // Product Name
                    columns.RelativeColumn();     // Unit Price
                    columns.RelativeColumn();     // Quantity
                    columns.RelativeColumn();     // Total
                });

                table.Header(header =>
                {
                    header.Cell().Element(CellStyle).Text("#");
                    header.Cell().Element(CellStyle).Text("Product");
                    header.Cell().Element(CellStyle).AlignRight().Text("Unit Price");
                    header.Cell().Element(CellStyle).AlignCenter().Text("Qty");
                    header.Cell().Element(CellStyle).AlignRight().Text("Total");

                    static IContainer CellStyle(IContainer container)
                    {
                        return container.DefaultTextStyle(x => x.SemiBold())
                                        .PaddingVertical(5)
                                        .BorderBottom(1)
                                        .BorderColor(Colors.Black);
                    }
                });

                int index = 1;
                foreach (var item in _order.OrderItems)
                {
                    table.Cell().Element(ValueCellStyle).Text(index.ToString());
                    table.Cell().Element(ValueCellStyle).Text(item.ProductName);
                    table.Cell().Element(ValueCellStyle).AlignRight().Text($"${item.Price:F2}");
                    table.Cell().Element(ValueCellStyle).AlignCenter().Text(item.Quantity.ToString());
                    table.Cell().Element(ValueCellStyle).AlignRight().Text($"${(item.Price * item.Quantity):F2}");

                    index++;

                    static IContainer ValueCellStyle(IContainer container)
                    {
                        return container.PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Grey.Lighten2);
                    }
                }
            });
        }

        private void ComposeTotals(IContainer container)
        {
            container.AlignRight().Column(column =>
            {
                column.Spacing(5);

                column.Item().Text(text =>
                {
                    text.Span("Subtotal: ").FontSize(12);
                    text.Span($"${_order.Subtotal:F2}").FontSize(12);
                });

                column.Item().Text(text =>
                {
                    text.Span("Total: ").FontSize(16).SemiBold();
                    text.Span($"${_order.Total():F2}").FontSize(16).SemiBold().FontColor(Colors.Blue.Medium);
                });

                column.Item().PaddingTop(20).Text("Thank you for shopping with Ventro!")
                      .FontSize(12).Italic().FontColor(Colors.Grey.Medium);
            });
        }

        private void ComposeFooter(IContainer container)
        {
            container.AlignCenter().Text(text =>
            {
                text.Span("Page ");
                text.CurrentPageNumber();
                text.Span(" / ");
                text.TotalPages();
            });
        }
    }
}