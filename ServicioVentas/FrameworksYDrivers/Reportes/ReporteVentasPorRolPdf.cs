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
            byte[]? logoBytes = ResolverLogoVitalCare();
            int totalVentas = detalle.Sum(x => x.CantidadVentas);
            decimal totalRecaudado = detalle.Sum(x => x.TotalRecaudado);
            ReporteVentasPorRolDto? rolConMasVentas = detalle
                .OrderByDescending(x => x.CantidadVentas)
                .ThenByDescending(x => x.TotalRecaudado)
                .FirstOrDefault();
            ReporteVentasPorRolDto? rolConMasRecaudacion = detalle
                .OrderByDescending(x => x.TotalRecaudado)
                .ThenByDescending(x => x.CantidadVentas)
                .FirstOrDefault();
            decimal maximoRecaudado = detalle.Any() ? detalle.Max(x => x.TotalRecaudado) : 0m;

            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(36);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(11).FontColor(Colors.Black));

                    page.Header().Row(header =>
                    {
                        header.ConstantItem(76).Height(76).Element(c =>
                        {
                            if (logoBytes != null)
                            {
                                c.Image(logoBytes);
                            }
                            else
                            {
                                c.Border(1)
                                    .BorderColor(Colors.Grey.Lighten2)
                                    .AlignCenter()
                                    .AlignMiddle()
                                    .Text("LOGO")
                                    .SemiBold();
                            }
                        });

                        header.RelativeItem().PaddingLeft(12).Column(column =>
                        {
                            column.Spacing(3);
                            column.Item().Text("Farmacia VitalCare")
                                .FontSize(16)
                                .SemiBold()
                                .FontColor(Colors.Green.Darken2);
                            column.Item().Text("REPORTE DE VENTAS POR ROL")
                                .FontSize(21)
                                .SemiBold();
                            column.Item().Text($"Desde: {fechaInicio:dd/MM/yyyy} al {fechaFin:dd/MM/yyyy}")
                                .FontSize(11)
                                .FontColor(Colors.Grey.Darken2);
                            column.Item().Text($"Generado: {DateTime.Now:dd/MM/yyyy HH:mm}")
                                .FontSize(10)
                                .FontColor(Colors.Grey.Darken1);
                        });
                    });

                    page.Content().PaddingTop(20).Column(content =>
                    {
                        content.Spacing(18);

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

                        content.Item().Text("Resumen ejecutivo").FontSize(13).SemiBold();

                        content.Item().Row(row =>
                        {
                            row.RelativeItem().Element(card =>
                                CardInfo(card, "Total de ventas", totalVentas.ToString(), "Ventas activas del periodo"));
                            row.ConstantItem(10);
                            row.RelativeItem().Element(card =>
                                CardInfo(card, "Total recaudado", $"Bs. {totalRecaudado:N2}", "Suma de ventas activas"));
                            row.ConstantItem(10);
                            row.RelativeItem().Element(card =>
                                CardInfo(card,
                                    "Rol destacado",
                                    rolConMasRecaudacion?.Rol ?? "Sin datos",
                                    rolConMasRecaudacion == null
                                        ? "No hay información disponible"
                                        : $"Mayor recaudación: Bs. {rolConMasRecaudacion.TotalRecaudado:N2}"));
                        });

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

                        content.Item().Text("Gráfico estadístico: recaudación por rol").FontSize(13).SemiBold();

                        content.Item()
                            .Border(1)
                            .BorderColor(Colors.Grey.Lighten2)
                            .Padding(12)
                            .Column(chart =>
                            {
                                chart.Spacing(10);

                                foreach (ReporteVentasPorRolDto item in detalle.OrderByDescending(x => x.TotalRecaudado))
                                {
                                    decimal porcentaje = maximoRecaudado <= 0
                                        ? 0
                                        : Math.Round((item.TotalRecaudado / maximoRecaudado) * 100m, 2);

                                    chart.Item().Column(bar =>
                                    {
                                        bar.Spacing(4);
                                        bar.Item().Row(info =>
                                        {
                                            info.RelativeItem().Text(item.Rol).SemiBold();
                                            info.ConstantItem(110).AlignRight().Text($"{item.CantidadVentas} venta(s)");
                                            info.ConstantItem(115).AlignRight().Text($"Bs. {item.TotalRecaudado:N2}").SemiBold();
                                        });

                                        bar.Item().Height(18).Row(progress =>
                                        {
                                            progress.RelativeItem().Background(Colors.Grey.Lighten3).Row(inner =>
                                            {
                                                inner.ConstantItem(2.4f * (float)porcentaje).Background(Colors.Green.Medium);
                                                inner.RelativeItem();
                                            });
                                        });
                                    });
                                }
                            });

                        content.Item().AlignRight().Column(totales =>
                        {
                            totales.Item().Text($"Total de ventas: {totalVentas}").SemiBold();
                            totales.Item().Text($"Total recaudado: Bs. {totalRecaudado:N2}").SemiBold();

                            if (rolConMasVentas != null)
                            {
                                totales.Item()
                                    .Text($"Rol con más ventas: {rolConMasVentas.Rol}")
                                    .FontColor(Colors.Grey.Darken2);
                            }
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

        private static byte[]? ResolverLogoVitalCare()
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string baseDirectory = AppContext.BaseDirectory;

            string[] candidatos =
            {
                Path.Combine(currentDirectory, "wwwroot", "images", "vitalcare.png"),
                Path.Combine(currentDirectory, "FrontendVitalCare", "wwwroot", "images", "vitalcare.png"),
                Path.Combine(currentDirectory, "..", "FrontendVitalCare", "wwwroot", "images", "vitalcare.png"),
                Path.Combine(baseDirectory, "wwwroot", "images", "vitalcare.png"),
                Path.Combine(baseDirectory, "..", "..", "..", "..", "FrontendVitalCare", "wwwroot", "images", "vitalcare.png")
            };

            foreach (string ruta in candidatos)
            {
                string rutaCompleta = Path.GetFullPath(ruta);
                if (File.Exists(rutaCompleta))
                    return File.ReadAllBytes(rutaCompleta);
            }

            return null;
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

        private static void CardInfo(IContainer container, string titulo, string valor, string descripcion)
        {
            container
                .Border(1)
                .BorderColor(Colors.Green.Lighten2)
                .Background(Colors.Green.Lighten5)
                .Padding(10)
                .Column(column =>
                {
                    column.Spacing(3);
                    column.Item().Text(titulo).FontSize(10).FontColor(Colors.Grey.Darken2);
                    column.Item().Text(valor).FontSize(15).SemiBold().FontColor(Colors.Green.Darken3);
                    column.Item().Text(descripcion).FontSize(9).FontColor(Colors.Grey.Darken1);
                });
        }
    }
}
