﻿using RussellLib.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RussellLib.Assets
{
    public class GMSound : StreamBase
    {
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

        public GMSound(BinaryReader reader)
        {
            Name = ReadString(reader);
            LastChanged = ReadDate(reader);
            int Version = reader.ReadInt32();
            if (Version != 800)
            {
                throw new InvalidDataException("This library only supports .gmk GM8.0 files.");
            }

            Kind = (SoundKind)reader.ReadInt32();
            FileType = ReadString(reader);
            FileName = ReadString(reader);
            Data = null;
            if (ReadBool(reader))
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
            Preload = ReadBool(reader);

            reader.Dispose();
        }
    }
}
