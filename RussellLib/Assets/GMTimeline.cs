using RussellLib.Base;
using RussellLib.Misc;
using System;
using System.Collections.Generic;
using System.IO;

namespace RussellLib.Assets
{
    public class GMTimeline
    {
        public string Name;
        public DateTime LastChanged;
        public List<TimelineMoment> Moments;

        public GMTimeline(ProjectReader reader)
        {
            Name = reader.ReadString();
            LastChanged = reader.ReadDate();
            int version = reader.ReadInt32();

            int momentcount = reader.ReadInt32();
            Moments = new List<TimelineMoment>(momentcount);
            for (int i = 0; i < momentcount; i++)
            {
                Moments.Add(new TimelineMoment(reader));
            }

            reader.Dispose();
        }
    }
}
