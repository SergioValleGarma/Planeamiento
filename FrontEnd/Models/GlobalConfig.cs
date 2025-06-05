using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using FrontEnd.Seguridad;

namespace FrontEnd.Models
{
    public static class GlobalConfig
    {
        #region Usuario

        public static int iCodUsuario => int.TryParse(UserManager.Usuario?.iCodUsuario?.ToString(), out int userId) ? userId : 0;

        public static string vUsuarioNombre => UserManager.Usuario?.vUsuarioNombre?.ToLower() ?? "";

        public static string nvDocumento => UserManager.Usuario?.vPersonaNroDocumento?.ToString() ?? "";

        public static string iCodPerfilUsuario => UserManager.Usuario?.perfiles.FirstOrDefault()?.iCodPer.ToString() ?? "";

        public static string iCodDependencia => UserManager.Usuario?.iCodDependencia?.ToString() ?? "";

        public static string vNombreCompleto
        {
            get
            {
                string nombres = UserManager.Usuario?.vPersonaNombres ?? "";
                string apePaterno = UserManager.Usuario?.vPersonaApellidoPaterno ?? "";
                string apeMaterno = UserManager.Usuario?.vPersonaApellidoMaterno ?? "";
                return $"{nombres} {apePaterno} {apeMaterno}";
            }
        }

        public static string vNombrePerfil => UserManager.Usuario?.perfiles.FirstOrDefault()?.vNomPer ?? "";

        #endregion

        #region Perfiles

        public static string iCodOperador => ConfigurationManager.AppSettings["Operador"] ?? "";
        public static string iCodCoordinador => ConfigurationManager.AppSettings["Coordinador"] ?? "";
        public static string iCodSupervisor => ConfigurationManager.AppSettings["Supervisor"] ?? "";

        public static bool isOperador => iCodPerfilUsuario == iCodOperador;
        public static bool isCoordinador => iCodPerfilUsuario == iCodCoordinador;
        public static bool isSupervisor => iCodPerfilUsuario == iCodSupervisor;

        #endregion

        public static Dictionary<string, string> usuariosExternos
        {
            get
            {
                return JsonConvert.DeserializeObject<Dictionary<string, string>>(ConfigurationManager.AppSettings["Usuarios_externos"]) ?? new Dictionary<string, string>();
            }
        }

        public static string unidadOrganica
        {
            get
            {
                string unidadOrganicaTrabajador = iCodDependencia;
                string userUnidadOrganica = usuariosExternos.FirstOrDefault(kvp => kvp.Key.ToLower() == vUsuarioNombre).Value ?? "";
                return !string.IsNullOrEmpty(userUnidadOrganica) ? userUnidadOrganica : unidadOrganicaTrabajador;
            }
        }

    }
}