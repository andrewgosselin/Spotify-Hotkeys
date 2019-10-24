using SpotifyBoilerplate.Properties;
using SpotifyBoilerplate.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpotifyBoilerplate.Models
{
    public class Hotkeys
    {

        public Main mainForm = null;
        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);
        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        public Hotkeys(Main mainForm) {
            this.mainForm = mainForm;
            registerHotkeys();
        }

        public void registerHotkeys() {
            // 3. Register HotKeys

            // Set an unique id to your Hotkey, it will be used to
            // identify which hotkey was pressed in your code to execute something
            int FirstHotkeyId = 1;
            // Set the Hotkey triggerer the F9 key 
            // Expected an integer value for F9: 0x78, but you can convert the Keys.KEY to its int value
            // See: https://msdn.microsoft.com/en-us/library/windows/desktop/dd375731(v=vs.85).aspx
            int FirstHotKeyKey = (int)Keys.F9;
            // Register the "F9" hotkey
            Boolean F9Registered = RegisterHotKey(
                this.mainForm.Handle, FirstHotkeyId, 0x0000, FirstHotKeyKey
            );

            // Repeat the same process but with F10
            int SecondHotkeyId = 2;
            int SecondHotKeyKey = (int)Keys.F10;
            Boolean F10Registered = RegisterHotKey(
                this.mainForm.Handle, SecondHotkeyId, 0x0000, SecondHotKeyKey
            );

            // 4. Verify if both hotkeys were succesfully registered, if not, show message in the console
            if (!F9Registered)
            {
                Console.WriteLine("Global Hotkey F9 couldn't be registered !");
            }

            if (!F10Registered)
            {
                Console.WriteLine("Global Hotkey F10 couldn't be registered !");
            }
        }

        public void handleKeyPress(ref Message m) {
            int id = m.WParam.ToInt32();
            // MessageBox.Show(string.Format("Hotkey #{0} pressed", id));

            if (id == 1) // Add Song 1
            {
                MessageBox.Show("F9 Was pressed !");
            }
        }



    }
}
