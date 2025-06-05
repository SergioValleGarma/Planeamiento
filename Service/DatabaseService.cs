using Common.Models;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IDatabaseService
    {
        List<Database> Listar(int anio);
        List<ReporteSemaforo> ConsultarReporte(int iCodDependencia, int nAnio, int nMes);
        List<ParametrosReporteSemaforo> ConsultarParametrosAnualSemaforo(int nMes);
        List<ParametrosReporteSemaforo> ConsultarParametrosMensualSemaforo();

    }
    public class DatabaseService : IDatabaseService
    {
        private readonly DatabaseDAO _DatabaseDAO = new DatabaseDAO();

        public List<Database> Listar(int anio)
        {
            return _DatabaseDAO.Listar(anio);
        }

        public List<ReporteSemaforo> ConsultarReporte(int iCodDependencia, int nAnio, int nMes)
        {
            return _DatabaseDAO.ConsultarReporte(iCodDependencia, nAnio, nMes);
        }
        public List<ParametrosReporteSemaforo> ConsultarParametrosAnualSemaforo(int nMes)
        {
            return _DatabaseDAO.ConsultarParametrosAnualSemaforo(nMes);
        }

        public List<ParametrosReporteSemaforo> ConsultarParametrosMensualSemaforo()
        {
            return _DatabaseDAO.ConsultarParametrosMensualSemaforo();
        }
    }
}
