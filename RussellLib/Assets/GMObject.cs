using RussellLib.Base;
using RussellLib.Code;
using System;
using System.Collections.Generic;
using System.IO;

namespace RussellLib.Assets
{
    public class GMObject
    {
        public int Version;
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

        public void Save(ProjectWriter writer, GMProject proj)
        {
            writer.Write(Name);
            writer.Write(LastChanged);
            writer.Write(Version);
            if (Sprite != null)
            {
                writer.Write(proj.Sprites.IndexOf(Sprite));
            }
            else writer.Write(-1);

            writer.Write(Solid);
            writer.Write(Visible);
            writer.Write(Depth);
            writer.Write(Persistent);
            if (Parent != null)
            {
                writer.Write(proj.Objects.IndexOf(Parent));
            }
            else writer.Write(-1);

            if (Mask != null)
            {
                writer.Write(proj.Sprites.IndexOf(Mask));
            }
            else writer.Write(-1);

            writer.Write(Events.Count - 1);
            for (int i = 0; i < Events.Count; i++)
            {
                var l = Events[i];
                for (int j = 0; j < l.Count; j++)
                {
                    var ev = l[j];
                    writer.Write(ev.Key);
                    ev.Save(writer, proj);
                }
                writer.Write(-1);
            }
        }

        public GMObject(ProjectReader reader, GMProject proj)
        {
            Name = reader.ReadString();
            LastChanged = reader.ReadDate();
            Version = reader.ReadInt32();
            if (Version != 430)
            {
                throw new InvalidDataException("Invalid Object version, got " + Version);
            }
            Sprite = null;
            int spr = reader.ReadInt32();
            if (spr > -1)
            {
                Sprite = proj.Sprites[spr];
            }

            Solid = reader.ReadBoolean();
            Visible = reader.ReadBoolean();
            Depth = reader.ReadInt32();
            Persistent = reader.ReadBoolean();
            Parent = null;
            _ParentInd = reader.ReadInt32();
            Mask = null;
            int mask = reader.ReadInt32();
            if (mask > -1)
            {
                Mask = proj.Sprites[mask];
            }

            int ev_count = reader.ReadInt32();
            Events = new List<List<GMEvent>>(ev_count + 1);
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
