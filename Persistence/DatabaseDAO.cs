using Common.Models;
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
    public class DatabaseDAO
    {
        private SqlConnection con;

        private void connection()
        {
            string constr = new MIDIS.Utiles.Crypto().Desencriptar(ConfigurationManager.ConnectionStrings["AppContext"].ToString());
            con = new SqlConnection(constr);
        }

        public List<Database> Listar(int nAnio)
        {
            connection();
            List<Database> List = new List<Database>();

            try
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("nAnio", nAnio);

                con.Open();
                List = con.Query<Database>("paPL_Consulta_BD", p, commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return List;
        }

        public List<ReporteSemaforo> ConsultarReporte(int iCodDependencia, int nAnio, int nMes)
        {
            connection();
            List<ReporteSemaforo> List = new List<ReporteSemaforo>();

            try
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("iCodDependencia", iCodDependencia);
                p.Add("nAnio", nAnio);
                p.Add("Mes", nMes);

                con.Open();
                List = con.Query<ReporteSemaforo>("paPL_ReporteSemaforo_Consultar", p, commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return List;
        }

        public List<ParametrosReporteSemaforo> ConsultarParametrosAnualSemaforo(int nMes)
        {
            connection();
            List<ParametrosReporteSemaforo> List = new List<ParametrosReporteSemaforo>();

            try
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("nMes", nMes);

                con.Open();
                List = con.Query<ParametrosReporteSemaforo>("pa_PLReporteParametrosAnualSemaforo_Consultar", p, commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return List;
        }

        public List<ParametrosReporteSemaforo> ConsultarParametrosMensualSemaforo()
        {
            connection();
            List<ParametrosReporteSemaforo> List = new List<ParametrosReporteSemaforo>();

            try
            {
                con.Open();
                List = con.Query<ParametrosReporteSemaforo>("pa_PLReporteParametrosRangoMensualSemaforo_Consultar", null, commandType: CommandType.StoredProcedure).ToList();
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
