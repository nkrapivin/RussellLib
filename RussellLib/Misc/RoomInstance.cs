using RussellLib.Assets;
using RussellLib.Base;
using System.Drawing;

namespace RussellLib.Misc
{
    public struct RoomInstance
    {
        public Point Position;
        public GMObject Object;
        public int ID;
        public string CreationCode;
        public bool IsLocked;

        public void Load(ProjectReader reader, GMProject proj)
        {
            int _x, _y, _objind;
            _x = reader.ReadInt32();
            _y = reader.ReadInt32();
            Position = new Point(_x, _y);
            Object = null;
            _objind = reader.ReadInt32();
            if (_objind > -1) Object = proj.Objects[_objind];
            ID = reader.ReadInt32();
            CreationCode = reader.ReadString();
            IsLocked = reader.ReadBoolean();
        }

        public void Save(ProjectWriter writer, GMProject proj)
        {
            writer.Write(Position);
            if (Object != null) writer.Write(proj.Objects.IndexOf(Object));
            else throw new System.Exception("Cannot write an instance with a non existing object!");
            writer.Write(ID);
            writer.Write(CreationCode);
            writer.Write(IsLocked);
        }
    }
}
