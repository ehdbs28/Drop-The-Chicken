using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    [SerializeField] private List<PoolableMono> _poolingList;

    private void Awake() {
        if(Instance == null)
            Instance = this;

        PoolManager.Instance = new PoolManager(transform);
        foreach(PoolableMono p in _poolingList){
            PoolManager.Instance.CreatePool(p, 10);
        }
    }
}
