using Common.ViewModels;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FrontEnd.Controllers
{
    [Authorize]
    public class CargaFinancieraController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Carga()
        {
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                var fileCarga = Request.Files["cargaFile"];
                var lstCarga = new List<ProgramacionFinancieraActualizadaViewModel>();
                bool errorExists = false;

                if (fileCarga == null || fileCarga.ContentLength == 0)
                {
                    return Json(new { success = "false", message = "No se ha cargado el archivo" });
                }

                using (var memoryStream = new MemoryStream())
                {
                    fileCarga.InputStream.CopyTo(memoryStream);
                    using (var package = new ExcelPackage(fileCarga.InputStream))
                    {
                        var worksheet = package.Workbook.Worksheets[0];
                        int rowCount = worksheet.Dimension.Rows;

                        int colCount = 23;
                        int startRow = 2;

                        Type[] typeArray =
                         {
                            typeof(double),
                            typeof(string),
                            typeof(double),
                            typeof(double),
                            typeof(string),
                            typeof(string),
                            typeof(string),
                            typeof(double),
                            typeof(double),
                            typeof(double),
                            typeof(string),
                            typeof(double),
                            typeof(double),
                            typeof(double),
                            typeof(double),
                            typeof(double),
                            typeof(double),
                            typeof(double),
                            typeof(double),
                            typeof(double),
                            typeof(double),
                            typeof(double),
                            typeof(double),
                        };

                        while (startRow <= rowCount)
                        {
                            bool rowError = false;
                            var model = new ProgramacionFinancieraActualizadaViewModel();

                            //Validate types of cell Values
                            for (int colStart = 1; colStart <= colCount; colStart++)
                            {
                                var typeValue = typeArray[colStart - 1];
                                var cell = worksheet.Cells[startRow, colStart];
                                rowError = MarkCellsError(typeValue, cell);
                                
                                if (rowError) errorExists = true;
                            }

                            if (!rowError)
                            {
                                model.nAnio = Convert.ToInt32(worksheet.Cells[startRow, 1].Value);
                                model.Codigo_Finalidad = Convert.ToString(worksheet.Cells[startRow, 2].Value);
                                model.Meta = Convert.ToString(worksheet.Cells[startRow, 3].Value);
                                model.Sec_Func = Convert.ToString(worksheet.Cells[startRow, 4].Value);
                                model.Clasificador = Convert.ToString(worksheet.Cells[startRow, 5].Value);
                                model.Descripcion_Clasificador = Convert.ToString(worksheet.Cells[startRow, 6].Value);
                                model.Clasificador_Gasto = Convert.ToString(worksheet.Cells[startRow, 7].Value);
                                model.PIA = Convert.ToDecimal(worksheet.Cells[startRow, 8].Value);
                                model.PIM = Convert.ToDecimal(worksheet.Cells[startRow, 9].Value);
                                model.Certificado = Convert.ToDecimal(worksheet.Cells[startRow, 10].Value);
                                model.Fuente_Financiamiento = Convert.ToString(worksheet.Cells[startRow, 11].Value);
                                model.Monto_Devengado_01 = Convert.ToDecimal(worksheet.Cells[startRow, 12].Value);
                                model.Monto_Devengado_02 = Convert.ToDecimal(worksheet.Cells[startRow, 13].Value);
                                model.Monto_Devengado_03 = Convert.ToDecimal(worksheet.Cells[startRow, 14].Value);
                                model.Monto_Devengado_04 = Convert.ToDecimal(worksheet.Cells[startRow, 15].Value);
                                model.Monto_Devengado_05 = Convert.ToDecimal(worksheet.Cells[startRow, 16].Value);
                                model.Monto_Devengado_06 = Convert.ToDecimal(worksheet.Cells[startRow, 17].Value);
                                model.Monto_Devengado_07 = Convert.ToDecimal(worksheet.Cells[startRow, 18].Value);
                                model.Monto_Devengado_08 = Convert.ToDecimal(worksheet.Cells[startRow, 19].Value);
                                model.Monto_Devengado_09 = Convert.ToDecimal(worksheet.Cells[startRow, 20].Value);
                                model.Monto_Devengado_10 = Convert.ToDecimal(worksheet.Cells[startRow, 21].Value);
                                model.Monto_Devengado_11 = Convert.ToDecimal(worksheet.Cells[startRow, 22].Value);
                                model.Monto_Devengado_12 = Convert.ToDecimal(worksheet.Cells[startRow, 23].Value);

                                lstCarga.Add(model);
                            }

                            startRow++;
                        }

                        if (errorExists)
                        { 
                            var stream = new MemoryStream();
                            package.SaveAs(stream);
                            stream.Position = 0;
                            string nombreArchivo = $"Errores_{ System.DateTime.Now.ToString("yyyyMMddhhmmss")}.xlsx";
                            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nombreArchivo);
                        }

                        return Json(new { success = "true", message = "No hay errores" });
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool MarkCellsError(Type type, ExcelRange cell)
        {
            var cellValue = cell.Value.GetType();
            if (type != cell.Value.GetType())
            {
                cell.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                cell.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#ff3333"));
                return true;
            }

            return false;
        }
    }
}