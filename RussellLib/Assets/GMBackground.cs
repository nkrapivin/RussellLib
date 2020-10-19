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

        public void SaveGMBCK(ProjectWriter writer)
        {
            const int Magic = 1234321;
            writer.Write(Magic);

            var zlib_w = new ProjectWriter(new MemoryStream());
            zlib_w.Write(Version);

            zlib_w.Write(UseAsTileset);
            zlib_w.Write(TileWidth);
            zlib_w.Write(TileHeight);
            zlib_w.Write(OffsetH);
            zlib_w.Write(OffsetV);
            zlib_w.Write(SepH);
            zlib_w.Write(SepV);
            zlib_w.Write(FrameVersion);
            if (Background != null)
            {
                zlib_w.Write(Background.Size);
                zlib_w.Write(Background, true);
            }
            else
            {
                // w = 0, h = 0, no image.
                zlib_w.Write(0);
                zlib_w.Write(0);
            }

            writer.WriteZlibChunk(zlib_w);
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

            Version = dec_reader.ReadInt32();
            if (Version != 710)
            {
                throw new InvalidDataException("Unknown GMBCK version, got " + magic);
            }

            UseAsTileset = dec_reader.ReadBoolean();
            TileWidth = dec_reader.ReadInt32();
            TileHeight = dec_reader.ReadInt32();
            OffsetH = dec_reader.ReadInt32();
            OffsetV = dec_reader.ReadInt32();
            SepH = dec_reader.ReadInt32();
            SepV = dec_reader.ReadInt32();

            FrameVersion = dec_reader.ReadInt32();
            if (FrameVersion != 800)
            {
                throw new InvalidDataException("Unknown GMBCK image version, got " + magic);
            }

            int Width, Height;
            Width = dec_reader.ReadInt32();
            Height = dec_reader.ReadInt32();
            Background = null;
            if (Width * Height != 0) Background = dec_reader.ReadBGRAImage(Width, Height);

            dec_reader.Dispose();
        }
    }
}
