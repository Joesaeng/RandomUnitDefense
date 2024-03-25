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
        // Ÿ�ϸ� ������ �� ȭ�� ��ũ���� ���
        Texture2D screenshot = TakeScreenshot();

        // ��ũ������ �̹��� ���Ϸ� ����
        SaveScreenshotToFile(screenshot, filePath);
    }

    private Texture2D TakeScreenshot()
    {
        // ���� ȭ�� ũ�⿡ �´� �ؽ�ó ����
        RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 24);
        renderCamera.targetTexture = rt;
        Texture2D screenshot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);

        // ī�޶� ������
        renderCamera.Render();

        // �ȼ� ���� �б�
        RenderTexture.active = rt;
        screenshot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);

        // ���ҽ� ����
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
