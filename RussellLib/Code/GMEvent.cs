using System.Collections.Generic;
using System.IO;

namespace RussellLib.Code
{
    public class GMEvent
    {
        public List<GMAction> Actions;
        public int Key; // only used in GMObject.

        public GMEvent(BinaryReader reader)
        {
            int version = reader.ReadInt32(); // 400?
            if (version != 400)
            {
                throw new InvalidDataException("Invalid GMEvent version number, got " + version);
            }

            int Count = reader.ReadInt32();
            Actions = new List<GMAction>(Count);
            for (int i = 0; i < Count; i++)
            {
                Actions.Add(new GMAction(reader));
            }
        }
    }
}
