using RussellLib.Base;
using System;
using System.IO;

namespace RussellLib.Assets
{
    public class GMFont
    {
        public int Version;
        public string Name;
        public DateTime LastChanged;
        public string FontName;
        public int Size;
        public bool Bold;
        public bool Italic;
        public int RangeMin;
        public int RangeMax;

        public void Save(ProjectWriter writer)
        {
            writer.Write(Name);
            writer.Write(LastChanged);
            writer.Write(Version);
            writer.Write(FontName);
            writer.Write(Size);
            writer.Write(Bold);
            writer.Write(Italic);
            writer.Write(RangeMin);
            writer.Write(RangeMax);
        }

        public GMFont(ProjectReader reader)
        {
            Name = reader.ReadString();
            LastChanged = reader.ReadDate();
            Version = reader.ReadInt32();
            if (Version != 800)
            {
                throw new InvalidDataException("This library only supports .gmk GM8.0 files.");
            }

            FontName = reader.ReadString();
            Size = reader.ReadInt32();
            Bold = reader.ReadBoolean();
            Italic = reader.ReadBoolean();
            RangeMin = reader.ReadInt32();
            RangeMax = reader.ReadInt32();
        }
    }
}
