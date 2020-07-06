using RussellLib.Base;
using System;
using System.Drawing;
using System.IO;
using System.Security.Policy;

namespace RussellLib.Assets
{
    public class GMOptions : StreamBase
    {
        public bool StartInFullscreen;
        public bool InterpolatePixels;
        public bool DontDrawBorder;
        public bool DisplayCursor;
        public int Scaling;
        public bool AllowWindowResize;
        public bool AlwaysOnTop;
        public Color OutsideRoom;
        public bool SetResolution;
        public ColourDepth ColorDepth;
        public Resolution ScreenResolution;
        public ScreenFrequency Frequency;
        public bool Borderless;
        public bool VSync;
        public bool DisableScreensavers;
        public bool LetF4Fullscreen;
        public bool LetF1GameInfo;
        public bool LetESCEndGame;
        public bool LetF5F6SaveLoad;
        public bool LetF9Screenshot;
        public bool TreatCloseAsESC;
        public GamePriority Priority;
        public bool FreezeWhenFocusLost;
        public ProgBars LoadingBarMode;
        public Image BackLoadingBar;
        public Image FrontLoadingBar;
        public bool ShowCustomLoadImage;
        public Image LoadingImage;
        public bool LoadimgImagePartTransparent;
        public int LoadImageAlpha;
        public bool ScaleProgressBar;
        public Icon GameIcon;
        public bool DisplayErrors;
        public bool WriteToLog;
        public bool AbortOnAllErrors;
        public bool TreatUninitAsZero;
        public string Author;
        public string Version; // technically it's an integer, but in IDE you can type anything.
        public DateTime LastChanged;
        public string Information;
        public Version GameVersion;
        public string Company;
        public string Product;
        public string Copyright;
        public string Description;
        public DateTime OptionsLastChanged; // different from LastChanged, LastChanged is the date when project was changed, not the settings.

        public enum ProgBars
        {
            BAR_NONE,
            BAR_DEFAULT,
            BAR_CUSTOM
        }

        public enum GamePriority
        {
            PRI_NORMAL,
            PRI_HIGH,
            PRI_HIGHEST
        }

        public enum ScreenFrequency
        {
            FREQ_NOCHANGE,
            FREQ_60,
            FREQ_70,
            FREQ_85,
            FREQ_100,
            FREQ_120
        }

        public enum ColourDepth
        {
            DEPTH_NOCHANGE,
            DEPTH_16BIT,
            DEPTH_32BIT
        }

        public enum Resolution
        {
            RES_NOCHANGE,
            RES_320X240,
            RES_640X480,
            RES_800X600,
            RES_1024X768,
            RES_1280X1024,
            RES_1600X1200
        }

        public GMOptions(BinaryReader reader)
        {
            int version = reader.ReadInt32();
            if (version != 800)
            {
                throw new InvalidDataException("This library only supports .gmk GM8.0 files.");
            }

            var dec_reader = MakeReaderZlib(reader);
            StartInFullscreen = ReadBool(dec_reader);
            InterpolatePixels = ReadBool(dec_reader);
            DontDrawBorder = ReadBool(dec_reader);
            DisplayCursor = ReadBool(dec_reader);
            Scaling = dec_reader.ReadInt32();
            AllowWindowResize = ReadBool(dec_reader);
            AlwaysOnTop = ReadBool(dec_reader);
            OutsideRoom = ReadColor(dec_reader);
            SetResolution = ReadBool(dec_reader);
            ColorDepth = (ColourDepth)dec_reader.ReadInt32();
            ScreenResolution = (Resolution)dec_reader.ReadInt32();
            Frequency = (ScreenFrequency)dec_reader.ReadInt32();
            Borderless = ReadBool(dec_reader);
            VSync = ReadBool(dec_reader);
            DisableScreensavers = ReadBool(dec_reader);
            LetF4Fullscreen = ReadBool(dec_reader);
            LetF1GameInfo = ReadBool(dec_reader);
            LetESCEndGame = ReadBool(dec_reader);
            LetF5F6SaveLoad = ReadBool(dec_reader);
            LetF9Screenshot = ReadBool(dec_reader);
            TreatCloseAsESC = ReadBool(dec_reader);
            Priority = (GamePriority)dec_reader.ReadInt32();
            FreezeWhenFocusLost = ReadBool(dec_reader);
            LoadingBarMode = (ProgBars)dec_reader.ReadInt32();
            BackLoadingBar = null;
            FrontLoadingBar = null;
            if (LoadingBarMode == ProgBars.BAR_CUSTOM)
            {
                if (ReadBool(dec_reader))
                {
                    BackLoadingBar = ReadZlibImage(dec_reader);
                }
                if (ReadBool(dec_reader))
                {
                    FrontLoadingBar = ReadZlibImage(dec_reader);
                }
            }
            ShowCustomLoadImage = ReadBool(dec_reader);
            LoadingImage = null;
            if (ShowCustomLoadImage)
            {
                if (ReadBool(dec_reader))
                {
                    LoadingImage = ReadZlibImage(dec_reader);
                }
            }
            LoadimgImagePartTransparent = ReadBool(dec_reader);
            LoadImageAlpha = dec_reader.ReadInt32();
            ScaleProgressBar = ReadBool(dec_reader);
            GameIcon = ReadIcon(dec_reader);
            DisplayErrors = ReadBool(dec_reader);
            WriteToLog = ReadBool(dec_reader);
            AbortOnAllErrors = ReadBool(dec_reader);
            TreatUninitAsZero = ReadBool(dec_reader);
            Author = ReadString(dec_reader);
            Version = ReadString(dec_reader);
            LastChanged = ReadDate(dec_reader);
            Information = ReadString(dec_reader);
            GameVersion = ReadVersion(dec_reader);
            Company = ReadString(dec_reader);
            Product = ReadString(dec_reader);
            Copyright = ReadString(dec_reader);
            Description = ReadString(dec_reader);
            OptionsLastChanged = ReadDate(dec_reader);
        }
    }
}
