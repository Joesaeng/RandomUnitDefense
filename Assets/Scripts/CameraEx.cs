using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEx : MonoBehaviour
{
    //void Awake()
    //{
    //    var camera = Camera.main.GetComponent<Camera>();
    //    var r = camera.rect;
    //    var scaleheight = ((float)Screen.width / Screen.height) / (9f / 16f);
    //    var scalewidth = 1f / scaleheight;
    //    if (scaleheight < 1f)
    //    { r.height = scaleheight; r.y = (1f - scaleheight) / 2f; }
    //    else
    //    { r.width = scalewidth; r.x = (1f - scalewidth) / 2f; }
    //    camera.rect = r;
    //}

    private void Awake()
    {
        SetResolution(); // �ʱ⿡ ���� �ػ� ����
    }

    /* �ػ� �����ϴ� �Լ� */
    public void SetResolution()
    {
        int setWidth = 540; // ����� ���� �ʺ�
        int setHeight = 960; // ����� ���� ����

        int deviceWidth = Screen.width; // ��� �ʺ� ����
        int deviceHeight = Screen.height; // ��� ���� ����

        Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), true); // SetResolution �Լ� ����� ����ϱ�

        if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight) // ����� �ػ� �� �� ū ���
        {
            float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight); // ���ο� �ʺ�
            Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); // ���ο� Rect ����
        }
        else // ������ �ػ� �� �� ū ���
        {
            float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight); // ���ο� ����
            Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); // ���ο� Rect ����
        }
    }
}
