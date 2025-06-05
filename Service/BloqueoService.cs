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
    public interface IBloqueoService
    {
        List<BloqueoViewModel> Listar(int Id_Modulo, int nAnio);
        GeneralResponse Registrar_Bloqueo(Bloqueo model);
        List<DetalleBloqueo> Listar_DetalleBloqueo(int iCodigoDependencia, int nAnio, int iCodModulo);
    }
    public class BloqueoService : IBloqueoService
    {
        private readonly BloqueoDAO _BloqueoDAO = new BloqueoDAO();
        public List<BloqueoViewModel> Listar(int Id_Modulo, int nAnio)
        {
            return _BloqueoDAO.Listar(Id_Modulo, nAnio);
        }

        public List<DetalleBloqueo> Listar_DetalleBloqueo(int iCodigoDependencia, int nAnio, int iCodModulo)
        {
            return _BloqueoDAO.Listar_DetalleBloqueo(iCodigoDependencia, nAnio, iCodModulo);
        }

        public GeneralResponse Registrar_Bloqueo(Bloqueo model)
        {
            return _BloqueoDAO.Registrar_Bloqueo(model);
        }
    }
}
