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

            
            obj.transform.position = GetUnitMovePos(i);
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
        // 이동할 슬롯에 다른 유닛이 있다면 스왑한다.
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

    private void DestroyUnit(int slotIndex)
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
            DestroyUnit(i);
        }
    }

}
