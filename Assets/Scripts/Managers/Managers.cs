using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers s_instance; // 유일성이 보장된다.
    static Managers Instance { get { Init(); return s_instance; } }

    #region Contents
    GameManagerEx _game = new GameManagerEx();
    UnitStatusManager _unitStatus = new UnitStatusManager();
    InGameItemManager _inGameItem = new InGameItemManager();
    PlayerManager _player = new PlayerManager();


    public static GameManagerEx Game { get { return Instance._game; } }
    public static UnitStatusManager UnitStatus { get {  return Instance._unitStatus; } }
    public static InGameItemManager InGameItem { get {  return Instance._inGameItem; } }
    public static PlayerManager Player { get {  return Instance._player; } }
    #endregion

    #region Core
    DataManager _data = new DataManager();
    InputManager _input = new InputManager();
    PoolManager _pool = new PoolManager();
    ResourceManager _resource = new ResourceManager();
    SceneManagerEx _scene;
    SoundManager _sound = new SoundManager();
    TimeManager _time = new TimeManager();
    UIManager _UI = new UIManager();

    public static DataManager Data { get { return Instance._data; } }
    public static InputManager Input { get { return Instance._input; } }
    public static PoolManager Pool { get { return Instance._pool; } }
    public static ResourceManager Resource { get { return Instance._resource; } }
    public static SceneManagerEx Scene { get { return Instance._scene; } }
    public static SoundManager Sound { get { return Instance._sound; } }
    public static TimeManager Time { get { return Instance._time; } }
    public static UIManager UI { get { return Instance._UI; } }
    #endregion

    void Start()
    {
        #region Content
        Application.targetFrameRate = 60; // 프레임 고정

        #endregion
        Init();
    }

    void Update()
    {
        _input.OnUpdate();
        _time.OnUpdate();
    }

    public static void Init()
    {
        if(s_instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
            if(go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }

            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<Managers>();

            // 초기화가 필요한 매니저들 초기화 작업
            GameObject sceneManager = new GameObject {name = "@SceneManagerEX"};
            sceneManager.AddComponent<SceneManagerEx>();
            sceneManager.transform.SetParent(go.transform);
            s_instance._scene = sceneManager.GetComponent<SceneManagerEx>();

            s_instance._data.Init();
            s_instance._pool.Init();
            s_instance._player.Init();
            s_instance._sound.Init();
        }
    }

    public static void Clear()
    {
        Sound.Clear();
        Input.Clear();
        Scene.Clear();
        UI.Clear();

        Pool.Clear();
    }
}
