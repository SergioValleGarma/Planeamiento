using Common.ViewModels;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IObjetivoEstrategicoService
    {
        List<ObjetivoEstrategicoViewModel> Listar_Objetivo_Estrategico();
    }
    public class ObjetivoEstrategicoService : IObjetivoEstrategicoService
    {
        private readonly ObjetivoEstrategicoDAO _ObjetivoEstrategicoDAO = new ObjetivoEstrategicoDAO();
        public List<ObjetivoEstrategicoViewModel> Listar_Objetivo_Estrategico()
        {
            return _ObjetivoEstrategicoDAO.Listar_Objetivo_Estrategico();
        }
    }
}
