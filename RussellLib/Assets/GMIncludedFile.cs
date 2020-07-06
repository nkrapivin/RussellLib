using RussellLib.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RussellLib.Assets
{
    public class GMIncludedFile : StreamBase
    {
        public DateTime LastChanged;
        public string FileName;
        public string FilePath;
        public bool Original;
        public int FileSize; // ???? weird, since GM also writes file size if you choose "Store in Project"
        public bool StoreInProject;
        public byte[] Data;
        public ExportActionKind ExportKind;
        public string ExportFolder;
        public bool Overwrite;
        public bool FreeMemory;
        public bool RemoveAtGameEnd;

        public enum ExportActionKind
        {
            DONT_EXPORT,
            TEMP_DIRECTORY,
            SAME_FOLDER,
            CUSTOM_FOLDER
        }

        public GMIncludedFile(BinaryReader reader)
        {
            LastChanged = ReadDate(reader);
            int version = reader.ReadInt32();
            if (version != 800)
            {
                throw new InvalidDataException("Wrong Included File version, got " + version);
            }

            FileName = ReadString(reader);
            FilePath = ReadString(reader);
            Original = ReadBool(reader);
            FileSize = reader.ReadInt32();
            StoreInProject = ReadBool(reader);
            Data = null;
            if (StoreInProject)
            {
                int size = reader.ReadInt32(); // ??? why it's repeated twice?
                Data = reader.ReadBytes(size);
            }
            ExportKind = (ExportActionKind)reader.ReadInt32();
            ExportFolder = ReadString(reader);
            Overwrite = ReadBool(reader);
            FreeMemory = ReadBool(reader);
            RemoveAtGameEnd = ReadBool(reader);
        }
    }
}
