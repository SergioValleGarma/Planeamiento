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
    public class SeguimientoTareaDAO
    {
        private SqlConnection con;

        private void connection()
        {
            string constr = new MIDIS.Utiles.Crypto().Desencriptar(ConfigurationManager.ConnectionStrings["AppContext"].ToString());
            con = new SqlConnection(constr);
        }

        public List<SeguimientoTareaViewModel> Listar_Seg_Tareas(int Id_Actividad_Incrementable)
        {
            connection();
            List<SeguimientoTareaViewModel> List = new List<SeguimientoTareaViewModel>();
            try
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("Id_Actividad_Incrementable", Id_Actividad_Incrementable);

                con.Open();
                var consultas = con.QueryMultiple("paPL_SegTareaActualizada_BuscarPorActividad", p, commandType: CommandType.StoredProcedure);
                var cabeceras = consultas.Read<Cab_SeguimientoTareaViewModel>().ToList();
                var detalles = consultas.Read<Det_SeguimientoTareaViewModel>().ToList();
                con.Close();

                List = cabeceras.GroupJoin(detalles,
                                            cabecera => cabecera.iCodTarea_Act,
                                            detalle => detalle.iCodTarea_Act,
                                            (cabecera, cabeceraDetalles) => new SeguimientoTareaViewModel
                                            {
                                                Cab_Tarea = cabecera,
                                                Detalle = cabeceraDetalles
                                            }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return List;
        }

        public GeneralResponse Registrar_Seguimiento(SeguimientoFisico model)
        {
            GeneralResponse response = new GeneralResponse();
            connection();
            try
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("iCodTarea_Act", model.iCodTarea_Act);
                p.Add("nMes", model.nMes);
                p.Add("nvJustificacion", model.nvJustificacion);
                p.Add("nCantidad", model.nCantidad);
                p.Add("iCodUsuario", model.iCodUsuario);
                p.Add("vResponse", dbType: DbType.String, size: 250, direction: ParameterDirection.Output);
                p.Add("statusResponse", dbType: DbType.String, size: 100, direction: ParameterDirection.Output);

                con.Open();
                con.Execute("paPL_SegTareaActualizada_Registrar", p, commandType: CommandType.StoredProcedure);
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
