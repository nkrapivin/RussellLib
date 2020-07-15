using RussellLib.Base;
using System.IO;

namespace RussellLib.Assets
{
    public class GMTrigger
    {
        public int Version;
        public string Name;
        public string Condition;
        public TriggerEvent Event;
        public string ConstName;

        public enum TriggerEvent
        {
            Step,
            BeginStep,
            EndStep
        }

        public void Save(ProjectWriter writer)
        {
            writer.Write(Version);
            writer.Write(Name);
            writer.Write(Condition);
            writer.Write((int)Event);
            writer.Write(ConstName);
        }

        public GMTrigger(ProjectReader reader)
        {
            Version = reader.ReadInt32();

            if (Version != 800)
            {
                throw new InvalidDataException("Wrong project version.");
            }

            Name = reader.ReadString();
            Condition = reader.ReadString();
            Event = (TriggerEvent)reader.ReadInt32();
            ConstName = reader.ReadString();
        }
    }
}
