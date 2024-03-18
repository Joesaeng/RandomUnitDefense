using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx : MonoBehaviour
{
    public BaseScene CurrentScene {get {return GameObject.FindObjectOfType<BaseScene>();}}

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

        WaitForSecondsRealtime forFakeLoad = new WaitForSecondsRealtime(0.01f);
        float fakeRatioIncrease;
        float fakeRatio = 0f;
        if (type == Define.Scene.Combat)
            fakeRatioIncrease = 0.01f;
        else
            fakeRatioIncrease = 0.02f;

        while (true)
        {
            loadingScene.UpdateFillGauge(fakeRatio);
            fakeRatio += fakeRatioIncrease;
            yield return forFakeLoad;
            if (fakeRatio >= 0.9f)
                break;
        }
        AsyncOperation async = SceneManager.LoadSceneAsync(GetSceneName(type));

        while (!async.isDone)
        {
            if(async.progress > fakeRatio)
                loadingScene.UpdateFillGauge(async.progress);
            yield return null;
        }

        while (true)
        {
            loadingScene.UpdateFillGauge(fakeRatio);
            fakeRatio += fakeRatioIncrease;
            yield return forFakeLoad;
            if (fakeRatio >= 1f)
                break;
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
