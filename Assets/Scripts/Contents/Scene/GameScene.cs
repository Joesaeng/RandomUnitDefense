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
    }


    public void OnSpawnUnit()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            SpawnPlayerUnit();
        }
    }

    float _curTime = 0f;
    private void Update()
    {
        // TEMP
        OnSpawnUnit();
        _curTime += Time.deltaTime;
        if(_curTime > ConstantData.MonsterRespawnTime)
        {
            SpawnMonster(1/*��������*/);
            _curTime = 0f;
        }
        //


        // ������Ʈ ������ ���� �� �ֱ� ������ ���ӽſ���
        // ������Ʈ�� �ʿ��� ��� ������Ʈ���� ������Ʈ�� �����Ѵ�.
        foreach(KeyValuePair<int,GameObject> pair in _unitDict)
        {
            pair.Value.GetComponent<Unit>().UnitUpdate();
        }
        foreach(GameObject monster in Managers.Game.Monsters)
        {
            monster.GetComponent<Monster>().MonsterUpdate();
        }
        while(Managers.Game._dyingMonsters.Count > 0)
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
        if(curSlotIndex == nextSlotIndex)
        {
            _unitDict[curSlotIndex].transform.position = GetUnitMovePos(curSlotIndex);
            return;
        }
        // �̵��� ���Կ� ������ ���ٸ�
        if(_unitDict.ContainsKey(nextSlotIndex) == false)
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
        if(_unitDict.ContainsKey(curSlotIndex) && _unitDict.ContainsKey(nextSlotIndex))
        {
            if (AreUnitsComposeable(_unitDict[curSlotIndex], _unitDict[nextSlotIndex]))
            {
                // �ռ��� �����ϴٸ�
                // �� ������ ����, ���� 3�̻� ������ �ƴ϶��. �ռ��Ѵ�.

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

        if(left.ID == right.ID && left.Lv == right.Lv && left.Lv < ConstantData.PlayerUnitHighestLevel)
        {
            isComposeable = true;
        }
        
        return isComposeable;
    }

    // �÷��̾ ���� ��ȯ ��ư Ŭ�� �� ȣ��Ǵ� �޼���
    private void SpawnPlayerUnit()
    {
        int randSlotIndex = -1;
        for(int i = 0; i < _unitSlots.Length; ++i)
        {
            int rand = UnityEngine.Random.Range(0, _unitSlots.Length);
            if (_unitDict.ContainsKey(rand) == true)
                continue;
            randSlotIndex = rand;
            break;
        }
        if(randSlotIndex == -1)
        {
            Debug.Log("unitSlots is Full!");
            return;
        }
        int[] ids = {105};
        int randId = ids[UnityEngine.Random.Range(0, ids.Length)];

        // randId�� �κ񿡼� ����� ���ֵ��� Id�� �����ͼ� n���� 1���� �����ϴ� ������� �Ѵ�.


        CreatePlayerUnit(randSlotIndex, randId);
    }

    // �÷��̾� ���� ���� �޼���
    private void CreatePlayerUnit(int slotIndex, int id, int level = 1)
    {
        GameObject obj = Managers.Game.Spawn("Unit",newParentName:"Units");

        obj.transform.position = GetUnitMovePos(slotIndex);

        obj.GetOrAddComponent<Unit>().Init(slotIndex,id,level);

        //Temp
        GameObject temp = new GameObject();
        TextMesh tm = temp.AddComponent<TextMesh>();
        tm.fontSize = 500;
        tm.characterSize = 0.02f;
        tm.color = Color.black;
        tm.text = $"{Managers.Data.BaseUnitDict[id].baseUnit} Level [{level}]";
        Instantiate(temp, obj.transform);
        Destroy(temp);
        

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
            //TEMP
            Destroy(Util.FindChild(obj));

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
