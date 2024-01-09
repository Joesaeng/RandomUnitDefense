using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;

public class GameScene : BaseScene
{
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

        HashSet<int> randomIndex = new HashSet<int>();
        while(randomIndex.Count < 5)
        {
            int i = Random.Range(0,unitSlots.Length);
            if(randomIndex.Contains(i) == false)
                randomIndex.Add(i);
        }

        foreach(int i in  randomIndex)
        {
            GameObject obj = Managers.Resource.Instantiate("Unit");

            // TEMP
            float randomR = Random.value;
            float randomG = Random.value;
            float randomB = Random.value;
            obj.GetComponent<SpriteRenderer>().color = new Color(randomR, randomG, randomB);
            //

            // ������ ������ �� ī�޶������� �����Ÿ� �̵���ŵ�ϴ�.
            // UnitSlot ������Ʈ ��� ���ϼ��� ������ Unit�� ���콺 �̺�Ʈ�� ����������
            // ������ �ȵǴ� ���װ� �־����ϴ�.
            obj.transform.position = unitSlots[i].transform.position - new Vector3(0,0,1f);
            obj.GetOrAddComponent<Unit>().Init(i);

            unitDict.Add(i, obj);
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

            unitDict[nextSlotIndex].transform.position = GetUnitMovePos(nextSlotIndex);
            return;
        }
        if(unitDict.ContainsKey(curSlotIndex) && unitDict.ContainsKey(nextSlotIndex))
        {
            GameObject obj = Instantiate(unitDict[nextSlotIndex]);
            obj.name = unitDict[nextSlotIndex].name;
            obj.GetOrAddComponent<Unit>().Init(nextSlotIndex);
            Destroy(unitDict[nextSlotIndex]);

            unitDict[nextSlotIndex] = unitDict[curSlotIndex];
            unitDict[curSlotIndex] = obj;

            unitDict[nextSlotIndex].transform.position = GetUnitMovePos(nextSlotIndex);
            unitDict[curSlotIndex].transform.position = GetUnitMovePos(curSlotIndex);

            unitDict[nextSlotIndex].GetComponent<Unit>().SlotChange(nextSlotIndex);
            unitDict[curSlotIndex].GetComponent<Unit>().SlotChange(curSlotIndex);
        }
    }

    private Vector3 GetUnitMovePos(int slotIndex)
    {
        // �����Ҷ��� �����ϰ� ������ �����϶��� ī�޶�������� �����Ÿ� �̵��� ��ġ��ŵ�ϴ�.
        return unitSlots[slotIndex].transform.position - new Vector3(0, 0, 1f);
    }

    public override void Clear()
    {

    }

}
