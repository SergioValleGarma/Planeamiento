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
    public interface ITareaService
    {
        List<TareaViewModel> Listar_Tareas(int Id_Actividad_Incrementable);
        GeneralResponse Crear_Tarea(Tarea model);
        GeneralResponse Actualizar_Tarea(Tarea model);
        GeneralResponse Desactivar_Tarea(Tarea model);
        ProcesoViewModel Listar_Procesos(string vBuscar, int viewRows, int page = 1);
        UnidadMedidaViewModel Listar_Unidad_Medida(string vBuscar, int viewRows, int page = 1);
    }
    public class TareaService : ITareaService
    {
        private readonly TareaDAO _TareaDAO = new TareaDAO();

        public GeneralResponse Actualizar_Tarea(Tarea model)
        {
            return _TareaDAO.Actualizar_Tarea(model);
        }

        public GeneralResponse Crear_Tarea(Tarea model)
        {
            return _TareaDAO.Crear_Tarea(model);
        }

        public GeneralResponse Desactivar_Tarea(Tarea model)
        {
            return _TareaDAO.Desactivar_Tarea(model);
        }

        public ProcesoViewModel Listar_Procesos(string vBuscar, int viewRows, int page = 1)
        {
            return _TareaDAO.Listar_Procesos(vBuscar, viewRows, page);
        }

        public List<TareaViewModel> Listar_Tareas(int Id_Actividad_Incrementable)
        {
            return _TareaDAO.Listar_Tareas(Id_Actividad_Incrementable);
        }

        public UnidadMedidaViewModel Listar_Unidad_Medida(string vBuscar, int viewRows, int page = 1)
        {
            return _TareaDAO.Listar_Unidad_Medida(vBuscar, viewRows, page);
        }
    }
}
