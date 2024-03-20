using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedPanel : MonoBehaviour
{
    enum MovePos
    {
        Show,
        Hide
    }

    MovePos _movePos;

    Vector3[] _movingPos = new Vector3[2]{ new Vector3(0f, -300f, 0f), new Vector3(0f, -1200f, 0f) };
    bool _isMoving = false;

    float _scrollSpeed = 10f;
    public void ShowSelectedPanel()
    {
        _movePos = MovePos.Show;
        _isMoving = true;
    }
    public void HideSelectedPanel() 
    { 
        _movePos = MovePos.Hide;
        _isMoving = true;
    }

    void Update()
    {
        if(_isMoving)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, _movingPos[(int)_movePos], _scrollSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.localPosition, _movingPos[(int)_movePos]) <= 1f)
            {
                _isMoving = false;
            }
        }
    }
}
