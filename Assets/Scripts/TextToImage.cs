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
        // 텍스트 렌더링
        textObject.SetActive(true);

        // 텍스트를 이미지로 캡처하여 저장
        CaptureTextToImage();
    }

    void CaptureTextToImage()
    {
        // 카메라 설정
        textCamera.gameObject.SetActive(true);
        textCamera.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);

        // 텍스트 위치에 카메라 이동
        textCamera.transform.position = textObject.transform.position;
        textCamera.transform.rotation = textObject.transform.rotation;

        // 카메라 렌더링
        textCamera.Render();

        // 캡처한 이미지 저장
        RenderTexture.active = textCamera.targetTexture;
        Texture2D textTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGBA32, false);
        textTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        textTexture.Apply();

        byte[] bytes = textTexture.EncodeToPNG();
        System.IO.File.WriteAllBytes(outputFilePath, bytes);

        // 리소스 정리
        RenderTexture.active = null;
        textCamera.targetTexture = null;
        textCamera.gameObject.SetActive(false);

        Debug.Log("Text image saved to: " + outputFilePath);
    }
}
