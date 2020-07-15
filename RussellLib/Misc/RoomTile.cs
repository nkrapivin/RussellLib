using RussellLib.Assets;
using RussellLib.Base;
using System.Drawing;

namespace RussellLib.Misc
{
    public struct RoomTile
    {
        public Point RoomPosition;
        public GMBackground Background;
        public Rectangle BGCoords; // position of a tile in the background
        public int Depth; // "tile layer"
        public int ID;
        public bool IsLocked;

        public void Save(ProjectWriter writer, GMProject proj)
        {
            writer.Write(RoomPosition);
            if (Background != null) writer.Write(proj.Backgrounds.IndexOf(Background));
            else writer.Write(-1);
            writer.Write(BGCoords);
            writer.Write(Depth);
            writer.Write(ID);
            writer.Write(IsLocked);
        }

        public void Load(ProjectReader reader, GMProject proj)
        {
            int _x, _y, _w, _h, _bgind;
            _x = reader.ReadInt32();
            _y = reader.ReadInt32();
            RoomPosition = new Point(_x, _y);
            Background = null;
            _bgind = reader.ReadInt32();
            if (_bgind > -1) Background = proj.Backgrounds[_bgind];
            _x = reader.ReadInt32();
            _y = reader.ReadInt32();
            _w = reader.ReadInt32();
            _h = reader.ReadInt32();
            BGCoords = new Rectangle(_x, _y, _w, _h);
            Depth = reader.ReadInt32();
            ID = reader.ReadInt32();
            IsLocked = reader.ReadBoolean();
        }
    }
}
