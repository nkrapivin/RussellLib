using RussellLib.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RussellLib.Assets
{
    public class GMSound
    {
        public int Version;
        public string Name;
        public DateTime LastChanged;
        public SoundKind Kind;
        public string FileType;
        public string FileName;
        public byte[] Data;


        public bool[] Effects;

        public double Volume;
        public double Panning;

        public bool Preload;

        public enum SoundEffects
        {
            CHORUS,
            ECHO,
            FLANGER,
            GARGLE,
            REVERB,
            __LENGTH
        }

        public enum SoundKind
        {
            NORMAL,
            BACKGROUND,
            SPATIAL,
            MULTIMEDIA
        }

        public void Save(ProjectWriter writer)
        {
            writer.Write(Name);
            writer.Write(LastChanged);
            writer.Write(Version);
            writer.Write((int)Kind);
            writer.Write(FileType);
            writer.Write(FileName);
            if (Data != null)
            {
                writer.Write(1);
                writer.Write(Data.Length);
                writer.Write(Data);
            }
            else writer.Write(0);

            writer.Write(GetEffectsInt());

            writer.Write(Volume);
            writer.Write(Panning);
            writer.Write(Preload);
        }

        // thx LateralGM for this one.
        private int GetEffectsInt()
        {
            int effects = 0;
            int n = 1;
            int efflen = (int)SoundEffects.__LENGTH;
            for (int i = 0; i < efflen; i++)
            {
                if (Effects[i]) effects |= n;
                n <<= 1;
            }

            return effects;
        }

        public GMSound(ProjectReader reader)
        {
            Name = reader.ReadString();
            LastChanged = reader.ReadDate();
            Version = reader.ReadInt32();
            if (Version != 800)
            {
                throw new InvalidDataException("This library only supports .gmk GM8.0 files.");
            }

            Kind = (SoundKind)reader.ReadInt32();
            FileType = reader.ReadString();
            FileName = reader.ReadString();
            Data = null;
            if (reader.ReadBoolean())
            {
                int size = reader.ReadInt32();
                Data = reader.ReadBytes(size);
            }

            int eff = reader.ReadInt32();
            int efflen = (int)SoundEffects.__LENGTH;
            Effects = new bool[efflen];
            for (int i = 0; i < efflen; i++)
            {
                Effects[i] = (eff & 1) != 0;
                eff >>= 1;
            }

            // to check for an effect you do:
            // if (Effects[(int)SoundEffects.CHORUS]) { }

            Volume = reader.ReadDouble();
            Panning = reader.ReadDouble();
            Preload = reader.ReadBoolean();

            reader.Dispose();
        }
    }
}
