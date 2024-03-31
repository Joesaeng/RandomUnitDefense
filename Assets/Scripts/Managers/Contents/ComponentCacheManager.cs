using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentCacheManager
{
    public Dictionary<GameObject,DamageText> DamageTextCache = new();
    public Dictionary<GameObject,UnitBullet> UnitBulletCache = new();

    public void AddComponentCache(GameObject gameObject, out DamageText component)
    {
        BindComponent(gameObject, out component);
        DamageTextCache.Add(gameObject, component);
    }

    public void AddComponentCache(GameObject gameObject, out UnitBullet component)
    {
        BindComponent(gameObject, out component);
        UnitBulletCache.Add(gameObject, component);
    }

    public void BindComponent<T>(GameObject gameObject, out T component) where T : Component
    {
        component = gameObject.GetOrAddComponent<T>();
    }

    public void Clear()
    {
        DamageTextCache.Clear();
        UnitBulletCache.Clear();
    }
}
