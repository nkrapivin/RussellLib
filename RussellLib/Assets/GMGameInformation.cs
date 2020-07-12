using RussellLib.Base;
using System;
using System.Drawing;
using System.IO;

namespace RussellLib.Assets
{
    public class GMGameInformation
    {
        public Color BackgroundColor;
        public bool SeparateWindow;
        public string Caption;
        public Rectangle Position;
        public bool ShowBorder;
        public bool AllowResize;
        public bool AlwaysOnTop; // Modal
        public bool Freeze; // freeze the game while showing help or not
        public DateTime LastChanged;
        public string Text; // it's an rtf string.

        public GMGameInformation(ProjectReader reader)
        {
            BackgroundColor = reader.ReadColor();
            SeparateWindow = reader.ReadBoolean();
            Caption = reader.ReadString();
            int _l, _t, _w, _h;
            _l = reader.ReadInt32();
            _t = reader.ReadInt32();
            _w = reader.ReadInt32();
            _h = reader.ReadInt32();
            Position = new Rectangle(_l, _t, _w, _h);
            ShowBorder = reader.ReadBoolean();
            AllowResize = reader.ReadBoolean();
            AlwaysOnTop = reader.ReadBoolean();
            Freeze = reader.ReadBoolean();
            LastChanged = reader.ReadDate();
            Text = reader.ReadString();

            reader.Dispose();
        }
    }
}
