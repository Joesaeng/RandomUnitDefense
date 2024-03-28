using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UITempEditor : Editor
{
    [MenuItem("MyEditor/UI Matarial")]
    private static void ResolutionUIPrefabMaterial()
    {
        // Load Main Material
        Material uiMat = AssetDatabase.LoadAssetAtPath("Assets/Resources/Art/UIdefault.mat", typeof(Material)) as Material;
        Sprite sprtie = AssetDatabase.LoadAssetAtPath("Assets/Resources/Art/UI_true", typeof(Sprite)) as Sprite;
        // Load UI Prefabs
        string folderPath = "Assets/Resources/Prefabs/UI/";
        string[] prefabGUIDs = AssetDatabase.FindAssets("t:Prefab", new[] { folderPath });
        for (int i = 0; i < prefabGUIDs.Length; i++)
        {
            string prefabGUID = prefabGUIDs[i];
            string prefabPath = AssetDatabase.GUIDToAssetPath(prefabGUID);
            var go = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

            // Update Image Material
            Image[] images = go.GetComponentsInChildren<Image>();
            for (int j = 0; j < images.Length; j++)
            {
                Image image = images[j];
                if(image.sprite == null)
                {
                    image.sprite = sprtie;
                    image.color = new Color(1f, 1f, 1f, 0f);
                }
                if(image.material == image.defaultMaterial)
                    image.material = uiMat;
            }
            EditorUtility.SetDirty(go);
        }

        // Save
        AssetDatabase.SaveAssets();

        Debug.Log("Resolution UI Prefab Material!");
    }

    [MenuItem("MyEditor/Texts RaycastTarget Off")]
    private static void OffTextsRaycastTarget()
    {
        // Load UI Prefabs
        string folderPath = "Assets/Resources/Prefabs/UI/";
        string[] prefabGUIDs = AssetDatabase.FindAssets("t:Prefab", new[] { folderPath });
        for (int i = 0; i < prefabGUIDs.Length; i++)
        {
            string prefabGUID = prefabGUIDs[i];
            string prefabPath = AssetDatabase.GUIDToAssetPath(prefabGUID);
            var go = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

            // Update Image Material
            TextMeshProUGUI[] texts = go.GetComponentsInChildren<TextMeshProUGUI>();
            for (int j = 0; j < texts.Length; j++)
            {
                texts[j].raycastTarget = false;
            }
            EditorUtility.SetDirty(go);
        }

        // Save
        AssetDatabase.SaveAssets();

        Debug.Log("TextRaycastTarget Off Complete!");
    }
}
