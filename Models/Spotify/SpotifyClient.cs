using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace SpotifyHotkeys.Models.Spotify
{
    public class SpotifyClient
    {
        public string access_token = null;
        public string token_type = null;
        public long expires_in;

        public SpotifyClient()
        {
        }

        public async Task post_authenticateClient()
        {
            if (Globals.client_credentials())
            {
                List<KeyValuePair<string, string>> requestData = new List<KeyValuePair<string, string>>();
                requestData.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
                FormUrlEncodedContent requestBody = new FormUrlEncodedContent(requestData);
                var request = await Globals.http.PostAsync("https://accounts.spotify.com/api/token", requestBody);
                string content = await request.Content.ReadAsStringAsync();
                var json_object = JObject.Parse(content);
                this.access_token = (string)json_object["access_token"];
                this.token_type = (string)json_object["token_type"];
                this.expires_in = (long)json_object["expires_in"];
            }
        }
    }
}
