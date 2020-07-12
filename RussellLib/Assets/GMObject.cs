using RussellLib.Base;
using RussellLib.Code;
using System;
using System.Collections.Generic;
using System.IO;

namespace RussellLib.Assets
{
    public class GMObject
    {
        public string Name;
        public DateTime LastChanged;
        public GMSprite Sprite;
        public bool Solid;
        public bool Visible;
        public int Depth;
        public bool Persistent;
        private int _ParentInd;
        public GMObject Parent;
        public GMSprite Mask;
        public List<List<GMEvent>> Events;

        public GMObject(ProjectReader reader, GMProject proj)
        {
            Name = reader.ReadString();
            LastChanged = reader.ReadDate();
            int version = reader.ReadInt32();
            if (version != 430)
            {
                throw new InvalidDataException("Invalid Object version, got " + version);
            }
            int spr = reader.ReadInt32();
            if (spr > -1)
            {
                Sprite = proj.Sprites[spr];
            }

            Solid = reader.ReadBoolean();
            Visible = reader.ReadBoolean();
            Depth = reader.ReadInt32();
            Persistent = reader.ReadBoolean();
            _ParentInd = reader.ReadInt32();
            int mask = reader.ReadInt32();
            if (mask > -1)
            {
                Mask = proj.Sprites[mask];
            }

            int ev_count = reader.ReadInt32();
            Events = new List<List<GMEvent>>(ev_count);
            for (int i = 0; i <= ev_count; i++)
            {
                var l = new List<GMEvent>();
                bool done = false;
                while (!done)
                {
                    int first = reader.ReadInt32();
                    if (first != -1)
                    {
                        var ev = new GMEvent(reader);
                        ev.Key = first;
                        l.Add(ev);
                    }
                    else done = true;
                }
                Events.Add(l);
            }

            reader.Dispose();
        }

        public void PostLoad(GMProject proj)
        {
            if (_ParentInd > -1)
            {
                Parent = proj.Objects[_ParentInd];
            }
        }
    }
}
