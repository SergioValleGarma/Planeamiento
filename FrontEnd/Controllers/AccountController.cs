using Common.Auth;

using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FrontEnd.Models;

namespace FrontEnd.Controllers
{
    public class AccountController : Controller
    {
        private static readonly string enviroment = ConfigurationManager.AppSettings.Get("Lx_EndPoint");
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly string LxEndPoint = ConfigurationManager.AppSettings.Get("Lx_EndPoint");

        [HttpGet]
        public ActionResult Login()
        {
            string AuthenticationUrl = ConfigurationManager.AppSettings.Get("LoginEndPoint");
            string iCodAplicacion = ConfigurationManager.AppSettings.Get("iCodAplicacion");
            return Redirect(AuthenticationUrl + $"?id=" + iCodAplicacion);
        }

        [HttpGet]
        public async Task<ActionResult> Connect(string access_token)
        {
            #region Perfiles - Cargos heredados de SISPAD
            //iCodPerfil	vNombrePerfil
            //  13          IDM-OPERADOR
            //  14          IDM-ADMINISTRADOR
            #endregion

            #region Instancias
            ApiUser user;
            string[] token; // for split 3 parts
            string base64Content; // base64 ready for deserialize
            #endregion

            try
            {
                token = access_token.Split('.');
                int mod4 = token[1].Length % 4;
                if (mod4 > 0)
                {
                    token[1] += new string('=', 4 - mod4);
                }

                base64Content = Encoding.UTF8.GetString(Convert.FromBase64String(token[1]));
                base64Content = base64Content.Replace("\"[", "[").Replace("]\"", "]").Replace("\\", "");
                user = JsonConvert.DeserializeObject<ApiUser>(base64Content);

                var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier, user.iCodUsuario),
                            new Claim(ClaimTypes.Name, user.vUsuarioNombre),
                            new Claim(ClaimTypes.Actor, user.perfiles[0].vNomPer),
                            new Claim(ClaimTypes.UserData, JsonConvert.SerializeObject(user))
                        };
                var claimsIdentity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
                HttpContext.GetOwinContext().Authentication.SignIn(new AuthenticationProperties { IsPersistent = false }, claimsIdentity);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
            }
            return RedirectToAction("Index", "Planeamiento");
        }

        //[HttpGet]
        //public async Task<ActionResult> Connect(string access_token)
        //{
        //    #region Perfiles - Cargos heredados de SISPAD
        //    //iCodPerfil	vNombrePerfil
        //    //  13          IDM-OPERADOR
        //    //  14          IDM-ADMINISTRADOR
        //    #endregion

        //    #region Instancias
        //    ApiUser user;
        //    #endregion

        //    try
        //    {
        //        var client = new HttpClient();
        //        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", access_token);
        //        var result = await client.PostAsync($"{LxEndPoint}v1/identity/validate_token", null);
        //        if (!result.IsSuccessStatusCode)
        //        {
        //            return Redirect("Login");
        //        }

        //        string responseBody = await result.Content.ReadAsStringAsync();
        //        mServiceResponse response = JsonConvert.DeserializeObject<mServiceResponse>(responseBody);
        //        if (response.statusCode != "200")
        //        {
        //            return Redirect("Login");
        //        }

        //        user = response.person;
        //        var claims = new List<Claim>
        //                {
        //                    new Claim(ClaimTypes.NameIdentifier, user.iCodUsuario),
        //                    new Claim(ClaimTypes.Name, user.vUsuarioNombre),
        //                    new Claim(ClaimTypes.Actor, user.perfiles[0].vNomPer),
        //                    new Claim(ClaimTypes.UserData, JsonConvert.SerializeObject(user))
        //                    //new Claim("access_token", access_token)
        //                };
        //        var claimsIdentity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
        //        HttpContext.GetOwinContext().Authentication.SignIn(new AuthenticationProperties { IsPersistent = false }, claimsIdentity);
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex.ToString());
        //    }
        //    return RedirectToAction("Index", "Planeamiento");
        //}

        public ActionResult Logout()
        {
            HttpContext.GetOwinContext().Authentication.SignOut();
            TempData.Clear();
            string AuthenticationUrl = ConfigurationManager.AppSettings.Get("LoginEndPoint");
            string iCodAplicacion = ConfigurationManager.AppSettings.Get("iCodAplicacion");
            return Redirect(AuthenticationUrl + $"?id=" + iCodAplicacion);
            //return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public ActionResult ChangePasswordResult()
        {
            Session.Abandon();
            return View();
        }
    }
}