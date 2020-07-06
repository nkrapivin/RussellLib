using RussellLib.Assets;
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
    }
}
