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
    Transform monsterSpawnPoint;

    Define.Map curMap = Define.Map.Basic;

    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.Game;
    }

    Dictionary<int,GameObject> unitDict = new Dictionary<int,GameObject>();

    UnitSlot[] unitSlots = null;

    private void Start()
    {
        unitSlots = GameObject.Find("UnitSlots").gameObject.GetComponentsInChildren<UnitSlot>();
        Managers.Game.OnMoveUnitEvent -= OnMoveUnitBetweenSlots;
        Managers.Game.OnMoveUnitEvent += OnMoveUnitBetweenSlots;
    }

    public void OnSpawnUnit()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            SpawnPlayerUnit();
        }
    }

    float spawnTime = 0.5f;
    float curTime = 0f;

    private void Update()
    {
        // TEMP
        OnSpawnUnit();
        curTime += Time.deltaTime;
        if(curTime > spawnTime)
        {
            SpawnMonster(1);
            curTime = 0f;
        }
        //


        // ������Ʈ ������ ���� �� �ֱ� ������ ���ӽſ���
        // ������Ʈ�� �ʿ��� ��� ������Ʈ���� ������Ʈ�� �����Ѵ�.
        foreach(KeyValuePair<int,GameObject> pair in unitDict)
        {
            pair.Value.GetComponent<Unit>().UnitUpdate();
        }
        foreach(GameObject monster in Managers.Game.Monsters)
        {
            monster.GetComponent<Monster>().MonsterUpdate();
        }
        while(Managers.Game.dyingMonsters.Count > 0)
        {
            GameObject dyingMonster = Managers.Game.dyingMonsters.Pop();
            DestroyMonster(dyingMonster);
            // TODO
            // ���� ī��Ʈ ���̱�, ���� óġ ��ȭ ȹ�� �� ���Ͱ� ����� �� ����� �̺�Ʈ�� ����
        }
    }

    // ������ ���԰� �̵��� ȣ��Ǵ� �޼���
    private void OnMoveUnitBetweenSlots(int curSlotIndex, int nextSlotIndex)
    {
        if(curSlotIndex == nextSlotIndex)
        {
            unitDict[curSlotIndex].transform.position = GetUnitMovePos(curSlotIndex);
            return;
        }
        // �̵��� ���Կ� ������ ���ٸ�
        if(unitDict.ContainsKey(nextSlotIndex) == false)
        {
            GameObject obj = unitDict[curSlotIndex];
            unitDict[nextSlotIndex] = obj;
            obj.GetComponent<Unit>().SlotChange(nextSlotIndex);
            unitDict.Remove(curSlotIndex);

            unitDict[nextSlotIndex].transform.position = GetUnitMovePos(nextSlotIndex);
            return;
        }
        // �̵��� ���Կ� �ٸ� ������ �ִٸ� �����Ѵ�
        // �ռ� ������ �����̶�� nextSlot�� �ռ��� ���� ����
        if(unitDict.ContainsKey(curSlotIndex) && unitDict.ContainsKey(nextSlotIndex))
        {
            if (AreUnitsComposeable(unitDict[curSlotIndex], unitDict[nextSlotIndex]))
            {
                // �ռ��� �����ϴٸ�
                // �� ������ ����, ���� 3�̻� ������ �ƴ϶��. �ռ��Ѵ�.

                int id = unitDict[curSlotIndex].GetComponent<Unit>().Stat.id + 1;

                DestroyPlayerUnit(curSlotIndex);
                DestroyPlayerUnit(nextSlotIndex);

                CreatePlayerUnit(nextSlotIndex, id);
            }
            else
            {
                GameObject obj = unitDict[nextSlotIndex];

                unitDict[nextSlotIndex] = unitDict[curSlotIndex];
                unitDict[curSlotIndex] = obj;

                unitDict[nextSlotIndex].transform.position = GetUnitMovePos(nextSlotIndex);
                unitDict[curSlotIndex].transform.position = GetUnitMovePos(curSlotIndex);

                unitDict[nextSlotIndex].GetComponent<Unit>().SlotChange(nextSlotIndex);
                unitDict[curSlotIndex].GetComponent<Unit>().SlotChange(curSlotIndex);
            }
        }
    }

    // ���� �ΰ��� ���� �ռ��� �����Ѱ�?
    private bool AreUnitsComposeable(GameObject obj1, GameObject obj2)
    {
        bool isComposeable = false;
        UnitStat obj1Stat = obj1.GetComponent<Unit>().Stat;
        UnitStat obj2Stat = obj2.GetComponent<Unit>().Stat;

        if(obj1Stat.id == obj2Stat.id && obj1Stat.level < 3)
        {
            isComposeable = true;
        }
        
        return isComposeable;
    }

    // �÷��̾ ���� ��ȯ ��ư Ŭ�� �� ȣ��Ǵ� �޼���
    private void SpawnPlayerUnit()
    {
        int randSlotIndex = -1;
        for(int i = 0; i < unitSlots.Length; ++i)
        {
            int rand = UnityEngine.Random.Range(0, unitSlots.Length);
            if (unitDict.ContainsKey(rand) == true)
                continue;
            randSlotIndex = rand;
            break;
        }
        if(randSlotIndex == -1)
        {
            Debug.Log("unitSlots is Full!");
            return;
        }

        int randId = 1;
        // randId�� �κ񿡼� ����� ���ֵ��� Id�� �����ͼ� n���� 1���� �����ϴ� ������� �Ѵ�.


        CreatePlayerUnit(randSlotIndex, randId);
    }

    // �÷��̾� ���� ���� �޼���
    private void CreatePlayerUnit(int slotIndex, int Id)
    {
        GameObject obj = Managers.Game.Spawn(Define.WorldObject.PlayerUnit,"Unit",newParentName:"Units");

        obj.transform.position = GetUnitMovePos(slotIndex);
        Unit unit = obj.GetOrAddComponent<Unit>();
        unit.Init(slotIndex, Id);
        obj.name = $"{unit.Stat.name} Level [{unit.Stat.level}]";

        unitDict.Add(slotIndex, obj);
    }

    // �÷��̾� ���� ���� �޼���
    private void DestroyPlayerUnit(int slotIndex)
    {
        GameObject obj;
        if (unitDict.TryGetValue(slotIndex, out obj))
        {
            Managers.Resource.Destroy(obj);
            unitDict.Remove(slotIndex);
        }
    }

    // �÷��̾� ������ �̵��� ��ġ�� position�� ����ִ� �޼���
    private Vector3 GetUnitMovePos(int slotIndex)
    {
        // ������ ��ġ�� UnitSlot ������Ʈ ��� ���ϼ��� ������
        // Unit�� ���콺 �̺�Ʈ�� ����������
        // ������ �ȵǴ� ����? ����
        // Unit�� ���������� ī�޶������� 1��ŭ �̵��ϰ� �Ͽ� �ذ�
        return unitSlots[slotIndex].transform.position + Vector3.back;
    }

    public override void Clear()
    {
        for (int i = 0; i < unitSlots.Length; i++)
        {
            DestroyPlayerUnit(i);
        }
    }

    // ���� ���� ��ȯ �޼���
    private void SpawnMonster(int stageNum)
    {
        GameObject monster = Managers.Game.Spawn(Define.WorldObject.Monster,"Monster",newParentName:"Monsters");
        monster.GetOrAddComponent<Monster>().Init(stageNum, curMap);

        monster.transform.position = monsterSpawnPoint.position;
        // monsters�� Count ������ ������ GameOver�� ������.
        // if(monsters.Count >= gameOverCount)
        //    GameOver(); �� �̷���?

    }

    // ���� ���� ���� �޼���
    public void DestroyMonster(GameObject monster)
    {
        Managers.Game.Despawn(monster);
    }
}
