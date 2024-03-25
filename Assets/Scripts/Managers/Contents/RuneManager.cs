using Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EquipedRuneStatus
{
    Rune[] _equipedRunes;
    public Rune[] EquipedRunes => _equipedRunes;

    public Dictionary<BaseRune, float> BaseRuneEffects { get => _baseRuneEffects; }
    public Dictionary<AdditionalEffectName, float> AdditionalEffects { get => _additionalEffects; }

    Dictionary<BaseRune,float> _baseRuneEffects = new Dictionary<BaseRune,float>();
    Dictionary<AdditionalEffectName,float> _additionalEffects = new Dictionary<AdditionalEffectName,float>();
    public void SetEquipedRune()
    {
        _equipedRunes = Managers.Player.Data.EquipedRunes;

        foreach (Rune rune in _equipedRunes)
        {
            if (rune == null)
                continue;

            // 장착된 룬의 기본 효과 적용
            if (_baseRuneEffects.ContainsKey(rune.baseRune) == false)
            {
                _baseRuneEffects.Add(rune.baseRune, 0f);
            }
            _baseRuneEffects[rune.baseRune] += rune.baseRuneEffectValue;

            // 장착된 룬의 추가 효과 적용
            foreach (AdditionalEffectOfRune effect in rune.additionalEffects)
            {
                if (_additionalEffects.ContainsKey(effect.name) == false)
                {
                    _additionalEffects.Add(effect.name, 0f);
                }
                _additionalEffects[effect.name] += effect.value;
            }
        }
    }
}

public class RuneManager
{
    Dictionary<GradeOfRune, Sprite> _runeSprites;
    public Dictionary<GradeOfRune, Sprite> RuneSprites { get => _runeSprites; }

    Dictionary<BaseRune,Sprite> _runeTextImages;
    public Dictionary<BaseRune, Sprite> RuneTextImages { get => _runeTextImages; }

    public void Init()
    {
        Sprite[] runeImages = Resources.LoadAll<Sprite>("Art/UIImages/Runes");
        Sprite[] runeTextImages = Resources.LoadAll<Sprite>("Art/UIImages/RuneTextImage");

        _runeSprites = new Dictionary<GradeOfRune, Sprite>();
        _runeTextImages = new Dictionary<BaseRune, Sprite>();

        // 리소스에 있는 룬 이미지를 딕셔너리에 채움
        foreach (Sprite runeSprite in runeImages)
        {
            _runeSprites.Add(Util.Parse<GradeOfRune>(runeSprite.name), runeSprite);
        }
        foreach (Sprite runeTextSprite in runeTextImages)
        {
            _runeTextImages.Add(Util.Parse<BaseRune>(runeTextSprite.name), runeTextSprite);
        }
    }

    // 랜덤한 룬을 생성하는 함수, 룬 그레이드 설정 가능
    public Rune CreateRandomRune(GradeOfRune setGrade = GradeOfRune.None)
    {
        Rune rune = new Rune();

        int         randomBase  = Random.Range(0,(int)BaseRune.Count);
        BaseRune    runeBase    = (BaseRune)randomBase;

        float       randomGradeSelect   = Random.value;
        GradeOfRune runeGrade;

        // 룬 그레이드 선택
        if (setGrade == GradeOfRune.None)
        {
            if (randomGradeSelect <= ConstantData.PercentOfMythRune)
                runeGrade = GradeOfRune.Myth;
            else if (randomGradeSelect <= ConstantData.PercentOfLegendRune)
                runeGrade = GradeOfRune.Legend;
            else if (randomGradeSelect <= ConstantData.PercentOfUniqueRune)
                runeGrade = GradeOfRune.Unique;
            else if (randomGradeSelect <= ConstantData.PercentOfRareRune)
                runeGrade = GradeOfRune.Rare;
            else
                runeGrade = GradeOfRune.Common;
        }
        else
            runeGrade = setGrade;

        rune.baseRune = runeBase;
        rune.gradeOfRune = runeGrade;

        // Json데이터로 저장해둔 베이스룬 밸류 가져오기
        rune.baseRuneEffectValue = Managers.Data.RunesDict[$"{runeGrade}{runeBase}"].value;
        rune.isEquip = false;

        // 룬 등급에 맞는 추가 효과 생성 및 부여
        int additionalEftCount = ConstantData.AdditionalEftCountOfRunes[(int)runeGrade];

        for (int i = 0; i < additionalEftCount; ++i)
        {
            rune.additionalEffects.Add(CreateRandomAdditionalEffect());
        }

        return rune;
    }
    // 랜덤한 추가 옵션 생성 함수
    public AdditionalEffectOfRune CreateRandomAdditionalEffect()
    {
        AdditionalEffectOfRune effect = new AdditionalEffectOfRune();

        int randomEffect = Random.Range(0,(int)AdditionalEffectName.Count);
        AdditionalEffectName additionalEffect = (AdditionalEffectName)randomEffect;

        effect.name = additionalEffect;
        AdditionalEffectOfRuneValueMinMax minMax = Managers.Data.EffectMinMaxs[$"{effect.name}"];
        effect.value = Random.Range(minMax.min, minMax.max);

        // 추가데미지는 소수점이 필요 없으니 없앰
        if (effect.name == AdditionalEffectName.AddedDamage)
            effect.value = System.MathF.Round(effect.value);
        // 이 외는 소수점 두자리까지만 사용
        else
            effect.value = System.MathF.Round(effect.value, 2);

        return effect;
    }
    // 플레이어가 보유한 룬을 종류/등급 별로 정렬한다
    public void SortOwnedRunes(Define.SortModeOfRunes sortMode)
    {
        Rune[] runes = Managers.Player.Data.ownedRunes.ToArray();
        switch(sortMode)
        {
            case Define.SortModeOfRunes.Grade:
            {
                RuneComparerToGrade comparer = new RuneComparerToGrade();
                System.Array.Sort(runes, comparer);
                break;
            }
            case Define.SortModeOfRunes.Type:
            {
                RuneComparerToBase comparer = new RuneComparerToBase();
                System.Array.Sort(runes, comparer);
                break;
            }
        }
        Managers.Player.Data.ownedRunes.Clear();
        Managers.Player.Data.ownedRunes = runes.ToList<Rune>();
    }
}
public class RuneComparerToGrade : IComparer<Rune>
{
    public int Compare(Rune x, Rune y)
    {
        return (((int)y.gradeOfRune * 1000) + (int)y.baseRune)
            -
            (((int)x.gradeOfRune * 1000) + (int)x.baseRune);
    }
}

public class RuneComparerToBase : IComparer<Rune>
{
    public int Compare(Rune x, Rune y)
    {
        return (int)y.gradeOfRune + ((int)y.baseRune * 1000) -
            ((int)x.gradeOfRune + ((int)x.baseRune * 1000));
    }
}
