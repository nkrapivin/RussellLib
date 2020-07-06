using RussellLib.Base;
using RussellLib.Misc;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace RussellLib.Assets
{
    public class GMPath : StreamBase
    {
        public string Name;
        public DateTime LastChanged;
        public bool Smooth;
        public bool Closed;
        public uint Precision; // you can't put negative values in the editor.
        public int BackgroundRoom; // TODO GMRoom...
        public Point Snap;
        public List<PathPoint> Points;

        public GMPath(BinaryReader reader)
        {
            Name = ReadString(reader);
            LastChanged = ReadDate(reader);
            reader.ReadInt32();
            Smooth = ReadBool(reader);
            Closed = ReadBool(reader);
            Precision = reader.ReadUInt32();
            BackgroundRoom = reader.ReadInt32();

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
    }
}
