using Common.Models;
using Common.ViewModels;
using FrontEnd.Models;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FrontEnd.Controllers
{
    public class SeguimientoController : Controller
    {
        private readonly ActividadOperativaActualizadaService _ActividadOperativaActualizadaService = new ActividadOperativaActualizadaService();
        private readonly DependenciaService _DependenciaService = new DependenciaService();
        private readonly SeguimientoTareaService _SeguimientoTareaService = new SeguimientoTareaService();
        private readonly BloqueoService _BloqueoService = new BloqueoService();
        
        public SeguimientoController(
            ActividadOperativaActualizadaService actividadOperativaActualizadaService,
            DependenciaService dependenciaService,
            SeguimientoTareaService seguimientoTareaService,
            BloqueoService bloqueoService
        )
        {
            _ActividadOperativaActualizadaService = actividadOperativaActualizadaService;
            _DependenciaService = dependenciaService;
            _SeguimientoTareaService = seguimientoTareaService;
            _BloqueoService = bloqueoService;
        }

        public ActionResult Index()
        {
            ViewBag.lstDependenciaGrouped = ListarDependencia();
            TempData["unidadOrganica"] = TempData["unidadOrganica"] ?? GlobalConfig.unidadOrganica;
            return View();
        }

        #region Seguimiento Físico
        [HttpGet]
        public ActionResult Seguimiento_Fisico(int Id_Actividad = 0)
        {
            var actividad_operativa = _ActividadOperativaActualizadaService.Buscar_Actividad_Operativa(Id_Actividad);

            /* Si la actividad operativa no existe, entonces devolvemos que no se encontró */
            if (actividad_operativa == null)
            {
                return HttpNotFound();
            }

            /* Si el usuario es operador y la unidad orgánica no pertenece a la del usuario, entonces no devolvemos la vista */
            if (actividad_operativa.Codigo_Unidad_Organizacion.ToString() != GlobalConfig.unidadOrganica && GlobalConfig.isOperador)
            {
                return HttpNotFound();
            }

            ViewBag.Id_Actividad = Id_Actividad;
            ViewBag.Codigo_Actividad = actividad_operativa.Codigo_Actividad;
            ViewBag.Denominacion = $"{actividad_operativa.Codigo_Finalidad_Presupuestal}. {actividad_operativa.Denominacion}";
            ViewBag.Dependencia = $"{actividad_operativa.Dependencia}";
            ViewBag.Anio = $"{actividad_operativa.nAnio}";
            ViewBag.MesesBloqueados = _BloqueoService.Listar_DetalleBloqueo(actividad_operativa.Codigo_Unidad_Organizacion, actividad_operativa.nAnio, 2);
            return View();
        }

        [HttpPost]
        public JsonResult Listar_Tareas(int Id_Actividad = 0)
        {
            try
            {
                var response = _SeguimientoTareaService.Listar_Tareas(Id_Actividad);
                var result = Json(response, JsonRequestBehavior.AllowGet);
                return Json(new { success = true, data = response }); ;
            }
            catch (Exception ex)
            {
                return Json(new { success = false, data = ex.Message }); ;
            }
        }

        [HttpPost]
        public JsonResult Registrar_Seguimiento(Det_SeguimientoTareaCreateViewModel model)
        {
            GeneralResponse response;
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState
                    .Where(m => m.Value.Errors.Count > 0)  // Filtrar solo los campos con errores
                    .Select(m => new
                    {
                        Errors = m.Value.Errors.Select(e => e.ErrorMessage).ToList()
                    }).ToList();

                    return Json(new { success = false, errors = errors });
                }


                var segFisico = new SeguimientoFisico
                {
                    iCodDetTarea_Act = model.iCodDetTarea_Act,
                    iCodTarea_Act = model.iCodTarea_Act,
                    nvJustificacion = model.nvJustificacion,
                    nCantidad = model.nCantidad,
                    nMes = model.nMes,
                    iCodUsuario = GlobalConfig.iCodUsuario
                };

                
                response = _SeguimientoTareaService.Registrar_Seguimiento(segFisico);

                return Json(new { success = true, data = response });
            }
            catch (Exception ex)
            {
                return Json("Error occurred");
            }
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

        #region Generar Ficha
        public ActionResult GenerarFicha(int Id_Actividad)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var actividad_operativa = _ActividadOperativaActualizadaService.Buscar_Actividad_Operativa(Id_Actividad);
            var tareas = _SeguimientoTareaService.Listar_Tareas(Id_Actividad);

            var rutaArchivo = Server.MapPath("~/Archives/Reporte_Seguimiento.xlsx");
            var fileInfo = new FileInfo(rutaArchivo);

            using (var package = new ExcelPackage(fileInfo))
            {
                var worksheet = package.Workbook.Worksheets["reporte"];

                #region Accion Estrategica - CABECERA
                worksheet.Cells[2, 2].Value = actividad_operativa.Dependencia;
                worksheet.Cells[3, 2].Value = actividad_operativa.Codigo_Actividad;
                worksheet.Cells[6, 1].Value = $"{actividad_operativa.Codigo_Finalidad_Presupuestal}. {actividad_operativa.Denominacion}";
                worksheet.Cells[6, 2].Value = actividad_operativa.Unidad_Medida;

                #region Ejecución Física
                int colStartMonthCabecera = 4;

                var groupedTotals = tareas
                .Where(tarea => tarea.Cab_Tarea.bCePlan == true)
                .SelectMany(seguimiento => seguimiento.Detalle)
                .Where(detalle => detalle.Mes_Programado >= 1 && detalle.Mes_Programado <= 12)
                .GroupBy(detalle => detalle.Mes_Programado)
                .ToDictionary(
                    group => group.Key,
                    group => new
                    {
                        TotalCantidadProgramado = group.Sum(detalle => detalle.Cantidad_Programado),
                        TotalCantidadSeguimiento = group.Sum(detalle => detalle.Cantidad_Seguimiento)
                    }
                );

                foreach (var month in groupedTotals)
                {
                    int monthNumber = month.Key;
                    var totals = month.Value;

                    worksheet.Cells[6, colStartMonthCabecera].Value = totals.TotalCantidadProgramado;
                    worksheet.Cells[7, colStartMonthCabecera].Value = totals.TotalCantidadSeguimiento;

                    if (totals.TotalCantidadSeguimiento < totals.TotalCantidadProgramado)
                    {
                        worksheet.Cells[7, colStartMonthCabecera].Style.Font.Bold = true;
                        worksheet.Cells[7, colStartMonthCabecera].Style.Font.Color.SetColor(System.Drawing.Color.Red);
                    }
                    else if(totals.TotalCantidadSeguimiento > totals.TotalCantidadProgramado)
                    {
                        worksheet.Cells[7, colStartMonthCabecera].Style.Font.Bold = true;
                        worksheet.Cells[7, colStartMonthCabecera].Style.Font.Color.SetColor(System.Drawing.Color.Green);
                    }

                    colStartMonthCabecera++;
                }
                #endregion

                #endregion

                #region Ejecución Física Detalle
                int rowProgramado = 9;
                int rowEjecutado = 0;
                foreach (var seguimiento in tareas)
                {
                    int colStartMonth = 4;
                    rowEjecutado = rowProgramado + 1;

                    worksheet.Cells[rowProgramado, 1].Value = seguimiento.Cab_Tarea.Desagregacion;
                    worksheet.Cells[rowProgramado, 2].Value = seguimiento.Cab_Tarea.Unidad_Medida;
                    worksheet.Cells[rowProgramado, 3].Value = "PROGRAMADO";
                    worksheet.Cells[rowEjecutado, 3].Value = "EJECUTADO";

                    foreach (var detalle in seguimiento.Detalle.OrderBy(e => e.Mes_Programado))
                    {
                        worksheet.Cells[rowProgramado, colStartMonth].Value = detalle.Cantidad_Programado;
                        worksheet.Cells[rowEjecutado, colStartMonth].Value = detalle.Cantidad_Seguimiento;
                        
                        if (detalle.Cantidad_Seguimiento < detalle.Cantidad_Programado)
                        {
                            worksheet.Cells[rowEjecutado, colStartMonth].Style.Font.Bold = true;
                            worksheet.Cells[rowEjecutado, colStartMonth].Style.Font.Color.SetColor(System.Drawing.Color.Red);
                        }
                        else if (detalle.Cantidad_Seguimiento > detalle.Cantidad_Programado) 
                        {
                            worksheet.Cells[rowEjecutado, colStartMonth].Style.Font.Bold = true;
                            worksheet.Cells[rowEjecutado, colStartMonth].Style.Font.Color.SetColor(System.Drawing.Color.Green);
                        }
                        colStartMonth++;
                    }

                    worksheet.Cells[rowProgramado, colStartMonth].Formula = $"SUM(D{rowProgramado}:O{rowProgramado})";
                    worksheet.Cells[rowEjecutado, colStartMonth].Formula = $"SUM(D{rowEjecutado}:O{rowEjecutado})";

                    #region Estilos para la primera y segunda columna de tarea
                    worksheet.Cells[rowProgramado, 1, rowEjecutado, 1].Merge = true;
                    worksheet.Cells[rowProgramado, 2, rowEjecutado, 2].Merge = true;
                    worksheet.Cells[rowProgramado, 1, rowEjecutado, 1].Style.WrapText = true;
                    worksheet.Cells[rowProgramado, 1, rowEjecutado, 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[rowProgramado, 1, rowEjecutado, 1].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#CCFFFF"));

                    worksheet.Row(rowProgramado).CustomHeight = true;
                    worksheet.Row(rowEjecutado).CustomHeight = true;

                    worksheet.Row(rowProgramado).Height = -1;
                    worksheet.Row(rowEjecutado).Height = -1;

                    #endregion

                    #region Estilos para las tareas
                    var rangoTarea = worksheet.Cells[rowProgramado, 1, rowEjecutado, 16];
                    rangoTarea.Style.Font.Size = 8;
                    rangoTarea.Style.Font.Name = "Calibri";
                    rangoTarea.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rangoTarea.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    rangoTarea.Style.Border.BorderAround(ExcelBorderStyle.Medium, System.Drawing.Color.Black);
                    rangoTarea.Style.Border.Top.Style = ExcelBorderStyle.Medium;
                    rangoTarea.Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                    rangoTarea.Style.Border.Left.Style = ExcelBorderStyle.Medium;
                    rangoTarea.Style.Border.Right.Style = ExcelBorderStyle.Medium;
                    #endregion

                    var rangoProgramado = worksheet.Cells[rowProgramado, 4, rowProgramado, 15];
                    var rangoEjecutado = worksheet.Cells[rowEjecutado, 4, rowEjecutado, 15];

                    rangoProgramado.Style.Border.Top.Style = ExcelBorderStyle.Medium;
                    rangoProgramado.Style.Border.Bottom.Style = ExcelBorderStyle.Dotted;
                    rangoProgramado.Style.Border.Left.Style = ExcelBorderStyle.Dotted;  
                    rangoProgramado.Style.Border.Right.Style = ExcelBorderStyle.Dotted; 

                    rangoEjecutado.Style.Border.Top.Style = ExcelBorderStyle.Dotted;
                    rangoEjecutado.Style.Border.Bottom.Style = ExcelBorderStyle.Medium; 
                    rangoEjecutado.Style.Border.Left.Style = ExcelBorderStyle.Dotted;
                    rangoEjecutado.Style.Border.Right.Style = ExcelBorderStyle.Dotted;

                    rowProgramado += 2;
                }
                #endregion

                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                string nombreArchivo = $"Reporte_Seguimiento_{ System.DateTime.Now.ToString("yyyyMMddhhmmss")}.xlsx";
                return File(stream,
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            nombreArchivo);
            }
        }

        private double CalculateHeight(string text)
        {
            const int charactersPerLine = 145;
            int numberOfLines = (int)Math.Ceiling((double)text.Length / charactersPerLine);

            double lineHeight = 15;

            return numberOfLines * lineHeight;
        }
        #endregion
    }
}