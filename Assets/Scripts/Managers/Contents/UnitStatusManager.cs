using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnitStatus : ICloneable
{
    public UnitNames    unit = UnitNames.Knight;
    public UnitType     unitType = UnitType.Common;
    public int          attackDamage = 0;
    public float        attackRate = 0;
    public float        attackRange = 0;
    public float        wideAttackArea = 0;
    public float        debuffDuration = 0;
    public float        debuffRatio = 0;
    public float        damagePerSecond = 0;

    public UnitStatus MyClone()
    {
        return (UnitStatus)Clone();
    }

    public object Clone()
    {
        return this.MemberwiseClone();
    }
}

public class UnitStatusManager
{
    // 기능 : 
    // Id,Lv 을 인자로 받아서 정보를 뱉어줌
    // 업그레이드 할 때 유닛의 정보 업데이트 (unitStateMachine의 공격속도, 공격범위 업데이트)
    // <Id<Lv,Info>> 딕셔너리 
    Dictionary<UnitNames,Dictionary<int,UnitStatus>> _unitStatusDict;
    Dictionary<UnitNames,int> _unitUpgradLv;

    public Dictionary<UnitNames,int> UnitUpgradLv => _unitUpgradLv;

    public Action<int> OnUnitUpgradeSlot;
    public Action OnUnitUpgrade;

    EquipedRuneStatus _runeStatus;
    public EquipedRuneStatus RuneStatus => _runeStatus;

    public UnitStatus GetUnitStatus(UnitNames baseUnit, int unitLv)
    {
        UnitStatus status = _unitStatusDict[baseUnit][unitLv];

        return ApplyEquipedItemStatus(status);
    }

    public void Init()
    {
        _runeStatus = new EquipedRuneStatus();
        _runeStatus.SetEquipedRune();
        MakeUnitStatusDict();
    }

    public void ClickedUnitUpgrade(UnitNames baseunit,int slot)
    {
        for(int lv = 1; lv <= ConstantData.PlayerUnitHighestLevel; lv++)
        {
            Upgrade(_unitStatusDict[baseunit][lv],lv);
        }
        _unitUpgradLv[baseunit]++;
        Util.CheckTheEventAndCall(OnUnitUpgradeSlot, slot);
        Util.CheckTheEventAndCall(OnUnitUpgrade);
    }

    private void Upgrade(UnitStatus status,int lv)
    {
        int basicAttackDamage = Managers.Data.GetUnitData(status.unit,lv).attackDamage;
        float basicDamagePerSecond = 0f;
        // damagePerSecond가 필요한 경우
        if(status.unit == UnitNames.PoisonBowMan)
        {
            PoisonBowMan poison = (PoisonBowMan)Managers.Data.GetUnitData(status.unit,lv);
            basicDamagePerSecond = poison.poisonDamagePerSecond;
        }

        status.attackDamage     += basicAttackDamage;
        status.damagePerSecond  += basicDamagePerSecond;

        status.attackRate       *= ConstantData.IncreaseAttackRate;
        status.debuffDuration   *= ConstantData.IncreaseDebuffDuration;
        status.debuffRatio      *= ConstantData.IncreaseDebuffRatio;

        status.attackRate       = (float)Math.Round(status.attackRate, 2);
        status.debuffDuration   = (float)Math.Round(status.debuffDuration, 2);
        status.debuffRatio      = (float)Math.Round(status.debuffRatio, 2);
    }

    // 인게임 아이템의 스텟을 유닛 스테이터스에 적용
    private UnitStatus ApplyEquipedItemStatus(UnitStatus status)
    {
        UnitStatus _status = status.MyClone();

        EquipedItemStatus equipedItemStatus = Managers.InGameItem.CurrentStatusOnEquipedItem;
        float attackDamage = _status.attackDamage * equipedItemStatus.increaseDamage;
        _status.attackDamage = (int)attackDamage;
        _status.attackRate /= equipedItemStatus.decreaseAttackRate;
        _status.attackRange += equipedItemStatus.increaseAttackRange;
        _status.wideAttackArea *= equipedItemStatus.increaseAOEArea;

        _status.attackRate = (float)Math.Round(_status.attackRate, 2);
        _status.attackRange = (float)Math.Round(_status.attackRange, 2);
        _status.wideAttackArea = (float)Math.Round(_status.wideAttackArea, 2);

        return _status;
    }

    // Dictionary<유닛<레벨,스텟>> 형식으로 딕셔너리를 만들어 줌
    private void MakeUnitStatusDict()
    {
        if(_unitStatusDict != null)
            _unitStatusDict.Clear();
        if (_unitUpgradLv != null)
            _unitStatusDict.Clear();

        _unitStatusDict = new Dictionary<UnitNames, Dictionary<int, UnitStatus>>();
        _unitUpgradLv = new Dictionary<UnitNames, int>();
        foreach (BaseUnit baseUnit in Managers.Data.BaseUnitDict.Values)
        {
            UnitNames type = baseUnit.baseUnit;
            UnitType attackType = baseUnit.type;
            Dictionary<int,UnitStatus> dict = new Dictionary<int, UnitStatus>();

            for (int lv = 1; lv <= ConstantData.PlayerUnitHighestLevel; lv++)
            {
                dict.Add(lv, MakeUnitStatus(type, attackType, lv));
            }
            _unitStatusDict.Add(type, dict);
            _unitUpgradLv.Add(type, 1);
        }
    }

    // 유닛의 스테이터스를 만듬
    private UnitStatus MakeUnitStatus(UnitNames baseUnit, UnitType unitType, int unitLv)
    {
        UnitStat_Base baseStat = Managers.Data.GetUnitData(baseUnit, unitLv);
        UnitStatus status = new UnitStatus();
        // 장착된 룬 효과 적용


        status.unit = baseUnit;
        status.unitType = unitType;
        switch (unitType)
        {
            // Common
            case UnitType.Common:
            {
                status.attackDamage = baseStat.attackDamage;
                status.attackRate = baseStat.attackRate;
                status.attackRange = baseStat.attackRange;
            }
            break;
            // AOE
            case UnitType.AOE:
            {
                if (baseStat is AOE aoe)
                {
                    status.attackDamage = aoe.attackDamage;
                    status.attackRate = aoe.attackRate;
                    status.attackRange = aoe.attackRange;
                    status.wideAttackArea = aoe.wideAttackArea;
                }
                else
                {
                    Debug.LogError("SetDictValue Error!");
                    break;
                }
            }
            break;
            // Debuffer
            case UnitType.Debuffer:
            {
                switch (baseUnit)
                {
                    case UnitNames.SlowMagician:
                    {
                        if (baseStat is SlowMagician slow)
                        {
                            status.attackDamage = slow.attackDamage;
                            status.attackRate = slow.attackRate;
                            status.attackRange = slow.attackRange;
                            status.wideAttackArea = slow.wideAttackArea;
                            status.debuffDuration = slow.slowDuration;
                            status.debuffRatio = slow.slowRatio;
                        }
                        else
                        {
                            Debug.LogError("SetDictValue Error!");
                            break;
                        }
                    }
                    break;
                    case UnitNames.StunGun:
                    {
                        if (baseStat is StunGun stun)
                        {
                            status.attackDamage = stun.attackDamage;
                            status.attackRate = stun.attackRate;
                            status.attackRange = stun.attackRange;
                            status.debuffDuration = stun.stunDuration;
                        }
                        else
                        {
                            Debug.LogError("SetDictValue Error!");
                            break;
                        }
                    }
                    break;
                    case UnitNames.PoisonBowMan:
                    {
                        if (baseStat is PoisonBowMan poison)
                        {
                            status.attackDamage = poison.attackDamage;
                            status.attackRate = poison.attackRate;
                            status.attackRange = poison.attackRange;
                            status.debuffDuration = poison.poisonDuration;
                            status.damagePerSecond = poison.poisonDamagePerSecond;
                        }
                        else
                        {
                            Debug.LogError("SetDictValue Error!");
                            break;
                        }
                    }
                    break;
                }
            }
            break;
        }

        // status에 장착된 룬의 효과를 부여하기

        return status;
    }


}
