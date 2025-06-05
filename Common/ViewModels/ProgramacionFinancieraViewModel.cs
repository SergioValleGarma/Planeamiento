using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.ViewModels
{
    public class ProgramacionFinancieraViewModel
    {
        public Int64 Id_ProgramacionFinanciera { get; set; }
        public string Categoria_Presupuestal { get; set; }
        public string Producto { get; set; }
        public string Actividad_Presupuestal { get; set; }
        public string Finalidad_Presupuestal { get; set; }
        public string Fuente_Financiamiento { get; set; }
        public string Codigo_Generica_Gasto { get; set; }
        public string Generica_Gasto { get; set; }
        public string Codigo_Clasificador { get; set; }
        public string Clasificador { get; set; }
        public decimal Monto_PIA { get; set; }
        public decimal Monto_ActividadOperativa { get; set; }
        public decimal Ene { get; set; }
        public decimal Feb { get; set; }
        public decimal Mar { get; set; }
        public decimal Abr { get; set; }
        public decimal May { get; set; }
        public decimal Jun { get; set; }
        public decimal Jul { get; set; }
        public decimal Ago { get; set; }
        public decimal Set { get; set; }
        public decimal Oct { get; set; }
        public decimal Nov { get; set; }
        public decimal Dic { get; set; }
        public decimal Total { get; set; }
        public decimal Falta_Registrar => Monto_ActividadOperativa - Total;
        public string Clasificador_Completo => $"{Codigo_Clasificador} {Clasificador}";
    }

    public class ProgramacionFinancieraActualizadaViewModel
    {
        public string Codigo_Finalidad { get; set; }
        public decimal PIA { get; set; }
        public decimal PIM { get; set; }
        public string Meta { get; set; } = null;
        public string Sec_Func { get; set; } = null;
        public decimal Certificado { get; set; }
        public decimal Devengado { get; set; }
        public string Clasificador { get; set; }
        public string Descripcion_Clasificador { get; set; }
        public string Clasificador_Gasto { get; set; }
        public string Fuente_Financiamiento { get; set; }
        public decimal Monto_Devengado_01 { get; set; }
        public decimal Monto_Devengado_02 { get; set; }
        public decimal Monto_Devengado_03 { get; set; }
        public decimal Monto_Devengado_04 { get; set; }
        public decimal Monto_Devengado_05 { get; set; }
        public decimal Monto_Devengado_06 { get; set; }
        public decimal Monto_Devengado_07 { get; set; }
        public decimal Monto_Devengado_08 { get; set; }
        public decimal Monto_Devengado_09 { get; set; }
        public decimal Monto_Devengado_10 { get; set; }
        public decimal Monto_Devengado_11 { get; set; }
        public decimal Monto_Devengado_12 { get; set; }
        public int nAnio { get; set; }
        public string Generica_Gasto {
            get {
                int secondDotIndex = Clasificador.IndexOf('.', Clasificador.IndexOf('.') + 1);
                string result = secondDotIndex != -1 ? Clasificador.Substring(0, secondDotIndex) : Clasificador;
                return result;        
            }
        }

    }

    public class ProgramacionFinancieraTotalViewModel
    {
        public decimal TotalPIA { get; set; }
        public decimal TotalUsado { get; set; }
    }

    public class ProgramacionFinancieraCreateViewModel
    {
        [Required]
        public Int64 Id_Programacion { get; set; }
        [Required]
        public int Id_Actividad { get; set; }

        [Required(ErrorMessage = "Debe ingresar monto de actividad operativa")]
        [Range(0.00, 99999999.99, ErrorMessage = "El valor debe ser entre 0.0 hasta 9,999,999.99")]
        public decimal Monto_ActividadOperativa { get; set; }
        public List<DetalleProgramacion> listaMeses { get; set; }
    }
    public class DetalleProgramacion
    {
        public int Mes { get; set; }
        public decimal Cantidad { get; set; }
    }
}
