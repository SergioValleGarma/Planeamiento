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
    public class TareaDAO
    {
        private SqlConnection con;

        private void connection()
        {
            string constr = new MIDIS.Utiles.Crypto().Desencriptar(ConfigurationManager.ConnectionStrings["AppContext"].ToString());
            con = new SqlConnection(constr);
        }

        public List<TareaViewModel> Listar_Tareas(int Id_Actividad_Incrementable)
        {
            connection();
            List<TareaViewModel> List = new List<TareaViewModel>();

            try
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("Id_Actividad_Incrementable", Id_Actividad_Incrementable);

                con.Open();
                List = con.Query<TareaViewModel>("paPL_Tarea_BuscarPorActividad", p, commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return List;
        }

        public GeneralResponse Crear_Tarea(Tarea model)
        {
            GeneralResponse response = new GeneralResponse();
            connection();
            try
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("iCodActividadIncrementable", model.iCodActividadIncrementable);
                p.Add("nvDesagregacion", model.nvDesagregacion);
                p.Add("iCodProceso", model.iCodProceso);
                p.Add("iCodUnidadMedida", model.iCodUnidadMedida);
                p.Add("bCePlan", model.bCePlan);
                p.Add("iCodUsuarioRegistro", model.iCodUsuario);
                p.Add("tpListMesesCantidad", dtListaMeses(model.DetalleTarea).AsTableValuedParameter("dbo.tpListaMesesCantidad"));
                p.Add("vResponse", dbType: DbType.String, size: 250, direction: ParameterDirection.Output);
                p.Add("statusResponse", dbType: DbType.String, size: 100, direction: ParameterDirection.Output);
                p.Add("Id_Registro", dbType: DbType.Int32, direction: ParameterDirection.Output);

                con.Open();
                con.Execute("paPL_Tarea_Registrar", p, commandType: CommandType.StoredProcedure);
                response.Message = p.Get<string>("vResponse");
                response.Status = p.Get<string>("statusResponse");
                response.Codigo_Registro = p.Get<int>("Id_Registro").ToString();
                con.Close();
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = "error";
            }

            return response;
        }

        public GeneralResponse Actualizar_Tarea(Tarea model)
        {
            GeneralResponse response = new GeneralResponse();
            connection();
            try
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("iCodTarea", model.iCodTarea);
                p.Add("nvDesagregacion", model.nvDesagregacion);
                p.Add("iCodProceso", model.iCodProceso);
                p.Add("iCodUnidadMedida", model.iCodUnidadMedida);
                p.Add("bCePlan", model.bCePlan);
                p.Add("tpListMesesCantidad", dtListaMeses(model.DetalleTarea).AsTableValuedParameter("dbo.tpListaMesesCantidad"));
                p.Add("iCodUsuarioRegistro", model.iCodUsuario);
                p.Add("vResponse", dbType: DbType.String, size: 250, direction: ParameterDirection.Output);
                p.Add("statusResponse", dbType: DbType.String, size: 100, direction: ParameterDirection.Output);
                p.Add("Id_Registro", dbType: DbType.Int32, direction: ParameterDirection.Output);

                con.Open();
                con.Execute("paPL_Tarea_Actualizar", p, commandType: CommandType.StoredProcedure);
                response.Message = p.Get<string>("vResponse");
                response.Status = p.Get<string>("statusResponse");
                response.Codigo_Registro = p.Get<int>("Id_Registro").ToString();
                con.Close();
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = "error";
            }

            return response;
        }

        public GeneralResponse Desactivar_Tarea(Tarea model)
        {
            GeneralResponse response = new GeneralResponse();
            connection();
            try
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("iCodTarea", model.iCodTarea);
                p.Add("iCodUsuarioRegistro", model.iCodTarea);
                p.Add("vResponse", dbType: DbType.String, size: 250, direction: ParameterDirection.Output);
                p.Add("statusResponse", dbType: DbType.String, size: 100, direction: ParameterDirection.Output);

                con.Open();
                con.Execute("paPL_Tarea_Desactivar", p, commandType: CommandType.StoredProcedure);
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

        public ProcesoViewModel Listar_Procesos(string vBuscar, int viewRows, int page = 1)
        {
            connection();
            ProcesoViewModel response = new ProcesoViewModel();

            int nPageInit = (page * viewRows) - viewRows;

            try
            {
                DynamicParameters p1 = new DynamicParameters();

                p1.Add("vBuscar", vBuscar);
                p1.Add("nPageInit", nPageInit);
                p1.Add("nPageFin", viewRows);
                con.Open();
                List<Proceso> List = con.Query<Proceso>("paPL_Procesos_Consultar", p1, commandType: CommandType.StoredProcedure).ToList();
                con.Close();

                response.item = List;

                DynamicParameters p2 = new DynamicParameters();
                p2.Add("vBuscar", vBuscar);
                con.Open();
                response.total_count = con.ExecuteScalar<int>("paPL_ProcesosConsultarCountRowsTotal", p2, commandType: CommandType.StoredProcedure);
                con.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return response;
        }


        public UnidadMedidaViewModel Listar_Unidad_Medida(string vBuscar, int viewRows, int page = 1)
        {
            connection();
            UnidadMedidaViewModel response = new UnidadMedidaViewModel();

            int nPageInit = (page * viewRows) - viewRows;

            try
            {
                DynamicParameters p1 = new DynamicParameters();

                p1.Add("vBuscar", vBuscar);
                p1.Add("nPageInit", nPageInit);
                p1.Add("nPageFin", viewRows);
                con.Open();
                List<UnidadMedida> List = con.Query<UnidadMedida>("paPL_UnidadMedida_Consultar", p1, commandType: CommandType.StoredProcedure).ToList();
                con.Close();

                response.item = List;

                DynamicParameters p2 = new DynamicParameters();
                p2.Add("vBuscar", vBuscar);
                con.Open();
                response.total_count = con.ExecuteScalar<int>("paPL_UnidadMedidaConsultarCountRowsTotal", p2, commandType: CommandType.StoredProcedure);
                con.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return response;
        }


        private static DataTable dtListaMeses(List<DetTarea> listaItem)
        {

            DataTable dtListaMeses = new DataTable();
            dtListaMeses.Columns.Add("nMes", typeof(int));
            dtListaMeses.Columns.Add("nCantidad", typeof(int));
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
