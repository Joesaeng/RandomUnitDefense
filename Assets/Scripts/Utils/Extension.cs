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

    public static T GetOrAddComponent<T>(this GameObject go) where T : UnityEngine.Component
    {
        return Util.GetOrAddComponent<T>(go);
    }

    public static bool IsValid(this GameObject go)
    {
        return go != null && go.activeSelf;
    }

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
