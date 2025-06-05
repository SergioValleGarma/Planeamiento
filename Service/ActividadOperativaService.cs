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
    public interface IActividadOperativaService
    {
        List<ActividadOperativaViewModel> Listar_Actividad_Operativa(int nAnio, int Codigo_Dependencia);
        GeneralResponse Crear_Actividad_Operativa(ActividadOperativa model);
        GeneralResponse Actualizar_Actividad_Operativa(ActividadOperativa model);
        GeneralResponse Desactivar_Actividad_Operativa(ActividadOperativa model);
        ActividadOperativaViewModel Buscar_Actividad_Operativa(int Id_Actividad_Incr);
    }
    public class ActividadOperativaService : IActividadOperativaService
    {
        private readonly ActividadOperativaDAO _ActividadOperativaDAO = new ActividadOperativaDAO();

        public GeneralResponse Actualizar_Actividad_Operativa(ActividadOperativa model)
        {
            return _ActividadOperativaDAO.Actualizar_Actividad_Operativa(model);
        }

        public ActividadOperativaViewModel Buscar_Actividad_Operativa(int Id_Actividad_Incr)
        {
            return _ActividadOperativaDAO.Buscar_Actividad_Operativa(Id_Actividad_Incr);
        }

        public GeneralResponse Crear_Actividad_Operativa(ActividadOperativa model)
        {
            return _ActividadOperativaDAO.Crear_Actividad_Operativa(model);
        }

        public GeneralResponse Desactivar_Actividad_Operativa(ActividadOperativa model)
        {
            return _ActividadOperativaDAO.Desactivar_Actividad_Operativa(model);
        }

        public List<ActividadOperativaViewModel> Listar_Actividad_Operativa(int nAnio, int Codigo_Dependencia)
        {
            return _ActividadOperativaDAO.Listar_Actividad_Operativa(nAnio, Codigo_Dependencia);
        }
    }
}
