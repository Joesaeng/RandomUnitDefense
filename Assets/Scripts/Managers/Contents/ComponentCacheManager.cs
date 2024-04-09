using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 컴포넌트를 캐싱하고 관리하는 매니저
public class ComponentCacheManager
{
    public Dictionary<GameObject,List<Component>> CompCache = new();

    public void GetOrAddComponentCache<T>(GameObject gameObject, out T component) where T : Component
    {
        List <Component> comps;
        if (CompCache.TryGetValue(gameObject, out comps))
        {
            foreach(var comp in comps)
            {
                if (comp.GetType() == typeof(T))
                {
                    component = (T)comp;
                    return;
                }
            }
            component = gameObject.GetOrAddComponent<T>();
            comps.Add(component);
        }
        else
        {
            component = gameObject.GetOrAddComponent<T>();
            if (CompCache.TryGetValue(gameObject, out comps))
                comps.Add(component);
            else
            {
                comps = new List<Component>{ component };
                CompCache.Add(gameObject, comps);
            }
        }
    }

    public void Clear()
    {
        CompCache.Clear();
    }
}
