using RussellLib.Base;
using System.IO;

namespace RussellLib.Assets
{
    public class GMTrigger : StreamBase
    {
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

        public GMTrigger(BinaryReader reader)
        {
            int Version = reader.ReadInt32();

            if (Version != 800)
            {
                throw new InvalidDataException("Wrong project version.");
            }

            Name = ReadString(reader);
            Condition = ReadString(reader);
            Event = (TriggerEvent)reader.ReadInt32();
            ConstName = ReadString(reader);
        }
    }
}
