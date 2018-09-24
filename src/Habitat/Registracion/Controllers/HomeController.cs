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
using System.Threading.Tasks;
using Registracion.Models;
using GoogleApi;

namespace Registracion.Controllers
{
    public class HomeController : Controller
    {
        // Pasar a Clase Google
        protected string googleplus_client_id = "110144614287-bcinde4obkpuotu2tqbpumd2ttta71sq.apps.googleusercontent.com";
        protected string googleplus_client_secret = "XKTAaw-Wh9OsIl6eF0ijiF7H";
        protected string googleplus_redirect_url = "http://localhost:56055/Home/Login_Inicio";     // Replace this with your Redirect URL; Your Redirect URL from your developer.google application should match this URL.
        protected string Parameters;

        public async Task getgoogleplususerdataSer(string access_token, DatosGoogle datosUser)
        {
            try
            {
                HttpClient client = new HttpClient();
                var urlProfile = "https://www.googleapis.com/oauth2/v1/userinfo?access_token=" + access_token;

                client.CancelPendingRequests();
                HttpResponseMessage output = await client.GetAsync(urlProfile);

                if (output.IsSuccessStatusCode)
                {
                    //GoogleUserOutputData clase = new GoogleUserOutputData();
                    string outputData = await output.Content.ReadAsStringAsync();
                    GoogleUserOutputData serStatus = JsonConvert.DeserializeObject<GoogleUserOutputData>(outputData);

                    if (serStatus != null)
                    {
                        datosUser.nombre = serStatus.name;
                        datosUser.urlFoto = serStatus.picture;
                        datosUser.mail = serStatus.email; 
                    }
                }
            }
            catch (Exception ex)
            {
                //catching the exception
            }
        }

        public async Task<ActionResult> Login_Inicio()
        {
            DatosGoogle vm = new DatosGoogle();
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
                                await getgoogleplususerdataSer(accessToken, vm);
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

            return View(vm);
        }


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About ()
        {

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

      
        public async Task<ActionResult> Home_inicio()
        {
            //if ((Session.Contents.Count > 0) && (Session["loginWith"] != null) && (Session["loginWith"].ToString() == "google"))

                return View();
        }
    }
}