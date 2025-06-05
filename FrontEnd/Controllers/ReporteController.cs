using Common.Models;
using Common.ViewModels;
using FrontEnd.Filters;
using FrontEnd.Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Service;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FrontEnd.Controllers
{
    [Authorize]
    public class ReporteController : Controller
    {
        private readonly DatabaseService _DatabaseService;
        private readonly DependenciaService _DependenciaService;

        public ReporteController(DatabaseService databaseService, DependenciaService dependenciaService)
        {
            _DatabaseService = databaseService;
            _DependenciaService = dependenciaService;
        }

        [CustomAuthorize(ControllerName = "Reporte", ActionName = "Index")]
        public ActionResult Index()
        {
            ViewBag.lstDependenciaGrouped = ListarDependencia();
            TempData["unidadOrganica"] = TempData["unidadOrganica"] ?? GlobalConfig.unidadOrganica;
            return View();
        }

        #region Reportes

        public ActionResult ExportarBD(int anio)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            try
            {
                var datos = _DatabaseService.Listar(anio);

                var rutaArchivo = Server.MapPath("~/Archives/Plantilla_BaseDatos.xlsx");
                var fileInfo = new FileInfo(rutaArchivo);

                using (var package = new ExcelPackage(fileInfo))
                {
                    var worksheet = package.Workbook.Worksheets["Base de Datos"];

                    #region Datos
                    var startRow = 3;

                    datos.ForEach(data => {

                        #region Actividad Operativa
                        worksheet.Cells[startRow, 1].Value = data.Unidad_resp;
                        worksheet.Cells[startRow, 2].Value = data.Periodo;
                        worksheet.Cells[startRow, 3].Value = data.Cod_Actividad_Operativa;
                        worksheet.Cells[startRow, 4].Value = data.Finalidad;
                        worksheet.Cells[startRow, 5].Value = data.Actividad_Operativa;
                        worksheet.Cells[startRow, 6].Value = data.OEI;
                        worksheet.Cells[startRow, 7].Value = data.OEI_Desc;
                        worksheet.Cells[startRow, 8].Value = data.AEI;
                        worksheet.Cells[startRow, 9].Value = data.AEI_Desc;
                        worksheet.Cells[startRow, 10].Value = data.Desagregacion;
                        worksheet.Cells[startRow, 11].Value = data.Unidad_Medida;
                        worksheet.Cells[startRow, 12].Value = data.Proceso;
                        worksheet.Cells[startRow, 13].Value = data.bCePlan ? "Activo" : "";
                        #endregion

                        #region Programado
                        worksheet.Cells[startRow, 14].Value = data.Ene_Prog;
                        worksheet.Cells[startRow, 15].Value = data.Feb_Prog;
                        worksheet.Cells[startRow, 16].Value = data.Mar_Prog;
                        worksheet.Cells[startRow, 17].Value = data.Abr_Prog;
                        worksheet.Cells[startRow, 18].Value = data.May_Prog;
                        worksheet.Cells[startRow, 19].Value = data.Jun_Prog;
                        worksheet.Cells[startRow, 20].Value = data.Jul_Prog;
                        worksheet.Cells[startRow, 21].Value = data.Ago_Prog;
                        worksheet.Cells[startRow, 22].Value = data.Set_Prog;
                        worksheet.Cells[startRow, 23].Value = data.Oct_Prog;
                        worksheet.Cells[startRow, 24].Value = data.Nov_Prog;
                        worksheet.Cells[startRow, 25].Value = data.Dic_Prog;

                        #endregion

                        #region Seguimiento
                        worksheet.Cells[startRow, 26].Value = data.Ene_Seg;
                        worksheet.Cells[startRow, 27].Value = data.Ene_Justificacion;

                        worksheet.Cells[startRow, 28].Value = data.Feb_Seg;
                        worksheet.Cells[startRow, 29].Value = data.Feb_Justificacion;

                        worksheet.Cells[startRow, 30].Value = data.Mar_Seg;
                        worksheet.Cells[startRow, 31].Value = data.Mar_Justificacion;

                        worksheet.Cells[startRow, 32].Value = data.Abr_Seg;
                        worksheet.Cells[startRow, 33].Value = data.Abr_Justificacion;

                        worksheet.Cells[startRow, 34].Value = data.May_Seg;
                        worksheet.Cells[startRow, 35].Value = data.May_Justificacion;

                        worksheet.Cells[startRow, 36].Value = data.Jun_Seg;
                        worksheet.Cells[startRow, 37].Value = data.Jun_Justificacion;

                        worksheet.Cells[startRow, 38].Value = data.Jul_Seg;
                        worksheet.Cells[startRow, 39].Value = data.Jul_Justificacion;

                        worksheet.Cells[startRow, 40].Value = data.Ago_Seg;
                        worksheet.Cells[startRow, 41].Value = data.Ago_Justificacion;

                        worksheet.Cells[startRow, 42].Value = data.Set_Seg;
                        worksheet.Cells[startRow, 43].Value = data.Set_Justificacion;

                        worksheet.Cells[startRow, 44].Value = data.Oct_Seg;
                        worksheet.Cells[startRow, 45].Value = data.Oct_Justificacion;

                        worksheet.Cells[startRow, 46].Value = data.Nov_Seg;
                        worksheet.Cells[startRow, 47].Value = data.Nov_Justificacion;

                        worksheet.Cells[startRow, 48].Value = data.Dic_Seg;
                        worksheet.Cells[startRow, 49].Value = data.Dic_Justificacion;
                        #endregion

                        var rango = worksheet.Cells[startRow, 1, startRow, 49];

                        rango.Style.WrapText = true;
                        rango.Style.Border.Top.Style = ExcelBorderStyle.Dashed;
                        rango.Style.Border.Bottom.Style = ExcelBorderStyle.Dashed;
                        rango.Style.Border.Left.Style = ExcelBorderStyle.Dashed;
                        rango.Style.Border.Right.Style = ExcelBorderStyle.Dashed;
                        
                        startRow++;
                    });

                    #endregion

                    var stream = new MemoryStream();
                    package.SaveAs(stream);
                    stream.Position = 0; // Reiniciar el puntero del stream

                    // Retornar el archivo como descarga
                    string nombreArchivo = $"Reporte_Base+de+datos_{ System.DateTime.Now.ToString("yyyyMMddhhmmss")}.xlsx";
                    return File(stream,
                                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                nombreArchivo);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult GenerarReporte(int CodigoDependencia, int anio, int mes)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            try
            {
                var datos = _DatabaseService.ConsultarReporte(CodigoDependencia, anio, mes);

                var rutaArchivo = Server.MapPath("~/Archives/Reporte_Semaforo.xlsx");
                var fileInfo = new FileInfo(rutaArchivo);
                var rangoMensual = _DatabaseService.ConsultarParametrosMensualSemaforo();
                var rangoAnual = _DatabaseService.ConsultarParametrosAnualSemaforo(mes);

                using (var package = new ExcelPackage(fileInfo))
                {
                    var worksheet = package.Workbook.Worksheets["Reporte"];

                    List<string> meses = Helper.ListarMeses();

                    #region Encabezado
                    worksheet.Cells[2, 5].Value = $"POI {anio}";
                    worksheet.Cells[2, 7].Value = $"Meta FISICA Acumulada Programada a {meses[mes - 1]} (c )";
                    worksheet.Cells[2, 8].Value = $"Ejecución fisica acumulada a {meses[mes - 1]} (d)";
                    worksheet.Cells[2, 9].Value = $"Ejecucion financiera a {meses[mes - 1]} (e)";
                    worksheet.Cells[3, 10].Value = $"Avance Físico Ene - {meses[mes - 1].ToString().Substring(0,3)} (d / c)";
                    #endregion

                    #region Detalle
                    int startRow = 4;

                    var dataGrouped = datos.
                        GroupBy(t => new { t.vSiglas })
                        .Select(group => new 
                        { 
                            vSiglas = group.Key.vSiglas,
                            Detalle = group.ToList() 
                        }).ToList();

                    dataGrouped.ForEach(cabecera =>
                    {
                        worksheet.Cells[startRow, 1].Value = cabecera.vSiglas;
                        worksheet.Cells[startRow, 1, startRow + cabecera.Detalle.Count() - 1, 1].Merge = true;
                        cabecera.Detalle.ForEach(detalle =>
                        {
                            worksheet.Cells[startRow, 2].Value = detalle.nvCodAccion;
                            worksheet.Cells[startRow, 3].Value = $"{detalle.nvCodFinalidadPresupuestal}. {detalle.nvDenominacionActividad}";
                            worksheet.Cells[startRow, 4].Value = detalle.Unidad_Medida;
                            worksheet.Cells[startRow, 5].Value = detalle.Meta_Fisica_Anual;
                            
                            worksheet.Cells[startRow, 6].Value = detalle.Meta_Financiera_Anual;
                            worksheet.Cells[startRow, 6].Style.Numberformat.Format = "#,##0.00";
                            
                            worksheet.Cells[startRow, 7].Value = detalle.Meta_Programada_Hasta;
                            worksheet.Cells[startRow, 8].Value = detalle.Ejecucion_Fisica_Hasta;
                            
                            worksheet.Cells[startRow, 9].Value = detalle.Ejecucion_Financiera_Hasta;
                            worksheet.Cells[startRow, 9].Style.Numberformat.Format = "#,##0.00";

                            worksheet.Cells[startRow, 10].Formula = $"=IFERROR((H{startRow}/G{startRow}), 0)";
                            worksheet.Cells[startRow, 11].Formula = $"=IFERROR((H{startRow}/E{startRow}), 0)";
                            worksheet.Cells[startRow, 12].Formula = $"=IFERROR((I{startRow}/F{startRow}), 0)";


                            var rangoDesempeñoMensual = worksheet.Cells[startRow, 10, startRow, 10];
                            rangoDesempeñoMensual.Style.Numberformat.Format = "0.00%";
                            rangoDesempeñoMensual.Style.Font.Color.SetColor(Color.White);
                            rangoDesempeñoMensual.Style.Font.Bold = true;
                            AplicarFormatoCondicionalPorcentaje(worksheet, rangoDesempeñoMensual, rangoMensual);

                            var rangoDesempeñoAnual = worksheet.Cells[startRow, 11, startRow, 12];
                            rangoDesempeñoAnual.Style.Numberformat.Format = "0.00%";
                            rangoDesempeñoAnual.Style.Font.Color.SetColor(Color.White);
                            rangoDesempeñoAnual.Style.Font.Bold = true;
                            AplicarFormatoCondicionalPorcentaje(worksheet, rangoDesempeñoAnual, rangoAnual);

                            #region Estilos
                            var rango = worksheet.Cells[startRow, 1, startRow, 12];
                            rango.Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                            rango.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            rango.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            rango.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            rango.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;  // Centrado horizontal
                            rango.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            rango.Style.WrapText = true;
                            #endregion

                            startRow++;
                        });
                    });
                    #endregion

                    var stream = new MemoryStream();
                    package.SaveAs(stream);
                    stream.Position = 0; // Reiniciar el puntero del stream

                    // Retornar el archivo como descarga
                    string nombreArchivo = $"Reporte_{ System.DateTime.Now.ToString("yyyyMMddhhmmss")}.xlsx";
                    return File(stream,
                                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                nombreArchivo);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void AplicarFormatoCondicionalPorcentaje(ExcelWorksheet worksheet, ExcelRange rango, List<ParametrosReporteSemaforo> paramsSemaforo)
        {
            var bajo = paramsSemaforo.SingleOrDefault(p => p.Clasificacion == "BAJO");
            var medio = paramsSemaforo.SingleOrDefault(p => p.Clasificacion == "MEDIO");
            var alto = paramsSemaforo.SingleOrDefault(p => p.Clasificacion == "ALTO");

            if (bajo == null || medio == null || alto == null)
                throw new Exception("Faltan parámetros de semáforo");

            string celda = rango.Start.Address.Replace("$", "");

            // ROJO
            var rojo = worksheet.ConditionalFormatting.AddExpression(rango);
            rojo.Formula = $"AND({celda}>={bajo.Rango_Primer_Valor.ToString(CultureInfo.InvariantCulture)}, {celda}<{bajo.Rango_Segundo_Valor.ToString(CultureInfo.InvariantCulture)})";
            rojo.Style.Fill.PatternType = ExcelFillStyle.Solid;
            rojo.Style.Fill.BackgroundColor.SetColor(Color.Red);

            // AMARILLO
            var amarillo = worksheet.ConditionalFormatting.AddExpression(rango);
            amarillo.Formula = $"AND({celda}>={medio.Rango_Primer_Valor.ToString(CultureInfo.InvariantCulture)}, {celda}<{medio.Rango_Segundo_Valor.ToString(CultureInfo.InvariantCulture)})";
            amarillo.Style.Fill.PatternType = ExcelFillStyle.Solid;
            amarillo.Style.Fill.BackgroundColor.SetColor(Color.Orange);

            // VERDE
            var verde = worksheet.ConditionalFormatting.AddExpression(rango);
            verde.Formula = $"{celda}>={alto.Rango_Segundo_Valor.ToString(CultureInfo.InvariantCulture)}";
            verde.Style.Fill.PatternType = ExcelFillStyle.Solid;
            verde.Style.Fill.BackgroundColor.SetColor(Color.Green);
        }

        #endregion

        #region Dependencia
        [HttpPost]
        public JsonResult Listar_Dependencia()
        {
            try
            {
                var lstDependencia = ListarDependencia();
                var result = Json(lstDependencia, JsonRequestBehavior.AllowGet);
                return result;
            }
            catch (Exception ex)
            {
                return Json("Error occurred");
            }
        }

        public List<DependenciaViewModel> ListarDependencia()
        {
            return _DependenciaService.Listar_Dependencia(!GlobalConfig.isOperador, Convert.ToInt32(GlobalConfig.unidadOrganica));
        }
        #endregion
    }
}