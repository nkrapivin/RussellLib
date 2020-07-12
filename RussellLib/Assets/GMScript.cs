using RussellLib.Base;
using System;
using System.IO;

namespace RussellLib.Assets
{
    public class GMScript
    {
        public string Name;
        public DateTime LastChanged;
        public string Code;

        public GMScript(ProjectReader reader)
        {
            Name = reader.ReadString();
            LastChanged = reader.ReadDate();
            int version = reader.ReadInt32();
            if (version != 800)
            {
                throw new InvalidDataException("This library only supports .gmk GM8.0 files.");
            }
            Code = reader.ReadString();

            reader.Dispose();
        }
    }
}
