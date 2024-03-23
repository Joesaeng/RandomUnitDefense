using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Define;

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
    
    Dictionary<UnitNames,Dictionary<int,UnitStatus>> _unitStatusDict; // <Id<Lv,Info>>
    Dictionary<UnitNames,int> _unitUpgradLv;
   
    Dictionary<UnitNames,Dictionary<int,int>> _unitUpgradeDamage; // Id<Lv,룬효과가 적용된 데미지>

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
        int basicAttackDamage = _unitUpgradeDamage[status.unit][lv];
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
        if(_unitUpgradeDamage != null)
            _unitUpgradeDamage.Clear();

        _unitStatusDict = new Dictionary<UnitNames, Dictionary<int, UnitStatus>>();
        _unitUpgradLv = new Dictionary<UnitNames, int>();
        _unitUpgradeDamage = new Dictionary<UnitNames, Dictionary<int, int>>();

        foreach (BaseUnit baseUnit in Managers.Data.BaseUnitDict.Values)
        {
            UnitNames type = baseUnit.baseUnit;
            UnitType attackType = baseUnit.type;
            Dictionary<int,UnitStatus> dict = new Dictionary<int, UnitStatus>();
            Dictionary<int,int> levelPerUpgradeDamageDict = new Dictionary<int,int>();

            for (int lv = 1; lv <= ConstantData.PlayerUnitHighestLevel; lv++)
            {
                int upgradeDamage;
                dict.Add(lv, MakeUnitStatus(type, attackType, lv, out upgradeDamage));
                levelPerUpgradeDamageDict.Add(lv, upgradeDamage);
            }
            _unitStatusDict.Add(type, dict);
            _unitUpgradLv.Add(type, 1);
            _unitUpgradeDamage.Add(type, levelPerUpgradeDamageDict);
        }
    }

    // 유닛의 스테이터스를 만듬
    private UnitStatus MakeUnitStatus(UnitNames baseUnit, UnitType unitType, int unitLv, out int upgradeDamage)
    {
        UnitStat_Base baseStat = Managers.Data.GetUnitData(baseUnit, unitLv);
        UnitStatus status = new UnitStatus();
        
        status.unit = baseUnit;
        status.unitType = unitType;

        float increaseDamageOfRunes = 0f;           // 장착된 룬의 데미지 증가량
        float increaseAttackSpeedOfRunes = 0f;      // 장착된 룬의 공격속도 증가량

        switch (unitType)
        {
            // Common
            case UnitType.Common:
            {
                status.attackDamage = baseStat.attackDamage;
                status.attackRate = baseStat.attackRate;
                status.attackRange = baseStat.attackRange;

                float runeValue = 0f;
                if(_runeStatus.AdditionalEffects.TryGetValue(AdditionalEffectName.IncreaseDamageOfCommon, out runeValue))
                    increaseDamageOfRunes += runeValue;
                if(_runeStatus.AdditionalEffects.TryGetValue(AdditionalEffectName.InCreaseAttackSpeedOfCommon, out runeValue))
                    increaseAttackSpeedOfRunes += runeValue;
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

                    float runeValue = 0f;
                    if (_runeStatus.AdditionalEffects.TryGetValue(AdditionalEffectName.IncreaseDamageOfAOE, out runeValue))
                        increaseDamageOfRunes += runeValue;
                    if (_runeStatus.AdditionalEffects.TryGetValue(AdditionalEffectName.InCreaseAttackSpeedOfAOE, out runeValue))
                        increaseAttackSpeedOfRunes += runeValue;
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
                            float poisonBowManRuneValue = 0f;
                            _runeStatus.BaseRuneEffects.TryGetValue(BaseRune.PoisonBowMan, out poisonBowManRuneValue);
                            poisonBowManRuneValue += 1f;
                            status.attackDamage = poison.attackDamage;
                            status.attackRate = poison.attackRate;
                            status.attackRange = poison.attackRange;
                            status.debuffDuration = poison.poisonDuration;
                            status.damagePerSecond = poison.poisonDamagePerSecond * poisonBowManRuneValue;

                            float runeValue = 0f;
                            if (_runeStatus.AdditionalEffects.TryGetValue(AdditionalEffectName.InCreaseAttackSpeedOfCommon, out runeValue))
                                increaseAttackSpeedOfRunes += runeValue;
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

        float baseRuneValue = 0f;
        // status에 장착된 룬의 효과를 부여하기
        bool isUnitRune = false;
        for(int i = 0; i < (int)BaseRune.Count; ++i)
        {
            if($"{baseUnit}" == $"{(BaseRune)i}")
            {
                isUnitRune = true;
                break;
            }
        }
        if (isUnitRune == true && 
            _runeStatus.BaseRuneEffects.TryGetValue(Util.Parse<BaseRune>($"{baseUnit}"), out baseRuneValue))
        {
            if(baseUnit != UnitNames.PoisonBowMan)
            {
                increaseDamageOfRunes += baseRuneValue;
            }
        }
        if (_runeStatus.BaseRuneEffects.TryGetValue(BaseRune.Fighter, out baseRuneValue))
            increaseAttackSpeedOfRunes += baseRuneValue;

        // 옵션들이 소수이고, 곱연산 해줘야 하기 때문에 1을 더해줌
        increaseDamageOfRunes += 1;
        increaseAttackSpeedOfRunes += 1;

        float attackDamage = status.attackDamage * increaseDamageOfRunes;

        status.attackDamage = (int)attackDamage;
        status.attackRate /= increaseAttackSpeedOfRunes;

        upgradeDamage = status.attackDamage;

        return status;
    }


}
