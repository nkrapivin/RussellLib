using RussellLib.Base;
using System.IO;

namespace RussellLib.Assets
{
    public class GMConstant
    {
        public string Name;
        public string Value;

        public GMConstant(ProjectReader reader)
        {
            Name = reader.ReadString();
            Value = reader.ReadString();
        }
    }
}
