using Common.ViewModels;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Service
{
    public interface IAccionEstrategicaService
    {
        List<AccionEstrategicaViewModel> Listar_Accion_Estrategico(int Id_Objetivo);
    }
    public class AccionEstrategicaService : IAccionEstrategicaService
    {
        private readonly AccionEstrategicaDAO _AccionEstrategicaDAO = new AccionEstrategicaDAO();
        public List<AccionEstrategicaViewModel> Listar_Accion_Estrategico(int Id_Objetivo)
        {
            return _AccionEstrategicaDAO.Listar_Accion_Estrategica(Id_Objetivo);
        }
    }
}
