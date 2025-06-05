using Common.ViewModels;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IDependenciaService
    {
        List<DependenciaViewModel> Listar_Dependencia(bool isCoordinador, int iCodigoDependencia);
    }
    public class DependenciaService: IDependenciaService
    {
        private readonly DependenciaDAO _DependenciaDAO = new DependenciaDAO();
        public List<DependenciaViewModel> Listar_Dependencia(bool isCoordinador, int iCodigoDependencia)
        {
            return _DependenciaDAO.Listar_Dependencia(isCoordinador, iCodigoDependencia);
        }
    }
}
