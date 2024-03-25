using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class screenshot : MonoBehaviour
{
    public Camera renderCamera;
    private void Update()
    {
        string path = "Assets/Resources/CombatSceneTileImage.png";
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeScreenshotAndSave(path);
        }
    }
    public void TakeScreenshotAndSave(string filePath)
    {
        // 타일맵 렌더링 후 화면 스크린샷 찍기
        Texture2D screenshot = TakeScreenshot();

        // 스크린샷을 이미지 파일로 저장
        SaveScreenshotToFile(screenshot, filePath);
    }

    private Texture2D TakeScreenshot()
    {
        // 현재 화면 크기에 맞는 텍스처 생성
        RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 24);
        renderCamera.targetTexture = rt;
        Texture2D screenshot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);

        // 카메라 렌더링
        renderCamera.Render();

        // 픽셀 정보 읽기
        RenderTexture.active = rt;
        screenshot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);

        // 리소스 정리
        renderCamera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);

        return screenshot;
    }

    private void SaveScreenshotToFile(Texture2D screenshot, string filePath)
    {
        byte[] bytes = screenshot.EncodeToPNG();
        System.IO.File.WriteAllBytes(filePath, bytes);
        Debug.Log("Screenshot saved to: " + filePath);
    }
}
