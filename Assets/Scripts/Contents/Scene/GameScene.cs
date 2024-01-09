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

            // 유닛을 스폰할 때 카메라쪽으로 일정거리 이동시킵니다.
            // UnitSlot 오브젝트 등과 동일선상에 있으니 Unit의 마우스 이벤트가 간헐적으로
            // 동작이 안되는 버그가 있었습니다.
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
        // 스폰할때와 동일하게 유닛을 움직일때도 카메라방향으로 일정거리 이동한 위치시킵니다.
        return unitSlots[slotIndex].transform.position - new Vector3(0, 0, 1f);
    }

    public override void Clear()
    {

    }

}
