using RussellLib.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RussellLib.Assets
{
    public class GMIncludedFile
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

        public GMIncludedFile(ProjectReader reader)
        {
            LastChanged = reader.ReadDate();
            int version = reader.ReadInt32();
            if (version != 800)
            {
                throw new InvalidDataException("Wrong Included File version, got " + version);
            }

            FileName = reader.ReadString();
            FilePath = reader.ReadString();
            Original = reader.ReadBoolean();
            FileSize = reader.ReadInt32();
            StoreInProject = reader.ReadBoolean();
            Data = null;
            if (StoreInProject)
            {
                int size = reader.ReadInt32(); // ??? why it's repeated twice?
                Data = reader.ReadBytes(size);
            }
            ExportKind = (ExportActionKind)reader.ReadInt32();
            ExportFolder = reader.ReadString();
            Overwrite = reader.ReadBoolean();
            FreeMemory = reader.ReadBoolean();
            RemoveAtGameEnd = reader.ReadBoolean();
        }
    }
}
