using RussellLib.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace RussellLib.Assets
{
    public class GMSprite
    {
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

        public GMSprite(ProjectReader reader)
        {
            Name = reader.ReadString();
            LastChanged = reader.ReadDate();
            int Version = reader.ReadInt32();
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
            int version = dec_reader.ReadInt32();
            if (version != 800)
            {
                throw new InvalidDataException("Unknown GMSPR version, got " + version);
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
                    throw new InvalidDataException("Unknown GMSPR frame version, got " + version);
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
