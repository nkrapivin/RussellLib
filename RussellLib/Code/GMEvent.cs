using RussellLib.Base;
using System.Collections.Generic;
using System.IO;

namespace RussellLib.Code
{
    public class GMEvent
    {
        public int Version;
        public List<GMAction> Actions;
        public int Key; // only used in GMObject.

        public void Save(ProjectWriter writer, GMProject proj)
        {
            writer.Write(Version);
            writer.Write(Actions.Count);
            for (int i = 0; i < Actions.Count; i++)
            {
                var action = Actions[i];
                action.Save(writer, proj);
            }
        }

        public GMEvent(ProjectReader reader)
        {
            Version = reader.ReadInt32(); // 400?
            if (Version != 400)
            {
                throw new InvalidDataException("Invalid GMEvent version number, got " + Version);
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
