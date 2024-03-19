using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NestedScrollManager : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField]
    Scrollbar _scrollbar;

    [SerializeField]
    Image[] _tabBtnImages;

    [SerializeField]
    Image[] _menuImages;

    const int CountOfLobbyMenu = 3;
    float[] _pos = new float[CountOfLobbyMenu];

    Vector2[] _btnTargetSizes = new Vector2[CountOfLobbyMenu];
    Color[] _btnTargetColors = new Color[CountOfLobbyMenu];

    Vector3[] _menuTargetPoss = new Vector3[CountOfLobbyMenu];
    Vector2[] _menuTargetSizes = new Vector2[CountOfLobbyMenu];

    float _distance;
    float _curPos;
    float _targetPos;

    float _scrollSpeed = 10;

    int _targetIndex;

    bool _isDrag;

    public void Init()
    {
        #region NestedScrollBar Setting
        _scrollbar = GameObject.Find("NestedScrollBar").GetComponent<Scrollbar>();

        _tabBtnImages = new Image[] 
        {
            GameObject.Find("CombatBtn").GetComponent<Image>(), 
            GameObject.Find("BarrackBtn").GetComponent<Image>(), 
            GameObject.Find("RuneBtn").GetComponent<Image>() 
        };
        _menuImages = new Image[]
        {
            GameObject.Find("CombatImage").GetComponent<Image>(),
            GameObject.Find("BarrackImage").GetComponent<Image>(),
            GameObject.Find("RuneImage").GetComponent<Image>(),
        };

        Button tabBtn = GameObject.Find("CombatBtn").GetComponent<Button>();
        tabBtn.onClick.AddListener(() => TabClick(0));
        tabBtn = GameObject.Find("BarrackBtn").GetComponent<Button>();
        tabBtn.onClick.AddListener(() => TabClick(1));
        tabBtn = GameObject.Find("RuneBtn").GetComponent<Button>();
        tabBtn.onClick.AddListener(() => TabClick(2));

        #endregion

        // 거리에 따라 0~1인 pos 대입
        _distance = 1f / (CountOfLobbyMenu - 1);
        for (int i = 0; i < CountOfLobbyMenu; i++)
        { _pos[i] = _distance * i; }
        SetTargetPos(0);
    }

    private float SetPos()
    {
        // 절반 거리를 기준으로 가까운 위치를 반환
        for (int i = 0; i < CountOfLobbyMenu; i++)
        {
            if (_scrollbar.value < _pos[i] + _distance * 0.5f && _scrollbar.value > _pos[i] - _distance * 0.5f)
            {
                _targetIndex = i;
                return _pos[i];
            }
        }
        return 0f;
    }

    private void SetTargetPos(float pos)
    {
        _targetPos = pos;
        for(int i = 0; i < _tabBtnImages.Length;++i)
        {
            _btnTargetSizes[i] = new Vector2(320f, 190f);
            _btnTargetColors[i] = new Color(0.6f, 0.6f, 0.6f);
            _menuTargetPoss[i] = new Vector3(0f, 30f, 0f);
            _menuTargetSizes[i] = new Vector2(100f, 100f);
        }
        _btnTargetSizes[_targetIndex] = new Vector2(440f, 250f);
        _btnTargetColors[_targetIndex] = Color.white;
        _menuTargetPoss[_targetIndex] = new Vector3(0f, 80f, 0f);
        _menuTargetSizes[_targetIndex] = new Vector2(200f, 200f);
    }

    public void OnBeginDrag(PointerEventData eventData) => _curPos = SetPos();

    public void OnDrag(PointerEventData eventData) => _isDrag = true;

    public void OnEndDrag(PointerEventData eventData)
    {
        _isDrag = false;

        SetTargetPos(SetPos());

        if(_curPos == _targetPos)
        {
            // 스크롤이 왼쪽으로 빠르게 이동 시 목표가 하나 감소
            if(eventData.delta.x > 18 && _curPos - _distance >= 0)
            {
                --_targetIndex;
                SetTargetPos(_curPos - _distance);
            }

            else if (eventData.delta.x < -18 && _curPos + _distance <= 1.01f)
            {
                ++_targetIndex;
                SetTargetPos(_curPos + _distance);
            }
        }
    }

    public void TabClick(int tabIndex)
    {
        Managers.Sound.Play(Define.SFXNames.Click);
        _targetIndex = tabIndex;
        SetTargetPos(_pos[tabIndex]);
    }

    void Update()
    {
        if (!_isDrag)
        {
            _scrollbar.value = Mathf.Lerp(_scrollbar.value, _targetPos, _scrollSpeed * Time.deltaTime);
            for (int i = 0; i < _tabBtnImages.Length; ++i)
            {
                _tabBtnImages[i].rectTransform.sizeDelta = Vector2.Lerp(_tabBtnImages[i].rectTransform.sizeDelta, _btnTargetSizes[i], _scrollSpeed * Time.deltaTime);
                _tabBtnImages[i].color = Color.Lerp(_tabBtnImages[i].color, _btnTargetColors[i], _scrollSpeed * Time.deltaTime);
                _menuImages[i].rectTransform.sizeDelta = Vector2.Lerp(_menuImages[i].rectTransform.sizeDelta, _menuTargetSizes[i], _scrollSpeed * Time.deltaTime);
                _menuImages[i].rectTransform.localPosition = Vector3.Lerp(_menuImages[i].rectTransform.localPosition, _menuTargetPoss[i], _scrollSpeed * Time.deltaTime);
            }
        }
        
    }
}
