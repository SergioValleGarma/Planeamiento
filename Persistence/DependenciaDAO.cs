using Common.Models;
using Common.ViewModels;
using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Persistence
{
    public class DependenciaDAO
    {
        private SqlConnection con;

        private void connection()
        {
            string constr = new MIDIS.Utiles.Crypto().Desencriptar(ConfigurationManager.ConnectionStrings["AppContext"].ToString());
            con = new SqlConnection(constr);
        }

        public List<DependenciaViewModel> Listar_Dependencia(bool isCoordinador, int iCodigoDependencia)
        {
            connection();
            List<DependenciaViewModel> List = new List<DependenciaViewModel>();

            try
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("iCodigoDependencia", iCodigoDependencia);
                con.Open();
                if (isCoordinador)
                {
                    List = con.Query<DependenciaViewModel>("paPL_Dependencia_Consultar", null, commandType: CommandType.StoredProcedure).ToList();
                }
                else
                { 
                    List = con.Query<DependenciaViewModel>("paPL_Dependencia_Consultar_iCodDependencia", p, commandType: CommandType.StoredProcedure).ToList();
                }
                con.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return List;
        }
    }
}
