using RussellLib.Assets;
using RussellLib.Base;
using System;
using System.Collections.Generic;
using System.IO;

namespace RussellLib
{
    public class GMProject : StreamBase
    {
        // main header at the top.
        public int Version;
        public int GameID;
        public Guid DirectPlayGuid; // used in mplay_* ???

        public GMOptions Options;

        public List<GMTrigger> Triggers;
        public DateTime TriggersLastChanged;
        public List<GMConstant> Constants;
        public DateTime ConstantsLastChanged;
        public List<GMSound> Sounds;
        public List<GMSprite> Sprites;
        public List<GMBackground> Backgrounds;
        public List<GMPath> Paths;
        public List<GMScript> Scripts;
        public List<GMFont> Fonts;
        public List<GMTimeline> Timelines;

        public GMProject(BinaryReader reader)
        {
            Load_Main(reader);
        }

        private void Load_Main(BinaryReader reader)
        {
            int Magic = reader.ReadInt32();
            if (Magic != 1234321)
            {
                throw new InvalidDataException("Magic is not 1234321, got " + Magic);
            }

            Version = reader.ReadInt32();
            if (Version != 800)
            {
                throw new InvalidDataException("This library only supports .gmk GM8.0 files.");
            }

            GameID = reader.ReadInt32();
            DirectPlayGuid = ReadGuid(reader);
            Load_Options(reader);
            Load_Triggers(reader);
            Load_Constants(reader);
            Load_Sounds(reader);
            Load_Sprites(reader);
            Load_Backgrounds(reader);
            Load_Paths(reader);
            Load_Scripts(reader);
            Load_Fonts(reader);
            Load_Timelines(reader);
            // TODO: finish all assets & resource tree.
            Console.WriteLine("breakpoint on this line to check stuff.");
        }

        private void Load_Options(BinaryReader reader)
        {
            Options = new GMOptions(reader);
        }

        private void Load_Constants(BinaryReader reader)
        {
            int Version = reader.ReadInt32();
            if (Version != 800)
            {
                throw new InvalidDataException("This library only supports .gmk GM8.0 files.");
            }

            int Count = reader.ReadInt32();
            Constants = new List<GMConstant>(Count);
            for (int i = 0; i < Count; i++)
            {
                Constants.Add(new GMConstant(reader));
            }

            ConstantsLastChanged = ReadDate(reader);
        }

        private void Load_Timelines(BinaryReader reader)
        {
            int Version = reader.ReadInt32();
            if (Version != 800)
            {
                throw new InvalidDataException("This library only supports .gmk GM8.0 files.");
            }

            int Count = reader.ReadInt32();
            Timelines = new List<GMTimeline>(Count);
            for (int i = 0; i < Count; i++)
            {
                var dec_reader = MakeReaderZlib(reader);
                if (ReadBool(dec_reader))
                {
                    Timelines.Add(new GMTimeline(dec_reader));
                }
                else Timelines.Add(null);
            }
        }

        private void Load_Fonts(BinaryReader reader)
        {
            int Version = reader.ReadInt32();
            if (Version != 800)
            {
                throw new InvalidDataException("This library only supports .gmk GM8.0 files.");
            }

            int Count = reader.ReadInt32();
            Fonts = new List<GMFont>(Count);
            for (int i = 0; i < Count; i++)
            {
                var dec_reader = MakeReaderZlib(reader);
                if (ReadBool(dec_reader))
                {
                    Fonts.Add(new GMFont(dec_reader));
                }
                else Fonts.Add(null);
            }
        }

        private void Load_Scripts(BinaryReader reader)
        {
            int Version = reader.ReadInt32();
            if (Version != 800)
            {
                throw new InvalidDataException("This library only supports .gmk GM8.0 files.");
            }

            int Count = reader.ReadInt32();
            Scripts = new List<GMScript>(Count);
            for (int i = 0; i < Count; i++)
            {
                var dec_reader = MakeReaderZlib(reader);
                if (ReadBool(dec_reader))
                {
                    Scripts.Add(new GMScript(dec_reader));
                }
                else Scripts.Add(null);
            }
        }

        private void Load_Paths(BinaryReader reader)
        {
            int Version = reader.ReadInt32();
            if (Version != 800)
            {
                throw new InvalidDataException("This library only supports .gmk GM8.0 files.");
            }

            int Count = reader.ReadInt32();
            Paths = new List<GMPath>(Count);
            for (int i = 0; i < Count; i++)
            {
                var dec_reader = MakeReaderZlib(reader);
                if (ReadBool(dec_reader))
                {
                    Paths.Add(new GMPath(dec_reader));
                }
                else Paths.Add(null);
            }
        }

        private void Load_Sounds(BinaryReader reader)
        {
            int Version = reader.ReadInt32();
            if (Version != 800)
            {
                throw new InvalidDataException("This library only supports .gmk GM8.0 files.");
            }

            int Count = reader.ReadInt32();
            Sounds = new List<GMSound>(Count);
            for (int i = 0; i < Count; i++)
            {
                var dec_reader = MakeReaderZlib(reader);
                if (ReadBool(dec_reader))
                {
                    Sounds.Add(new GMSound(dec_reader));
                }
                else Sounds.Add(null);
            }
        }

        private void Load_Backgrounds(BinaryReader reader)
        {
            int Version = reader.ReadInt32();
            if (Version != 800)
            {
                throw new InvalidDataException("This library only supports .gmk GM8.0 files.");
            }

            int Count = reader.ReadInt32();
            Backgrounds = new List<GMBackground>(Count);
            for (int i = 0; i < Count; i++)
            {
                var dec_reader = MakeReaderZlib(reader);
                if (ReadBool(dec_reader))
                {
                    Backgrounds.Add(new GMBackground(dec_reader));
                }
                else Backgrounds.Add(null);
            }
        }

        private void Load_Sprites(BinaryReader reader)
        {
            int Version = reader.ReadInt32();
            if (Version != 800)
            {
                throw new InvalidDataException("This library only supports .gmk GM8.0 files.");
            }

            int Count = reader.ReadInt32();
            Sprites = new List<GMSprite>(Count);
            for (int i = 0; i < Count; i++)
            {
                var dec_reader = MakeReaderZlib(reader);
                if (ReadBool(dec_reader))
                {
                    Sprites.Add(new GMSprite(dec_reader));
                }
                else Sprites.Add(null);
            }
        }

        private void Load_Triggers(BinaryReader reader)
        {
            int Version = reader.ReadInt32();
            if (Version != 800)
            {
                throw new InvalidDataException("This library only supports .gmk GM8.0 files.");
            }

            int Count = reader.ReadInt32();
            Triggers = new List<GMTrigger>(Count);
            for (int i = 0; i < Count; i++)
            {
                var dec_reader = MakeReaderZlib(reader);
                if (ReadBool(dec_reader))
                {
                    Triggers.Add(new GMTrigger(dec_reader));
                }
                else Triggers.Add(null);
            }

            TriggersLastChanged = ReadDate(reader);
        }
    }
}
