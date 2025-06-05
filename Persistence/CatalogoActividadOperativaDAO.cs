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
    public class CatalogoActividadOperativaDAO
    {
        private SqlConnection con;

        private void connection()
        {
            string constr = new MIDIS.Utiles.Crypto().Desencriptar(ConfigurationManager.ConnectionStrings["AppContext"].ToString());
            con = new SqlConnection(constr);
        }

        public List<CabeceraActividadOperativaViewModel> Listar_Catalogo(int nAnio, int iCodigoDependencia)
        {
            connection();
            List<CabeceraActividadOperativaViewModel> List = new List<CabeceraActividadOperativaViewModel>();

            try
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("nAnio", nAnio);
                p.Add("iCodigoDependencia", iCodigoDependencia);
                con.Open();
                List = con.Query<CabeceraActividadOperativaViewModel>("paPL_MaeActividadOperativa_Consultar", p, commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return List;
        }

        public GeneralResponse Crear_Catalogo(CabActividadOperativa model)
        {
            GeneralResponse response = new GeneralResponse();
            connection();
            try
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("nvCodActividad", model.nvCodActividad);
                p.Add("iCodigoDependencia", model.iCodigoDependencia);
                p.Add("nAnio", model.nAnio);
                p.Add("vResponse", dbType: DbType.String, size: 250, direction: ParameterDirection.Output);
                p.Add("statusResponse", dbType: DbType.String, size: 100, direction: ParameterDirection.Output);

                con.Open();
                con.Execute("paPL_MaeActividadOperativa_Registrar", p, commandType: CommandType.StoredProcedure);
                response.Message = p.Get<string>("vResponse");
                response.Status = p.Get<string>("statusResponse");
                con.Close();

            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627)
                {
                    response.Message = "La clave es única";
                    response.Status = "error";
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = "error";
            }

            return response;
        }


    }
}
