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
    [Authorize]
    public class PlaneamientoController : Controller
    {
        private readonly ActividadOperativaService _ActividadOperativaService;
        private readonly ObjetivoEstrategicoService _ObjetivoEstrategicoService;
        private readonly AccionEstrategicaService _AccionEstrategicaService;
        private readonly DependenciaService _DependenciaService;
        private readonly TareaService _TareaService;
        private readonly ProgramacionFinancieraService _ProgramacionFinancieraService;
        private readonly CatalogoActividadOperativaService _CatalogoActividadOperativaService;

        public PlaneamientoController(
            ActividadOperativaService actividadOperativaService,
            ObjetivoEstrategicoService objetivoEstrategicoService,
            AccionEstrategicaService accionEstrategicaService,
            DependenciaService dependenciaService,
            TareaService tareaService,
            ProgramacionFinancieraService programacionFinancieraService,
            CatalogoActividadOperativaService catalogoActividadOperativaService
        )
        {
            _ActividadOperativaService = actividadOperativaService;
            _ObjetivoEstrategicoService = objetivoEstrategicoService;
            _AccionEstrategicaService = accionEstrategicaService;
            _DependenciaService = dependenciaService;
            _TareaService = tareaService;
            _ProgramacionFinancieraService = programacionFinancieraService;
            _CatalogoActividadOperativaService = catalogoActividadOperativaService;
        }

        public ActionResult Inicio()
        {
            return View();
        }

        #region Actividad Operativa
        public ActionResult Index()
        {
            ViewBag.lstDependenciaGrouped = ListarDependencia();
            TempData["unidadOrganica"] = TempData["unidadOrganica"] ?? GlobalConfig.unidadOrganica;
            return View();
        }

        [HttpGet]
        public ActionResult CreateActividadOperativa(ActividadOperativaCreateViewModel model)
        {
            if (model.Codigo_Actividad_Incrementable != 0)
            {
                var actividad_operativa = _ActividadOperativaService.Buscar_Actividad_Operativa(model.Codigo_Actividad_Incrementable);

                if (actividad_operativa == null)
                {
                    return HttpNotFound();
                }

                model = new ActividadOperativaCreateViewModel
                {
                    Codigo_Actividad_Incrementable = actividad_operativa.Id_Actividad_Incrementable,
                    Id_Codigo_Actividad_Cat = actividad_operativa.Id_Codigo_Actividad_Cat,
                    Codigo_Finalidad_Presupuestal = actividad_operativa.Codigo_Finalidad_Presupuestal,
                    Denominacion = actividad_operativa.Denominacion,
                    Cod_Objetivo_Est = actividad_operativa.Codigo_Objetivo,
                    Cod_Accion_Est = actividad_operativa.Codigo_Accion,
                    Descripcion_Actividad = actividad_operativa.Descripcion_Actividad,
                    Cod_Unidad_Organica = actividad_operativa.Codigo_Unidad_Organizacion,
                    nAnio = actividad_operativa.nAnio,
                    Id_Unidad_Medida = actividad_operativa.Id_Unidad_Medida,
                    Unidad_Medida = actividad_operativa.Unidad_Medida,
                    iCodUsuarioAccion = GlobalConfig.iCodUsuario
                };

            }

            ViewBag.CodigoActividadOperativa = new SelectList(_CatalogoActividadOperativaService.Listar_Catalogo(model.nAnio, model.Cod_Unidad_Organica), "Id_Codigo_Actividad_Cat", "Numero_Actividad", model.Id_Codigo_Actividad_Cat);
            ViewBag.Dependencia = new SelectList(ListarDependencia(), "iCodigoDependencia", "vDependencia");
            ViewBag.Objetivos = new SelectList(_ObjetivoEstrategicoService.Listar_Objetivo_Estrategico(), "Id_Objetivo", "DescripcionCompleta");
            ViewBag.Acciones = new SelectList(_AccionEstrategicaService.Listar_Accion_Estrategico(model.Cod_Objetivo_Est), "Id_Accion", "DescripcionCompleta");

            return PartialView("_CreateActividadOperativa", model);
        }

        [HttpGet]
        public ActionResult CreateCodigoActividadOperativa(CabeceraActividadOperativaCreateViewModel model)
        {
            ViewBag.Dependencia = new SelectList(ListarDependencia(), "iCodigoDependencia", "vDependencia");
            return PartialView("_CreateCodigoActividadOperativa", model);
        }

        [HttpPost]
        public ActionResult Create_Codigo_Actividad_Operativa(CabeceraActividadOperativaCreateViewModel model)
        {
            GeneralResponse response;

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Select(m => new
                {
                    Field = m.Key,
                    Errors = m.Value.Errors.Select(e => e.ErrorMessage).ToList()
                }).ToList();

                return Json(new { success = false, errors = errors });
            }

            var cabActividadOperativa = new CabActividadOperativa
            {
                iCodigoDependencia = model.Catalogo_AO_Codigo_Dependencia,
                nAnio = model.nAnio,
                nvCodActividad = model.Numero_Actividad
            };

            response = _CatalogoActividadOperativaService.Crear_Catalogo(cabActividadOperativa);
            return Json(new { success = true, data = response });
        }

        [HttpPost]
        public ActionResult Create_Actividad_Operativa(ActividadOperativaCreateViewModel model)
        {
            GeneralResponse response;

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Select(m => new
                {
                    Field = m.Key,
                    Errors = m.Value.Errors.Select(e => e.ErrorMessage).ToList()
                }).ToList();

                return Json(new { success = false, errors = errors });
            }

            var actividadOperativa = new ActividadOperativa
            {
                iCodActividadIncrementable = model.Codigo_Actividad_Incrementable,
                iCodTabActividadOperativa = model.Id_Codigo_Actividad_Cat,
                nvCodFinalidadPresupuestal = model.Codigo_Finalidad_Presupuestal,
                nvDenominacionActividad = model.Denominacion,
                nvDescripcionActividad = model.Descripcion_Actividad,
                iCodAccion = model.Cod_Accion_Est,
                iCodUnidadMedida = model.Id_Unidad_Medida,
                iCodigoDependencia = model.Cod_Unidad_Organica,
                nAnio = model.nAnio,
                iCodUsuario = GlobalConfig.iCodUsuario
            };

            //Registrar una nueva actividad operativa
            if (model.Codigo_Actividad_Incrementable == 0)
            {
                response = _ActividadOperativaService.Crear_Actividad_Operativa(actividadOperativa);
            }
            //Actualizar una nueva actividad operativa
            else
            {
                response = _ActividadOperativaService.Actualizar_Actividad_Operativa(actividadOperativa);
            }

            return Json(new { success = true, data = response });
        }

        [HttpPost]
        public JsonResult Listar_Actividad_Operativa(int nAnio, int Codigo_Dependencia)
        {
            try
            {
                TempData["unidadOrganica"] = Codigo_Dependencia.ToString();
                var lstActividadOperativa = _ActividadOperativaService.Listar_Actividad_Operativa(nAnio, Codigo_Dependencia);
                var result = Json(lstActividadOperativa, JsonRequestBehavior.AllowGet);
                return result;
            }
            catch (Exception ex)
            {
                return Json("Error occurred");
            }
        }

        [HttpPost]
        public JsonResult Desactivar_Actividad_Operativa(int iCodActividad)
        {
            try
            {
                var actividadOperativa = new ActividadOperativa
                {
                    iCodActividadIncrementable = iCodActividad,
                    iCodUsuario = GlobalConfig.iCodUsuario
                };

                var response = _ActividadOperativaService.Desactivar_Actividad_Operativa(actividadOperativa);
                var result = Json(response, JsonRequestBehavior.AllowGet);
                return Json(new { success = true, data = response }); ;
            }
            catch (Exception ex)
            {
                return Json(new { success = false, data = ex.Message }); ;
            }
        }

        #endregion

        #region Tareas
        [HttpGet]
        public ActionResult Tareas(int Id_Actividad = 0)
        {
            var actividad_operativa = _ActividadOperativaService.Buscar_Actividad_Operativa(Id_Actividad);

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
            return View();
        }

        [HttpPost]
        public JsonResult Crear_Tarea(TareaCreateViewModel model)
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


                var tarea = new Tarea
                {
                    iCodActividadIncrementable = model.Id_Actividad,
                    iCodTarea = model.Id_Tarea,
                    nvDesagregacion = model.Desagregacion,
                    iCodProceso = model.Codigo_Proceso,
                    iCodUnidadMedida = model.Codigo_Unidad_Medida,
                    DetalleTarea = model.listaMeses.Select(m => new DetTarea { nMes = m.Mes, nCantidad = m.Cantidad }).ToList(),
                    bCePlan = model.CePlan,
                    iCodUsuario = GlobalConfig.iCodUsuario
                };

                if (model.Id_Tarea == 0)
                {
                    response = _TareaService.Crear_Tarea(tarea);
                }
                else
                {
                    response = _TareaService.Actualizar_Tarea(tarea);
                }

                return Json(new { success = true, data = response });
            }
            catch (Exception ex)
            {
                return Json("Error occurred");
            }
        }

        [HttpPost]
        public JsonResult Desactivar_Tarea(int iCodTarea)
        {
            try
            {
                var tarea = new Tarea
                {
                    iCodTarea = iCodTarea,
                    iCodUsuario = GlobalConfig.iCodUsuario
                };

                var response = _TareaService.Desactivar_Tarea(tarea);
                var result = Json(response, JsonRequestBehavior.AllowGet);
                return Json(new { success = true, data = response }); ;
            }
            catch (Exception ex)
            {
                return Json(new { success = false, data = ex.Message }); ;
            }
        }

        [HttpPost]
        public JsonResult Listar_Tareas(int Id_Actividad = 0)
        {
            try
            {
                var response = _TareaService.Listar_Tareas(Id_Actividad);
                var result = Json(response, JsonRequestBehavior.AllowGet);
                return Json(new { success = true, data = response }); ;
            }
            catch (Exception ex)
            {
                return Json(new { success = false, data = ex.Message }); ;
            }
        }

        [HttpPost]
        public ActionResult ListarUnidadMedidaPaginacion(string vBuscar, int viewRows, int page = 1)
        {
            var response = _TareaService.Listar_Unidad_Medida(vBuscar, viewRows, page);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ListarProcesoPaginacion(string vBuscar, int viewRows, int page = 1)
        {
            var response = _TareaService.Listar_Procesos(vBuscar, viewRows, page);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Plan Financiero
        public ActionResult PlanFinanciero(int Id_Actividad = 0)
        {
            var actividad_operativa = _ActividadOperativaService.Buscar_Actividad_Operativa(Id_Actividad);

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

            var total = _ProgramacionFinancieraService.Consultar_Total(actividad_operativa.Id_Actividad_Incrementable);
            ViewBag.TotalPIA = total.TotalPIA;
            ViewBag.TotalUsado = total.TotalUsado;

            ViewBag.Id_Actividad = actividad_operativa.Id_Actividad_Incrementable;
            ViewBag.Codigo_Actividad = actividad_operativa.Codigo_Actividad;
            ViewBag.Denominacion = $"{actividad_operativa.Codigo_Finalidad_Presupuestal}. {actividad_operativa.Denominacion}";
            ViewBag.Dependencia = $"{actividad_operativa.Dependencia}";
            ViewBag.Anio = $"{actividad_operativa.nAnio}";

            return View();
        }

        [HttpPost]
        public JsonResult Listar_Programacion(int Id_Actividad = 0, int nAnio = 0)
        {
            try
            {
                var response = _ProgramacionFinancieraService.Listar_Programacion(Id_Actividad, nAnio);
                var result = Json(response, JsonRequestBehavior.AllowGet);
                return Json(new { success = true, data = response }); ;
            }
            catch (Exception ex)
            {
                return Json(new { success = false, data = ex.Message }); ;
            }
        }

        [HttpPost]
        public JsonResult Actualizar_Programacion(ProgramacionFinancieraCreateViewModel model)
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

                var programacion = new ProgramacionFinanciera
                {
                    iCodProgramacionFinanciera = model.Id_Programacion,
                    iCodActividad = model.Id_Actividad,
                    nTechoActividadOperativa = model.Monto_ActividadOperativa,
                    DetalleProgramacion = model.listaMeses.Select(m => new DetProgramacionFinanciera { nMes = m.Mes, nCantidad = m.Cantidad }).ToList(),
                    iCodUsuario = GlobalConfig.iCodUsuario
                };

                response = _ProgramacionFinancieraService.Actualizar_Programacion(programacion);

                return Json(new { success = true, data = response });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, data = ex.Message }); ;
            }
        }

        [HttpPost]
        public JsonResult Desactivar_Programacion(int iCodProgramacion)
        {
            try
            {
                var programacion = new ProgramacionFinanciera
                {
                    iCodProgramacionFinanciera = iCodProgramacion,
                    iCodUsuario = GlobalConfig.iCodUsuario
                };

                var response = _ProgramacionFinancieraService.Desactivar_Programacion(programacion);
                var result = Json(response, JsonRequestBehavior.AllowGet);
                return Json(new { success = true, data = response }); ;
            }
            catch (Exception ex)
            {
                return Json(new { success = false, data = ex.Message }); ;
            }
        }

        [HttpPost]
        public JsonResult Consultar_Total(int Id_Actividad = 0) 
        {
            try
            {
                var response = _ProgramacionFinancieraService.Consultar_Total(Id_Actividad);
                var result = Json(response, JsonRequestBehavior.AllowGet);
                return Json(new { success = true, data = response }); ;
            }
            catch (Exception ex)
            {
                return Json(new { success = false, data = ex.Message }); ;
            }
        }
        #endregion

        #region Objetivo Estratégico
        [HttpPost]
        public JsonResult Listar_Accion_Por_Objetivo(int Id_Objetivo)
        {
            try
            {
                var lstAcciones = _AccionEstrategicaService.Listar_Accion_Estrategico(Id_Objetivo);
                var result = Json(lstAcciones, JsonRequestBehavior.AllowGet);
                return result;
            }
            catch (Exception)
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

            var actividad_operativa = _ActividadOperativaService.Buscar_Actividad_Operativa(Id_Actividad);
            var tareas = _TareaService.Listar_Tareas(Id_Actividad);
            var programacionFinanciera = _ProgramacionFinancieraService.Listar_Programacion(Id_Actividad, actividad_operativa.nAnio);

            var rutaArchivo = Server.MapPath("~/Archives/Plantilla_Ficha.xlsx");
            var fileInfo = new FileInfo(rutaArchivo);

            using (var package = new ExcelPackage(fileInfo))
            {
                var worksheet = package.Workbook.Worksheets["Ficha_Tecnica"];

                #region Titulo
                worksheet.Cells[1, 2].Value = $"FICHA TÉCNICA DE LA ACTIVIDAD OPERATIVA(POI {actividad_operativa.nAnio})";
                #endregion

                #region Accion Estrategica - INFO
                worksheet.Cells[3, 2].Value = actividad_operativa.Dependencia;
                worksheet.Cells[5, 4].Value = actividad_operativa.Objetivo_Estrategico;
                worksheet.Cells[6, 4].Value = actividad_operativa.Accion_Estrategico;
                worksheet.Cells[8, 2].Value = $"{actividad_operativa.Codigo_Finalidad_Presupuestal}. {actividad_operativa.Denominacion}";
                worksheet.Cells[10, 2].Value = actividad_operativa.Descripcion_Actividad;
                #endregion

                #region Tareas

                #region Tareas Cabecera
                int CabTareasRowPosition = 14;
                var registrosCePlanActivos = tareas.Where(t => t.CePlan == true);
                var prog_fisica = new
                {
                    Ene = registrosCePlanActivos.Sum(m => m.Ene),
                    Feb = registrosCePlanActivos.Sum(m => m.Feb),
                    Mar = registrosCePlanActivos.Sum(m => m.Mar),
                    Abr = registrosCePlanActivos.Sum(m => m.Abr),
                    May = registrosCePlanActivos.Sum(m => m.May),
                    Jun = registrosCePlanActivos.Sum(m => m.Jun),
                    Jul = registrosCePlanActivos.Sum(m => m.Jul),
                    Ago = registrosCePlanActivos.Sum(m => m.Ago),
                    Set = registrosCePlanActivos.Sum(m => m.Set),
                    Oct = registrosCePlanActivos.Sum(m => m.Oct),
                    Nov = registrosCePlanActivos.Sum(m => m.Nov),
                    Dic = registrosCePlanActivos.Sum(m => m.Dic)
                };

                worksheet.InsertRow(CabTareasRowPosition, 1);
                worksheet.Cells[CabTareasRowPosition, 2].Value = $"{actividad_operativa.Codigo_Finalidad_Presupuestal}. {actividad_operativa.Denominacion}";
                worksheet.Cells[CabTareasRowPosition, 3].Value = $"{actividad_operativa.Unidad_Medida}";
                worksheet.Cells[CabTareasRowPosition, 4].Value = registrosCePlanActivos.Count();
                worksheet.Cells[CabTareasRowPosition, 5].Value = prog_fisica.Ene;
                worksheet.Cells[CabTareasRowPosition, 6].Value = prog_fisica.Feb;
                worksheet.Cells[CabTareasRowPosition, 7].Value = prog_fisica.Mar;
                worksheet.Cells[CabTareasRowPosition, 8].Value = prog_fisica.Abr;
                worksheet.Cells[CabTareasRowPosition, 9].Value = prog_fisica.May;
                worksheet.Cells[CabTareasRowPosition, 10].Value = prog_fisica.Jun;
                worksheet.Cells[CabTareasRowPosition, 11].Value = prog_fisica.Jul;
                worksheet.Cells[CabTareasRowPosition, 12].Value = prog_fisica.Ago;
                worksheet.Cells[CabTareasRowPosition, 13].Value = prog_fisica.Set;
                worksheet.Cells[CabTareasRowPosition, 14].Value = prog_fisica.Oct;
                worksheet.Cells[CabTareasRowPosition, 15].Value = prog_fisica.Nov;
                worksheet.Cells[CabTareasRowPosition, 16].Value = prog_fisica.Dic;
                worksheet.Cells[CabTareasRowPosition, 17].Formula = $"SUM(E{CabTareasRowPosition}:P{CabTareasRowPosition})";

                var rango = worksheet.Cells[CabTareasRowPosition, 2, CabTareasRowPosition, 17];

                // Aplicamos los estilos al rango dinámico
                rango.Style.Font.Size = 12;
                rango.Style.Font.Name = "Century Schoolbook";
                rango.Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                rango.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                rango.Style.WrapText = true;
                #endregion

                #region Tareas Detalle

                int tareasRowPosition = CabTareasRowPosition + 4;
                tareas.ForEach(tarea =>
                {
                    worksheet.InsertRow(tareasRowPosition, 1);
                    worksheet.Cells[tareasRowPosition, 2].Value = tarea.Desagregacion;
                    worksheet.Cells[tareasRowPosition, 3].Value = tarea.Proceso;
                    worksheet.Cells[tareasRowPosition, 4].Value = tarea.Unidad_Medida;
                    worksheet.Cells[tareasRowPosition, 5].Value = tarea.Ene;
                    worksheet.Cells[tareasRowPosition, 6].Value = tarea.Feb;
                    worksheet.Cells[tareasRowPosition, 7].Value = tarea.Mar;
                    worksheet.Cells[tareasRowPosition, 8].Value = tarea.Abr;
                    worksheet.Cells[tareasRowPosition, 9].Value = tarea.May;
                    worksheet.Cells[tareasRowPosition, 10].Value = tarea.Jun;
                    worksheet.Cells[tareasRowPosition, 11].Value = tarea.Jul;
                    worksheet.Cells[tareasRowPosition, 12].Value = tarea.Ago;
                    worksheet.Cells[tareasRowPosition, 13].Value = tarea.Set;
                    worksheet.Cells[tareasRowPosition, 14].Value = tarea.Oct;
                    worksheet.Cells[tareasRowPosition, 15].Value = tarea.Nov;
                    worksheet.Cells[tareasRowPosition, 16].Value = tarea.Dic;
                    worksheet.Cells[tareasRowPosition, 17].Value = tarea.Total;

                    var rangoTareas = worksheet.Cells[tareasRowPosition, 2, tareasRowPosition, 17];

                    // Aplicamos los estilos al rango dinámico
                    rangoTareas.Style.Font.Size = 12;
                    rangoTareas.Style.Font.Name = "Century Schoolbook";
                    rangoTareas.Style.Border.BorderAround(ExcelBorderStyle.Dotted, System.Drawing.Color.Black);
                    rangoTareas.Style.Border.Top.Style = ExcelBorderStyle.Dotted;
                    rangoTareas.Style.Border.Bottom.Style = ExcelBorderStyle.Dotted;
                    rangoTareas.Style.Border.Left.Style = ExcelBorderStyle.Dotted;
                    rangoTareas.Style.Border.Right.Style = ExcelBorderStyle.Dotted;
                    rangoTareas.Style.WrapText = true;
                    tareasRowPosition++;
                });

                #endregion

                #endregion

                #region Programacion Financiera

                #region Programacion Financiera Cabecera
                int CabProgramacionFinanciera = tareasRowPosition + 3;
                var progFinancieraGrouped = programacionFinanciera
                .GroupBy(t => new { t.Codigo_Generica_Gasto })
                .Select(group => new
                {
                    Codigo_Generica_Gasto = group.Key.Codigo_Generica_Gasto,
                    Fuente_Financiamiento = group.Select(e => e.Fuente_Financiamiento),
                    Ene = group.Sum(m => m.Ene),
                    Feb = group.Sum(m => m.Feb),
                    Mar = group.Sum(m => m.Mar),
                    Abr = group.Sum(m => m.Abr),
                    May = group.Sum(m => m.May),
                    Jun = group.Sum(m => m.Jun),
                    Jul = group.Sum(m => m.Jul),
                    Ago = group.Sum(m => m.Ago),
                    Set = group.Sum(m => m.Set),
                    Oct = group.Sum(m => m.Oct),
                    Nov = group.Sum(m => m.Nov),
                    Dic = group.Sum(m => m.Dic),
                }).ToList();

                progFinancieraGrouped.ForEach(prog =>
                {
                    worksheet.InsertRow(CabProgramacionFinanciera, 1);
                    worksheet.Cells[CabProgramacionFinanciera, 2, CabProgramacionFinanciera, 3].Merge = true;
                    worksheet.Cells[CabProgramacionFinanciera, 2].Value = $"5-{prog.Codigo_Generica_Gasto}";
                    worksheet.Cells[CabProgramacionFinanciera, 4].Value = prog.Fuente_Financiamiento;
                    worksheet.Cells[CabProgramacionFinanciera, 5].Value = prog.Ene;
                    worksheet.Cells[CabProgramacionFinanciera, 6].Value = prog.Feb;
                    worksheet.Cells[CabProgramacionFinanciera, 7].Value = prog.Mar;
                    worksheet.Cells[CabProgramacionFinanciera, 8].Value = prog.Abr;
                    worksheet.Cells[CabProgramacionFinanciera, 9].Value = prog.May;
                    worksheet.Cells[CabProgramacionFinanciera, 10].Value = prog.Jun;
                    worksheet.Cells[CabProgramacionFinanciera, 11].Value = prog.Jul;
                    worksheet.Cells[CabProgramacionFinanciera, 12].Value = prog.Ago;
                    worksheet.Cells[CabProgramacionFinanciera, 13].Value = prog.Set;
                    worksheet.Cells[CabProgramacionFinanciera, 14].Value = prog.Oct;
                    worksheet.Cells[CabProgramacionFinanciera, 15].Value = prog.Nov;
                    worksheet.Cells[CabProgramacionFinanciera, 16].Value = prog.Dic;
                    worksheet.Cells[CabProgramacionFinanciera, 17].Formula = $"SUM(E{CabProgramacionFinanciera}:P{CabProgramacionFinanciera})";

                    var rangoProgramacion = worksheet.Cells[CabProgramacionFinanciera, 2, CabProgramacionFinanciera, 17];

                    // Aplicamos los estilos al rango dinámico
                    rangoProgramacion.Style.Font.Size = 11;
                    rangoProgramacion.Style.Font.Name = "Century Schoolbook";
                    rangoProgramacion.Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                    rangoProgramacion.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rangoProgramacion.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rangoProgramacion.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rangoProgramacion.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rangoProgramacion.Style.WrapText = true;

                    var rangoResult = worksheet.Cells[CabProgramacionFinanciera, 5, CabProgramacionFinanciera, 17];
                    rangoResult.Style.Numberformat.Format = "#,##0.00";
                    rangoResult.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rangoResult.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#ddebf7"));
                    rangoResult.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;  // Centrado horizontal
                    rangoResult.Style.VerticalAlignment = ExcelVerticalAlignment.Center;      // Centrado vertical

                    CabProgramacionFinanciera++;
                });

                var rowDetTareaTotal = CabProgramacionFinanciera - 1;
                var rowDetTareaStart = rowDetTareaTotal - (progFinancieraGrouped.Count() - 1);
                for (var column = 5; column <= 17; column++)
                {
                    worksheet.Cells[CabProgramacionFinanciera, column].Formula = $"SUM({worksheet.Cells[rowDetTareaStart, column].Address}:{worksheet.Cells[rowDetTareaTotal, column].Address})";
                    worksheet.Cells[CabProgramacionFinanciera, column].Style.Numberformat.Format = "#,##0.00";
                }
                #endregion

                #region Programacion Financiera Detalle
                int DetProgramacionFinancieraRow = CabProgramacionFinanciera + 3;

                var DetprogFinancieraGrouped = programacionFinanciera
                  .GroupBy(t => new { t.Codigo_Generica_Gasto })
                  .Select(group => new
                  {
                      Codigo_Generica_Gasto = group.Key.Codigo_Generica_Gasto,
                      DetalleProgramacion = group.ToList()
                  }).ToList();

                var totalProgramacionRow = new List<int>();

                DetprogFinancieraGrouped.ForEach(prog =>
                {
                    prog.DetalleProgramacion.ForEach(DetalleTarea =>
                    {
                        worksheet.InsertRow(DetProgramacionFinancieraRow, 1);
                        worksheet.Cells[DetProgramacionFinancieraRow, 2].Value = DetalleTarea.Clasificador.ToUpper();
                        worksheet.Cells[DetProgramacionFinancieraRow, 2].Style.Font.Bold = true;
                        worksheet.Cells[DetProgramacionFinancieraRow, 2].Style.Font.Name = "Calibri";
                        worksheet.Cells[DetProgramacionFinancieraRow, 3].Value = DetalleTarea.Codigo_Clasificador;
                        worksheet.Cells[DetProgramacionFinancieraRow, 5].Value = DetalleTarea.Ene;
                        worksheet.Cells[DetProgramacionFinancieraRow, 6].Value = DetalleTarea.Feb;
                        worksheet.Cells[DetProgramacionFinancieraRow, 7].Value = DetalleTarea.Mar;
                        worksheet.Cells[DetProgramacionFinancieraRow, 8].Value = DetalleTarea.Abr;
                        worksheet.Cells[DetProgramacionFinancieraRow, 9].Value = DetalleTarea.May;
                        worksheet.Cells[DetProgramacionFinancieraRow, 10].Value = DetalleTarea.Jun;
                        worksheet.Cells[DetProgramacionFinancieraRow, 11].Value = DetalleTarea.Jul;
                        worksheet.Cells[DetProgramacionFinancieraRow, 12].Value = DetalleTarea.Ago;
                        worksheet.Cells[DetProgramacionFinancieraRow, 13].Value = DetalleTarea.Set;
                        worksheet.Cells[DetProgramacionFinancieraRow, 14].Value = DetalleTarea.Oct;
                        worksheet.Cells[DetProgramacionFinancieraRow, 15].Value = DetalleTarea.Nov;
                        worksheet.Cells[DetProgramacionFinancieraRow, 16].Value = DetalleTarea.Dic;
                        worksheet.Cells[DetProgramacionFinancieraRow, 17].Formula = $"SUM(E{DetProgramacionFinancieraRow}:P{DetProgramacionFinancieraRow})";
                        var rangoP = worksheet.Cells[DetProgramacionFinancieraRow, 2, DetProgramacionFinancieraRow, 17];
                        rangoP.Style.Font.Size = 11;
                        rangoP.Style.Font.Name = "Century Schoolbook";
                        rangoP.Style.WrapText = true;

                        var rangoProgramacion = worksheet.Cells[DetProgramacionFinancieraRow, 5, DetProgramacionFinancieraRow, 17];

                        rangoProgramacion.Style.Numberformat.Format = "#,##0.00";
                        rangoProgramacion.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rangoProgramacion.Style.Font.Name = "Century Schoolbook";
                        rangoProgramacion.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#ddebf7"));
                        rangoProgramacion.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;  // Centrado horizontal
                        rangoProgramacion.Style.VerticalAlignment = ExcelVerticalAlignment.Center;      // Centrado vertical
                        rangoProgramacion.Style.Border.BorderAround(ExcelBorderStyle.Dotted, System.Drawing.Color.Black);
                        rangoProgramacion.Style.Border.Top.Style = ExcelBorderStyle.Dotted;
                        rangoProgramacion.Style.Border.Bottom.Style = ExcelBorderStyle.Dotted;
                        rangoProgramacion.Style.Border.Left.Style = ExcelBorderStyle.Dotted;
                        rangoProgramacion.Style.Border.Right.Style = ExcelBorderStyle.Dotted;

                        DetProgramacionFinancieraRow++;
                    });

                    worksheet.InsertRow(DetProgramacionFinancieraRow, 1);

                    var rowTotal = DetProgramacionFinancieraRow - 1;
                    var rowStart = rowTotal - (prog.DetalleProgramacion.Count() - 1);

                    worksheet.Cells[DetProgramacionFinancieraRow, 2, DetProgramacionFinancieraRow, 4].Merge = true;
                    worksheet.Cells[DetProgramacionFinancieraRow, 2].Value = $"5-{prog.Codigo_Generica_Gasto}";

                    for (var column = 5; column <= 17; column++)
                    {
                        worksheet.Cells[DetProgramacionFinancieraRow, column].Formula = $"SUM({worksheet.Cells[rowStart, column].Address}:{worksheet.Cells[rowTotal, column].Address})";
                        worksheet.Cells[DetProgramacionFinancieraRow, column].Style.Numberformat.Format = "#,##0.00";
                    }

                    var rangoTotalProgramacion = worksheet.Cells[DetProgramacionFinancieraRow, 2, DetProgramacionFinancieraRow, 17];
                    rangoTotalProgramacion.Style.Font.Name = "Century Schoolbook";
                    rangoTotalProgramacion.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rangoTotalProgramacion.Style.Font.Bold = true;
                    rangoTotalProgramacion.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2F5496"));
                    rangoTotalProgramacion.Style.Font.Color.SetColor(Color.White);
                    rangoTotalProgramacion.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;  // Centrado horizontal
                    rangoTotalProgramacion.Style.VerticalAlignment = ExcelVerticalAlignment.Center;      // Centrado vertical
                    worksheet.Row(DetProgramacionFinancieraRow).Height = 30;

                    totalProgramacionRow.Add(DetProgramacionFinancieraRow);
                    DetProgramacionFinancieraRow++;
                });

                for (var column = 5; column <= 17; column++)
                {
                    var cellReferences = new List<string>();

                    foreach (var row in totalProgramacionRow)
                    {
                        var cellAddress = worksheet.Cells[row, column].Address;
                        cellReferences.Add(cellAddress);
                    }

                    string sumFormula = string.Join(",", cellReferences);

                    worksheet.Cells[DetProgramacionFinancieraRow, column].Formula = $"SUM({sumFormula})";
                }
                #endregion

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