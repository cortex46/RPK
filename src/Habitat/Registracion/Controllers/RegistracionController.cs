using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

//Para login con google
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using GoogleApi;

namespace Registracion.Controllers
{
    public class RegistracionController : Controller
    {
        // Pasar a Clase Google
        protected string googleplus_client_id = "110144614287-bcinde4obkpuotu2tqbpumd2ttta71sq.apps.googleusercontent.com";
        protected string googleplus_client_secret = "XKTAaw-Wh9OsIl6eF0ijiF7H";
        protected string googleplus_redirect_url = "http://localhost:56055/Home/Login_Inicio";                                         // Replace this with your Redirect URL; Your Redirect URL from your developer.google application should match this URL.
        protected string Parameters;

        public ActionResult Inicio()
        {
            validarLogin(); //Si esta logueado no mostrar registracion..
            return View();
        }

        public ActionResult LoginGoogle() 
        {
            var Googleurl = "https://accounts.google.com/o/oauth2/auth?response_type=code&redirect_uri=" + googleplus_redirect_url + "&scope=https://www.googleapis.com/auth/userinfo.email%20https://www.googleapis.com/auth/userinfo.profile&client_id=" + googleplus_client_id;
            Response.Redirect(Googleurl);

            return View();
        }

        public bool validarLogin()
        {
            if ((Session.Contents.Count > 0) && (Session["loginWith"] != null) && (Session["loginWith"].ToString() == "google"))
            {
                try
                {
                    var url = Request.Url.Query;
                    if (url != "")
                    {
                        string queryString = url.ToString();
                        char[] delimiterChars = { '=' };
                        string[] words = queryString.Split(delimiterChars);
                        string code = words[1];

                        if (code != null)
                        {
                            //get the access token 
                            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create("https://accounts.google.com/o/oauth2/token");
                            webRequest.Method = "POST";
                            Parameters = "code=" + code + "&client_id=" + googleplus_client_id + "&client_secret=" + googleplus_client_secret + "&redirect_uri=" + googleplus_redirect_url + "&grant_type=authorization_code";
                            byte[] byteArray = Encoding.UTF8.GetBytes(Parameters);
                            webRequest.ContentType = "application/x-www-form-urlencoded";
                            webRequest.ContentLength = byteArray.Length;
                            Stream postStream = webRequest.GetRequestStream();
                            // Add the post data to the web request
                            postStream.Write(byteArray, 0, byteArray.Length);
                            postStream.Close();

                            WebResponse response = webRequest.GetResponse();
                            postStream = response.GetResponseStream();
                            StreamReader reader = new StreamReader(postStream);
                            string responseFromServer = reader.ReadToEnd();

                            GooglePlusAccessToken serStatus = JsonConvert.DeserializeObject<GooglePlusAccessToken>(responseFromServer);

                            if (serStatus != null)
                            {
                                string accessToken = string.Empty;
                                accessToken = serStatus.access_token;

                                if (!string.IsNullOrEmpty(accessToken))
                                {
                                    // This is where you want to add the code if login is successful.
                                    // getgoogleplususerdataSer(accessToken);
                                }
                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    //throw new Exception(ex.Message, ex);
                    Response.Redirect("index.aspx");
                }
            }

            return true;
        }
    }

}