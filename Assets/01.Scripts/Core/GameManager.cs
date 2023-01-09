using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    [SerializeField] private List<PoolableMono> _poolingList;

    public GameState State {get; private set;}
    private readonly List<IManager> _managers = new List<IManager>();

    private void Awake() {
        if(Instance == null)
            Instance = this;

        PoolManager.Instance = new PoolManager(transform);
        foreach(PoolableMono p in _poolingList){
            PoolManager.Instance.CreatePool(p, 10);
        }
    }

    private void Start() {
        _managers.Add(GetComponent<GradientBackGroundColor>());
        _managers.Add(new PlayerManager());

        UpdateState(GameState.INIT);
    }

    public void UpdateState(GameState state){
        foreach(var manager in _managers){
            manager.UpdateState(state);
        }

        State = state;

        if(State == GameState.INIT){
            UpdateState(GameState.STANDBY);
        }
    }

    public T GetGameComponent<T>() where T : class, IManager{
        var value = default(T);

        foreach(var manager in _managers.OfType<T>()){
            value = manager;
        }

        return value;
    }
}
