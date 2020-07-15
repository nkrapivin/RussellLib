using RussellLib.Base;
using RussellLib.Misc;
using System;
using System.Collections.Generic;

namespace RussellLib.Assets
{
    public class GMTimeline
    {
        public int Version;
        public string Name;
        public DateTime LastChanged;
        public List<TimelineMoment> Moments;

        public void Save(ProjectWriter writer, GMProject proj)
        {
            writer.Write(Name);
            writer.Write(LastChanged);
            writer.Write(Version);
            writer.Write(Moments.Count);
            for (int i = 0; i < Moments.Count; i++)
            {
                var moment = Moments[i];
                moment.Save(writer, proj);
            }
        }

        public GMTimeline(ProjectReader reader)
        {
            Name = reader.ReadString();
            LastChanged = reader.ReadDate();
            Version = reader.ReadInt32();

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
