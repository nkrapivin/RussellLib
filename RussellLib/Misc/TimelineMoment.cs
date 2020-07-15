using RussellLib.Base;
using RussellLib.Code;

namespace RussellLib.Misc
{
    public class TimelineMoment
    {
        public int Point;
        public GMEvent Event;

        public void Save(ProjectWriter writer, GMProject proj)
        {
            writer.Write(Point);
            Event.Save(writer, proj);
        }

        public TimelineMoment(ProjectReader reader)
        {
            Point = reader.ReadInt32();
            Event = new GMEvent(reader);
        }
    }
}
