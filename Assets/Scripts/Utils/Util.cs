using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Util
{
    public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        T component = go.GetComponent<T>();
        if (component == null)
            component = go.AddComponent<T>();

        return component;
    }

    /// <summary>
    /// ������Ʈ�� �ڽ� ������Ʈ�� ã���ϴ�.
    /// </summary>
    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(go,name,recursive);
        if (transform == null)
            return null;

        return transform.gameObject;
    }
    /// <summary>
    /// [T] ������Ʈ�� ������ �ִ� ������Ʈ�� �ڽ��� ã���ϴ�.
    /// [name] �̸��� ��ġ�ϴ� �ڽ��� ã�� ��, [recursive] �ڽ��� �ڽı��� ã�� ��,
    /// </summary>
    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (go == null)
            return null;

        if (recursive == false)
        {
            for (int i = 0; i < go.transform.childCount; ++i)
            {
                Transform transform = go.transform.GetChild(i);
                if (string.IsNullOrEmpty(name) || transform.name == name)
                {
                    T component = transform.GetComponent<T>();
                    if (component != null)
                        return component;
                }
            }
        }
        else
        {
            foreach (T component in go.GetComponentsInChildren<T>())
            {
                if (string.IsNullOrEmpty(name) || component.name == name)
                    return component;
            }
        }

        return null;
    }

    public static float GetDistance(GameObject target, GameObject go)
    {
        if (target == go)
            return 0f;
        if (target == null || go == null)
            return 0f;
        return (target.transform.position - go.transform.position).magnitude;
    }

}
