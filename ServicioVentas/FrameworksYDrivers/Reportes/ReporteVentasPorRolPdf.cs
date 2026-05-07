using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using VitalCareSBA.ServicioVentas.Entidades.DTOs;

namespace VitalCareSBA.ServicioVentas.FrameworksYDrivers.Reportes
{
    public static class ReporteVentasPorRolPdf
    {
        public static byte[] Generar(DateTime fechaInicio, DateTime fechaFin, IEnumerable<ReporteVentasPorRolDto> datos)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            List<ReporteVentasPorRolDto> detalle = datos.ToList();

            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(40);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(11).FontColor(Colors.Black));

                    page.Header().Column(header =>
                    {
                        header.Item().AlignCenter().Text("REPORTE DE VENTAS POR ROL").FontSize(20).SemiBold();
                        header.Item().AlignCenter().Text($"Desde: {fechaInicio:dd/MM/yyyy} al {fechaFin:dd/MM/yyyy}")
                            .FontSize(11)
                            .FontColor(Colors.Grey.Darken2);
                    });

                    page.Content().PaddingTop(20).Column(content =>
                    {
                        content.Spacing(16);

                        if (!detalle.Any())
                        {
                            content.Item()
                                .PaddingVertical(40)
                                .AlignCenter()
                                .Text("No hay ventas registradas para el periodo actual.")
                                .FontSize(13)
                                .FontColor(Colors.Grey.Darken2);
                            return;
                        }

                        content.Item().Text("Detalle de ventas por rol").FontSize(13).SemiBold();

                        content.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(3);
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(2);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Element(CellHeader).Text("Rol");
                                header.Cell().Element(CellHeader).Text("Cantidad de Ventas");
                                header.Cell().Element(CellHeader).Text("Total Recaudado (Bs.)");
                            });

                            foreach (ReporteVentasPorRolDto item in detalle)
                            {
                                table.Cell().Element(CellBody).Text(item.Rol);
                                table.Cell().Element(CellBody).AlignCenter().Text(item.CantidadVentas.ToString());
                                table.Cell().Element(CellBody).AlignRight().Text(item.TotalRecaudado.ToString("N2"));
                            }
                        });

                        int totalVentas = detalle.Sum(x => x.CantidadVentas);
                        decimal totalRecaudado = detalle.Sum(x => x.TotalRecaudado);

                        content.Item().PaddingTop(8).AlignRight().Column(totales =>
                        {
                            totales.Item().Text($"Total de ventas: {totalVentas}").SemiBold();
                            totales.Item().Text($"Total recaudado: Bs. {totalRecaudado:N2}").SemiBold();
                        });
                    });

                    page.Footer().AlignCenter().Text(text =>
                    {
                        text.Span("VitalCare - Página ").FontSize(10);
                        text.CurrentPageNumber().FontSize(10);
                        text.Span(" de ").FontSize(10);
                        text.TotalPages().FontSize(10);
                    });
                });
            }).GeneratePdf();
        }

        private static IContainer CellHeader(IContainer container)
        {
            return container
                .Background(Colors.Green.Lighten4)
                .BorderBottom(1)
                .BorderColor(Colors.Green.Darken2)
                .Padding(6)
                .AlignCenter()
                .DefaultTextStyle(x => x.SemiBold());
        }

        private static IContainer CellBody(IContainer container)
        {
            return container
                .BorderBottom(1)
                .BorderColor(Colors.Grey.Lighten2)
                .Padding(6);
        }
    }
}
