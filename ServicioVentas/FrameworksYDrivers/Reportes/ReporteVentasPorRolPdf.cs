using System.Linq;
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
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(50);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(12).FontColor(Colors.Black));

                    page.Header().Row(header =>
                    {
                        header.ConstantItem(60).Height(60).AlignCenter().AlignMiddle().Padding(5).Border(1).BorderColor(Colors.Grey.Lighten3).Background(Colors.Grey.Lighten4)
                            .Text("A").FontSize(28).SemiBold();

                        header.ConstantItem(20);

                        header.RelativeItem().Column(column =>
                        {
                            column.Item().AlignCenter().Text("REPORTE DE VENTAS POR USUARIO").FontSize(20).SemiBold();
                            column.Item().PaddingTop(5).AlignCenter().Text($"Desde: {fechaInicio:dd/MM/yyyy} al {fechaFin:dd/MM/yyyy}").FontSize(11).FontColor(Colors.Grey.Darken2);
                        });
                    });

                    page.Content().Column(x =>
                    {
                        x.Spacing(18);
                        x.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
                        x.Item().PaddingTop(8).Row(row =>
                        {
                            row.RelativeItem().Text("VitalCareSBA - Farmacia").FontSize(10).FontColor(Colors.Grey.Darken2);
                            row.RelativeItem().AlignRight().Text(DateTime.Now.ToString("dd/MM/yyyy HH:mm")).FontSize(10).FontColor(Colors.Grey.Darken2);
                        });

                        if (!datos.Any())
                        {
                            x.Item().PaddingVertical(40).AlignCenter().Text("No hay ventas registradas para el mes actual.").FontSize(14).FontColor(Colors.Grey.Darken2);
                        }
                        else
                        {
                            x.Item().Text("Detalle de ventas por rol").FontSize(14).SemiBold();
                            x.Item().PaddingTop(10).Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(3);
                                    columns.RelativeColumn(2);
                                    columns.RelativeColumn(2);
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Element(CellHeader).Text("Rol del Usuario").FontSize(11).SemiBold();
                                    header.Cell().Element(CellHeader).Text("Cantidad Ventas").FontSize(11).SemiBold();
                                    header.Cell().Element(CellHeader).Text("Total Recaudado Bs.").FontSize(11).SemiBold();
                                });

                                foreach (var item in datos)
                                {
                                    table.Cell().Element(CellBody).Text(item.Rol).FontSize(10);
                                    table.Cell().Element(CellBody).Text(item.CantidadVentas.ToString()).FontSize(10);
                                    table.Cell().Element(CellBody).Text(item.TotalRecaudado.ToString("N2")).FontSize(10);
                                }

                                static IContainer CellHeader(IContainer container)
                                {
                                    return container.BorderBottom(1).BorderColor(Colors.Grey.Darken2).Padding(5).AlignCenter();
                                }

                                static IContainer CellBody(IContainer container)
                                {
                                    return container.Padding(5);
                                }
                            });

                            var totalVentas = datos.Sum(d => d.CantidadVentas);
                            var totalRecaudado = datos.Sum(d => d.TotalRecaudado);

                            x.Item().PaddingTop(22).Row(row =>
                            {
                                row.RelativeItem().Column(col =>
                                {
                                    col.Spacing(6);
                                    col.Item().Text($"Total de ventas: {totalVentas}").FontSize(12).SemiBold();
                                    col.Item().Text($"Total recaudado Bs.: {totalRecaudado:N2}").FontSize(12).SemiBold();
                                });
                            });

                            x.Item().PaddingTop(24).Column(chart =>
                            {
                                chart.Item().Text("Gráfico de barras por rol").FontSize(13).SemiBold();
                                chart.Item().PaddingTop(8).Column(barChart =>
                                {
                                    var maxTotal = datos.Max(d => d.TotalRecaudado);
                                    foreach (var item in datos)
                                    {
                                        barChart.Item().PaddingVertical(6).Row(barRow =>
                                        {
                                            barRow.ConstantItem(110).Text(item.Rol).FontSize(10).FontColor(Colors.Grey.Darken2);
                                            barRow.RelativeItem().Height(18).Padding(2).Background(Colors.Grey.Lighten3).Row(inner =>
                                            {
                                                var barWidth = maxTotal > 0 ? item.TotalRecaudado / maxTotal : 0;
                                                inner.ConstantItem((int)(barWidth * 250)).Background(Colors.Blue.Medium);
                                                inner.RelativeItem().AlignMiddle().Text(item.TotalRecaudado.ToString("N2")).FontSize(9).FontColor(Colors.Grey.Darken2);
                                            });
                                        });
                                    }
                                });
                            });
                        }
                    });

                    page.Footer().AlignCenter().Text(text =>
                    {
                        text.Span("VitalCareSBA - Página ").FontSize(10);
                        text.CurrentPageNumber().FontSize(10);
                        text.Span(" de ").FontSize(10);
                        text.TotalPages().FontSize(10);
                    });
                });
            }).GeneratePdf();
        }
    }
}