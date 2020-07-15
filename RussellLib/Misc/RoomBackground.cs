using RussellLib.Assets;
using RussellLib.Base;
using System.Drawing;

namespace RussellLib.Misc
{
    public struct RoomBackground
    {
        public bool Visible;
        public bool IsForeground;
        public GMBackground Background;
        public Point Position;
        public bool TileHorizontal;
        public bool TileVertical;
        public int SpeedHorizontal;
        public int SpeedVertical;
        public bool Stretch;

        public void Load(ProjectReader reader, GMProject proj)
        {
            Visible = reader.ReadBoolean();
            IsForeground = reader.ReadBoolean();
            Background = null;
            int bgid, _bgx, _bgy;
            bgid = reader.ReadInt32();
            if (bgid > -1) Background = proj.Backgrounds[bgid];
            _bgx = reader.ReadInt32();
            _bgy = reader.ReadInt32();
            Position = new Point(_bgx, _bgy);
            TileHorizontal = reader.ReadBoolean();
            TileVertical = reader.ReadBoolean();
            SpeedHorizontal = reader.ReadInt32();
            SpeedVertical = reader.ReadInt32();
            Stretch = reader.ReadBoolean();
        }

        public void Save(ProjectWriter writer, GMProject proj)
        {
            writer.Write(Visible);
            writer.Write(IsForeground);
            if (Background != null) writer.Write(proj.Backgrounds.IndexOf(Background));
            else writer.Write(-1);
            writer.Write(Position);
            writer.Write(TileHorizontal);
            writer.Write(TileVertical);
            writer.Write(SpeedHorizontal);
            writer.Write(SpeedVertical);
            writer.Write(Stretch);
        }
    }
}
