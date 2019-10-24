using SpotifyBoilerplate.Models;
using SpotifyBoilerplate.Models.Spotify;
using SpotifyBoilerplate.Properties;
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

namespace SpotifyBoilerplate.Views
{
    public partial class Initialization : Form
    {
        public SpotifyClient _spotifyClient = null;
        public SpotifyUser _spotifyUser = null;

        public Initialization()
        {
            InitializeComponent();
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
    }
}
