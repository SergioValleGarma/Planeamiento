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
    public class ObjetivoEstrategicoDAO
    {
        private SqlConnection con;

        private void connection()
        {
            string constr = new MIDIS.Utiles.Crypto().Desencriptar(ConfigurationManager.ConnectionStrings["AppContext"].ToString());
            con = new SqlConnection(constr);
        }
        public List<ObjetivoEstrategicoViewModel> Listar_Objetivo_Estrategico()
        {
            connection();
            List<ObjetivoEstrategicoViewModel> List = new List<ObjetivoEstrategicoViewModel>();

            try
            {
                DynamicParameters p = new DynamicParameters();

                con.Open();
                List = con.Query<ObjetivoEstrategicoViewModel>("paPL_ObjetivoEst_Consultar", p, commandType: CommandType.StoredProcedure).ToList();
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
