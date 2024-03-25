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
}
