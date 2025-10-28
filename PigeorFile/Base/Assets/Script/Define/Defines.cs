using System;

public enum GameModeType
{
    GAME_INIT,
    MAINMENU,
    LOADING,
    RELOADING,
    MAINMENU_LOADING,
    DEFAULT,
    PAUSE,
    EXIT
}

public enum ResolutionType
{
    RES1280_720,
    RES1600_900,
    RES1920_1080,
    RES2560_1440,
    RES3840_2160,
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