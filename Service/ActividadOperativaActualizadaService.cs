using Common.Models;
using Common.ViewModels;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IActividadOperativaActualizadaService
    {
        List<ActividadOperativaActualizadaViewModel> Listar_Actividad_Operativa(int nAnio, int Codigo_Dependencia);
        GeneralResponse Crear_Actividad_Operativa(ActividadOperativa_Actualizada model);
        GeneralResponse Actualizar_Actividad_Operativa(ActividadOperativa_Actualizada model);
        GeneralResponse Desactivar_Actividad_Operativa(ActividadOperativa_Actualizada model);
        ActividadOperativaActualizadaViewModel Buscar_Actividad_Operativa(int Id_Actividad_Incr);
    }
    public class ActividadOperativaActualizadaService : IActividadOperativaActualizadaService
    {
        private readonly ActividadOperativaActualizadaDAO _ActividadOperativaActualizadaDAO = new ActividadOperativaActualizadaDAO();

        public GeneralResponse Actualizar_Actividad_Operativa(ActividadOperativa_Actualizada model)
        {
            return _ActividadOperativaActualizadaDAO.Actualizar_Actividad_Operativa(model);
        }

        public ActividadOperativaActualizadaViewModel Buscar_Actividad_Operativa(int Id_Actividad_Incr)
        {
            return _ActividadOperativaActualizadaDAO.Buscar_Actividad_Operativa(Id_Actividad_Incr);
        }

        public GeneralResponse Crear_Actividad_Operativa(ActividadOperativa_Actualizada model)
        {
            return _ActividadOperativaActualizadaDAO.Crear_Actividad_Operativa(model);
        }

        public GeneralResponse Desactivar_Actividad_Operativa(ActividadOperativa_Actualizada model)
        {
            return _ActividadOperativaActualizadaDAO.Desactivar_Actividad_Operativa(model);
        }

        public List<ActividadOperativaActualizadaViewModel> Listar_Actividad_Operativa(int nAnio, int Codigo_Dependencia)
        {
            return _ActividadOperativaActualizadaDAO.Listar_Actividad_Operativa(nAnio, Codigo_Dependencia);
        }
    }
}
