using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using GoogleApi;
using Newtonsoft.Json;

namespace Registracion.Controllers
{
    public class RegistracionController : Controller
    {
        public ActionResult Inicio()
        {
// Evaluar si se puede hacer sin la urll....

            validarLogin(); //Si esta logueado no mostrar registracion.. mostrar home..


            return View();
        }

        public ActionResult LoginGoogle() 
        {
            var Googleurl = "https://accounts.google.com/o/oauth2/auth?response_type=code&redirect_uri=" + GooglePlusAccessToken.googleplus_redirect_url + "&scope=https://www.googleapis.com/auth/userinfo.email%20https://www.googleapis.com/auth/userinfo.profile&client_id=" + GooglePlusAccessToken.googleplus_client_id;
            Response.Redirect(Googleurl);

            return View();
        }

        public bool validarLogin()
        {
            try
            {
                var url = Request.Url.Query;
                string Parameters;
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
                        Parameters = "code=" + code + "&client_id=" + GooglePlusAccessToken.googleplus_client_id + "&client_secret=" + GooglePlusAccessToken.googleplus_client_secret + "&redirect_uri=" + GooglePlusAccessToken.googleplus_redirect_url + "&grant_type=authorization_code";
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
                                //Llenar objeto cliente y retornar true..
                                return true;
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

            return false;
        }
    }

}