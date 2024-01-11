using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerEx
{
    HashSet<GameObject> _monsters = new HashSet<GameObject>();
    public HashSet<GameObject> Monsters { get { return _monsters; } }
    public Action<int> OnSpawnEvent;

    public Action<int,int> OnMoveUnitEvent;

    public GameObject Spawn(Define.WorldObject type, string path, Transform parent = null, string newParentName = null)
    {
        GameObject go = Managers.Resource.Instantiate(path, parent, newParentName);

        switch (type)
        {
            case Define.WorldObject.Monster:
                _monsters.Add(go);
                if (OnSpawnEvent != null)
                    OnSpawnEvent.Invoke(1);
                break;
            case Define.WorldObject.PlayerUnit:
                break;
        }

        return go;
    }

    public Define.WorldObject GetWorldObjectType(GameObject go)
    {
        BaseController bc = go.GetComponent<BaseController>();
        if (bc == null)
        {
            return Define.WorldObject.Unknown;
        }
        return bc.WorldObjectType;
    }

    public void Despawn(GameObject go)
    {
        Define.WorldObject type = GetWorldObjectType(go);

        switch (type)
        {
            case Define.WorldObject.Monster:
            {
                if (_monsters.Contains(go))
                {
                    _monsters.Remove(go);
                    if (OnSpawnEvent != null)
                        OnSpawnEvent.Invoke(-1);
                }
            }
            break;
            case Define.WorldObject.PlayerUnit:
            {

            }
            break;

        }
        Managers.Resource.Destroy(go);
    }

    public void MoveUnitBetweenSlots(int curSlotIndex, int moveSlotIndex)
    {
        if(OnMoveUnitEvent != null)
            OnMoveUnitEvent.Invoke(curSlotIndex, moveSlotIndex);
    }
}
