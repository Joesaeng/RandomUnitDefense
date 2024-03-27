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
    /// ������Ʈ�� index ��° �ڽ� ������Ʈ�� �ִ��� Ȯ���ϰ�, ������ child�� �����ɴϴ�
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
    /// Dictionary���� value������ key�� ã���ϴ�.
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
    /// str ���ڿ����� prefix ���ڿ��� ������ ���ڿ��� ��ȯ
    /// </summary>
    public static string RemovePrefix(this string str, string prefix)
    {
        // ���ڿ��� null�̰ų� ������� ��� �״�� ��ȯ
        if (string.IsNullOrEmpty(str))
            return str;

        // ���λ簡 ���ڿ��� ���۰� ��ġ�ϴ��� Ȯ���ϰ�, ��ġ�ϸ� �ش� �κ��� ������ ���ڿ� ��ȯ
        if (str.StartsWith(prefix))
            return str.Substring(prefix.Length);

        // ��ġ���� ������ ���� ���ڿ� �״�� ��ȯ
        return str;
    }
}
