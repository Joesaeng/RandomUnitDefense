using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManagerEx : MonoBehaviour
{
    public BaseScene CurrentScene { get { return GameObject.FindObjectOfType<BaseScene>(); } }

    UI_LoadingObject loadingScene;

    /// <summary>
    /// 현재 Scene을 클리어하고 type에 맞는 Scene을 동기적으로 로드합니다.
    /// </summary>
    public void LoadScene(Define.Scene type)
    {
        Managers.Clear();
        SceneManager.LoadScene(GetSceneName(type));
    }

    public void LoadSceneWithLoadingScene(Define.Scene type)
    {
        Managers.Clear();
        Managers.Time.GamePause();
        StartCoroutine(CoLoadGameAsync(type));
    }

    public IEnumerator CoLoadGameAsync(Define.Scene type)
    {
        if (loadingScene == null)
        {
            GameObject go = Managers.Resource.Instantiate($"UI/UI_LoadingObject",transform);
            loadingScene = Util.GetOrAddComponent<UI_LoadingObject>(go);
        }
        loadingScene.gameObject.SetActive(true);
        loadingScene.ResetEx();

        Image fillGauge = loadingScene.FillGauge;
        WaitForSecondsRealtime forFakeLoad = new WaitForSecondsRealtime(0.01f);
        float fakeRatioIncrease;
        float fakeRatio = 0f;
        if (type == Define.Scene.Combat)
            fakeRatioIncrease = 0.01f;
        else
            fakeRatioIncrease = 0.02f;

        AsyncOperation ao = SceneManager.LoadSceneAsync(GetSceneName(type));
        ao.allowSceneActivation = false;
        while(!ao.isDone)
        {
            yield return null;
            if(ao.progress < 0.9f && fakeRatio < 0.9f)
            {
                fillGauge.fillAmount = ao.progress;
                fakeRatio += fakeRatioIncrease;
            }
            else
            {
                fillGauge.fillAmount = fakeRatio;
                fakeRatio += fakeRatioIncrease;
                if(fakeRatio >= 1f)
                {
                    ao.allowSceneActivation = true;
                }
            }

        }
        loadingScene.gameObject.SetActive(false);
        Managers.Time.GameResume();
    }

    string GetSceneName(Define.Scene type)
    {
        return System.Enum.GetName(typeof(Define.Scene), type);
    }

    public void Clear()
    {
        CurrentScene.Clear();
    }
}
