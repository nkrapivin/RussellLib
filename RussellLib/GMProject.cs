using Ionic.Zlib;
using RussellLib.Assets;
using RussellLib.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;

namespace RussellLib
{
    public class GMProject
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

        public GMProject(Stream input, bool use_utf8 = true)
        {
            Load_Main(new ProjectReader(input, use_utf8));
        }

        public void Save(string path)
        {
            var fstream = File.OpenWrite(path);
            var fwriter = new ProjectWriter(fstream);
            Save_Main(fwriter);
            fwriter.Dispose();
        }

        public void Save(Stream stream)
        {
            var writer = new ProjectWriter(stream);
            Save_Main(writer);
            stream.Flush();
            writer.Dispose();
        }

        private void Save_Main(ProjectWriter writer)
        {
            const int Magic = 1234321;
            writer.Write(Magic);
            writer.Write(Version);
            Save_Header(writer);
            Save_Options(writer);
            Save_Triggers(writer);
            Save_Constants(writer);
            Save_Sounds(writer);
            Save_Sprites(writer);
            Save_Backgrounds(writer);
            Save_Paths(writer);
            Save_Scripts(writer);
            Save_Fonts(writer);
            Save_Timelines(writer);
            Save_Objects(writer);
            Save_Rooms(writer);
            Save_LastIDs(writer);
            Save_IncludedFiles(writer);
            Save_ExtensionPackages(writer);
            Save_GameInformation(writer);
            Save_LibCreationCode(writer);
            Save_RoomOrder(writer);
            Save_ResourceTree(writer);
        }

        private void Save_ResourceTree(ProjectWriter writer)
        {
            for (int i = 0; i < ResourceTree.Count; i++)
            {
                ResourceTree[i].Save(writer);
            }
        }

        private void Save_Header(ProjectWriter writer)
        {
            writer.Write(GameID);
            writer.Write(DirectPlayGuid);
        }

        private void Save_Triggers(ProjectWriter writer)
        {
            writer.Write(800); // version
            writer.Write(Triggers.Count);
            for (int i = 0; i < Triggers.Count; i++)
            {
                var t = Triggers[i];
                if (t == null) Save_NullItem(writer);
                else
                {
                    var z_writer = new ProjectWriter(new MemoryStream());
                    t.Save(z_writer);
                    writer.WriteZlibChunk(z_writer);
                }
            }
            writer.Write(TriggersLastChanged);
        }

        private void Save_NullItem(ProjectWriter writer)
        {
            var stream = new MemoryStream();
            var w = new ProjectWriter(stream);
            w.Write(0);
            writer.WriteZlibChunk(w);
        }

        private void Save_Options(ProjectWriter writer)
        {
            writer.Write(Options.FormatVersion);
            var z_writer = new ProjectWriter(new MemoryStream());
            Options.Write(z_writer);
            writer.WriteZlibChunk(z_writer);
        }

        private void Save_Constants(ProjectWriter writer)
        {
            writer.Write(800);
            writer.Write(Constants.Count);
            for (int i = 0; i < Constants.Count; i++)
            {
                var c = Constants[i];
                c.Save(writer);
            }
            writer.Write(ConstantsLastChanged);
        }

        private void Save_Sounds(ProjectWriter writer)
        {
            writer.Write(800);
            writer.Write(Sounds.Count);
            for (int i = 0; i < Sounds.Count; i++)
            {
                var s = Sounds[i];
                if (s == null) Save_NullItem(writer);
                else
                {
                    var z_writer = new ProjectWriter(new MemoryStream());
                    z_writer.Write(1);
                    s.Save(z_writer);
                    writer.WriteZlibChunk(z_writer);
                }
            }
        }

        private void Save_Sprites(ProjectWriter writer)
        {
            writer.Write(800);
            writer.Write(Sprites.Count);
            for (int i = 0; i < Sprites.Count; i++)
            {
                var s = Sprites[i];
                if (s == null) Save_NullItem(writer);
                else
                {
                    var z_writer = new ProjectWriter(new MemoryStream());
                    z_writer.Write(1);
                    s.Save(z_writer);
                    writer.WriteZlibChunk(z_writer);
                }
            }
        }

        private void Save_Backgrounds(ProjectWriter writer)
        {
            writer.Write(800);
            writer.Write(Backgrounds.Count);
            for (int i = 0; i < Backgrounds.Count; i++)
            {
                var b = Backgrounds[i];
                if (b == null) Save_NullItem(writer);
                else
                {
                    var z_writer = new ProjectWriter(new MemoryStream());
                    z_writer.Write(1);
                    b.Save(z_writer);
                    writer.WriteZlibChunk(z_writer);
                }
            }
        }

        private void Save_Paths(ProjectWriter writer)
        {
            writer.Write(800);
            writer.Write(Paths.Count);
            for (int i = 0; i < Paths.Count; i++)
            {
                var p = Paths[i];
                if (p == null) Save_NullItem(writer);
                else
                {
                    var z_writer = new ProjectWriter(new MemoryStream());
                    z_writer.Write(1);
                    p.Save(z_writer, this);
                    writer.WriteZlibChunk(z_writer);
                }
            }
        }

        private void Save_Scripts(ProjectWriter writer)
        {
            writer.Write(800);
            writer.Write(Scripts.Count);
            for (int i = 0; i < Scripts.Count; i++)
            {
                var s = Scripts[i];
                if (s == null) Save_NullItem(writer);
                else
                {
                    var z_writer = new ProjectWriter(new MemoryStream());
                    z_writer.Write(1);
                    s.Save(z_writer);
                    writer.WriteZlibChunk(z_writer);
                }
            }
        }

        private void Save_Timelines(ProjectWriter writer)
        {
            writer.Write(800);
            writer.Write(Timelines.Count);
            for (int i = 0; i < Timelines.Count; i++)
            {
                var t = Timelines[i];
                if (t == null) Save_NullItem(writer);
                else
                {
                    var z_writer = new ProjectWriter(new MemoryStream());
                    z_writer.Write(1);
                    t.Save(z_writer, this);
                    writer.WriteZlibChunk(z_writer);
                }
            }
        }

        private void Save_Fonts(ProjectWriter writer)
        {
            writer.Write(800);
            writer.Write(Fonts.Count);
            for (int i = 0; i < Fonts.Count; i++)
            {
                var f = Fonts[i];
                if (f == null) Save_NullItem(writer);
                else
                {
                    var z_writer = new ProjectWriter(new MemoryStream());
                    z_writer.Write(1);
                    f.Save(z_writer);
                    writer.WriteZlibChunk(z_writer);
                }
            }
        }

        private void Save_Objects(ProjectWriter writer)
        {
            writer.Write(800);
            writer.Write(Objects.Count);
            for (int i = 0; i < Objects.Count; i++)
            {
                var o = Objects[i];
                if (o == null) Save_NullItem(writer);
                else
                {
                    var z_writer = new ProjectWriter(new MemoryStream());
                    z_writer.Write(1);
                    o.Save(z_writer, this);
                    writer.WriteZlibChunk(z_writer);
                }
            }
        }

        private void Save_Rooms(ProjectWriter writer)
        {
            writer.Write(800);
            writer.Write(Rooms.Count);
            for (int i = 0; i < Rooms.Count; i++)
            {
                var r = Rooms[i];
                if (r == null) Save_NullItem(writer);
                else
                {
                    var z_writer = new ProjectWriter(new MemoryStream());
                    z_writer.Write(1);
                    r.Save(z_writer, this);
                    writer.WriteZlibChunk(z_writer);
                }
            }
        }

        private void Save_IncludedFiles(ProjectWriter writer)
        {
            writer.Write(800);
            writer.Write(IncludedFiles.Count);
            for (int i = 0; i < IncludedFiles.Count; i++)
            {
                var f = IncludedFiles[i];
                if (f == null) Save_NullItem(writer);
                else
                {
                    var z_writer = new ProjectWriter(new MemoryStream());
                    z_writer.Write(1);
                    f.Save(z_writer);
                    writer.WriteZlibChunk(z_writer);
                }
            }
        }

        private void Save_LibCreationCode(ProjectWriter writer)
        {
            writer.Write(500);
            writer.Write(LibraryCreationCode.Count);
            for (int i = 0; i < LibraryCreationCode.Count; i++)
            {
                var l = LibraryCreationCode[i];
                writer.Write(l);
            }
        }

        private void Save_RoomOrder(ProjectWriter writer)
        {
            writer.Write(700);
            writer.Write(RoomExecutionOrder.Count);
            for (int i = 0; i < RoomExecutionOrder.Count; i++)
            {
                var r = RoomExecutionOrder[i];
                writer.Write(RoomExecutionOrder.IndexOf(r));
            }
        }

        private void Save_ExtensionPackages(ProjectWriter writer)
        {
            writer.Write(700);
            writer.Write(ExtensionPackageNames.Count);
            for (int i = 0; i < ExtensionPackageNames.Count; i++)
            {
                var s = ExtensionPackageNames[i];
                writer.Write(s);
            }
        }

        private void Save_GameInformation(ProjectWriter writer)
        {
            writer.Write(GameInformation.Version);
            var z_writer = new ProjectWriter(new MemoryStream());
            GameInformation.Save(z_writer);
            writer.WriteZlibChunk(z_writer);
        }

        private void Save_LastIDs(ProjectWriter writer)
        {
            writer.Write(LastInstanceID);
            writer.Write(LastTileID);
        }

        private void Report(string text)
        {
            Debug.WriteLine(text);
        }

        private void Load_Main(ProjectReader reader)
        {
            int Magic = reader.ReadInt32();

            if (Magic != 1234321)
            {
                throw new InvalidDataException("Magic is not 1234321, got " + Magic);
            }

            Version = reader.ReadInt32();

            if (Version != 800 && Version != 810)
            {
                throw new InvalidDataException("This library only supports GM8.x files.");
            }

            Report("Header");
            Load_Header(reader);
            Report("Options");
            Load_Options(reader);
            Report("Triggers");
            Load_Triggers(reader);
            Report("Constants");
            Load_Constants(reader);
            Report("Sounds");
            Load_Sounds(reader);
            Report("Sprites");
            Load_Sprites(reader);
            Report("Backgrounds");
            Load_Backgrounds(reader);
            Report("Paths");
            Load_Paths(reader);
            Report("Scripts");
            Load_Scripts(reader);
            Report("Fonts");
            Load_Fonts(reader);
            Report("Timelines");
            Load_Timelines(reader);
            Report("Objects");
            Load_Objects(reader);
            Report("Rooms");
            Load_Rooms(reader);
            Report("Last IDs");
            Load_LastIDs(reader);
            Report("Included Files");
            Load_IncludedFiles(reader);
            Report("Extensions");
            Load_ExtensionPackages(reader);
            Report("Game Information");
            Load_GameInformation(reader);
            Report("Library creation code");
            Load_LibCreationCode(reader);
            Report("Room Execution order");
            Load_RoomOrder(reader);
            Report("Post fixups");
            PostLoad();

            // *try* to load the resource tree...
            Report("Resource tree");
            Load_ResourceTree(reader);

            Report("Done!");
        }

        private void Load_Header(ProjectReader reader)
        {
            GameID = reader.ReadInt32();
            DirectPlayGuid = reader.ReadGuid();
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
                    var e = o.Events[j];
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

        private void Load_ResourceTree(ProjectReader reader)
        {
            int rootnodes = 12; // 12 seems to be the value for GM8.0
            ResourceTree = new List<ResourceTreeItem>(rootnodes);
            while (rootnodes-- > 0)
            {
                ResourceTree.Add(new ResourceTreeItem(reader));
            }
        }

        private void Load_RoomOrder(ProjectReader reader)
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

        private void Load_LibCreationCode(ProjectReader reader)
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
                LibraryCreationCode.Add(reader.ReadString());
            }
        }

        private void Load_LastIDs(ProjectReader reader)
        {
            LastInstanceID = reader.ReadInt32();
            LastTileID = reader.ReadInt32();
        }

        private void Load_GameInformation(ProjectReader reader)
        {
            int Version = reader.ReadInt32();
            if (Version != 800)
            {
                throw new InvalidDataException("This library only supports .gmk GM8.0 files.");
            }

            var dec_reader = reader.MakeReaderZlib();
            GameInformation = new GMGameInformation(dec_reader);
            GameInformation.Version = Version;
        }

        private void Load_Options(ProjectReader reader)
        {
            Options = new GMOptions(reader);
        }

        private void Load_Constants(ProjectReader reader)
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

            ConstantsLastChanged = reader.ReadDate();
        }

        private void Load_ExtensionPackages(ProjectReader reader)
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
                ExtensionPackageNames.Add(reader.ReadString());
            }
        }

        private void Load_IncludedFiles(ProjectReader reader)
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
                var dec_reader = reader.MakeReaderZlib();
                if (dec_reader.ReadBoolean())
                {
                    IncludedFiles.Add(new GMIncludedFile(dec_reader));
                }
                else IncludedFiles.Add(null);
            }
        }

        private void Load_Rooms(ProjectReader reader)
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
                var dec_reader = reader.MakeReaderZlib();
                if (dec_reader.ReadBoolean())
                {
                    Rooms.Add(new GMRoom(dec_reader, this));
                }
                else Rooms.Add(null);
            }
        }

        private void Load_Objects(ProjectReader reader)
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
                var dec_reader = reader.MakeReaderZlib();
                if (dec_reader.ReadBoolean())
                {
                    Objects.Add(new GMObject(dec_reader, this));
                }
                else Objects.Add(null);
            }
        }

        private void Load_Timelines(ProjectReader reader)
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
                var dec_reader = reader.MakeReaderZlib();
                if (dec_reader.ReadBoolean())
                {
                    Timelines.Add(new GMTimeline(dec_reader));
                }
                else Timelines.Add(null);
            }
        }

        private void Load_Fonts(ProjectReader reader)
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
                var dec_reader = reader.MakeReaderZlib();
                if (dec_reader.ReadBoolean())
                {
                    Fonts.Add(new GMFont(dec_reader));
                }
                else Fonts.Add(null);
            }
        }

        private void Load_Scripts(ProjectReader reader)
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
                var dec_reader = reader.MakeReaderZlib();
                if (dec_reader.ReadBoolean())
                {
                    Scripts.Add(new GMScript(dec_reader));
                }
                else Scripts.Add(null);
            }
        }

        private void Load_Paths(ProjectReader reader)
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
                var dec_reader = reader.MakeReaderZlib();
                if (dec_reader.ReadBoolean())
                {
                    Paths.Add(new GMPath(dec_reader));
                }
                else Paths.Add(null);
            }
        }

        private void Load_Sounds(ProjectReader reader)
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
                var dec_reader = reader.MakeReaderZlib();
                if (dec_reader.ReadBoolean())
                {
                    Sounds.Add(new GMSound(dec_reader));
                }
                else Sounds.Add(null);
            }
        }

        private void Load_Backgrounds(ProjectReader reader)
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
                var dec_reader = reader.MakeReaderZlib();
                if (dec_reader.ReadBoolean())
                {
                    Backgrounds.Add(new GMBackground(dec_reader));
                }
                else Backgrounds.Add(null);
            }
        }

        private void Load_Sprites(ProjectReader reader)
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
                var dec_reader = reader.MakeReaderZlib();
                if (dec_reader.ReadBoolean())
                {
                    Sprites.Add(new GMSprite(dec_reader));
                }
                else Sprites.Add(null);
            }
        }

        private void Load_Triggers(ProjectReader reader)
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
                var dec_reader = reader.MakeReaderZlib();
                if (dec_reader.ReadBoolean())
                {
                    Triggers.Add(new GMTrigger(dec_reader));
                }
                else Triggers.Add(null);
            }

            TriggersLastChanged = reader.ReadDate();
        }
    }
}
