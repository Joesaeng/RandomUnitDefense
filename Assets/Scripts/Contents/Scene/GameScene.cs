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

        for(int i = 0; i < 5; ++i)
        {
            SpawnPlayerUnit();
        }
    }

    float spawnTime = 0.5f;
    float curTime = 0f;
    private void Update()
    {
        // TEMP
        curTime += Time.deltaTime;
        if(curTime > spawnTime)
        {
            SpawnMonster(1);
            curTime = 0f;
        }
        //

        foreach(KeyValuePair<int,GameObject> pair in unitDict)
        {
            pair.Value.GetComponent<Unit>().UnitUpdate();
        }
        foreach(GameObject monster in Managers.Game.Monsters)
        {
            monster.GetComponent<Monster>().MonsterUpdate();
        }
    }

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
            unitDict[nextSlotIndex].name = $"Unit Slot : {nextSlotIndex}";

            unitDict[nextSlotIndex].transform.position = GetUnitMovePos(nextSlotIndex);
            return;
        }
        // �̵��� ���Կ� �ٸ� ������ �ִٸ� �����Ѵ�.
        if(unitDict.ContainsKey(curSlotIndex) && unitDict.ContainsKey(nextSlotIndex))
        {
            GameObject obj = unitDict[nextSlotIndex];

            unitDict[nextSlotIndex] = unitDict[curSlotIndex];
            unitDict[curSlotIndex] = obj;

            unitDict[nextSlotIndex].name = $"Unit Slot : {nextSlotIndex}";
            unitDict[curSlotIndex].name = $"Unit Slot : {curSlotIndex}";

            unitDict[nextSlotIndex].transform.position = GetUnitMovePos(nextSlotIndex);
            unitDict[curSlotIndex].transform.position = GetUnitMovePos(curSlotIndex);

            unitDict[nextSlotIndex].GetComponent<Unit>().SlotChange(nextSlotIndex);
            unitDict[curSlotIndex].GetComponent<Unit>().SlotChange(curSlotIndex);
        }
    }

    private void SpawnPlayerUnit()
    {
        int randSlotIndex;
        while(true)
        {
            int i = Random.Range(0, unitSlots.Length);
            if (unitDict.ContainsKey(i) == true)
                continue;
            randSlotIndex = i;
            break;
        }

        GameObject obj = Managers.Game.Spawn(Define.WorldObject.PlayerUnit,"Unit");
        obj.name = $"{obj.name} Slot : {randSlotIndex}";

        // TEMP
        float randomR = Random.value;
        float randomG = Random.value;
        float randomB = Random.value;
        obj.GetComponent<SpriteRenderer>().color = new Color(randomR, randomG, randomB);
        //


        obj.transform.position = GetUnitMovePos(randSlotIndex);
        obj.GetOrAddComponent<Unit>().Init(randSlotIndex);

        unitDict.Add(randSlotIndex, obj);
    }

    private void DestroyPlayerUnit(int slotIndex)
    {
        GameObject obj;
        if (unitDict.TryGetValue(slotIndex, out obj))
        {
            Managers.Resource.Destroy(obj);
            unitDict.Remove(slotIndex);
        }
    }

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

    private void SpawnMonster(int stageNum)
    {
        GameObject monster = Managers.Game.Spawn(Define.WorldObject.Monster,"Monster",newParentName:"Monsters");
        monster.GetOrAddComponent<Monster>().Init(stageNum, curMap);

        monster.transform.position = monsterSpawnPoint.position;
        // monsters�� Count ������ ������ GameOver�� ������.
        // if(monsters.Count >= gameOverCount)
        //    GameOver(); �� �̷���?

    }

    public void DestroyMonster(GameObject monster)
    {
        // monster ��ũ��Ʈ���� ü���� 0 ���ϰ� �Ǹ� ȣ��
        Managers.Game.Despawn(monster);
    }
}
