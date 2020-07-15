using Ionic.Zlib;
using RussellLib.Misc;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace RussellLib.Base
{
    public class ProjectWriter : BinaryWriter
    {
        private bool UseUTF8;

        public ProjectWriter(Stream output, bool use_utf8 = true) : base(output)
        {
            UseUTF8 = use_utf8;
        }

        public void WriteZlibChunk(ProjectWriter w_in, bool dispose = true)
        {
            var stream = (MemoryStream)w_in.BaseStream;
            var data = ZlibStream.CompressBuffer(stream.ToArray());
            Write(data.Length);
            Write(data);
            if (dispose) w_in.Dispose();
        }

        public void Write(Guid value) => Write(value.ToByteArray());
        public void Write(DateTime value) => Write(value.Subtract(new DateTime(1899, 12, 30)).TotalDays);
        public void Write(Color value) => Write(value.R | value.G << 8 | value.B << 16);
        public void Write(Point value)
        {
            Write(value.X);
            Write(value.Y);
        }

        public void Write(Size value)
        {
            Write(value.Width);
            Write(value.Height);
        }

        public void Write(Rectangle value)
        {
            Write(value.X);
            Write(value.Y);
            Write(value.Width);
            Write(value.Height);
        }

        public override void Write(bool value) => Write(value ? 1 : 0); // ah, the dreaded bool longints...

        public override void Write(string value)
        {
            byte[] data;
            if (UseUTF8) data = Encoding.UTF8.GetBytes(value);
            else data = Encoding.UTF8.GetBytes(value);
            Write(data.Length);
            Write(data);
        }

        public void Write(Image img)
        {
            var stream = new MemoryStream();
            img.Save(stream, ImageFormat.Bmp);
            var data = ZlibStream.CompressBuffer(stream.ToArray());
            Write(data.Length);
            Write(data);
            stream.Dispose();
        }

        public void Write(PathPoint value)
        {
            Write(value.X);
            Write(value.Y);
            Write(value.Speed);
        }

        public void Write(Image img, bool _is_bgra)
        {
            var bitmap = new Bitmap(img);
            var bmdata = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
            var x = img.Height;
            var size = bmdata.Stride * x;
            byte[] data = new byte[size];
            Marshal.Copy(bmdata.Scan0, data, 0, size);
            Write(size);
            Write(data);
            bitmap.UnlockBits(bmdata);
            bitmap.Dispose();
        }

        public void Write(Icon ico)
        {
            var stream = new MemoryStream();
            ico.Save(stream);
            Write((int)stream.Length);
            Write(stream.ToArray());
            stream.Dispose();
        }

        public void Write(Version ver)
        {
            Write(ver.Major);
            Write(ver.Minor);
            Write(ver.Build);
            Write(ver.Revision);
        }
    }
}
