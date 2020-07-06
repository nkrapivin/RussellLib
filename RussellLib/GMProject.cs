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
        public List<GMObject> Objects;
        public List<GMRoom> Rooms;

        public int LastInstanceID;
        public int LastTileID;

        public List<GMIncludedFile> IncludedFiles;
        public List<string> ExtensionPackageNames;
        public GMGameInformation GameInformation;
        public List<string> LibraryCreationCode; // if you've ever used custom .lib files you know what this is.
        public List<GMRoom> RoomExecutionOrder; // order for room_goto_next/previous()

        public List<ResourceTreeItem> ResourceTree; // ......

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
            Load_Objects(reader);
            Load_Rooms(reader);
            Load_LastIDs(reader);
            Load_IncludedFiles(reader);
            Load_ExtensionPackages(reader);
            Load_GameInformation(reader);
            Load_LibCreationCode(reader);
            Load_RoomOrder(reader);
            PostLoad();

            // *try* to load the resource tree...
            Load_ResourceTree(reader);
        }

        private void PostLoad()
        {
            // Update parent/bg/object/etc stuff...

            for (int i = 0; i < Paths.Count; i++)
            {
                var p = Paths[i];
                if (p == null) continue;

                p.PostLoad(this);
            }

            for (int i = 0; i < Timelines.Count; i++)
            {
                var t = Timelines[i];
                if (t == null) continue;

                for (int j = 0; j < t.Moments.Count; j++)
                {
                    var m = t.Moments[i];
                    for (int k = 0; k < m.Event.Actions.Count; k++)
                    {
                        var a = m.Event.Actions[k];
                        a.PostLoad(this);
                    }
                }
            }

            for (int i = 0; i < Objects.Count; i++)
            {
                var o = Objects[i];
                if (o == null) continue;

                o.PostLoad(this);
                for (int j = 0; j < o.Events.Count; j++)
                {
                    var e = o.Events[i];
                    for (int k = 0; k < e.Count; k++)
                    {
                        var e_in = e[k];
                        for (int a = 0; a < e_in.Actions.Count; a++)
                        {
                            e_in.Actions[a].PostLoad(this);
                        }
                    }
                }
            }
        }

        private void Load_ResourceTree(BinaryReader reader)
        {
            int rootnodes = 12; // 12 seems to be the value for GM8.0
            ResourceTree = new List<ResourceTreeItem>(rootnodes);
            while (rootnodes-- > 0)
            {
                ResourceTree.Add(new ResourceTreeItem(reader));
            }
        }

        private void Load_RoomOrder(BinaryReader reader)
        {
            int Version = reader.ReadInt32();
            if (Version != 700)
            {
                throw new InvalidDataException("Invalid room execution order version.");
            }

            int Count = reader.ReadInt32();
            RoomExecutionOrder = new List<GMRoom>(Count);
            for (int i = 0; i < Count; i++)
            {
                int room_ind = reader.ReadInt32();
                RoomExecutionOrder.Add(Rooms[room_ind]);
            }
        }

        private void Load_LibCreationCode(BinaryReader reader)
        {
            int Version = reader.ReadInt32();
            if (Version != 500)
            {
                throw new InvalidDataException("Invalid library creation code version.");
            }

            int Count = reader.ReadInt32();
            LibraryCreationCode = new List<string>(Count);
            for (int i = 0; i < Count; i++)
            {
                LibraryCreationCode.Add(ReadString(reader));
            }
        }

        private void Load_LastIDs(BinaryReader reader)
        {
            LastInstanceID = reader.ReadInt32();
            LastTileID = reader.ReadInt32();
        }

        private void Load_GameInformation(BinaryReader reader)
        {
            int Version = reader.ReadInt32();
            if (Version != 800)
            {
                throw new InvalidDataException("This library only supports .gmk GM8.0 files.");
            }

            var dec_reader = MakeReaderZlib(reader);
            GameInformation = new GMGameInformation(dec_reader);
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

        private void Load_ExtensionPackages(BinaryReader reader)
        {
            int Version = reader.ReadInt32();
            if (Version != 700)
            {
                throw new InvalidDataException("Invalid extension package version.");
            }

            int Count = reader.ReadInt32();
            ExtensionPackageNames = new List<string>(Count);
            for (int i = 0; i < Count; i++)
            {
                ExtensionPackageNames.Add(ReadString(reader));
            }
        }

        private void Load_IncludedFiles(BinaryReader reader)
        {
            int Version = reader.ReadInt32();
            if (Version != 800)
            {
                throw new InvalidDataException("This library only supports .gmk GM8.0 files.");
            }

            int Count = reader.ReadInt32();
            IncludedFiles = new List<GMIncludedFile>(Count);
            for (int i = 0; i < Count; i++)
            {
                var dec_reader = MakeReaderZlib(reader);
                if (ReadBool(dec_reader))
                {
                    IncludedFiles.Add(new GMIncludedFile(dec_reader));
                }
                else IncludedFiles.Add(null);
            }
        }

        private void Load_Rooms(BinaryReader reader)
        {
            int Version = reader.ReadInt32();
            if (Version != 800)
            {
                throw new InvalidDataException("This library only supports .gmk GM8.0 files.");
            }

            int Count = reader.ReadInt32();
            Rooms = new List<GMRoom>(Count);
            for (int i = 0; i < Count; i++)
            {
                var dec_reader = MakeReaderZlib(reader);
                if (ReadBool(dec_reader))
                {
                    Rooms.Add(new GMRoom(dec_reader, this));
                }
                else Rooms.Add(null);
            }
        }

        private void Load_Objects(BinaryReader reader)
        {
            int Version = reader.ReadInt32();
            if (Version != 800)
            {
                throw new InvalidDataException("This library only supports .gmk GM8.0 files.");
            }

            int Count = reader.ReadInt32();
            Objects = new List<GMObject>(Count);
            for (int i = 0; i < Count; i++)
            {
                var dec_reader = MakeReaderZlib(reader);
                if (ReadBool(dec_reader))
                {
                    Objects.Add(new GMObject(dec_reader, this));
                }
                else Objects.Add(null);
            }
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
