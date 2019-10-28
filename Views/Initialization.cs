using Microsoft.Win32;
using SpotifyHotkeys.Models;
using SpotifyHotkeys.Models.Spotify;
using SpotifyHotkeys.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpotifyHotkeys.Views
{
    public partial class Initialization : Form
    {
        public SpotifyClient _spotifyClient = null;
        public SpotifyUser _spotifyUser = null;

        public Initialization()
        {
            InitializeComponent();
            SetRegistry();
            startSpotify();
        }

        public async void startSpotify() {
            _spotifyClient = new SpotifyClient();
            _spotifyUser = new SpotifyUser();

            // Authenticating Client
            await Task.Run(() => _spotifyClient.post_authenticateClient());
            // Authenticating User
            await Task.Run(() => _spotifyUser.get_authenticateUser(this));
        }

        private async void webBrowser_authentication_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            // Checking for the Redirect URL from Spotify
            if (e.Url.ToString().Contains(Resources.Spotify_Redirect_URL))
            {
                await Task.Run(() => _spotifyUser.parseUserInformation(e.Url.ToString(), this));

                // Instantiating the authorized Main View
                Main mainView = new Main();
                mainView._spotifyClient = this._spotifyClient;
                mainView._spotifyUser = this._spotifyUser;

                // Cycling Views
                this.Hide();
                mainView.Show();
                mainView.Activate();
            }
            
        }

        private bool SetRegistry()
        {
            try
            {
                using (var hklm = RegistryKey.OpenBaseKey(RegistryHive.ClassesRoot, RegistryView.Registry64))
                {
                    using (RegistryKey key = hklm.OpenSubKey(@"MIME\Database\Content Type\application/json", true))
                    {
                        if (key != null)
                        {
                            key.SetValue("CLSID", "{25336920-03F9-11cf-8FD0-00AA00686F13}");
                            key.SetValue("Encoding", new byte[] { 0x80, 0x00, 0x00, 0x00 });
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return false;
        }
    }
}
