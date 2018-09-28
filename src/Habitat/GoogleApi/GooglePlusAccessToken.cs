using System;

namespace GoogleApi
{
    public class GooglePlusAccessToken
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string id_token { get; set; }
        public string refresh_token { get; set; }

        // Pasar a Clase Google
        public const string googleplus_client_id = "110144614287-bcinde4obkpuotu2tqbpumd2ttta71sq.apps.googleusercontent.com";
        public const string googleplus_client_secret = "XKTAaw-Wh9OsIl6eF0ijiF7H";
        public const string googleplus_redirect_url = "http://localhost:56055/Home/Login_Inicio";                                         // Replace this with your Redirect URL; Your Redirect URL from your developer.google application should match this URL.


    }
}
