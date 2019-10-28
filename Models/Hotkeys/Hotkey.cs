using Newtonsoft.Json.Linq;
using SpotifyHotkeys.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpotifyHotkeys.Models.Hotkeys
{
    public class Hotkey
    {


        public int modifier;
        public int key;
        public IntPtr hWnd;
        public int id;
        public string hotkeyName;
        private Main mainForm;
        Hotkey tmpHotkeyF1;
        Hotkey tmpHotkeyF2;
        Hotkey tmpHotkeyF3;
        Hotkey tmpHotkeyF4;
        Hotkey tmpHotkeyF5;

        public Hotkey(string hotkeyName, int modifier, Keys key, Main mainForm)
        {
            this.mainForm = mainForm;
            this.hotkeyName = hotkeyName;
            this.modifier = modifier;
            this.key = (int)key;
            this.hWnd = mainForm.Handle;
            id = this.GetHashCode();

        }

        public bool Register()
        {
            return RegisterHotKey(hWnd, id, modifier, key);
        }

        public bool Unregister()
        {
            return UnregisterHotKey(hWnd, id);
        }

        public async void hotkeyAction() {
            this.mainForm.hotkeyExecuting = true;
            JObject song = null;
            switch (this.hotkeyName)
            {
                case "AddSongToPlaylist1":
                    addSongToPlaylist(1);
                    break;
                case "AddSongToPlaylist2":
                    addSongToPlaylist(2);
                    break;
                case "AddSongToPlaylist3":
                    addSongToPlaylist(3);
                    break;
                case "AddSongToPlaylist4":
                    addSongToPlaylist(4);
                    break;
                case "AddSongToPlaylist5":
                    addSongToPlaylist(5);
                    break;
            }
        }

        private async void addSongToPlaylist(int playlist) {
            JObject song = await this.mainForm._spotifyUser.post_getCurrentlyPlayingSong();
            if (song == null)
            {
                return;
            }
            await this.mainForm._spotifyUser.post_addSongToPlaylist(playlist--, song, this.mainForm);
        }

        public override int GetHashCode()
        {
            return modifier ^ key ^ hWnd.ToInt32();
        }

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);
    }
}
