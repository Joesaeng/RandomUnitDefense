using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
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

    public static float GetDistance(UnityEngine.Object left, UnityEngine.Object right)
    {
        Vector3 leftVec = Vector3.zero;
        Vector3 rightVec = Vector3.zero;
        if (left is GameObject leftObj)
        {
            leftVec = leftObj.transform.position;
        }
        else if (left is UnityEngine.Component leftComp)
        {
            leftVec = leftComp.transform.position;
        }
        else
        {
            Debug.Log("GetDistance()�� ������Ʈ��, ������Ʈ�� �ƴ� ���� ���Խ��ϴ�");
            return 0f;
        }
        if (right is GameObject rightObj)
        {
            rightVec = rightObj.transform.position;
        }
        else if (right is UnityEngine.Component rightComp)
        {
            rightVec = rightComp.transform.position;
        }
        else
        {
            Debug.Log("GetDistance()�� ������Ʈ��, ������Ʈ�� �ƴ� ���� ���Խ��ϴ�");
            return 0f;
        }
        return Vector3.Distance(leftVec, rightVec);
    }

    public static T Parse<T>(string stringData)
    {
        return (T)Enum.Parse(typeof(T), stringData);
    }

    public static float CalculatePercent(int left, int right)
    {
        if (left > right)
            return (float)right / (float)left;
        else
            return (float)left / (float)right;
    }

    
}
