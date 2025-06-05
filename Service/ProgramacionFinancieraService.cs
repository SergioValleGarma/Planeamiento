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
    public interface IProgramacionFinancieraService
    {
        List<ProgramacionFinancieraViewModel> Listar_Programacion(int Id_Actividad_Incrementable, int nAnio);
        ProgramacionFinancieraTotalViewModel Consultar_Total(int Id_Actividad_Incrementable);
        GeneralResponse Actualizar_Programacion(ProgramacionFinanciera model);
        GeneralResponse Desactivar_Programacion(ProgramacionFinanciera model);
        List<ProgramacionFinancieraActualizadaViewModel> Listar_ProgramacionActualizada(int Id_Actividad_Incrementable, int nAnio);

    }
    public class ProgramacionFinancieraService : IProgramacionFinancieraService
    {
        private readonly ProgramacionFinancieraDAO _ProgramacionFinancieraDAO = new ProgramacionFinancieraDAO();

        public GeneralResponse Actualizar_Programacion(ProgramacionFinanciera model)
        {
            return _ProgramacionFinancieraDAO.Actualizar_Programacion(model);
        }

        public ProgramacionFinancieraTotalViewModel Consultar_Total(int Id_Actividad_Incrementable)
        {
            return _ProgramacionFinancieraDAO.Consultar_Total(Id_Actividad_Incrementable);
        }

        public GeneralResponse Desactivar_Programacion(ProgramacionFinanciera model)
        {
            return _ProgramacionFinancieraDAO.Desactivar_Programacion(model);
        }

        public List<ProgramacionFinancieraViewModel> Listar_Programacion(int Id_Actividad_Incrementable, int nAnio)
        {
            return _ProgramacionFinancieraDAO.Listar_Programacion(Id_Actividad_Incrementable, nAnio);
        }

        public List<ProgramacionFinancieraActualizadaViewModel> Listar_ProgramacionActualizada(int Id_Actividad_Incrementable, int nAnio)
        {
            return _ProgramacionFinancieraDAO.Listar_ProgramacionActualizada(Id_Actividad_Incrementable, nAnio);
        }
    }
}
