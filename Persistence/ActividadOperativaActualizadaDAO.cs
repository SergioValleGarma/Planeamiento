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
    public class ActividadOperativaActualizadaDAO
    {
        private SqlConnection con;

        private void connection()
        {
            string constr = new MIDIS.Utiles.Crypto().Desencriptar(ConfigurationManager.ConnectionStrings["AppContext"].ToString());
            con = new SqlConnection(constr);
        }

        public List<ActividadOperativaActualizadaViewModel> Listar_Actividad_Operativa(int nAnio, int Codigo_Dependencia)
        {
            connection();
            List<ActividadOperativaActualizadaViewModel> List = new List<ActividadOperativaActualizadaViewModel>();

            try
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("nAnio", nAnio);
                p.Add("iCodigoDependencia", Codigo_Dependencia);

                con.Open();
                List = con.Query<ActividadOperativaActualizadaViewModel>("paPL_ActividadOperativaActualizada_Consultar", p, commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            return List;
        }

        public ActividadOperativaActualizadaViewModel Buscar_Actividad_Operativa(int Id_Actividad_Incr)
        {
            connection();
            ActividadOperativaActualizadaViewModel model = new ActividadOperativaActualizadaViewModel();
            try
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("iCodActividadIncrementable", Id_Actividad_Incr);

                con.Open();
                model = con.QuerySingleOrDefault<ActividadOperativaActualizadaViewModel>(
                    "paPL_ActividadOperativaActualizada_BuscarPorId", p,
                    commandType: CommandType.StoredProcedure);
                con.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return model;
        }

        public GeneralResponse Crear_Actividad_Operativa(ActividadOperativa_Actualizada model)
        {
            GeneralResponse response = new GeneralResponse();
            connection();
            try
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("iCodTabActividadOperativa", model.iCodTabActividadOperativa);
                p.Add("nvCodFinalidadPresupuestal", model.nvCodFinalidadPresupuestal);
                p.Add("nvDenominacionActividad", model.nvDenominacionActividad);
                p.Add("nvDescripcionActividad", model.nvDescripcionActividad);
                p.Add("iCodAccion", model.iCodAccion);
                p.Add("iCodUnidadMedida", model.iCodUnidadMedida);
                p.Add("iCodigoDependencia", model.iCodigoDependencia);
                p.Add("nAnio", model.nAnio);
                p.Add("iCodUsuarioRegistro", model.iCodUsuario);
                p.Add("vResponse", dbType: DbType.String, size: 250, direction: ParameterDirection.Output);
                p.Add("statusResponse", dbType: DbType.String, size: 100, direction: ParameterDirection.Output);

                con.Open();
                con.Execute("paPL_ActividadOperativaActualizada_Registrar", p, commandType: CommandType.StoredProcedure);
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

        public GeneralResponse Actualizar_Actividad_Operativa(ActividadOperativa_Actualizada model)
        {
            GeneralResponse response = new GeneralResponse();
            connection();
            try
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("iCodActividadIncrementable", model.iCodActividadIncrementable);
                p.Add("iCodTabActividadOperativa", model.iCodTabActividadOperativa);
                p.Add("nvCodFinalidadPresupuestal", model.nvCodFinalidadPresupuestal);
                p.Add("nvDenominacionActividad", model.nvDenominacionActividad);
                p.Add("nvDescripcionActividad", model.nvDescripcionActividad);
                p.Add("iCodAccion", model.iCodAccion);
                p.Add("iCodUnidadMedida", model.iCodUnidadMedida);
                p.Add("iCodigoDependencia", model.iCodigoDependencia);
                p.Add("nAnio", model.nAnio);
                p.Add("iCodUsuarioRegistro", model.iCodUsuario);
                p.Add("vResponse", dbType: DbType.String, size: 250, direction: ParameterDirection.Output);
                p.Add("statusResponse", dbType: DbType.String, size: 100, direction: ParameterDirection.Output);

                con.Open();
                con.Execute("paPL_ActividadOperativaActualizada_Actualizar", p, commandType: CommandType.StoredProcedure);
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

        public GeneralResponse Desactivar_Actividad_Operativa(ActividadOperativa_Actualizada model)
        {
            GeneralResponse response = new GeneralResponse();
            connection();
            try
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("iCodActividadIncrementable", model.iCodActividadIncrementable, DbType.Int32);
                p.Add("iCodUsuarioRegistro", model.iCodUsuario, DbType.Int32);
                p.Add("vResponse", dbType: DbType.String, size: 250, direction: ParameterDirection.Output);
                p.Add("statusResponse", dbType: DbType.String, size: 100, direction: ParameterDirection.Output);

                con.Open();
                con.Execute("paPL_ActividadOperativaActualizada_Desactivar", p, commandType: CommandType.StoredProcedure);
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
    }
}
