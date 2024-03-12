using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum EquipItemStatus
    {
        increaseDamage,
        decreaseAttackRate,
        increaseAttackRange,
        increaseAOEArea,
        addedDamage,
        Count,
    }

    public enum ItemName
    {
        None,
        increaseDamageItem,
        attackRateItem,
        attackRangeItem,
        AOEAreaItem,
        addedDamageItem
    }

    public enum UnitAnimationState
    {
        Idle,
        Run,
        Attack,
        Death,
        Stun,
    }

    public enum GameLanguage
    {
        English,
        Korean,
    }

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
        Combat,
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
