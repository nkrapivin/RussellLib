using RussellLib.Code;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RussellLib.Misc
{
    public class TimelineMoment
    {
        public int Point;
        public GMEvent Event;

        public TimelineMoment(BinaryReader reader)
        {
            Point = reader.ReadInt32();
            Event = new GMEvent(reader);
        }
    }
}
