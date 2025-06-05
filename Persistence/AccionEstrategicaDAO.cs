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
    public class AccionEstrategicaDAO
    {
        private SqlConnection con;

        private void connection()
        {
            string constr = new MIDIS.Utiles.Crypto().Desencriptar(ConfigurationManager.ConnectionStrings["AppContext"].ToString());
            con = new SqlConnection(constr);
        }

        public List<AccionEstrategicaViewModel> Listar_Accion_Estrategica(int Id_Objetivo)
        {
            connection();
            List<AccionEstrategicaViewModel> List = new List<AccionEstrategicaViewModel>();

            try
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("iCodObjetivo", Id_Objetivo);
                
                con.Open();
                List = con.Query<AccionEstrategicaViewModel>("paPL_AccionEstrategica_BuscarPorObjetivo", p, commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return List;
        }
    }
}
