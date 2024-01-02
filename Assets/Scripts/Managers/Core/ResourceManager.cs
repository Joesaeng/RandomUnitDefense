using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    /// <summary>
    /// Resources폴더에 있는 리소스를 {path}로 불러 로드합니다.
    /// </summary>
    public T Load<T>(string path) where T : Object
    {
        if(typeof(T) == typeof(GameObject))
        {
            string name = path;
            int index = name.LastIndexOf('/');
            if (index >= 0)
                name = name.Substring(index + 1);

            GameObject go = Managers.Pool.GetOriginal(name);
            if (go != null)
                return go as T;
        }
        return Resources.Load<T>(path);
    }

    /// <summary>
    /// 리소스>프리펩 폴더에 있는 프리펩을 주소값으로 불러 반환합니다.
    /// path => Resources/Prefabs/{path}
    /// </summary>
    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject original = Load<GameObject>($"Prefabs/{path}");
        if(original == null)
        {
            Debug.Log($"Failed to load prefab : {path}");
            return null;
        }

        if (original.GetComponent<Poolable>() != null)
            return Managers.Pool.Pop(original, parent).gameObject;

        GameObject go = Object.Instantiate(original, parent);
        go.name = original.name;

        return go;
    }

    public void Destroy(GameObject go)
    {
        if (go == null)
            return;

        Poolable poolable = go.GetComponent<Poolable>();
        if(poolable != null)
        {
            Managers.Pool.Push(poolable);
            return;
        }    

        Object.Destroy(go);
    }
}
