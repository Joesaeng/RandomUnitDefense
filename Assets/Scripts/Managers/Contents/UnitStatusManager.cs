using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStatus
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
}

public class UnitStatusManager
{
    // ��� : 
    // Id,Lv �� ���ڷ� �޾Ƽ� ������ �����
    // ���׷��̵� �� �� ������ ���� ������Ʈ (unitStateMachine�� ���ݼӵ�, ���ݹ��� ������Ʈ)
    // <Id<Lv,Info>> ��ųʸ� 
    Dictionary<UnitNames,Dictionary<int,UnitStatus>> _unitStatusDict;
    Dictionary<UnitNames,int> _unitUpgradLv;
    public Dictionary<UnitNames,int> UnitUpgradLv => _unitUpgradLv;

    public Action<int> OnUnitUpgrade;

    public UnitStatus GetUnitStatus(UnitNames baseUnit, int unitLv)
    {
        return _unitStatusDict[baseUnit][unitLv];
    }

    public void Init()
    {
        SetUnitStatusDict();
    }

    public void ClickedUnitUpgrade(UnitNames baseunit,int slot)
    {
        for(int lv = 1; lv <= ConstantData.PlayerUnitHighestLevel; lv++)
        {
            Upgrade(_unitStatusDict[baseunit][lv],lv);
        }
        _unitUpgradLv[baseunit]++;
        if (OnUnitUpgrade != null)
            OnUnitUpgrade.Invoke(slot);
    }
    private void Upgrade(UnitStatus status,int lv)
    {
        int basicAttackDamage = Managers.Data.GetUnitData(status.unit,lv).attackDamage;
        float basicDamagePerSecond = 0f;
        // damagePerSecond�� �ʿ��� ���
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

    private void SetUnitStatusDict()
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

    private UnitStatus MakeUnitStatus(UnitNames baseUnit, UnitType unitType, int unitLv)
    {
        UnitStat_Base baseStat = Managers.Data.GetUnitData(baseUnit, unitLv);
        UnitStatus status = new UnitStatus();
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

        return status;
    }
}