using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class temps : MonoBehaviour
{
    void Start()
    {
        for(int i = 0; i < 10; ++i)
        {
           Rune rune = Managers.Rune.CreateRandomRune(Data.GradeOfRune.Myth);
            string tt = "";
            for(int  j = 0; j < rune.additionalEffects.Count; j++)
            {
                tt += $"{rune.additionalEffects[j].name} : {rune.additionalEffects[j].value}\n";
            }
            Debug.Log(tt);
        }
    }

    void Update()
    {
        
    }
}
