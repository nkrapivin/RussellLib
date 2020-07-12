using RussellLib.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RussellLib.Assets
{
    public class GMFont
    {
        public string Name;
        public DateTime LastChanged;
        public string FontName;
        public int Size;
        public bool Bold;
        public bool Italic;
        public int RangeMin;
        public int RangeMax;

        public GMFont(ProjectReader reader)
        {
            Name = reader.ReadString();
            LastChanged = reader.ReadDate();
            int version = reader.ReadInt32();
            if (version != 800)
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
