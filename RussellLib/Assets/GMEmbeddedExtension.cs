using RussellLib.Base;

namespace RussellLib.Assets
{
    public class GMEmbeddedExtension
    {
        public string Name;
        public string Folder;

        public GMEmbeddedExtension(ProjectReader reader)
        {
            int ver = reader.ReadInt32();
            Name = reader.ReadString();
            Folder = reader.ReadString();

            int incl_count = reader.ReadInt32();
            for (int i = 0; i < incl_count; i++)
            {

            }
        }
    }

    public class GMExtensionInclude
    {
        public string Filename;
        public IncludeKind Kind;
        public string Init;
        public string Final;


        public enum IncludeKind
        {
            UNKNOWN,
            DLL,
            GML,
            UNKNOWN2,
            DEFAULT
        }
    }
}
