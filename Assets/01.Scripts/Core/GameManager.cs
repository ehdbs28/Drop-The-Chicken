using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    public bool Stop {get; set;} = false;
    public bool IsRevive {get; set;} = false;

    public bool OneRevive = false;

    [SerializeField] private List<PoolableMono> _poolingList;

    public GameState State {get; private set;}
    private readonly List<IManager> _managers = new List<IManager>();

    public bool GameStop {
        set {
            float timeValue = value ? 0 : 1;
            GetManager<TimeManager>().Stop(timeValue, 0f);
        }
    }

    private void Awake() {
        Screen.SetResolution(1080, 1920, false);

        if(Instance == null)
            Instance = this;

        PoolManager.Instance = new PoolManager(transform);
        foreach(PoolableMono p in _poolingList){
            PoolManager.Instance.CreatePool(p, 10);
        }
    }

    private void Start() {
        _managers.Add(new DataManager());
        _managers.Add(new MapManager());
        _managers.Add(new PlayerManager());
        _managers.Add(new ScoreManager());
        _managers.Add(new CashManager());
        _managers.Add(GetComponent<TimeManager>());
        _managers.Add(GetComponent<UIManager>());
        _managers.Add(new ESCManager());
        _managers.Add(new CameraManager());

        _managers.Add(GetComponent<AudioManager>());
        _managers.Add(GetComponent<GradientBackGroundColor>());

        UpdateState(GameState.INIT);
    }

    public void UpdateState(GameState state){
        for(int i = 0; i < _managers.Count; ++i)
        {
            _managers[i].UpdateState(state);
        }

        State = state;

        if(State == GameState.INIT){
            UpdateState(GameState.STANDBY);
        }

        if(IsRevive) IsRevive = false;
    }

    public T GetManager<T>() where T : class, IManager{
        var value = default(T);

        foreach(var manager in _managers.OfType<T>()){
            value = manager;
        }

        return value;
    }
}
