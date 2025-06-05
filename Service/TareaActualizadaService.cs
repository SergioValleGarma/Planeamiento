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
    public interface ITareaActualizadaService
    {
        List<TareaViewModel> Listar_Tareas(int Id_Actividad_Incrementable);
        GeneralResponse Crear_Tarea(Tarea model);
        GeneralResponse Actualizar_Tarea(Tarea model);
        GeneralResponse Desactivar_Tarea(Tarea model);
    }
    public class TareaActualizadaService : ITareaActualizadaService
    {
        private readonly TareaActualizadaDAO _TareaActualizadaDAO = new TareaActualizadaDAO();

        public GeneralResponse Actualizar_Tarea(Tarea model)
        {
            return _TareaActualizadaDAO.Actualizar_Tarea(model);
        }

        public GeneralResponse Crear_Tarea(Tarea model)
        {
            return _TareaActualizadaDAO.Crear_Tarea(model);
        }

        public GeneralResponse Desactivar_Tarea(Tarea model)
        {
            return _TareaActualizadaDAO.Desactivar_Tarea(model);
        }

        public List<TareaViewModel> Listar_Tareas(int Id_Actividad_Incrementable)
        {
            return _TareaActualizadaDAO.Listar_Tareas(Id_Actividad_Incrementable);
        }
    }
}
