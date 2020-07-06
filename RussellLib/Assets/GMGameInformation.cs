using RussellLib.Base;
using System;
using System.Drawing;
using System.IO;

namespace RussellLib.Assets
{
    public class GMGameInformation : StreamBase
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

        public GMGameInformation(BinaryReader reader)
        {
            BackgroundColor = ReadColor(reader);
            SeparateWindow = ReadBool(reader);
            Caption = ReadString(reader);
            int _l, _t, _w, _h;
            _l = reader.ReadInt32();
            _t = reader.ReadInt32();
            _w = reader.ReadInt32();
            _h = reader.ReadInt32();
            Position = new Rectangle(_l, _t, _w, _h);
            ShowBorder = ReadBool(reader);
            AllowResize = ReadBool(reader);
            AlwaysOnTop = ReadBool(reader);
            Freeze = ReadBool(reader);
            LastChanged = ReadDate(reader);
            Text = ReadString(reader);

            reader.Dispose();
        }
    }
}
