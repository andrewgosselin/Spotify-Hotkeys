using SpotifyHotkeys.Models;
using SpotifyHotkeys.Models.Hotkeys;
using SpotifyHotkeys.Models.Spotify;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpotifyHotkeys.Views
{
    public partial class Main : Form
    {
        public SpotifyUser _spotifyUser;
        public SpotifyClient _spotifyClient;

        public HotkeyManager _hotkeysManager = null;

        public string functionKeyPressed = "none";
        public bool hotkeyExecuting = false;

        public ContextMenu contextMenu1 = new ContextMenu();

        public Main()
        {
            InitializeComponent();
            _hotkeysManager = new HotkeyManager(this);
            _hotkeysManager.registerHotkeys();
            setupTrayIcon();
            view_home.Visible = false;
            view_settings.Visible = false;

        }
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == Models.Hotkeys.Constants.WM_HOTKEY_MSG_ID)
                _hotkeysManager.handleHotkey(m);
            base.WndProc(ref m);
        }



        // -- Navigation events
        private void navigate_home(object sender, EventArgs e)
        {
            view_settings.Visible = false;
            view_home.Visible = true;
            label_currentView.Text = "Home";
        }
        private void navigate_settings(object sender, EventArgs e)
        {
            view_home.Visible = false;
            view_settings.Visible = true;
            label_currentView.Text = "Settings";
        }
        private void navigate_about(object sender, EventArgs e)
        {
            label_currentView.Text = "About";
        }
        private void navigate_exit(object sender, EventArgs e)
        {
            exitWindow(null, null);
        }

        // -- Window events
        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            _hotkeysManager.unregisterHotkeys();
            notifyIcon1.Visible = false;
        }

        private void Main_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                minimizeWindow(null, null);
            }
        }
        private void maximizeWindow(object sender, MouseEventArgs e)
        {
            Show();
            Activate();
            this.WindowState = FormWindowState.Normal;
        }
        private void minimizeWindow(object sender, EventArgs e)
        {
            Hide();
            this.WindowState = FormWindowState.Minimized;
        }
        private void exitWindow(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to exit?", "Exit", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (result.Equals(DialogResult.OK))
            {
                Environment.Exit(1);
            }
        }

        // -- Misc Functions
        private void setupTrayIcon()
        {
            hotkeyInformationLabel.Hide();
            contextMenu1.MenuItems.Add("Open", (s, e) => maximizeWindow(null, null));
            contextMenu1.MenuItems.Add("Exit", (s, e) => exitWindow(null, null));
            notifyIcon1.ContextMenu = contextMenu1;
        }


        private async void hotkeyEdit_addSong_Click(object sender, EventArgs e)
        {
            hotkeyInformationLabel.Show();

            await Task.Delay(10000);

            hotkeyInformationLabel.Hide();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            label_userName.Text = _spotifyUser.display_name;
            label_userID.Text = _spotifyUser.id;
        }

        private void bunifuFlatButton6_Click(object sender, EventArgs e)
        {
            this.Hide();
            Initialization form = (Initialization)Application.OpenForms[0];
            form.Show();
            form.webBrowser_authentication.Url = new Uri("https://accounts.spotify.com/en/logout");
        }

        private void hotkeyToggle_AddSongToPlaylist1(object sender, EventArgs e)
        {
            if (checkbox_AddSongToPlaylist1Toggle.Checked)
            {
                _hotkeysManager.registerHotkey("AddSongToPlaylist1");
            }
            else
            {
                _hotkeysManager.unregisterHotkey("AddSongToPlaylist1");
            }
        }

        private void hotkeyToggle_AddSongToPlaylist2(object sender, EventArgs e)
        {
            if (checkbox_AddSongToPlaylist2Toggle.Checked)
            {
                _hotkeysManager.registerHotkey("AddSongToPlaylist2");
            }
            else
            {
                _hotkeysManager.unregisterHotkey("AddSongToPlaylist2");
            }
        }

        private void hotkeyToggle_AddSongToPlaylist3(object sender, EventArgs e)
        {
            if (checkbox_AddSongToPlaylist3Toggle.Checked)
            {
                _hotkeysManager.registerHotkey("AddSongToPlaylist3");
            }
            else
            {
                _hotkeysManager.unregisterHotkey("AddSongToPlaylist3");
            }
        }
    }
}
