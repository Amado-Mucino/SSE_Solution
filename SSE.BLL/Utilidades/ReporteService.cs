using System;
using System.Collections.Generic;
using System.IO;
using ClosedXML.Excel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using SSE.Entidades.Modelos;

namespace SSE.BLL.Utilidades
{
    public class ReporteService
    {
        // EXportación a Excel (Recuerden instalar ClosedXML)
        public bool ExportarEgresadosAExcel(List<Egresado> listaEgresados, string rutaArchivo, out string mensajeError)
        {
            mensajeError = string.Empty;
            try
            {
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Reporte de Egresados");

                    // 1. Crear los Encabezados
                    worksheet.Cell(1, 1).Value = "Matrícula";
                    worksheet.Cell(1, 2).Value = "Nombre Completo";
                    worksheet.Cell(1, 3).Value = "Carrera";
                    worksheet.Cell(1, 4).Value = "Estado Laboral";
                    worksheet.Cell(1, 5).Value = "Titulado";

                    // Formato para los encabezados (Negrita y fondo gris)
                    var rangoEncabezado = worksheet.Range("A1:E1");
                    rangoEncabezado.Style.Font.Bold = true;
                    rangoEncabezado.Style.Fill.BackgroundColor = XLColor.LightGray;

                    // 2. Llenar los datos
                    int fila = 2;
                    foreach (var egresado in listaEgresados)
                    {
                        worksheet.Cell(fila, 1).Value = egresado.Matricula;
                        worksheet.Cell(fila, 2).Value = $"{egresado.Nombre} {egresado.ApellidoPaterno} {egresado.ApellidoMaterno}";
                        worksheet.Cell(fila, 3).Value = egresado.NombreCarrera;
                        worksheet.Cell(fila, 4).Value = egresado.EstadoLaboral;
                        worksheet.Cell(fila, 5).Value = egresado.Titulado ? "SÍ" : "NO";
                        fila++;
                    }

                    // Ajustar el ancho de las columnas automáticamente
                    worksheet.Columns().AdjustToContents();

                    // 3. Guardar el archivo
                    workbook.SaveAs(rutaArchivo);
                    return true;
                }
            }
            catch (Exception ex)
            {
                mensajeError = $"Error al generar Excel: {ex.Message}";
                return false;
            }
        }

        // Exportación a PDF (Igualmente, recuerden instalar iTextSharp)
        public bool ExportarEgresadosAPdf(List<Egresado> listaEgresados, string rutaArchivo, out string mensajeError)
        {
            mensajeError = string.Empty;
            try
            {
                // 1. Crear el documento (Tamaño carta, horizontal)
                Document documento = new Document(PageSize.LETTER.Rotate(), 25, 25, 30, 30);
                PdfWriter.GetInstance(documento, new FileStream(rutaArchivo, FileMode.Create));

                documento.Open();

                // 2. Título del Reporte
                Font fontTitulo = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
                Paragraph titulo = new Paragraph("REPORTE INSTITUCIONAL DE EGRESADOS\n\n", fontTitulo);
                titulo.Alignment = Element.ALIGN_CENTER;
                documento.Add(titulo);

                // 3. Crear la Tabla (5 columnas)
                PdfPTable tabla = new PdfPTable(5);
                tabla.WidthPercentage = 100;

                // Configurar anchos relativos de las columnas
                float[] anchos = new float[] { 1.5f, 3f, 3f, 2f, 1f };
                tabla.SetWidths(anchos);

                // 4. Encabezados de la tabla
                Font fontEncabezado = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10, BaseColor.WHITE);
                string[] encabezados = { "Matrícula", "Nombre Completo", "Carrera", "Est. Laboral", "Titulado" };

                foreach (string texto in encabezados)
                {
                    PdfPCell celda = new PdfPCell(new Phrase(texto, fontEncabezado));
                    celda.BackgroundColor = new BaseColor(41, 128, 185); // Color azul institucional
                    celda.HorizontalAlignment = Element.ALIGN_CENTER;
                    celda.Padding = 5;
                    tabla.AddCell(celda);
                }

                // 5. Llenar los datos
                Font fontDatos = FontFactory.GetFont(FontFactory.HELVETICA, 9);
                foreach (var egresado in listaEgresados)
                {
                    tabla.AddCell(new PdfPCell(new Phrase(egresado.Matricula, fontDatos)));
                    tabla.AddCell(new PdfPCell(new Phrase($"{egresado.Nombre} {egresado.ApellidoPaterno}", fontDatos)));
                    tabla.AddCell(new PdfPCell(new Phrase(egresado.NombreCarrera, fontDatos)));
                    tabla.AddCell(new PdfPCell(new Phrase(egresado.EstadoLaboral, fontDatos)));

                    PdfPCell celdaTitulado = new PdfPCell(new Phrase(egresado.Titulado ? "SÍ" : "NO", fontDatos));
                    celdaTitulado.HorizontalAlignment = Element.ALIGN_CENTER;
                    tabla.AddCell(celdaTitulado);
                }

                documento.Add(tabla);
                documento.Close();

                return true;
            }
            catch (Exception ex)
            {
                mensajeError = $"Error al generar PDF: {ex.Message}";
                return false;
            }
        }
    }
}