using SpotifyHotkeys.Properties;
using SpotifyHotkeys.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpotifyHotkeys.Models.Hotkeys
{
    public class HotkeyManager
    {
        enum KeyModifier
        {
            None = 0,
            Alt = 1,
            Control = 2,
            Shift = 4,
            WinKey = 8
        }

        public Main mainForm = null;

        private List<Hotkey> hotkeys = new List<Hotkey>();

        public HotkeyManager(Main mainForm)
        {
            this.mainForm = mainForm;
            createHotkeys();
        }

        public void createHotkeys()
        {
            hotkeys.Clear();
            Debug.Write(Keys.A.GetHashCode());
            hotkeys.Add(new Hotkey("AddSongToPlaylist1", Constants.ALT, Keys.F1, this.mainForm));
            hotkeys.Add(new Hotkey("AddSongToPlaylist2", Constants.ALT, Keys.F2, this.mainForm));
            hotkeys.Add(new Hotkey("AddSongToPlaylist3", Constants.ALT, Keys.F3, this.mainForm));
            Debug.WriteLine(hotkeys);
        }

        public void registerHotkeys()
        {
            foreach (Hotkey hotkey in hotkeys)
            {
                CheckBox toggle = (CheckBox)this.mainForm.Controls.Find("checkbox_" + hotkey.hotkeyName + "Toggle", true)[0];

                if ((bool)Settings.Default["Hotkey_" + hotkey.hotkeyName])
                {
                    hotkey.Register();
                    toggle.CheckState = CheckState.Checked;
                } else
                {
                    toggle.CheckState = CheckState.Unchecked;
                }
            }
        }
        public void unregisterHotkeys()
        {
            foreach (Hotkey hotkey in hotkeys)
            {
                hotkey.Unregister();
                Debug.WriteLine("Unregistered.");
            }
        }

        public void unregisterHotkey(string name) {
            foreach (Hotkey hotkey in hotkeys)
            {
                if (hotkey.hotkeyName == name) {
                    hotkey.Unregister();
                }
            }
        }

        public void registerHotkey(string name)
        {
            foreach (Hotkey hotkey in hotkeys)
            {
                if (hotkey.hotkeyName == name)
                {
                    hotkey.Register();
                }
            }
        }

        public void handleHotkey(Message m) {
            Keys key = (Keys)(((int)m.LParam >> 16) & 0xFFFF);                  // The key of the hotkey that was pressed.
            KeyModifier modifier = (KeyModifier)((int)m.LParam & 0xFFFF);       // The modifier of the hotkey that was pressed.
            int id = m.WParam.ToInt32();
            Debug.WriteLine(modifier + " + " + key + " [" + id + "]");
            foreach (Hotkey hotkey in hotkeys) {
                if (hotkey.id == id)
                {
                    hotkey.hotkeyAction();
                }
            }
        }
    }
}
