using Newtonsoft.Json.Linq;
using SpotifyBoilerplate.Properties;
using SpotifyBoilerplate.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace SpotifyBoilerplate.Models.Spotify
{
    public class SpotifyUser
    {
        public string access_token = null;
        public string token_type = null;
        public long expires_in;
        public string state;

        public async Task get_authenticateUser(Initialization initializationForm)
        {
            initializationForm.webBrowser_authentication.Url = Globals.user_generateAuthURL();
        }

        public async Task parseUserInformation(string url, Initialization initializationForm)
        {
            if (url.Contains(Resources.Spotify_Redirect_URL + "#"))
            {
                var query_string = url.Replace(Resources.Spotify_Redirect_URL + "#", "");
                var dict = HttpUtility.ParseQueryString(query_string);
                var json = new JavaScriptSerializer().Serialize(
                    dict.AllKeys.ToDictionary(k => k, k => dict[k])
                );
                var json_object = JObject.Parse(json);
                this.access_token = (string)json_object["access_token"];
                this.token_type = (string)json_object["token_type"];
                this.expires_in = (long)json_object["expires_in"];
                this.state = (string)json_object["state"];
            }
            else {
                MessageBox.Show("You must accept the Spotify Authorization.");
                Application.Restart();
            }

        }
    }
}
