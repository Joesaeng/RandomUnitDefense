using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    public Define.CameraMode _mode = Define.CameraMode.QuaterView;

    [SerializeField]
    public Vector3 _delta = new Vector3(0f,6f,-5f); // 카메라 오프셋

    [SerializeField]
    GameObject _player = null;

    public void SetPlayer(GameObject player) {  _player = player; }

    void Start()
    {

    }

    void LateUpdate()
    {
        if (_mode == Define.CameraMode.QuaterView)
        {
            if (_player.IsValid() == false)
            {

                return;
            }
            RaycastHit hit;
            Vector3 playerHeadPos = _player.transform.position + Vector3.up * _player.GetComponent<Collider>().bounds.size.y;
            if (Physics.Raycast(playerHeadPos, _delta, out hit, _delta.magnitude, 1 << (int)Define.Layer.Block))
            {
                float dist = (hit.point - playerHeadPos).magnitude * 0.8f;
                transform.position = playerHeadPos + _delta.normalized * dist;
            }
            else
            {
                transform.position = playerHeadPos + _delta;
                transform.LookAt(_player.transform);
            }
        }
    }

    public void SetQuaterView(Vector3 delta)
    {
        _delta = delta;
        _mode = Define.CameraMode.QuaterView;
    }
}
