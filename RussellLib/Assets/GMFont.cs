using RussellLib.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RussellLib.Assets
{
    public class GMFont : StreamBase
    {
        public string Name;
        public DateTime LastChanged;
        public string FontName;
        public int Size;
        public bool Bold;
        public bool Italic;
        public int RangeMin;
        public int RangeMax;

        public GMFont(BinaryReader reader)
        {
            Name = ReadString(reader);
            LastChanged = ReadDate(reader);
            int version = reader.ReadInt32();
            if (version != 800)
            {
                throw new InvalidDataException("This library only supports .gmk GM8.0 files.");
            }

            FontName = ReadString(reader);
            Size = reader.ReadInt32();
            Bold = ReadBool(reader);
            Italic = ReadBool(reader);
            RangeMin = reader.ReadInt32();
            RangeMax = reader.ReadInt32();
        }
    }
}
