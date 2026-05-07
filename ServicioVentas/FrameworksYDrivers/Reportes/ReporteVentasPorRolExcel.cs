using System.Globalization;
using System.Security;
using System.Text;
using VitalCareSBA.ServicioVentas.Entidades.DTOs;

namespace VitalCareSBA.ServicioVentas.FrameworksYDrivers.Reportes
{
    public static class ReporteVentasPorRolExcel
    {
        private static readonly CultureInfo InvariantCulture = CultureInfo.InvariantCulture;

        public static byte[] Generar(DateTime fechaInicio, DateTime fechaFin, IEnumerable<ReporteVentasPorRolDto> datos)
        {
            List<ReporteVentasPorRolDto> detalle = datos
                .OrderByDescending(x => x.TotalRecaudado)
                .ThenByDescending(x => x.CantidadVentas)
                .ToList();

            int totalVentas = detalle.Sum(x => x.CantidadVentas);
            decimal totalRecaudado = detalle.Sum(x => x.TotalRecaudado);
            ReporteVentasPorRolDto? rolTop = detalle.FirstOrDefault();

            string periodo = $"{fechaInicio:dd/MM/yyyy} - {fechaFin:dd/MM/yyyy}";
            string totalRecaudadoTexto = $"Bs. {totalRecaudado:N2}";
            string rolTopTexto = rolTop?.Rol ?? "Sin datos";

            StringBuilder xml = new();
            xml.AppendLine("<?xml version=\"1.0\"?>");
            xml.AppendLine("<?mso-application progid=\"Excel.Sheet\"?>");
            xml.AppendLine("<Workbook xmlns=\"urn:schemas-microsoft-com:office:spreadsheet\"");
            xml.AppendLine(" xmlns:o=\"urn:schemas-microsoft-com:office:office\"");
            xml.AppendLine(" xmlns:x=\"urn:schemas-microsoft-com:office:excel\"");
            xml.AppendLine(" xmlns:ss=\"urn:schemas-microsoft-com:office:spreadsheet\"");
            xml.AppendLine(" xmlns:html=\"http://www.w3.org/TR/REC-html40\">");
            xml.AppendLine("  <Styles>");
            xml.AppendLine("    <Style ss:ID=\"Title\">");
            xml.AppendLine("      <Font ss:Bold=\"1\" ss:Size=\"16\" ss:Color=\"#157347\"/>");
            xml.AppendLine("    </Style>");
            xml.AppendLine("    <Style ss:ID=\"Subtitle\">");
            xml.AppendLine("      <Font ss:Bold=\"1\" ss:Size=\"11\" ss:Color=\"#4F4F4F\"/>");
            xml.AppendLine("    </Style>");
            xml.AppendLine("    <Style ss:ID=\"Header\">");
            xml.AppendLine("      <Font ss:Bold=\"1\"/>");
            xml.AppendLine("      <Interior ss:Color=\"#D1E7DD\" ss:Pattern=\"Solid\"/>");
            xml.AppendLine("      <Borders>");
            xml.AppendLine("        <Border ss:Position=\"Bottom\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/>");
            xml.AppendLine("      </Borders>");
            xml.AppendLine("    </Style>");
            xml.AppendLine("    <Style ss:ID=\"Money\">");
            xml.AppendLine("      <NumberFormat ss:Format=\"Standard\"/>");
            xml.AppendLine("    </Style>");
            xml.AppendLine("    <Style ss:ID=\"Metric\">");
            xml.AppendLine("      <Font ss:Bold=\"1\" ss:Color=\"#0F5132\"/>");
            xml.AppendLine("      <Interior ss:Color=\"#F6FFFB\" ss:Pattern=\"Solid\"/>");
            xml.AppendLine("    </Style>");
            xml.AppendLine("    <Style ss:ID=\"Bar\">");
            xml.AppendLine("      <Font ss:Color=\"#157347\" ss:FontName=\"Consolas\"/>");
            xml.AppendLine("    </Style>");
            xml.AppendLine("  </Styles>");

            xml.AppendLine("  <Worksheet ss:Name=\"Resumen\">");
            xml.AppendLine("    <Table>");
            xml.AppendLine("      <Column ss:Width=\"220\"/>");
            xml.AppendLine("      <Column ss:Width=\"180\"/>");
            xml.AppendLine("      <Column ss:Width=\"140\"/>");
            xml.AppendLine("      <Column ss:Width=\"260\"/>");
            xml.AppendLine("      " + TextoSimple("Reporte de Ventas por Rol", "Title"));
            xml.AppendLine("      <Row/>");
            xml.AppendLine("      " + TextoPar("Periodo", periodo));
            xml.AppendLine("      " + TextoPar("Total de ventas", totalVentas.ToString(InvariantCulture)));
            xml.AppendLine("      " + TextoPar("Total recaudado", totalRecaudadoTexto));
            xml.AppendLine("      " + TextoPar("Rol con mayor recaudacion", rolTopTexto));
            xml.AppendLine("      <Row/>");
            xml.AppendLine("      <Row>");
            xml.AppendLine("        <Cell ss:StyleID=\"Header\"><Data ss:Type=\"String\">Rol</Data></Cell>");
            xml.AppendLine("        <Cell ss:StyleID=\"Header\"><Data ss:Type=\"String\">Cantidad de Ventas</Data></Cell>");
            xml.AppendLine("        <Cell ss:StyleID=\"Header\"><Data ss:Type=\"String\">Total Recaudado (Bs.)</Data></Cell>");
            xml.AppendLine("        <Cell ss:StyleID=\"Header\"><Data ss:Type=\"String\">Participacion</Data></Cell>");
            xml.AppendLine("      </Row>");

            foreach (ReporteVentasPorRolDto item in detalle)
            {
                decimal participacion = totalRecaudado <= 0
                    ? 0
                    : Math.Round((item.TotalRecaudado / totalRecaudado) * 100m, 2);
                string participacionTexto = participacion.ToString("N2", CultureInfo.CurrentCulture) + "%";
                string barraParticipacion = BarraParticipacion(participacion);

                xml.AppendLine("      <Row>");
                xml.AppendLine("        <Cell><Data ss:Type=\"String\">" + Escapar(item.Rol) + "</Data></Cell>");
                xml.AppendLine("        <Cell><Data ss:Type=\"Number\">" + item.CantidadVentas + "</Data></Cell>");
                xml.AppendLine("        <Cell ss:StyleID=\"Money\"><Data ss:Type=\"Number\">" + item.TotalRecaudado.ToString(InvariantCulture) + "</Data></Cell>");
                xml.AppendLine("        <Cell ss:StyleID=\"Bar\"><Data ss:Type=\"String\">" + Escapar(barraParticipacion + " " + participacionTexto) + "</Data></Cell>");
                xml.AppendLine("      </Row>");
            }

            xml.AppendLine("    </Table>");
            xml.AppendLine("  </Worksheet>");

            xml.AppendLine("  <Worksheet ss:Name=\"Detalle\">");
            xml.AppendLine("    <Table>");
            xml.AppendLine("      <Column ss:Width=\"180\"/>");
            xml.AppendLine("      <Column ss:Width=\"150\"/>");
            xml.AppendLine("      <Column ss:Width=\"160\"/>");
            xml.AppendLine("      " + TextoSimple("Detalle de ventas por rol", "Subtitle"));
            xml.AppendLine("      <Row/>");
            xml.AppendLine("      <Row>");
            xml.AppendLine("        <Cell ss:StyleID=\"Header\"><Data ss:Type=\"String\">Rol</Data></Cell>");
            xml.AppendLine("        <Cell ss:StyleID=\"Header\"><Data ss:Type=\"String\">Cantidad de Ventas</Data></Cell>");
            xml.AppendLine("        <Cell ss:StyleID=\"Header\"><Data ss:Type=\"String\">Total Recaudado (Bs.)</Data></Cell>");
            xml.AppendLine("      </Row>");

            foreach (ReporteVentasPorRolDto item in detalle)
            {
                xml.AppendLine("      <Row>");
                xml.AppendLine("        <Cell><Data ss:Type=\"String\">" + Escapar(item.Rol) + "</Data></Cell>");
                xml.AppendLine("        <Cell><Data ss:Type=\"Number\">" + item.CantidadVentas + "</Data></Cell>");
                xml.AppendLine("        <Cell ss:StyleID=\"Money\"><Data ss:Type=\"Number\">" + item.TotalRecaudado.ToString(InvariantCulture) + "</Data></Cell>");
                xml.AppendLine("      </Row>");
            }

            xml.AppendLine("    </Table>");
            xml.AppendLine("  </Worksheet>");
            xml.AppendLine("</Workbook>");

            return new UTF8Encoding(encoderShouldEmitUTF8Identifier: true).GetBytes(xml.ToString());
        }

        private static string TextoSimple(string valor, string estilo)
        {
            return "<Row><Cell ss:StyleID=\"" + estilo + "\"><Data ss:Type=\"String\">" + Escapar(valor) + "</Data></Cell></Row>";
        }

        private static string TextoPar(string etiqueta, string valor)
        {
            return "<Row><Cell ss:StyleID=\"Metric\"><Data ss:Type=\"String\">" + Escapar(etiqueta) + "</Data></Cell><Cell><Data ss:Type=\"String\">" + Escapar(valor) + "</Data></Cell></Row>";
        }

        private static string BarraParticipacion(decimal participacion)
        {
            const int ancho = 20;
            int llenos = (int)Math.Round((participacion / 100m) * ancho, MidpointRounding.AwayFromZero);
            llenos = Math.Clamp(llenos, 0, ancho);

            return new string('█', llenos) + new string('░', ancho - llenos);
        }

        private static string Escapar(string? texto)
        {
            return SecurityElement.Escape(texto ?? string.Empty) ?? string.Empty;
        }
    }
}
