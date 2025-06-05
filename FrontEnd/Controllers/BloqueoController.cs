using Common.Models;
using Common.ViewModels;
using FrontEnd.Filters;
using FrontEnd.Models;
using Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FrontEnd.Controllers
{
    public class BloqueoController : Controller
    {
        private readonly BloqueoService _BloqueoService;

        public BloqueoController(BloqueoService bloqueoService)
        {
            _BloqueoService = bloqueoService;
        }

        [CustomAuthorize(ControllerName = "Bloqueo", ActionName = "Index")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Listar_Bloqueo(int Id_Modulo, int Anio)
        {
            try
            {
                var lstBloqueo = _BloqueoService.Listar(Id_Modulo, Anio);
                return Json(new { success = true, data = lstBloqueo }, JsonRequestBehavior.AllowGet); ;
            }
            catch (Exception ex)
            {
                return Json("Error occurred");
            }
        }

        [HttpPost]
        public JsonResult Registrar_Bloqueo(BloqueoCreateViewModel model)
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


                var bloqueo = new Bloqueo
                {
                    iCodModulo = model.Codigo_Modulo,
                    nAnio = model.Anio,
                    DetalleBloqueo = model.DetalleBloqueo.Select(m => new DetBloqueo { nMes = m.Mes, iCodigoDependencia = m.iCodigoDependencia, bBloqueado = m.Bloqueado }).ToList(),
                    iCodUsuario = GlobalConfig.iCodUsuario
                };

                response = _BloqueoService.Registrar_Bloqueo(bloqueo);

                return Json(new { success = true, data = response });
            }
            catch (Exception ex)
            {
                return Json("Error occurred");
            }
        }
    }
}