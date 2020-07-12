using RussellLib.Base;
using RussellLib.Misc;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace RussellLib.Assets
{
    public class GMPath
    {
        public string Name;
        public DateTime LastChanged;
        public bool Smooth;
        public bool Closed;
        public uint Precision; // you can't put negative values in the editor.
        public GMRoom BackgroundRoom;
        private int _BackgroundRoom;
        public Point Snap;
        public List<PathPoint> Points;

        public GMPath(ProjectReader reader)
        {
            Name = reader.ReadString();
            LastChanged = reader.ReadDate();
            reader.ReadInt32();
            Smooth = reader.ReadBoolean();
            Closed = reader.ReadBoolean();
            Precision = reader.ReadUInt32();
            _BackgroundRoom = reader.ReadInt32();

            int _x = reader.ReadInt32();
            int _y = reader.ReadInt32();
            Snap = new Point(_x, _y);

            int pointcnt = reader.ReadInt32();
            Points = new List<PathPoint>(pointcnt);
            for (int i = 0; i < pointcnt; i++)
            {
                double _px = reader.ReadDouble();
                double _py = reader.ReadDouble();
                double _pspeed = reader.ReadDouble();
                Points.Add(new PathPoint(_px, _py, _pspeed));
            }

            reader.Dispose();
        }

        public void PostLoad(GMProject proj)
        {
            if (_BackgroundRoom > -1)
            {
                BackgroundRoom = proj.Rooms[_BackgroundRoom];
            }
        }
    }
}
