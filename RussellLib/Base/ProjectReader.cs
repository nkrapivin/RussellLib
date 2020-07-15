using Ionic.Zlib;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace RussellLib.Base
{
    public class ProjectReader : BinaryReader
    {
        private bool UseUTF8;

        public ProjectReader(Stream input, bool use_utf8 = true) : base(input)
        {
            UseUTF8 = use_utf8;
        }

        public override bool ReadBoolean()
        {
            return ReadInt32() != 0;
        }

        public override string ReadString()
        {
            if (UseUTF8) return Encoding.UTF8.GetString(ReadBytes(ReadInt32())); // some project files don't use UTF8...
            else return Encoding.Default.GetString(ReadBytes(ReadInt32())); // sorta fix russian language in project files...
        }

        public Version ReadVersion()
        {
            int major = ReadInt32();
            int minor = ReadInt32();
            int release = ReadInt32();
            int build = ReadInt32();
            return new Version(major, minor, release, build); // I decided to swap release and build, sorry.
        }

        public Guid ReadGuid()
        {
            const int guid_size = 16;
            return new Guid(ReadBytes(guid_size));
        }

        public DateTime ReadDate()
        {
            return new DateTime(1899, 12, 30).AddDays(ReadDouble()); // okay thank you C# for not making my life a nightmare.;
        }

        public byte[] ReadCompressedStream()
        {
            int size = ReadInt32();
            byte[] data = null;
            if (size >= 0)
            {
                byte[] compressed_data = ReadBytes(size);
                data = ZlibStream.UncompressBuffer(compressed_data);
            }
            return data;
        }

        public Image ReadBGRAImage(int width, int height)
        {
            int size = ReadInt32();
            byte[] data = ReadBytes(size);
            var img = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            var imgdata = img.LockBits(new Rectangle(0, 0, img.Width, img.Height), ImageLockMode.ReadWrite, img.PixelFormat);
            var ptr = imgdata.Scan0;
            Marshal.Copy(data, 0, ptr, size);
            img.UnlockBits(imgdata);

            return img;
        }

        public Icon ReadIcon()
        {
            int size = ReadInt32();
            if (size <= 0) return null; // some bad GM8 decompilers don't include the icon, LGM fails, I don't. :)

            try
            {
                byte[] data = ReadBytes(size);
                var stream = new MemoryStream(data);
                var icon = new Icon(stream);
                stream.Dispose();
                return icon;
            }
            catch
            {
                return null;
            }
        }

        public Image ReadZlibImage()
        {
            var img_reader = MakeReaderZlib();
            var ret = Image.FromStream(img_reader.BaseStream);
            return ret;
        }

        public ProjectReader MakeReaderZlib()
        {
            return new ProjectReader(new MemoryStream(ReadCompressedStream()));
        }

        public Color ReadColor()
        {
            uint val = ReadUInt32();
            if (val > 0xFFFFFF) // Contains alpha.
            {
                uint r, g, b, a;
                r = (val & 0xFF);
                g = (val >> 8) & 0xFF;
                b = (val >> 16) & 0xFF;
                a = (val >> 24) & 0xFF;
                return Color.FromArgb((int)a, (int)r, (int)g, (int)b);
            }
            else
            {
                uint r, g, b;
                r = (val & 0xFF);
                g = (val >> 8) & 0xFF;
                b = (val >> 16) & 0xFF;
                return Color.FromArgb(0xFF, (int)r, (int)g, (int)b);
            }
        }

        public ProjectReader MakeEncryptedReader()
        {
            int skip = ReadInt32();
            int skip2 = ReadInt32();
            for (int i = 1; i <= skip; i++)
            {
                ReadInt32(); // skip
            }
            byte[] array = ReadBytes(256);
            for (int j = 1; j <= skip2; j++)
            {
                ReadInt32(); // skip
            }
            byte[] array2 = new byte[256];
            for (int k = 0; k < 256; k++)
            {
                array2[array[k]] = (byte)k;
            }
            int num3 = ReadInt32();
            byte[] array3 = ReadBytes(num3);
            for (int num4 = num3 - 1; num4 >= 1; num4--)
            {
                int num5 = (array2[array3[num4]] - array3[num4 - 1] - num4) % 256;
                if (num5 < 0)
                {
                    num5 += 256;
                }
                array3[num4] = (byte)num5;
            }
            for (int num6 = num3 - 1; num6 >= 0; num6--)
            {
                int num7 = Math.Max(0, num6 - array[num6 % 256]);
                byte b = array3[num6];
                array3[num6] = array3[num7];
                array3[num7] = b;
            }
            return new ProjectReader(new MemoryStream(array3));
        }
    }
}
