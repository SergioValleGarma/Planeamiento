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
    public interface ISeguimientoTareaActualizadaService
    {
        List<SeguimientoTareaViewModel> Listar_Tareas(int Id_Actividad_Incrementable);
        GeneralResponse Registrar_Seguimiento(SeguimientoFisico model);
    }
    public class SeguimientoTareaService : ISeguimientoTareaActualizadaService
    {
        private readonly SeguimientoTareaDAO _SegTareaActualizadaDAO = new SeguimientoTareaDAO();

        public List<SeguimientoTareaViewModel> Listar_Tareas(int Id_Actividad_Incrementable)
        {
            return _SegTareaActualizadaDAO.Listar_Seg_Tareas(Id_Actividad_Incrementable);
        }

        public GeneralResponse Registrar_Seguimiento(SeguimientoFisico model)
        {
            return _SegTareaActualizadaDAO.Registrar_Seguimiento(model);
        }
    }
}
