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

    float movetime = 0f;
    void Update()
    {
        if(_isMoving)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, _movingPos[(int)_movePos], _scrollSpeed * Time.deltaTime);
            movetime += Time.deltaTime;
            if (movetime >= 1.5f)
            {
                _isMoving = false;
                movetime = 0f;
            }
        }
    }
}
