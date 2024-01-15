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


        // 업데이트 순서가 꼬일 수 있기 때문에 게임신에서
        // 업데이트가 필요한 모든 오브젝트들의 업데이트를 관리한다.
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
            // 몬스터 카운트 줄이기, 몬스터 처치 재화 획득 등 몬스터가 사망할 때 생기는 이벤트들 실행
        }
    }

    // 유닛의 슬롯간 이동시 호출되는 메서드
    private void OnMoveUnitBetweenSlots(int curSlotIndex, int nextSlotIndex)
    {
        if(curSlotIndex == nextSlotIndex)
        {
            unitDict[curSlotIndex].transform.position = GetUnitMovePos(curSlotIndex);
            return;
        }
        // 이동할 슬롯에 유닛이 없다면
        if(unitDict.ContainsKey(nextSlotIndex) == false)
        {
            GameObject obj = unitDict[curSlotIndex];
            unitDict[nextSlotIndex] = obj;
            obj.GetComponent<Unit>().SlotChange(nextSlotIndex);
            unitDict.Remove(curSlotIndex);

            unitDict[nextSlotIndex].transform.position = GetUnitMovePos(nextSlotIndex);
            return;
        }
        // 이동할 슬롯에 다른 유닛이 있다면 스왑한다
        // 합성 가능한 유닛이라면 nextSlot에 합성된 유닛 생성
        if(unitDict.ContainsKey(curSlotIndex) && unitDict.ContainsKey(nextSlotIndex))
        {
            if (AreUnitsComposeable(unitDict[curSlotIndex], unitDict[nextSlotIndex]))
            {
                // 합성이 가능하다면
                // 두 유닛이 같고, 레벨 3이상 유닛이 아니라면. 합성한다.

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

    // 유닛 두개가 같고 합성이 가능한가?
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

    // 플레이어가 유닛 소환 버튼 클릭 시 호출되는 메서드
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
        // randId는 로비에서 등록한 유닛들의 Id를 가져와서 n개중 1개를 선택하는 방식으로 한다.


        CreatePlayerUnit(randSlotIndex, randId);
    }

    // 플레이어 유닛 생성 메서드
    private void CreatePlayerUnit(int slotIndex, int Id)
    {
        GameObject obj = Managers.Game.Spawn(Define.WorldObject.PlayerUnit,"Unit",newParentName:"Units");

        obj.transform.position = GetUnitMovePos(slotIndex);
        Unit unit = obj.GetOrAddComponent<Unit>();
        unit.Init(slotIndex, Id);
        obj.name = $"{unit.Stat.name} Level [{unit.Stat.level}]";

        unitDict.Add(slotIndex, obj);
    }

    // 플레이어 유닛 제거 메서드
    private void DestroyPlayerUnit(int slotIndex)
    {
        GameObject obj;
        if (unitDict.TryGetValue(slotIndex, out obj))
        {
            Managers.Resource.Destroy(obj);
            unitDict.Remove(slotIndex);
        }
    }

    // 플레이어 유닛이 이동할 위치의 position을 뱉어주는 메서드
    private Vector3 GetUnitMovePos(int slotIndex)
    {
        // 유닛의 위치가 UnitSlot 오브젝트 등과 동일선상에 있으니
        // Unit의 마우스 이벤트가 간헐적으로
        // 동작이 안되는 버그? 있음
        // Unit을 강제적으로 카메라쪽으로 1만큼 이동하게 하여 해결
        return unitSlots[slotIndex].transform.position + Vector3.back;
    }

    public override void Clear()
    {
        for (int i = 0; i < unitSlots.Length; i++)
        {
            DestroyPlayerUnit(i);
        }
    }

    // 몬스터 유닛 소환 메서드
    private void SpawnMonster(int stageNum)
    {
        GameObject monster = Managers.Game.Spawn(Define.WorldObject.Monster,"Monster",newParentName:"Monsters");
        monster.GetOrAddComponent<Monster>().Init(stageNum, curMap);

        monster.transform.position = monsterSpawnPoint.position;
        // monsters의 Count 정보를 가지고 GameOver를 결정함.
        // if(monsters.Count >= gameOverCount)
        //    GameOver(); 뭐 이런거?

    }

    // 몬스터 유닛 제거 메서드
    public void DestroyMonster(GameObject monster)
    {
        Managers.Game.Despawn(monster);
    }
}
