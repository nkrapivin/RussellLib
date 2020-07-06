using Ionic.Zlib;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace RussellLib.Base
{
    public class StreamBase
    {
        public static Version ReadVersion(BinaryReader reader)
        {
            int major = reader.ReadInt32();
            int minor = reader.ReadInt32();
            int release = reader.ReadInt32();
            int build = reader.ReadInt32();
            return new Version(major, minor, release, build); // I decided to swap release and build, sorry.
        }

        public static DateTime ReadDate(BinaryReader reader)
        {
            double value = reader.ReadDouble();
            DateTime start = new DateTime(1899, 12, 30); 
            DateTime end = start.AddDays(value); // okay thank you C# for not making my life a nightmare.
            return end;
        }

        public static Image ReadBGRAImage(BinaryReader reader, int width, int height)
        {
            int size = reader.ReadInt32();
            byte[] data = reader.ReadBytes(size);
            var img = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            var imgdata = img.LockBits(new Rectangle(0, 0, img.Width, img.Height), ImageLockMode.ReadWrite, img.PixelFormat);
            var ptr = imgdata.Scan0;
            Marshal.Copy(data, 0, ptr, size);
            img.UnlockBits(imgdata);

            return img;
        }

        public static Icon ReadIcon(BinaryReader reader)
        {
            int size = reader.ReadInt32();
            if (size <= 0) return null; // some bad GM8 decompilers don't include the icon, LGM fails, I don't. :)

            try
            {
                byte[] data = reader.ReadBytes(size);
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

        public static Image ReadZlibImage(BinaryReader reader)
        {
            var img_reader = MakeReaderZlib(reader);
            var ret = Image.FromStream(img_reader.BaseStream);
            return ret;
        }

        public static bool ReadBool(BinaryReader reader)
        {
            return reader.ReadInt32() != 0;
        }

        public static Guid ReadGuid(BinaryReader reader)
        {
            int size = Marshal.SizeOf(typeof(Guid)); // 16 bytes.
            byte[] guid_data = reader.ReadBytes(size);
            return new Guid(guid_data);
        }

        public static Color ReadColor(BinaryReader reader)
        {
            uint val = reader.ReadUInt32();
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

        public static byte[] ReadCompressedStream(BinaryReader reader)
        {
            int size = reader.ReadInt32();
            byte[] data = null;
            if (size >= 0)
            {
                byte[] compressed_data = reader.ReadBytes(size);
                data = ZlibStream.UncompressBuffer(compressed_data);
            }
            return data;
        }

        public static string ReadString(BinaryReader reader)
        {
            int Length = reader.ReadInt32();
            byte[] RawData = reader.ReadBytes(Length);
            return Encoding.UTF8.GetString(RawData); // some project files don't use UTF8...
        }

        public static BinaryReader MakeReaderZlib(BinaryReader reader)
        {
            return new BinaryReader(new MemoryStream(ReadCompressedStream(reader)));
        }
    }
}
