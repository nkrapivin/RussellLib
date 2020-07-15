using RussellLib.Base;
using System;
using System.Drawing;
using System.IO;

namespace RussellLib.Assets
{
    public class GMBackground
    {
        public int Version;
        public int FrameVersion;
        public string Name;
        public DateTime LastChanged;
        public bool UseAsTileset;
        public int TileWidth;
        public int TileHeight;
        public int OffsetH;
        public int OffsetV;
        public int SepH;
        public int SepV;
        public Image Background;

        public void Save(ProjectWriter writer)
        {
            writer.Write(Name);
            writer.Write(LastChanged);
            writer.Write(Version);
            writer.Write(UseAsTileset);
            writer.Write(TileWidth);
            writer.Write(TileHeight);
            writer.Write(OffsetH);
            writer.Write(OffsetV);
            writer.Write(SepH);
            writer.Write(SepV);
            writer.Write(FrameVersion);
            if (Background != null)
            {
                writer.Write(Background.Size);
                writer.Write(Background, true);
            }
            else
            {
                // w = 0, h = 0, no image.
                writer.Write(0);
                writer.Write(0);
            }
        }

        public GMBackground(ProjectReader reader)
        {
            Name = reader.ReadString();
            LastChanged = reader.ReadDate();
            Version = reader.ReadInt32(); // version that we don't care about here.
            UseAsTileset = reader.ReadBoolean();
            TileWidth = reader.ReadInt32();
            TileHeight = reader.ReadInt32();
            OffsetH = reader.ReadInt32();
            OffsetV = reader.ReadInt32();
            SepH = reader.ReadInt32();
            SepV = reader.ReadInt32();
            FrameVersion = reader.ReadInt32(); // frame version
            int Width, Height;
            Width = reader.ReadInt32();
            Height = reader.ReadInt32();
            Background = null;
            if (Width * Height != 0) Background = reader.ReadBGRAImage(Width, Height);

            reader.Dispose();
        }

        public GMBackground(ProjectReader reader, bool _gmbck)
        {
            // Same as .gmspr...

            int magic = reader.ReadInt32();
            if (magic != 1234321)
            {
                throw new InvalidDataException("Wrong GMBCK magic, got " + magic);
            }

            var dec_reader = reader.MakeReaderZlib();

            int version = dec_reader.ReadInt32();
            if (version != 710)
            {
                throw new InvalidDataException("Unknown GMBCK version, got " + magic);
            }

            UseAsTileset = reader.ReadBoolean();
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

            int Width, Height;
            Width = reader.ReadInt32();
            Height = reader.ReadInt32();
            Background = null;
            if (Width * Height != 0) Background = reader.ReadBGRAImage(Width, Height);

            dec_reader.Dispose();
        }
    }
}
