using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneManager
{
    Dictionary<GradeOfRune, Sprite> _runeSprites;
    public Dictionary<GradeOfRune, Sprite> RuneSprites { get => _runeSprites;}
    
    Dictionary<BaseRune,string> _runeTextImages;
    public Dictionary<BaseRune, string> RuneTextImages { get => _runeTextImages;}

    public void Init()
    {
        Sprite[] runeImages = Resources.LoadAll<Sprite>("Art/UIImages/Runes");

        _runeSprites = new Dictionary<GradeOfRune, Sprite>();
        _runeTextImages = new Dictionary<BaseRune, string>();

        // 리소스에 있는 룬 이미지를 딕셔너리에 채움
        foreach (Sprite runeSprite in runeImages)
        {
            _runeSprites.Add(Util.Parse<GradeOfRune>(runeSprite.name), runeSprite);
        }

        // 룬 이미지 내부에 들어갈 한자 채우기
        string[] runeTextImages = new string[(int)BaseRune.Count]
        {
            "骑", // 말탈 기
            "弓", // 활 궁
            "海", // 바다 해
            "火", // 불 화
            "矛", // 창 모
            "於", // 어조사 어(어리어..)
            "毒", // 독 독
            "富", // 부유할 부
            "战", // 싸움 전
            "福", // 복 복
            "咀", // 씹을 저
        };
        for(int i = 0; i < (int)BaseRune.Count; i++)
        {
            _runeTextImages.Add((BaseRune)i, runeTextImages[i]);
        }
    }

    // 랜덤한 룬을 생성하는 함수, 룬 그레이드 설정 가능
    public Rune CreateRandomRune(GradeOfRune setGrade = GradeOfRune.None)
    {
        Rune rune = new Rune();

        int         randomBase  = Random.Range(0,(int)BaseRune.Count - 1);
        BaseRune    runeBase    = (BaseRune)randomBase;

        float       randomGradeSelect   = Random.value;
        GradeOfRune runeGrade;

        // 룬 그레이드 선택
        if (setGrade == GradeOfRune.None)
        {
            if (randomGradeSelect       <= ConstantData.PercentOfMythRune)
                runeGrade = GradeOfRune.Myth;
            else if (randomGradeSelect  <= ConstantData.PercentOfLegendRune)
                runeGrade = GradeOfRune.Legend;
            else if (randomGradeSelect  <= ConstantData.PercentOfUniqueRune)
                runeGrade = GradeOfRune.Unique;
            else if (randomGradeSelect  <= ConstantData.PercentOfRareRune)
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

        int additionalEftCount = ConstantData.AdditionalEftCountOfRunes[(int)runeGrade];

        for(int i = 0; i < additionalEftCount; ++i)
        {
            rune.additionalEffects.Add(CreateRandomAdditionalEffect());
        }

        return rune;
    }

    public AdditionalEffectOfRune CreateRandomAdditionalEffect()
    {
        AdditionalEffectOfRune effect = new AdditionalEffectOfRune();

        int randomEffect = Random.Range(0,(int)AdditionalEffectName.Count - 1);
        AdditionalEffectName additionalEffect = (AdditionalEffectName)randomEffect;

        effect.name = additionalEffect;
        AdditionalEffectOfRuneValueMinMax minMax = Managers.Data.EffectMinMaxs[$"{effect.name}"];
        effect.value = Random.Range(minMax.min, minMax.max);

        // 추가데미지와 투사체속도증가는 소수점이 필요 없으니 없앰
        if (effect.name == AdditionalEffectName.AddedDamage
            || effect.name == AdditionalEffectName.IncreaseBulletSpeed)
            effect.value = System.MathF.Round(effect.value);
        // 이 외는 소수점 두자리까지만 사용
        else
            effect.value = System.MathF.Round(effect.value,2);

        return effect;
    }
}
