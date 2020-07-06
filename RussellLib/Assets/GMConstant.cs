using RussellLib.Base;
using System.IO;

namespace RussellLib.Assets
{
    public class GMConstant : StreamBase
    {
        public string Name;
        public string Value;

        public GMConstant(BinaryReader reader)
        {
            Name = ReadString(reader);
            Value = ReadString(reader);
        }
    }
}
