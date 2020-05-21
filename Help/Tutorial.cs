using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace CSACoreWin.Help {

    public class Tutorial {
        //================================================================================
        public const int                        TOOLTIP_DURATION = 600000;


        //================================================================================
        private TutorialSystem                  mTutorialSystem;

        private List<TooltipEntry>              mTooltips = new List<TooltipEntry>();


        // UPDATING ================================================================================
        //--------------------------------------------------------------------------------
        public virtual void Update() { }


        // EVENTS ================================================================================
        //--------------------------------------------------------------------------------
        protected virtual void OnEvent(string identifier, params object[] data) { }
        protected virtual void OnStart(params object[] arguments) { }
        protected virtual void OnStop() { }


        // SYSTEM ================================================================================
        //--------------------------------------------------------------------------------
        public TutorialSystem TutorialSystem { set { mTutorialSystem = value; } get { return mTutorialSystem; } }


        // EVENT NOTIFICATION ================================================================================
        //--------------------------------------------------------------------------------
        public void NotifyEvent(string identifier, params object[] data) {
            OnEvent(identifier, data);
        }

        //--------------------------------------------------------------------------------
        public void NotifyStart(params object[] arguments) {
            OnStart(arguments);
        }

        //--------------------------------------------------------------------------------
        public void NotifyStop() {
            OnStop();
        }


        // TOOLTIPS ================================================================================
        //--------------------------------------------------------------------------------
        protected void ShowTooltip(string text, IWin32Window window, bool balloon, int x, int y, bool hideTooltips, int duration) {
            if (hideTooltips)
                HideTooltips();
            ToolTip tooltip = new ToolTip();
            tooltip.IsBalloon = balloon;
            tooltip.Show(text, window, x, y, duration);
            mTooltips.Add(new TooltipEntry(tooltip, window));
        }

        //--------------------------------------------------------------------------------
        protected void ShowTooltip(string text, IWin32Window window, int x, int y, bool hideTooltips = true, int duration = TOOLTIP_DURATION) { ShowTooltip(text, window, false, x, y, hideTooltips, duration); }
        protected void ShowTooltip(string text, IWin32Window window, Point position, bool hideTooltips = true, int duration = TOOLTIP_DURATION) { ShowTooltip(text, window, false, position.X, position.Y, hideTooltips, duration); }
        protected void ShowBalloon(string text, IWin32Window window, int x, int y, bool hideTooltips = true, int duration = TOOLTIP_DURATION) { ShowTooltip(text, window, true, x, y, hideTooltips, duration); }
        protected void ShowBalloon(string text, IWin32Window window, Point position, bool hideTooltips = true, int duration = TOOLTIP_DURATION) { ShowTooltip(text, window, true, position.X, position.Y, hideTooltips, duration); }
        
        //--------------------------------------------------------------------------------
        protected void ShowTooltip(string text, Form form, string controlName, int xOffset, int yOffset, bool hideTooltips = true, int duration = TOOLTIP_DURATION) {
            Point position = TooltipPosition(form, controlName);
            position = new Point(position.X, position.Y - TooltipHeightOffset(text));
            ShowTooltip(text, form, new Point(position.X + xOffset, position.Y + yOffset), hideTooltips, duration);
        }

        //--------------------------------------------------------------------------------
        protected void ShowTooltip(string text, Form form, string controlName, bool hideTooltips = true, int duration = TOOLTIP_DURATION) { ShowTooltip(text, form, controlName, 0, 0, hideTooltips, duration); }

        //--------------------------------------------------------------------------------
        protected void ShowBalloon(string text, Form form, string controlName, int xOffset, int yOffset, bool hideTooltips = true, int duration = TOOLTIP_DURATION) {
            Point position = TooltipPosition(form, controlName);
            position = new Point(position.X, position.Y - TooltipHeightOffset(text));
            ShowBalloon(text, form, new Point(position.X + xOffset, position.Y + yOffset), hideTooltips, duration);
        }

        //--------------------------------------------------------------------------------
        protected void ShowBalloon(string text, Form form, string controlName, bool hideTooltips = true, int duration = TOOLTIP_DURATION) { ShowBalloon(text, form, controlName, 0, 0, hideTooltips, duration); }

        //--------------------------------------------------------------------------------
        protected void HideTooltips() {
            foreach (TooltipEntry t in mTooltips) {
                t.tooltip.Hide(t.window);
            }
            mTooltips.Clear();
        }
        
        //--------------------------------------------------------------------------------
        protected int TooltipHeightOffset(string text) {
            int newLineCount = text.Count(c => c.Equals('\n'));
            int height = 8;
            if (newLineCount == 1)
                height = 28;
            else if (newLineCount >= 2)
                height = 17 + 15 * newLineCount;
            return height;
        }


        // POSITIONING ================================================================================
        //--------------------------------------------------------------------------------
        protected Point TooltipPosition(Form form, Control control) {
            Point formPoint = form.PointToClient(control.Parent.PointToScreen(control.Location));
            return new Point(formPoint.X, formPoint.Y);
        }

        //--------------------------------------------------------------------------------
        protected Point TooltipPosition(Form form, string controlName) {
            Control control = FormControl<Control>(form, controlName);
            Point formPoint = form.PointToClient(control.Parent.PointToScreen(control.Location));
            return new Point(formPoint.X, formPoint.Y);
        }


        // CONTROLS ================================================================================
        //--------------------------------------------------------------------------------
        protected T FormControl<T>(Form form, string controlName) where T : Control {
            Control[] controls = form.Controls.Find(controlName, true);
            return (controls.Count() > 0 ? (T)controls.First() : (T)null);
        }


        // TEXT ================================================================================
        //--------------------------------------------------------------------------------
        protected string PaddedText(string text, string horizontalPadding = "", int newLineCount = 1) {
            StringBuilder builder = new StringBuilder();
            builder.Append("\n  ");
            builder.Append(text.Replace("\n", "  \n  "));
            builder.Append("  \n ");
            return builder.ToString();
        }


        //================================================================================
        //********************************************************************************
        private struct TooltipEntry {
            public ToolTip tooltip;
            public IWin32Window window;

            public TooltipEntry(ToolTip tooltip, IWin32Window window) {
                this.tooltip = tooltip;
                this.window = window;
            }
        }
    }

}
