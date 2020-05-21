using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace CSACoreWin.Hotkeys {

    public class HotkeyRegistrar : NativeWindow, IDisposable {
        //================================================================================
        private const int                       WM_HOTKEY = 0x0312;


        //================================================================================
        private int                             mNextHotkeyID = 1;

        public event EventHandler<HotkeyEvent>  KeyPressedEvent;


        //================================================================================
        //--------------------------------------------------------------------------------
        public HotkeyRegistrar() {
            // Handle
            CreateHandle(new CreateParams());
        }

        //--------------------------------------------------------------------------------
        public void Dispose() {
            // Hotkeys
            for (int i = mNextHotkeyID - 1; i > 0; --i) {
                UnregisterHotKey(Handle, i);
            }

            // Handle
            DestroyHandle();
        }


        // EVENTS ================================================================================
        //--------------------------------------------------------------------------------
        protected override void WndProc(ref Message m) {
            base.WndProc(ref m);

            // Hot key
            if (m.Msg == WM_HOTKEY) {
                Keys key = (Keys)(((int)m.LParam >> 16) & 0xFFFF);
                HotkeyEvent.Modifier modifier = (HotkeyEvent.Modifier)((int)m.LParam & 0xFFFF);
                if (KeyPressedEvent != null)
                    KeyPressedEvent(this, new HotkeyEvent(modifier, key));
            }
        }


        // HOTKEYS ================================================================================
        //--------------------------------------------------------------------------------
        public void RegisterHotkey(HotkeyEvent.Modifier modifier, Keys key) {
            if (!RegisterHotKey(Handle, mNextHotkeyID++, (uint)modifier, (uint)key))
                throw new InvalidOperationException();
        }


        //================================================================================
        //********************************************************************************
        [DllImport("user32.dll")] private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);
        [DllImport("user32.dll")] private static extern bool UnregisterHotKey(IntPtr hWnd, int id);
    }

}
