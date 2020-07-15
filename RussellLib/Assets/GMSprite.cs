using RussellLib.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace RussellLib.Assets
{
    public class GMSprite
    {
        public int Version;
        public string Name;
        public DateTime LastChanged;
        public Point Origin;
        public int Width;
        public int Height;
        public List<Image> Subimages;
        public SpriteMaskMode MaskMode;
        public int AlphaTolerance;
        public bool SeparateMasks;
        public SpriteBBoxMode BBoxMode;
        public Rectangle BBox;

        public enum SpriteMaskMode
        {
            PRECISE,
            RECTANGLE,
            DISK,
            DIAMOND
        }

        public enum SpriteBBoxMode
        {
            AUTO,
            FULL,
            MANUAL
        }

        public void Save(ProjectWriter writer)
        {
            writer.Write(Name);
            writer.Write(LastChanged);
            writer.Write(Version);
            writer.Write(Origin);

            writer.Write(Subimages.Count);
            for (int i = 0; i < Subimages.Count; i++)
            {
                var frame = Subimages[i];
                writer.Write(800);
                writer.Write(frame.Size);
                if (frame.Width * frame.Height != 0)
                {
                    writer.Write(frame, true);
                }
            }

            writer.Write((int)MaskMode);
            writer.Write(AlphaTolerance);
            writer.Write(SeparateMasks);
            writer.Write((int)BBoxMode);
            writer.Write(BBox);
        }

        public GMSprite(ProjectReader reader)
        {
            Name = reader.ReadString();
            LastChanged = reader.ReadDate();
            Version = reader.ReadInt32();
            if (Version != 800)
            {
                throw new InvalidDataException("This library only supports .gmk GM8.0 files.");
            }

            int _x = reader.ReadInt32();
            int _y = reader.ReadInt32();
            Origin = new Point(_x, _y);

            int framenum = reader.ReadInt32();
            Subimages = new List<Image>(framenum);
            for (int i = 0; i < framenum; i++)
            {
                int framever = reader.ReadInt32();
                if (framever != 800)
                {
                    throw new InvalidDataException("This library only supports .gmk GM8.0 files.");
                }
                Width = reader.ReadInt32();
                Height = reader.ReadInt32();
                if (Width * Height != 0)
                {
                    Subimages.Add(reader.ReadBGRAImage(Width, Height));
                }
                else Subimages.Add(null); // ????
            }

            MaskMode = (SpriteMaskMode)reader.ReadInt32();
            AlphaTolerance = reader.ReadInt32();
            SeparateMasks = reader.ReadBoolean();
            BBoxMode = (SpriteBBoxMode)reader.ReadInt32();

            int _l = reader.ReadInt32();
            int _r = reader.ReadInt32();
            int _b = reader.ReadInt32();
            int _t = reader.ReadInt32();
            BBox = new Rectangle(_l, _r, _b, _t);

            reader.Dispose();
        }

        public void SaveGMSPR(ProjectWriter writer)
        {
            const int Magic = 1234321;
            writer.Write(Magic);

            var zlib_w = new ProjectWriter(new MemoryStream());
            zlib_w.Write(Version);
            zlib_w.Write(Origin);

            zlib_w.Write(Subimages.Count);
            for (int i = 0; i < Subimages.Count; i++)
            {
                var frame = Subimages[i];
                zlib_w.Write(800);
                zlib_w.Write(frame.Size);
                if (frame.Width * frame.Height != 0)
                {
                    zlib_w.Write(frame, true);
                }
            }

            zlib_w.Write((int)MaskMode);
            zlib_w.Write(AlphaTolerance);
            zlib_w.Write(SeparateMasks);
            zlib_w.Write((int)BBoxMode);
            zlib_w.Write(BBox);

            writer.WriteZlibChunk(zlib_w); // zlib_w gets disposed automatically wew.
        }

        public GMSprite(ProjectReader reader, bool _gmspr)
        {
            // Basically the same code except that we read the magic and uncompress data.

            int magic = reader.ReadInt32();
            if (magic != 1234321)
            {
                throw new InvalidDataException("Invalid GMSPR header, got " + magic);
            }

            // use the zlib compressed reader from now on.
            var dec_reader = reader.MakeReaderZlib();
            Version = dec_reader.ReadInt32();
            if (Version != 800)
            {
                throw new InvalidDataException("Unknown GMSPR version, got " + Version);
            }

            int _x = reader.ReadInt32();
            int _y = reader.ReadInt32();
            Origin = new Point(_x, _y);

            int framenum = reader.ReadInt32();
            Subimages = new List<Image>(framenum);
            for (int i = 0; i < framenum; i++)
            {
                int framever = reader.ReadInt32();
                if (framever != 800)
                {
                    throw new InvalidDataException("Unknown GMSPR frame version, got " + framever);
                }
                Width = reader.ReadInt32();
                Height = reader.ReadInt32();
                if (Width * Height != 0)
                {
                    Subimages.Add(reader.ReadBGRAImage(Width, Height));
                }
                else Subimages.Add(null); // ????
            }

            MaskMode = (SpriteMaskMode)reader.ReadInt32();
            AlphaTolerance = reader.ReadInt32();
            SeparateMasks = reader.ReadBoolean();
            BBoxMode = (SpriteBBoxMode)reader.ReadInt32();

            int _l = reader.ReadInt32();
            int _r = reader.ReadInt32();
            int _b = reader.ReadInt32();
            int _t = reader.ReadInt32();
            BBox = new Rectangle(_l, _r, _b, _t);

            dec_reader.Dispose();
        }
    }
}
