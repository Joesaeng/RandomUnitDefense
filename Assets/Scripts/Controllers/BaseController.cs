using System.Collections;
using System.Collections.Generic;
using UnityEditor.Purchasing;
using UnityEngine;

// 모든 이동가능한 Object들이 상속받는 기본적인 Controller 컴포넌트
public abstract class BaseController : MonoBehaviour
{
    [SerializeField]
    protected Animator anim = null;

    [SerializeField]
    protected Define.State _state = Define.State.Idle;

    protected GameObject _lockTarget;

    public Define.WorldObject WorldObjectType { get; protected set; } = Define.WorldObject.Unknown;

    public virtual Define.State State
    {
        get { return _state; }
        set
        {
            _state = value;
            switch (_state)
            {
                case Define.State.Die:
                    break;
                case Define.State.Idle:
                    break;
                case Define.State.Moving:
                    break;
                case Define.State.Skill:
                    break;
            }
        }
    }

    private void Start()
    {
        Init();
    }


    void Update()
    {
        switch (State)
        {
            case Define.State.Die:
                UpdateDie();
                break;
            case Define.State.Moving:
                UpdateMoving();
                break;
            case Define.State.Idle:
                UpdateIdle();
                break;
            case Define.State.Skill:
                UpdateSkill();
                break;
        }
    }

    public abstract void Init();
    protected virtual void UpdateDie() { }
    protected virtual void UpdateMoving() { }
    protected virtual void UpdateIdle() { }
    protected virtual void UpdateSkill() { }
}
