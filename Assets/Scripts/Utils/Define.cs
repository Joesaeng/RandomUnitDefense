using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum Map
    {
        Basic,
    }

    public enum WorldObject
    {
        Unknown,
        PlayerUnit,
        Monster
    }

    public enum AttackType
    {
        Common,
        SlowMagic,
        Stun,
        Poison,
    }

    public enum Layer
    {
        Unit = 3,
        Monster = 6,
        Ground = 7,
        Block = 8,
    }
    public enum Scene
    {
        Unknown,
        Login,
        Lobby,
        Game,
    }
    public enum Sound
    {
        Bgm,
        Effect,
        MaxCount,
    }
    public enum UIEvent
    {
        Click,
        Drag,
    }
    public enum MouseEvent
    {
        Press,
        PointerDown,
        PointerUp,
        Click,
    }
    public enum CameraMode
    {
        QuaterView,
    }
}
