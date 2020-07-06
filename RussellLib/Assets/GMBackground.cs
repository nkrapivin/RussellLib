using RussellLib.Base;
using System;
using System.Drawing;
using System.IO;

namespace RussellLib.Assets
{
    public class GMBackground : StreamBase
    {
        public string Name;
        public DateTime LastChanged;
        public bool UseAsTileset;
        public int TileWidth;
        public int TileHeight;
        public int OffsetH;
        public int OffsetV;
        public int SepH;
        public int SepV;
        public int Width;
        public int Height;
        public Image Background;

        public GMBackground(BinaryReader reader)
        {
            Name = ReadString(reader);
            LastChanged = ReadDate(reader);
            reader.ReadInt32(); // version that we don't care about here.
            UseAsTileset = ReadBool(reader);
            TileWidth = reader.ReadInt32();
            TileHeight = reader.ReadInt32();
            OffsetH = reader.ReadInt32();
            OffsetV = reader.ReadInt32();
            SepH = reader.ReadInt32();
            SepV = reader.ReadInt32();
            reader.ReadInt32(); // frame version
            Width = reader.ReadInt32();
            Height = reader.ReadInt32();
            Background = null;
            if (Width * Height != 0) Background = ReadBGRAImage(reader, Width, Height);

            reader.Dispose();
        }

        public GMBackground(BinaryReader reader, bool _gmbck)
        {
            // Same as .gmspr...

            int magic = reader.ReadInt32();
            if (magic != 1234321)
            {
                throw new InvalidDataException("Wrong GMBCK magic, got " + magic);
            }

            var dec_reader = MakeReaderZlib(reader);

            int version = dec_reader.ReadInt32();
            if (version != 710)
            {
                throw new InvalidDataException("Unknown GMBCK version, got " + magic);
            }

            UseAsTileset = ReadBool(reader);
            TileWidth = reader.ReadInt32();
            TileHeight = reader.ReadInt32();
            OffsetH = reader.ReadInt32();
            OffsetV = reader.ReadInt32();
            SepH = reader.ReadInt32();
            SepV = reader.ReadInt32();

            int frameversion = reader.ReadInt32();
            if (frameversion != 800)
            {
                throw new InvalidDataException("Unknown GMBCK image version, got " + magic);
            }

            Width = reader.ReadInt32();
            Height = reader.ReadInt32();
            Background = null;
            if (Width * Height != 0) Background = ReadBGRAImage(reader, Width, Height);

            dec_reader.Dispose();
        }
    }
}
