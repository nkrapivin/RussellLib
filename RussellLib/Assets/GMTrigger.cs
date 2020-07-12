using RussellLib.Base;
using System.IO;

namespace RussellLib.Assets
{
    public class GMTrigger
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

        public GMTrigger(ProjectReader reader)
        {
            int Version = reader.ReadInt32();

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
