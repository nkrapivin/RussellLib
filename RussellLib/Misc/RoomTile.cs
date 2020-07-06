using RussellLib.Assets;
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
    }
}
