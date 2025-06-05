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
    public class BloqueoDAO
    {
        private SqlConnection con;

        private void connection()
        {
            string constr = new MIDIS.Utiles.Crypto().Desencriptar(ConfigurationManager.ConnectionStrings["AppContext"].ToString());
            con = new SqlConnection(constr);
        }

        public List<BloqueoViewModel> Listar(int Id_Modulo, int nAnio)
        {
            connection();
            List<BloqueoViewModel> List = new List<BloqueoViewModel>();

            try
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("Id_Modulo", Id_Modulo);
                p.Add("nAnio", nAnio);

                con.Open();
                List = con.Query<BloqueoViewModel>("paPL_Bloqueo_Consultar", p, commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return List;
        }

        public GeneralResponse Registrar_Bloqueo(Bloqueo model)
        {
            GeneralResponse response = new GeneralResponse();
            connection();
            try
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("nAnio", model.nAnio);
                p.Add("iCodModulo", model.iCodModulo);
                p.Add("iCodUsuario", model.iCodUsuario);
                p.Add("tpListaBloqueo", dtDetalleBloqueo(model.DetalleBloqueo).AsTableValuedParameter("dbo.tpListaBloqueo"));
                p.Add("vResponse", dbType: DbType.String, size: 250, direction: ParameterDirection.Output);
                p.Add("statusResponse", dbType: DbType.String, size: 100, direction: ParameterDirection.Output);

                con.Open();
                con.Execute("paPL_Bloqueo_Registrar", p, commandType: CommandType.StoredProcedure);
                response.Message = p.Get<string>("vResponse");
                response.Status = p.Get<string>("statusResponse");
                con.Close();
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = "error";
            }

            return response;
        }

        public List<DetalleBloqueo> Listar_DetalleBloqueo(int iCodigoDependencia, int nAnio, int iCodModulo)
        {
            connection();
            List<DetalleBloqueo> List = new List<DetalleBloqueo>();

            try
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("iCodigoDependencia", iCodigoDependencia);
                p.Add("nAnio", nAnio);
                p.Add("iCodModulo", iCodModulo);

                con.Open();
                List = con.Query<DetalleBloqueo>("paPL_Bloqueo_BuscarDetBloqueo", p, commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return List;
        }

        private static DataTable dtDetalleBloqueo(List<DetBloqueo> listaItem)
        {

            DataTable dtListaMeses = new DataTable();
            dtListaMeses.Columns.Add("iCodigoDependencia", typeof(int));
            dtListaMeses.Columns.Add("nMes", typeof(int));
            dtListaMeses.Columns.Add("bBloqueado", typeof(bool));
            foreach (var m in listaItem)
            {
                dtListaMeses.Rows.Add(
                    m.iCodigoDependencia,
                    m.nMes,
                    m.bBloqueado
                );
            }

            return dtListaMeses;
        }
    }
}
