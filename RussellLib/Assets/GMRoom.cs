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

        public GMRoom(ProjectReader reader, GMProject proj)
        {
            Name = reader.ReadString();
            LastChanged = reader.ReadDate();
            int version = reader.ReadInt32();
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
            DrawBackgroundColor = reader.ReadBoolean();
            CreationCode = reader.ReadString();

            // Read room backgrounds.
            int bgcount = reader.ReadInt32();
            Backgrounds = new List<RoomBackground>(bgcount);
            for (int i = 0; i < bgcount; i++)
            {
                var bgstruct = new RoomBackground();
                bgstruct.Visible = reader.ReadBoolean();
                bgstruct.IsForeground = reader.ReadBoolean();
                bgstruct.Background = null;
                int bgid = reader.ReadInt32();
                if (bgid > -1) bgstruct.Background = proj.Backgrounds[bgid];
                int _bgx = reader.ReadInt32();
                int _bgy = reader.ReadInt32();
                bgstruct.Position = new Point(_bgx, _bgy);
                bgstruct.TileHorizontal = reader.ReadBoolean();
                bgstruct.TileVertical = reader.ReadBoolean();
                bgstruct.SpeedHorizontal = reader.ReadInt32();
                bgstruct.SpeedVertical = reader.ReadInt32();
                bgstruct.Stretch = reader.ReadBoolean();

                Backgrounds.Add(bgstruct);
            }

            // Read views.
            EnableViews = reader.ReadBoolean();
            int viewcount = reader.ReadInt32();
            Views = new List<RoomView>(viewcount);
            for (int i = 0; i < viewcount; i++)
            {
                var viewstruct = new RoomView();
                int _x, _y, _w, _h;
                viewstruct.Visible = reader.ReadBoolean();
                _x = reader.ReadInt32();
                _y = reader.ReadInt32();
                _w = reader.ReadInt32();
                _h = reader.ReadInt32();
                viewstruct.ViewCoords = new Rectangle(_x, _y, _w, _h);
                _x = reader.ReadInt32();
                _y = reader.ReadInt32();
                _w = reader.ReadInt32();
                _h = reader.ReadInt32();
                viewstruct.PortCoords = new Rectangle(_x, _y, _w, _h);
                viewstruct.BorderHor = reader.ReadInt32();
                viewstruct.BorderVert = reader.ReadInt32();
                viewstruct.HSpeed = reader.ReadInt32();
                viewstruct.VSpeed = reader.ReadInt32();
                int _objind = reader.ReadInt32();
                if (_objind > -1) viewstruct.ViewObject = proj.Objects[_objind];
            }

            // Read room instances.
            int instcount = reader.ReadInt32();
            Instances = new List<RoomInstance>(instcount);
            for (int i = 0; i < instcount; i++)
            {
                var inststruct = new RoomInstance();
                int _x, _y;
                _x = reader.ReadInt32();
                _y = reader.ReadInt32();
                inststruct.Position = new Point(_x, _y);
                int _objind = reader.ReadInt32();
                if (_objind > -1) inststruct.Object = proj.Objects[_objind];
                inststruct.ID = reader.ReadInt32();
                inststruct.CreationCode = reader.ReadString();
                inststruct.IsLocked = reader.ReadBoolean();
            }

            // Read room tiles.
            int tilecount = reader.ReadInt32();
            Tiles = new List<RoomTile>(tilecount);
            for (int i = 0; i < tilecount; i++)
            {
                var tilestruct = new RoomTile();
                int _x, _y, _w, _h;
                _x = reader.ReadInt32();
                _y = reader.ReadInt32();
                tilestruct.RoomPosition = new Point(_x, _y);
                _x = reader.ReadInt32();
                _y = reader.ReadInt32();
                _w = reader.ReadInt32();
                _h = reader.ReadInt32();
                tilestruct.BGCoords = new Rectangle(_x, _y, _w, _h);
                tilestruct.Depth = reader.ReadInt32();
                tilestruct.ID = reader.ReadInt32();
                tilestruct.IsLocked = reader.ReadBoolean();
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
