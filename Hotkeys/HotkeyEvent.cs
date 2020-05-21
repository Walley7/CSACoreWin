using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace CSACoreWin.Hotkeys {

    public class HotkeyEvent {
        //================================================================================
        public enum Modifier : uint {
            None = 0,
            Alt = 1,
            Ctrl = 2,
            Shift = 4,
            Win = 8
        }


        //================================================================================
        private Modifier                        mModifiers;
        private Keys                            mKey;


        //================================================================================
        //--------------------------------------------------------------------------------
        public HotkeyEvent(Modifier modifiers, Keys key) {
            mModifiers = modifiers;
            mKey = key;
        }
        

        // MODIFIERS ================================================================================
        //--------------------------------------------------------------------------------
        public Modifier Modifiers { get { return mModifiers; } }
        public bool CtrlModifier { get { return (mModifiers & Modifier.Ctrl) != 0; } }
        public bool AltModifier { get { return (mModifiers & Modifier.Alt) != 0; } }
        public bool ShiftModifier { get { return (mModifiers & Modifier.Shift) != 0; } }


        // KEY ================================================================================
        //--------------------------------------------------------------------------------
        public Keys Key { get { return mKey; } }
    }

}
