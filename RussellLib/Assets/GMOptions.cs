using RussellLib.Base;
using System;
using System.Drawing;
using System.IO;

namespace RussellLib.Assets
{
    public class GMOptions
    {
        public int FormatVersion;
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
        public bool SoftwareVertex;
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
        public bool ErrorOnArguments;
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

        // not actually in the gmk??
        public bool SwapEventOrder; // GMK 820 only
        public int WebGL; // GMK 820 HTML5 only

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

        public void Write(ProjectWriter writer)
        {
            writer.Write(StartInFullscreen);
            writer.Write(InterpolatePixels);
            writer.Write(DontDrawBorder);
            writer.Write(DisplayCursor);
            writer.Write(Scaling);
            writer.Write(AllowWindowResize);
            writer.Write(AlwaysOnTop);
            writer.Write(OutsideRoom);
            writer.Write(SetResolution);
            writer.Write((int)ColorDepth);
            writer.Write((int)ScreenResolution);
            writer.Write((int)Frequency);
            writer.Write(Borderless);
            uint vsync = VSync ? 1U : 0U;
            if (SoftwareVertex) vsync |= 0x80000000;
            writer.Write(vsync);
            writer.Write(DisableScreensavers);
            writer.Write(LetF4Fullscreen);
            writer.Write(LetF1GameInfo);
            writer.Write(LetESCEndGame);
            writer.Write(LetF5F6SaveLoad);
            writer.Write(LetF9Screenshot);
            writer.Write(TreatCloseAsESC);
            writer.Write((int)Priority);
            writer.Write(FreezeWhenFocusLost);
            writer.Write((int)LoadingBarMode);
            if (LoadingBarMode == ProgBars.BAR_CUSTOM)
            {
                if (BackLoadingBar != null)
                {
                    writer.Write(1);
                    writer.Write(BackLoadingBar);
                }
                else writer.Write(0);

                if (FrontLoadingBar != null)
                {
                    writer.Write(1);
                    writer.Write(FrontLoadingBar);
                }
                else writer.Write(0);
            }
            writer.Write(ShowCustomLoadImage);
            if (ShowCustomLoadImage)
            {
                if (LoadingImage != null)
                {
                    writer.Write(1);
                    writer.Write(LoadingImage);
                }
                else writer.Write(0);
            }
            writer.Write(LoadimgImagePartTransparent);
            writer.Write(LoadImageAlpha);
            writer.Write(ScaleProgressBar);
            writer.Write(GameIcon);
            writer.Write(DisplayErrors);
            writer.Write(WriteToLog);
            writer.Write(AbortOnAllErrors);
            writer.Write(TreatUninitAsZero);

            writer.Write(Author);
            writer.Write(Version);
            writer.Write(LastChanged);
            writer.Write(Information);
            writer.Write(GameVersion);
            writer.Write(Company);
            writer.Write(Product);
            writer.Write(Copyright);
            writer.Write(Description);
            writer.Write(OptionsLastChanged);
        }

        public GMOptions(ProjectReader reader)
        {
            int version = reader.ReadInt32();
            int val;
            if (version < 800)
            {
                throw new InvalidDataException("This library only supports .gmk GM8.0 files.");
            }

            FormatVersion = version;
            var dec_reader = reader.MakeReaderZlib();
            StartInFullscreen = dec_reader.ReadBoolean();
            InterpolatePixels = dec_reader.ReadBoolean();
            DontDrawBorder = dec_reader.ReadBoolean();
            DisplayCursor = dec_reader.ReadBoolean();
            Scaling = dec_reader.ReadInt32();
            AllowWindowResize = dec_reader.ReadBoolean();
            AlwaysOnTop = dec_reader.ReadBoolean();
            OutsideRoom = dec_reader.ReadColor();
            SetResolution = dec_reader.ReadBoolean();
            ColorDepth = (ColourDepth)dec_reader.ReadInt32();
            ScreenResolution = (Resolution)dec_reader.ReadInt32();
            Frequency = (ScreenFrequency)dec_reader.ReadInt32();
            Borderless = dec_reader.ReadBoolean();
            val = dec_reader.ReadInt32();
            VSync = (val & 0x01) != 0;
            SoftwareVertex = (val & 0x80000000) != 0;
            DisableScreensavers = dec_reader.ReadBoolean();

            LetF4Fullscreen = dec_reader.ReadBoolean();
            LetF1GameInfo = dec_reader.ReadBoolean();
            LetESCEndGame = dec_reader.ReadBoolean();
            LetF5F6SaveLoad = dec_reader.ReadBoolean();
            LetF9Screenshot = dec_reader.ReadBoolean();
            TreatCloseAsESC = dec_reader.ReadBoolean();

            Priority = (GamePriority)dec_reader.ReadInt32();
            FreezeWhenFocusLost = dec_reader.ReadBoolean();
            LoadingBarMode = (ProgBars)dec_reader.ReadInt32();
            BackLoadingBar = null;
            FrontLoadingBar = null;
            if (LoadingBarMode == ProgBars.BAR_CUSTOM)
            {
                if (dec_reader.ReadBoolean())
                {
                    BackLoadingBar = dec_reader.ReadZlibImage();
                }
                if (dec_reader.ReadBoolean())
                {
                    FrontLoadingBar = dec_reader.ReadZlibImage();
                }
            }
            ShowCustomLoadImage = dec_reader.ReadBoolean();
            LoadingImage = null;
            if (ShowCustomLoadImage)
            {
                if (dec_reader.ReadBoolean())
                {
                    LoadingImage = dec_reader.ReadZlibImage();
                }
            }
            LoadimgImagePartTransparent = dec_reader.ReadBoolean();
            LoadImageAlpha = dec_reader.ReadInt32();
            ScaleProgressBar = dec_reader.ReadBoolean();
            GameIcon = dec_reader.ReadIcon();
            DisplayErrors = dec_reader.ReadBoolean();
            WriteToLog = dec_reader.ReadBoolean();
            AbortOnAllErrors = dec_reader.ReadBoolean();
            val = dec_reader.ReadInt32();
            TreatUninitAsZero = (val & 0x01) != 0;
            ErrorOnArguments = (val & 0x02) != 0;

            /*
            if (version >= 820)
            {
                WebGL = dec_reader.ReadInt32();
                SwapEventOrder = dec_reader.ReadBoolean();
            }
            */

            Author = dec_reader.ReadString();
            Version = dec_reader.ReadString();
            LastChanged = dec_reader.ReadDate();
            Information = dec_reader.ReadString();
            GameVersion = dec_reader.ReadVersion();
            Company = dec_reader.ReadString();
            Product = dec_reader.ReadString();
            Copyright = dec_reader.ReadString();
            Description = dec_reader.ReadString();
            OptionsLastChanged = dec_reader.ReadDate();
        }
    }
}
