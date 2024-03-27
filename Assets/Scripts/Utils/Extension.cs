using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Extension
{
    public static void AddUIEvent(this GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
    {
        UI_Base.BindEvent(go, action, type);
    }
    /// <summary>
    /// 오브젝트에 index 번째 자식 오브젝트가 있는지 확인하고, 있으면 child로 가져옵니다
    /// </summary>
    public static bool TryGetChild(this Transform parent, int index , out Transform child)
    {
        child = null;
        if (parent.childCount <= index)
            return false;

        child = parent.GetChild(index);
        return true;
    }

    public static T GetOrAddComponent<T>(this GameObject go) where T : UnityEngine.Component
    {
        return Util.GetOrAddComponent<T>(go);
    }

    public static bool IsValid(this GameObject go)
    {
        return go != null && go.activeSelf;
    }

    /// <summary>
    /// Dictionary에서 value값으로 key를 찾습니다.
    /// </summary>
    public static bool FindKeyByValueInDictionary<K, V>(this Dictionary<K,V> dict, V value, out K key)
    {
        foreach(KeyValuePair<K,V> pair in dict)
        {
            if (EqualityComparer<V>.Default.Equals(pair.Value, value))
            {
                key = pair.Key;
                return true;
            }
        }
        key = default(K);
        return false;
    }
    /// <summary>
    /// str 문자열에서 prefix 문자열을 제거한 문자열을 반환
    /// </summary>
    public static string RemovePrefix(this string str, string prefix)
    {
        // 문자열이 null이거나 비어있을 경우 그대로 반환
        if (string.IsNullOrEmpty(str))
            return str;

        // 접두사가 문자열의 시작과 일치하는지 확인하고, 일치하면 해당 부분을 제거한 문자열 반환
        if (str.StartsWith(prefix))
            return str.Substring(prefix.Length);

        // 일치하지 않으면 원래 문자열 그대로 반환
        return str;
    }
}
