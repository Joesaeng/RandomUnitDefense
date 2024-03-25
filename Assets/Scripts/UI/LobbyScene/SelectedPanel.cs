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
    WaitForSeconds _wfs = new WaitForSeconds(0.05f);

    float _scrollSpeed = 10f;
    public void ShowSelectedPanel()
    {
        _movePos = MovePos.Show;
        StartCoroutine("CoMovePanel");
    }
    public void HideSelectedPanel() 
    {
        _movePos = MovePos.Hide;
        StartCoroutine("CoMovePanel");
    }

    IEnumerator CoMovePanel()
    {
        _isMoving = true;
        while (_isMoving)
        {
            yield return null;
            transform.localPosition = Vector3.Lerp(transform.localPosition, _movingPos[(int)_movePos], _scrollSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.localPosition, _movingPos[(int)_movePos]) <= 1f)
            {
                _isMoving = false;
                if(_movePos == MovePos.Hide)
                    gameObject.SetActive(false);
                break;
            }
        }
    }

}
