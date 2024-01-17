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

    // �÷��̾� ���� �̵� �޼���
    public void MoveUnitBetweenSlots(int curSlotIndex, int moveSlotIndex)
    {
        if (OnMoveUnitEvent != null)
            OnMoveUnitEvent.Invoke(curSlotIndex, moveSlotIndex);
    }

    // ���� �����ӿ� ����ϴ� ���͵��� ����ϴ� �޼���
    public void RegisterDyingMonster(GameObject monster)
    {
        _dyingMonsters.Push(monster);
    }
}
