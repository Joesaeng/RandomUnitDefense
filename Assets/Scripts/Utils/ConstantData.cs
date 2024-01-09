using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantData
{
    #region 상수 데이터
    public static readonly int PopupUISortOrder = 10;
    public static readonly int SceneUISortOrder = 9;
    #endregion


    // ▽▽▽▽▽▽▽▽▽▽ Contents 데이터 ▽▽▽▽▽▽▽▽▽▽▽
    #region 몬스터 이동 포인트 벡터
    // Basic Map
    public static readonly Vector3[] BasicMapPoint =
    {
        new Vector3 (-3.7f,4.7f,0f),
        new Vector3 (-3.7f,-5.5f,0f),
        new Vector3 (4.6f,-5.5f,0f),
        new Vector3 (4.6f,4.7f,0f)
    };
    #endregion
}
