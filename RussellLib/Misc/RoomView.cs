using RussellLib.Assets;
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
    }
}
