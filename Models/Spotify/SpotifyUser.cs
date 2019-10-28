using Newtonsoft.Json.Linq;
using SpotifyHotkeys.Properties;
using SpotifyHotkeys.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace SpotifyHotkeys.Models.Spotify
{
    public class SpotifyUser
    {
        public string display_name, email, id, images = "";
        public string access_token = null;
        public string token_type = null;
        public long expires_in;
        public string state;

        public JObject currentDevice;

        public async Task get_authenticateUser(Initialization initializationForm)
        {
            initializationForm.webBrowser_authentication.Url = Globals.user_generateAuthURL();
        }

        public async Task get_userInformation() {
            Globals.user_credentials(this.access_token);
            var request = await Globals.http.GetAsync("https://api.spotify.com/v1/me");
            string content = await request.Content.ReadAsStringAsync();
            try
            {
                var json_object = JObject.Parse(content);
                Debug.WriteLine(json_object);
                this.display_name = (string)json_object["display_name"];
                this.id = (string)json_object["id"];
                this.email = (string)json_object["email"];
                this.images = (string)json_object["images"];
            }
            catch (Exception e)
            {
                Debug.WriteLine("User not found.");
            }
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
                await Task.Run(() => this.get_userInformation());
            }
            else {
                MessageBox.Show("You must accept the Spotify Authorization.");
                Application.Restart();
            }

        }

        public async Task<JObject> post_getCurrentlyPlayingSong()
        {
            Globals.user_credentials(this.access_token);
            var request = await Globals.http.GetAsync("https://api.spotify.com/v1/me/player/currently-playing");
            string content = await request.Content.ReadAsStringAsync();
            //
            try
            {
                var json_object = JObject.Parse(content);
                return json_object;
            }
            catch (Exception e) {
                Debug.WriteLine("No song playing.");
                return null;
            }
            
        }

        public async Task post_addSongToPlaylist(int playlistNumber, JObject song, Main form) {
            Globals.user_credentials(this.access_token);
            var request = await Globals.http.GetAsync("https://api.spotify.com/v1/me/playlists");
            string content = await request.Content.ReadAsStringAsync();
            try
            {
                var json_object = JObject.Parse(content);
                var playlistObj = json_object["items"][playlistNumber - 1];

                bool songExists = await this.get_checkPlaylistContainsSong(playlistObj["id"].ToString(), song["item"]["id"].ToString());

                if ((bool)playlistObj["collaborative"] || (string)playlistObj["owner"]["id"] == this.id && !songExists)
                {
                    Globals.user_credentials(this.access_token);
                    List<KeyValuePair<string, string>> requestData = new List<KeyValuePair<string, string>>();
                    FormUrlEncodedContent requestBody = new FormUrlEncodedContent(requestData);
                    request = await Globals.http.PostAsync("https://api.spotify.com/v1/playlists/" + playlistObj["id"].ToString() + "/tracks?uris=" + song["item"]["uri"].ToString(), requestBody);
                    content = await request.Content.ReadAsStringAsync();
                    System.IO.Stream sound = Properties.Resources.notify;
                    System.Media.SoundPlayer snd = new System.Media.SoundPlayer(sound);
                    snd.Play();
                }
                else {
                    System.IO.Stream sound = Properties.Resources.fail;
                    System.Media.SoundPlayer snd = new System.Media.SoundPlayer(sound);
                    snd.Play();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error.");
            }

        }

        public async Task put_nextSong()
        {
            await get_currentPlayback();

            string deviceID = (string)currentDevice["id"];


            List<KeyValuePair<string, string>> requestData = new List<KeyValuePair<string, string>>();
            FormUrlEncodedContent requestBody = new FormUrlEncodedContent(requestData);
            var request = await Globals.http.PostAsync("https://api.spotify.com/v1/me/playlists", requestBody);
            var content = await request.Content.ReadAsStringAsync();
            try
            {
                var json_object = JObject.Parse(content);
                Debug.WriteLine(json_object);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error.");
            }

        }

        public async Task get_currentPlayback()
        {
            Globals.user_credentials(this.access_token);
            var request = await Globals.http.GetAsync("https://api.spotify.com/v1/me/player");
            string content = await request.Content.ReadAsStringAsync();
            try
            {
                var json_object = JObject.Parse(content);
                currentDevice = (JObject)json_object["device"];
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error.");
            }

        }

        public async Task<bool> get_checkPlaylistContainsSong(string playlist_id, string song_id)
        {
            Globals.user_credentials(this.access_token);
            var request = await Globals.http.GetAsync("https://api.spotify.com/v1/playlists/" + playlist_id + "/tracks");
            string content = await request.Content.ReadAsStringAsync();
            try
            {
                var json_object = JObject.Parse(content);
                JArray songs = (JArray)json_object["items"];
                for (int i = 0; i < songs.Count; i++) {
                    if (songs[i]["track"]["id"].ToString() == song_id) {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error.");
                return true;
            }

        }
    }
}
