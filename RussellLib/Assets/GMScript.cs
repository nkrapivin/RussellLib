using RussellLib.Base;
using System;
using System.IO;

namespace RussellLib.Assets
{
    public class GMScript
    {
        public int Version;
        public string Name;
        public DateTime LastChanged;
        public string Code;

        public void Save(ProjectWriter writer)
        {
            writer.Write(Name);
            writer.Write(LastChanged);
            writer.Write(Version);
            writer.Write(Code);
        }

        public GMScript(ProjectReader reader)
        {
            Name = reader.ReadString();
            LastChanged = reader.ReadDate();
            Version = reader.ReadInt32();
            if (Version != 800)
            {
                throw new InvalidDataException("This library only supports .gmk GM8.0 files.");
            }
            Code = reader.ReadString();

            reader.Dispose();
        }
    }
}
