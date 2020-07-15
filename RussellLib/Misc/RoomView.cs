using RussellLib.Assets;
using RussellLib.Base;
using System.Drawing;

namespace RussellLib.Misc
{
    public struct RoomView
    {
        public bool Visible;
        public Rectangle ViewCoords;
        public Rectangle PortCoords;
        public int BorderHor;
        public int BorderVert;
        public int HSpeed;
        public int VSpeed;
        public GMObject ViewObject;

        public void Load(ProjectReader reader, GMProject proj)
        {
            int _x, _y, _w, _h;
            Visible = reader.ReadBoolean();
            _x = reader.ReadInt32();
            _y = reader.ReadInt32();
            _w = reader.ReadInt32();
            _h = reader.ReadInt32();
            ViewCoords = new Rectangle(_x, _y, _w, _h);
            _x = reader.ReadInt32();
            _y = reader.ReadInt32();
            _w = reader.ReadInt32();
            _h = reader.ReadInt32();
            PortCoords = new Rectangle(_x, _y, _w, _h);
            BorderHor = reader.ReadInt32();
            BorderVert = reader.ReadInt32();
            HSpeed = reader.ReadInt32();
            VSpeed = reader.ReadInt32();
            ViewObject = null;
            int _objind = reader.ReadInt32();
            if (_objind > -1) ViewObject = proj.Objects[_objind];
        }

        public void Save(ProjectWriter writer, GMProject proj)
        {
            writer.Write(Visible);
            writer.Write(ViewCoords);
            writer.Write(PortCoords);
            writer.Write(BorderHor);
            writer.Write(BorderVert);
            writer.Write(HSpeed);
            writer.Write(VSpeed);
            if (ViewObject != null) writer.Write(proj.Objects.IndexOf(ViewObject));
            else writer.Write(-1);
        }
    }
}
