using System;

public enum GameModeType
{
    GAMEINIT,
    MAINMENU,
    LOADING,
    RELOADING,
    DEFAULT,
    PAUSE,
    EXIT
}

public enum ResolutionType
{
    Res1280x720,
    Res1600x900,
    Res1920x1080,
    Res2560x1440,
    Res3840x2160,
}

public enum MouseMode
{
    ORIGIN,
    DEFAULT,
    HIDE,
}

public enum MusicClip
{
    BGM1,
}

public enum SoundClip
{
    BTN_CLICK,
}

[Flags]
public enum UIAnimProperty
{
    NONE = 0,
    POSITION = 1 << 0,
    SCALE = 1 << 1,
    ROTATION = 1 << 2,
    COLOR = 1 << 3,
}

[Flags]
public enum UIAnimState
{
    IDLE = 0,
    FADEIN  = 1 << 0,
    FADEOUT = 1 << 1,
    HOVERIN = 1 << 2,
    HOVEROUT = 1 << 3,
    SHAKE = 1 << 4,
}