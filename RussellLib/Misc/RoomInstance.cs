using RussellLib.Assets;
using System.Drawing;

namespace RussellLib.Misc
{
    public struct RoomInstance
    {
        public Point Position;
        public GMObject Object;
        public int ID;
        public string CreationCode;
        public bool IsLocked;
    }
}
