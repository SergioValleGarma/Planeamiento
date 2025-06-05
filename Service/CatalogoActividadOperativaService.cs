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
    public interface ICatalogoActividadOperativaService
    {
        List<CabeceraActividadOperativaViewModel> Listar_Catalogo(int nAnio, int Codigo_Dependencia);
        GeneralResponse Crear_Catalogo(CabActividadOperativa model);
    }
    public class CatalogoActividadOperativaService : ICatalogoActividadOperativaService
    {
        private readonly CatalogoActividadOperativaDAO _CatalogoActividadOperativaDAO = new CatalogoActividadOperativaDAO();

        public GeneralResponse Crear_Catalogo(CabActividadOperativa model)
        {
            return _CatalogoActividadOperativaDAO.Crear_Catalogo(model);
        }

        public List<CabeceraActividadOperativaViewModel> Listar_Catalogo(int nAnio, int Codigo_Dependencia)
        {
            return _CatalogoActividadOperativaDAO.Listar_Catalogo(nAnio, Codigo_Dependencia);
        }
    }
}
