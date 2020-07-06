using RussellLib.Base;
using System;
using System.IO;

namespace RussellLib.Assets
{
    public class GMScript : StreamBase
    {
        public string Name;
        public DateTime LastChanged;
        public string Code;

        public GMScript(BinaryReader reader)
        {
            Name = ReadString(reader);
            LastChanged = ReadDate(reader);
            int version = reader.ReadInt32();
            if (version != 800)
            {
                throw new InvalidDataException("This library only supports .gmk GM8.0 files.");
            }
            Code = ReadString(reader);

            reader.Dispose();
        }
    }
}
