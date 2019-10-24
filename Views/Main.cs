using SpotifyBoilerplate.Models;
using SpotifyBoilerplate.Models.Spotify;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpotifyBoilerplate.Views
{
    public partial class Main : Form
    {
        public SpotifyUser _spotifyUser;
        public SpotifyClient _spotifyClient;

        public Hotkeys _hotkeysManager = null;

        public Main()
        {
            InitializeComponent();
            _hotkeysManager = new Hotkeys(this);
        }

        protected override void WndProc(ref Message m)
        {
            // 5. Catch when a HotKey is pressed !
            if (m.Msg == 0x0312)
            {
                _hotkeysManager.handleKeyPress(ref m);
            }

            base.WndProc(ref m);
            
        }
    }
}
