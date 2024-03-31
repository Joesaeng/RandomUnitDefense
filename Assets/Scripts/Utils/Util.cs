using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public static class Util
{
    public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        T component = go.GetComponent<T>();
        if (component == null)
            component = go.AddComponent<T>();

        return component;
    }

    /// <summary>
    /// 오브젝트의 자식 오브젝트를 찾습니다.
    /// </summary>
    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(go,name,recursive);
        if (transform == null)
            return null;

        return transform.gameObject;
    }
    /// <summary>
    /// [T] 컴포넌트를 가지고 있는 오브젝트의 자식을 찾습니다.
    /// [name] 이름이 일치하는 자식을 찾을 때, [recursive] 자식의 자식까지 찾을 때,
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
            foreach (T component in go.GetComponentsInChildren<T>(true))
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
            Debug.Log("GetDistance()에 오브젝트도, 컴포넌트도 아닌 것이 들어왔습니다");
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
            Debug.Log("GetDistance()에 오브젝트도, 컴포넌트도 아닌 것이 들어왔습니다");
            return 0f;
        }
        return Vector3.Distance(leftVec, rightVec);
    }

    public static T Parse<T>(string stringData)
    {
        return (T)Enum.Parse(typeof(T), stringData);
    }

    public static string RemovePrefix(string str, string prefix)
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

    public static float CalculatePercent(int left, int right)
    {
        if (left > right)
            return (float)right / (float)left;
        else
            return (float)left / (float)right;
    }

    /// <summary>
    /// 수를 1000의자리마다 끊어서 요약합니다
    /// </summary>
    public static string ChangeNumber(int number)
    {
        return ChangeNumber(number.ToString());
    }
    public static string ChangeNumber(string number)
    {
        char[] unitAlphabet = new char[3] { 'K', 'M', 'B' };
        int unit = 0;
        while (number.Length > 6)
        {
            unit++;
            number = number.Substring(0, number.Length - 3);
        }
        if (number.Length > 3)
        {
            int newInt = int.Parse(number);
            if (number.Length > 4)
            {
                return (newInt / 1000).ToString() + unitAlphabet[unit];
            }
            else
            {
                return (newInt / 1000f).ToString("0.0") + unitAlphabet[unit];
            }
        }
        else
        {
            int newInt = int.Parse(number);
            return (newInt).ToString();
        }
    }

    public static bool CheckTheEventAndCall(Action action)
    {
        if (action != null)
        {
            action.Invoke();
            return true;
        }
        return false;
    }

    public static bool CheckTheEventAndCall<T>(Action<T> action, T value)
    {
        if (action != null)
        {
            action.Invoke(value);
            return true;
        }
        return false;
    }

    public static bool CheckTheEventAndCall<T,K>(Action<T,K> action, T t,K k)
    {
        if (action != null)
        {
            action.Invoke(t,k);
            return true;
        }
        return false;
    }


}
