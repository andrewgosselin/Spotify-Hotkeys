using SpotifyHotkeys.Properties;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace SpotifyHotkeys
{
    class Globals
    {


        public static readonly HttpClient http = new HttpClient();
        public static bool client_credentials()
        {
            string credentials = string.Format("{0}:{1}", Resources.Spotify_Client_ID, Resources.Spotify_Client_Secret);
            http.DefaultRequestHeaders.Accept.Clear();
            http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes(credentials)));
            return true;
        }

        public static bool user_credentials(string access_token)
        {
            string credentials = string.Format("{0}:{1}", Resources.Spotify_Client_ID, Resources.Spotify_Client_Secret);
            http.DefaultRequestHeaders.Accept.Clear();
            http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", access_token);
            return true;
        }

        public static Uri user_generateAuthURL()
        {
            string url = "https://accounts.spotify.com/authorize";
            url += "?response_type=token";
            url += "&client_id=" + WebUtility.UrlEncode(Resources.Spotify_Client_ID);
            url += "&scope=" + WebUtility.UrlEncode(Resources.Spotify_Scope);
            url += "&redirect_uri=" + WebUtility.UrlEncode(Resources.Spotify_Redirect_URL);
            url += "&state=" + WebUtility.UrlEncode(randomString(16));
            return new Uri(url);
        }

        private static string randomString(int size)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            return builder.ToString();
        }
    }
}
