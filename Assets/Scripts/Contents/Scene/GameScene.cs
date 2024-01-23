using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;

public class GameScene : BaseScene
{
    [SerializeField]
    Transform _monsterSpawnPoint;

    [SerializeField]
    Define.Map _curMap = Define.Map.Basic;

    Dictionary<int,GameObject> _unitDict = new Dictionary<int,GameObject>();

    UnitSlot[] _unitSlots = null;

    protected override void Init()
    {
        base.Init();
        SceneType = Define.Scene.Game;

        _unitSlots = GameObject.Find("UnitSlots").gameObject.GetComponentsInChildren<UnitSlot>();
        Managers.Game.OnMoveUnitEvent -= OnMoveUnitBetweenSlots;
        Managers.Game.OnMoveUnitEvent += OnMoveUnitBetweenSlots;

        Managers.Time.OnMonsterRespawnTime -= TheRespawnTime;
        Managers.Time.OnMonsterRespawnTime += TheRespawnTime;
    }


    public void InputSpawnUnitTemp()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnPlayerUnit();
        }
    }

    private void Update()
    {
        if (Managers.Time.IsPause)
            return;
        // TEMP
        InputSpawnUnitTemp();

        // ������Ʈ ������ ���� �� �ֱ� ������ ���ӽſ���
        // ������Ʈ�� �ʿ��� ��� ������Ʈ���� ������Ʈ�� �����Ѵ�.
        foreach (KeyValuePair<int, GameObject> pair in _unitDict)
        {
            pair.Value.GetComponent<Unit>().UnitUpdate();
        }
        foreach (GameObject monster in Managers.Game.Monsters)
        {
            monster.GetComponent<Monster>().MonsterUpdate();
        }
        while (Managers.Game._dyingMonsters.Count > 0)
        {
            GameObject dyingMonster = Managers.Game._dyingMonsters.Pop();
            DestroyMonster(dyingMonster);
            // TODO
            // ���� ī��Ʈ ���̱�, ���� óġ ��ȭ ȹ�� �� ���Ͱ� ����� �� ����� �̺�Ʈ�� ����
        }
    }

    // ������ ���԰� �̵��� ȣ��Ǵ� �޼���
    private void OnMoveUnitBetweenSlots(int curSlotIndex, int nextSlotIndex)
    {
        if (curSlotIndex == nextSlotIndex)
        {
            _unitDict[curSlotIndex].transform.position = GetUnitMovePos(curSlotIndex);
            return;
        }
        // �̵��� ���Կ� ������ ���ٸ�
        if (_unitDict.ContainsKey(nextSlotIndex) == false)
        {
            GameObject obj = _unitDict[curSlotIndex];
            _unitDict[nextSlotIndex] = obj;
            obj.GetComponent<Unit>().SlotChange(nextSlotIndex);
            _unitDict.Remove(curSlotIndex);

            _unitDict[nextSlotIndex].transform.position = GetUnitMovePos(nextSlotIndex);
            return;
        }
        // �̵��� ���Կ� �ٸ� ������ �ִٸ� �����Ѵ�
        // �ռ� ������ �����̶�� nextSlot�� �ռ��� ���� ����
        if (_unitDict.ContainsKey(curSlotIndex) && _unitDict.ContainsKey(nextSlotIndex))
        {
            if (AreUnitsComposeable(_unitDict[curSlotIndex], _unitDict[nextSlotIndex]))
            {
                // �ռ��� �����ϴٸ�
                // �� ������ ����, ���� 3�̻� ������ �ƴ϶��. �ռ�����.

                int id = _unitDict[curSlotIndex].GetComponent<Unit>().ID;
                int nextLevel = _unitDict[nextSlotIndex].GetComponent<Unit>().Lv + 1;

                DestroyPlayerUnit(curSlotIndex);
                DestroyPlayerUnit(nextSlotIndex);

                CreatePlayerUnit(nextSlotIndex, id, nextLevel);
            }
            else
            {
                GameObject obj = _unitDict[nextSlotIndex];

                _unitDict[nextSlotIndex] = _unitDict[curSlotIndex];
                _unitDict[curSlotIndex] = obj;

                _unitDict[nextSlotIndex].transform.position = GetUnitMovePos(nextSlotIndex);
                _unitDict[curSlotIndex].transform.position = GetUnitMovePos(curSlotIndex);

                _unitDict[nextSlotIndex].GetComponent<Unit>().SlotChange(nextSlotIndex);
                _unitDict[curSlotIndex].GetComponent<Unit>().SlotChange(curSlotIndex);
            }
        }
    }

    // ���� �ΰ��� ���� �ռ��� �����Ѱ�?
    private bool AreUnitsComposeable(GameObject obj1, GameObject obj2)
    {
        bool isComposeable = false;
        Unit left = obj1.GetComponent<Unit>();
        Unit right = obj2.GetComponent<Unit>();

        if (left.ID == right.ID && left.Lv == right.Lv && left.Lv < ConstantData.PlayerUnitHighestLevel)
        {
            isComposeable = true;
        }

        return isComposeable;
    }

    // �÷��̾ ���� ��ȯ ��ư Ŭ�� �� ȣ��Ǵ� �޼���
    private void SpawnPlayerUnit()
    {
        List<int> randIndexList = new List<int>();
        int randSlotIndex = -1;
        for (int i = 0; i < _unitSlots.Length; ++i)
        {
            // 0���� unitSlots.Length ������ ���� �߿���, �̹� ���õ� ���ڴ� �����Ѵ�.
            //int rand = UnityEngine.Random.Range(0, _unitSlots.Length);
            if (_unitDict.ContainsKey(i) == true)
                continue;

            randIndexList.Add(i);
        }
        if (randIndexList.Count > 0)
        {
            int idx = UnityEngine.Random.Range(0, randIndexList.Count);
            randSlotIndex = randIndexList[idx];
        }
        if (randSlotIndex == -1)
        {
            Debug.Log("unitSlots is Full!");
            return;
        }
        int[] ids = {100,101,102,103,104,105,106,107,108};
        int randId = ids[UnityEngine.Random.Range(0, ids.Length)];

        // randId�� �κ񿡼� ����� ���ֵ��� Id�� �����ͼ� n���� 1���� �����ϴ� ������� �Ѵ�.


        CreatePlayerUnit(randSlotIndex, randId);
    }

    // �÷��̾� ���� ���� �޼���
    private void CreatePlayerUnit(int slotIndex, int id, int level = 1)
    {
        GameObject obj = Managers.Game.Spawn("Unit",newParentName:"Units");

        obj.transform.position = GetUnitMovePos(slotIndex);

        obj.GetOrAddComponent<Unit>().Init(slotIndex, id, level);

        obj.name = $"{Managers.Data.BaseUnitDict[id].baseUnit} Level [{level}]";

        _unitDict.Add(slotIndex, obj);
    }

    // �÷��̾� ���� ���� �޼���
    private void DestroyPlayerUnit(int slotIndex)
    {
        GameObject obj;
        if (_unitDict.TryGetValue(slotIndex, out obj))
        {
            obj.name = "Unit";

            Managers.Resource.Destroy(obj);
            _unitDict.Remove(slotIndex);
        }
    }

    // �÷��̾� ������ �̵��� ��ġ�� position�� ����ִ� �޼���
    private Vector3 GetUnitMovePos(int slotIndex)
    {
        // ������ ��ġ�� UnitSlot ������Ʈ ��� ���ϼ��� ������
        // Unit�� ���콺 �̺�Ʈ�� ����������
        // ������ �ȵǴ� ����? ����
        // Unit�� ���������� ī�޶������� 1��ŭ �̵��ϰ� �Ͽ� �ذ�
        return _unitSlots[slotIndex].transform.position + Vector3.back;
    }

    public override void Clear()
    {
        for (int i = 0; i < _unitSlots.Length; i++)
        {
            DestroyPlayerUnit(i);
        }
    }

    // ���͸� �������� �ð��� �Ǿ���! �� �̺�Ʈ
    private void TheRespawnTime()
    {
        if (Managers.Game.CurStageMonsterCount < ConstantData.OneStageSpawnCount)
            SpawnMonster(Managers.Game.CurStage);
    }

    // ���� ���� ��ȯ �޼���
    private void SpawnMonster(int stageNum)
    {
        GameObject monster = Managers.Game.Spawn("Monster",newParentName:"Monsters");
        monster.GetOrAddComponent<Monster>().Init(stageNum, _curMap);

        monster.transform.position = _monsterSpawnPoint.position;
        // monsters�� Count ������ ������ GameOver�� ������.
        // if(monsters.Count >= gameOverCount)
        //    GameOver(); �� �̷���?

    }

    // ���� ���� ���� �޼���
    private void DestroyMonster(GameObject monster)
    {
        Managers.Game.Despawn(monster);
    }
}
