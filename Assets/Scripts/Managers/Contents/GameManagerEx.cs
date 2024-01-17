using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerEx
{
    HashSet<GameObject> _monsters = new HashSet<GameObject>();
    public HashSet<GameObject> Monsters { get { return _monsters; } }

    public Stack<GameObject> _dyingMonsters = new Stack<GameObject>();

    public Action<int> OnSpawnEvent;

    public Action<int,int> OnMoveUnitEvent;

    public GameObject Spawn(string path, Transform parent = null, string newParentName = null)
    {
        GameObject go = Managers.Resource.Instantiate(path, parent, newParentName);

        if(path == "Monster")
            _monsters.Add(go);

        return go;
    }

    public void Despawn(GameObject go)
    {
        if (_monsters.Contains(go))
        {
            _monsters.Remove(go);
            if (OnSpawnEvent != null)
                OnSpawnEvent.Invoke(-1);
        }

        Managers.Resource.Destroy(go);
    }

    // 플레이어 유닛 이동 메서드
    public void MoveUnitBetweenSlots(int curSlotIndex, int moveSlotIndex)
    {
        if (OnMoveUnitEvent != null)
            OnMoveUnitEvent.Invoke(curSlotIndex, moveSlotIndex);
    }

    // 현재 프레임에 사망하는 몬스터들을 등록하는 메서드
    public void RegisterDyingMonster(GameObject monster)
    {
        _dyingMonsters.Push(monster);
    }
}
