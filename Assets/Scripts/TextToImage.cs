using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextToImage : MonoBehaviour
{
    public Camera textCamera;
    public GameObject textObject;
    public string outputFilePath = "Assets/Resources/TextImage.png";

    void Start()
    {
        // �ؽ�Ʈ ������
        textObject.SetActive(true);

        // �ؽ�Ʈ�� �̹����� ĸó�Ͽ� ����
        CaptureTextToImage();
    }

    void CaptureTextToImage()
    {
        // ī�޶� ����
        textCamera.gameObject.SetActive(true);
        textCamera.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);

        // �ؽ�Ʈ ��ġ�� ī�޶� �̵�
        textCamera.transform.position = textObject.transform.position;
        textCamera.transform.rotation = textObject.transform.rotation;

        // ī�޶� ������
        textCamera.Render();

        // ĸó�� �̹��� ����
        RenderTexture.active = textCamera.targetTexture;
        Texture2D textTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGBA32, false);
        textTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        textTexture.Apply();

        byte[] bytes = textTexture.EncodeToPNG();
        System.IO.File.WriteAllBytes(outputFilePath, bytes);

        // ���ҽ� ����
        RenderTexture.active = null;
        textCamera.targetTexture = null;
        textCamera.gameObject.SetActive(false);

        Debug.Log("Text image saved to: " + outputFilePath);
    }
}
