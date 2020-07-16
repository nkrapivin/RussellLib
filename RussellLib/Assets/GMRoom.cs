using RussellLib.Base;
using RussellLib.Misc;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace RussellLib.Assets
{
    public class GMRoom
    {
        public int Version;
        public string Name;
        public DateTime LastChanged;
        public string Caption;
        public uint Width; // same as Speed
        public uint Height;
        public Point Snap;
        public bool Isometric;
        public uint Speed; // You can't set negative speed.
        public bool Persistent; // a friendly reminder, persistent rooms are evil
        public Color BackgroundColor;
        public bool DrawBackgroundColor;
        public string CreationCode;
        public List<RoomBackground> Backgrounds;
        public bool EnableViews;
        public bool ClearBGWithWindowColor; // GM >=8.1 only.
        public List<RoomView> Views;
        public List<RoomInstance> Instances;
        public List<RoomTile> Tiles;

        // IDE only settings, isn't really interesting.
        public bool REI;
        public int EditorWidth;
        public int EditorHeight;
        public bool ShowGrid;
        public bool ShowObjects;
        public bool ShowTiles;
        public bool ShowBGs;
        public bool ShowFGs;
        public bool ShowViews;
        public bool DeleteUnderlyingObj;
        public bool DeleteUnderlyingTil;
        public EditorTab Tab;
        public Point Scrollbar;

        public enum EditorTab
        {
            Objects,
            Settings,
            Tiles,
            Backgrounds,
            Views
        }

        public void Save(ProjectWriter writer, GMProject proj)
        {
            writer.Write(Name);
            writer.Write(LastChanged);
            writer.Write(Version);
            writer.Write(Caption);
            writer.Write(Width);
            writer.Write(Height);
            writer.Write(Snap);
            writer.Write(Isometric);
            writer.Write(Speed);
            writer.Write(Persistent);
            writer.Write(BackgroundColor);
            int val = DrawBackgroundColor ? 1 : 0;
            if (ClearBGWithWindowColor) val |= 0b10;
            writer.Write(val);
            writer.Write(CreationCode);
            writer.Write(Backgrounds.Count);
            for (int i = 0; i < Backgrounds.Count; i++)
            {
                Backgrounds[i].Save(writer, proj);
            }
            writer.Write(EnableViews);
            writer.Write(Views.Count);
            for (int i = 0; i < Views.Count; i++)
            {
                Views[i].Save(writer, proj);
            }
            writer.Write(Instances.Count);
            for (int i = 0; i < Instances.Count; i++)
            {
                Instances[i].Save(writer, proj);
            }
            writer.Write(Tiles.Count);
            for (int i = 0; i < Tiles.Count; i++)
            {
                Tiles[i].Save(writer, proj);
            }

            // weird Room Editor settings...
            writer.Write(REI);
            writer.Write(EditorWidth);
            writer.Write(EditorHeight);
            writer.Write(ShowGrid);
            writer.Write(ShowObjects);
            writer.Write(ShowTiles);
            writer.Write(ShowBGs);
            writer.Write(ShowFGs);
            writer.Write(ShowViews);
            writer.Write(DeleteUnderlyingObj);
            writer.Write(DeleteUnderlyingTil);
            writer.Write((int)Tab);
            writer.Write(Scrollbar);
        }

        public GMRoom(ProjectReader reader, GMProject proj)
        {
            Name = reader.ReadString();
            LastChanged = reader.ReadDate();
            Version = reader.ReadInt32();
            Caption = reader.ReadString();
            Width = reader.ReadUInt32();
            Height = reader.ReadUInt32();
            int _snx = reader.ReadInt32();
            int _sny = reader.ReadInt32();
            Snap = new Point(_snx, _sny);
            Isometric = reader.ReadBoolean();
            Speed = reader.ReadUInt32();
            Persistent = reader.ReadBoolean();
            BackgroundColor = reader.ReadColor();
            int val = reader.ReadInt32();
            DrawBackgroundColor = (val & 1) != 0;
            ClearBGWithWindowColor = (val & 0b10) == 0;
            CreationCode = reader.ReadString();

            // Read room backgrounds.
            int bgcount = reader.ReadInt32();
            Backgrounds = new List<RoomBackground>(bgcount);
            for (int i = 0; i < bgcount; i++)
            {
                var bgstruct = new RoomBackground();
                bgstruct.Load(reader, proj);
                Backgrounds.Add(bgstruct);
            }

            // Read views.
            EnableViews = reader.ReadBoolean();
            int viewcount = reader.ReadInt32();
            Views = new List<RoomView>(viewcount);
            for (int i = 0; i < viewcount; i++)
            {
                var viewstruct = new RoomView();
                viewstruct.Load(reader, proj);
                Views.Add(viewstruct);
            }

            // Read room instances.
            int instcount = reader.ReadInt32();
            Instances = new List<RoomInstance>(instcount);
            for (int i = 0; i < instcount; i++)
            {
                var inststruct = new RoomInstance();
                inststruct.Load(reader, proj);
                Instances.Add(inststruct);
            }

            // Read room tiles.
            int tilecount = reader.ReadInt32();
            Tiles = new List<RoomTile>(tilecount);
            for (int i = 0; i < tilecount; i++)
            {
                var tilestruct = new RoomTile();
                tilestruct.Load(reader, proj);
                Tiles.Add(tilestruct);
            }

            // weird editor settings (aren't really important unless you make an IDE)
            REI = reader.ReadBoolean();
            EditorWidth = reader.ReadInt32();
            EditorHeight = reader.ReadInt32();
            ShowGrid = reader.ReadBoolean();
            ShowObjects = reader.ReadBoolean();
            ShowTiles = reader.ReadBoolean();
            ShowBGs = reader.ReadBoolean();
            ShowFGs = reader.ReadBoolean();
            ShowViews = reader.ReadBoolean();
            DeleteUnderlyingObj = reader.ReadBoolean();
            DeleteUnderlyingTil = reader.ReadBoolean();
            Tab = (EditorTab)reader.ReadInt32();
            int _hx = reader.ReadInt32();
            int _hy = reader.ReadInt32();
            Scrollbar = new Point(_hx, _hy);

            reader.Dispose();
        }
    }
}
