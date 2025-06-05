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
    public class ProgramacionFinancieraDAO
    {
        private SqlConnection con;

        private void connection()
        {
            string constr = new MIDIS.Utiles.Crypto().Desencriptar(ConfigurationManager.ConnectionStrings["AppContext"].ToString());
            con = new SqlConnection(constr);
        }

        public List<ProgramacionFinancieraViewModel> Listar_Programacion(int Id_Actividad_Incrementable, int nAnio)
        {
            connection();
            List<ProgramacionFinancieraViewModel> List = new List<ProgramacionFinancieraViewModel>();

            try
            {

                DynamicParameters p = new DynamicParameters();
                p.Add("iCodActividadIncrementable", Id_Actividad_Incrementable);
                p.Add("nAnio", nAnio);

                con.Open();
                List = con.Query<ProgramacionFinancieraViewModel>("paPL_ProgramacionFinanciera_BuscarPorActividad", p, commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return List;
        }

        public ProgramacionFinancieraTotalViewModel Consultar_Total(int Id_Actividad_Incrementable)
        {
            connection();
            ProgramacionFinancieraTotalViewModel result = new ProgramacionFinancieraTotalViewModel();

            try
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("iCodActividadIncrementable", Id_Actividad_Incrementable);

                con.Open();
                result = con.QuerySingleOrDefault<ProgramacionFinancieraTotalViewModel>("paPL_ProgramacionFinanciera_ConsultarTotal", p, commandType: CommandType.StoredProcedure);
                con.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        public List<ProgramacionFinancieraActualizadaViewModel> Listar_ProgramacionActualizada(int Id_Actividad_Incrementable, int nAnio)
        {
            connection();
            List<ProgramacionFinancieraActualizadaViewModel> List = new List<ProgramacionFinancieraActualizadaViewModel>();

            try
            {

                DynamicParameters p = new DynamicParameters();
                p.Add("nAnio", nAnio);
                p.Add("iCodActividadIncrementable", Id_Actividad_Incrementable);
                p.Add("nAnio", nAnio);

                con.Open();
                List = con.Query<ProgramacionFinancieraActualizadaViewModel>("paPL_ProgramacionFinancieraActualizada_Consultar", p, commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return List;
        }

        public GeneralResponse Actualizar_Programacion(ProgramacionFinanciera model)
        {
            GeneralResponse response = new GeneralResponse();
            connection();
            try
            {
                DynamicParameters p = new DynamicParameters();

                p.Add("iCodDetProgamacionFinanciera", model.iCodProgramacionFinanciera);
                p.Add("nTechoActividadOperativa", model.nTechoActividadOperativa);
                p.Add("tpListMesesCantidad", dtListaMeses(model.DetalleProgramacion).AsTableValuedParameter("dbo.tpListaProgramacionListaMeses"));
                p.Add("iCodUsuario", model.iCodUsuario);
                p.Add("vResponse", dbType: DbType.String, size: 250, direction: ParameterDirection.Output);
                p.Add("statusResponse", dbType: DbType.String, size: 100, direction: ParameterDirection.Output);


                con.Open();
                con.Execute("paPL_ProgramacionFinanciera_Actualizar", p, commandType: CommandType.StoredProcedure);
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

        public GeneralResponse Desactivar_Programacion(ProgramacionFinanciera model)
        {
            GeneralResponse response = new GeneralResponse();
            connection();
            try
            {
                DynamicParameters p = new DynamicParameters();

                SqlCommand com = new SqlCommand("paPL_ProgramacionFinanciera_Desactivar", con);
                p.Add("iCodDetProgamacionFinanciera", model.iCodProgramacionFinanciera);
                p.Add("iCodUsuario", model.iCodUsuario);
                p.Add("vResponse", dbType: DbType.String, size: 250, direction: ParameterDirection.Output);
                p.Add("statusResponse", dbType: DbType.String, size: 100, direction: ParameterDirection.Output);

                con.Open();
                con.Execute("paPL_ProgramacionFinanciera_Desactivar", p, commandType: CommandType.StoredProcedure);
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

        private static DataTable dtListaMeses(List<DetProgramacionFinanciera> listaItem)
        {

            DataTable dtListaMeses = new DataTable();
            dtListaMeses.Columns.Add("nMes", typeof(int));
            dtListaMeses.Columns.Add("nCantidad", typeof(float));
            foreach (var m in listaItem)
            {
                dtListaMeses.Rows.Add(
                    m.nMes,
                    m.nCantidad
                );
            }

            return dtListaMeses;
        }
    }
}
