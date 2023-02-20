using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SummonObj
{
    public PoolableMono summonObj;
    public int minXPos = -2;
    public int maxXPos = 2;
}

public class MapSystem : MonoBehaviour
{
    [SerializeField]
    private FeverObj[] _feverObjs;
    private Player _player;

    [SerializeField]
    private SummonObj[] _objs;

    private List<PoolableMono> _mapObj = new List<PoolableMono>();
    private List<PoolableMono> _lastMapObj = new List<PoolableMono>();

    private int _summonY = -2;

    private int _objMinSpace = 2;
    private int _objMaxSpace = 4;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public void ResetMap()
    {
        ResetGame();
        AddObj();
    }

    private void AddFever()
    {
        // Fever »ý¼º
    }

    public void AddMap()
    {
        ResetObject();
        AddObj();
    }

    private void AddObj()
    {
        int endSummonY = _summonY - 40;

        AddLastMapObjs();

        while (_summonY > endSummonY)
        {
            int summonObjIndex = Random.Range(0, _objs.Length);
            SummonObject(_objs[summonObjIndex]);

            _summonY -= Random.Range(_objMinSpace, _objMaxSpace);
        }

        
    }

    private void AddLastMapObjs()
    {
        if (_mapObj.Count == 0) return;
        for(int i = 0; i < _mapObj.Count; i++)
        {
            _lastMapObj.Add(_mapObj[i]);
        }
        _mapObj.Clear();
    }

    public bool ObjSummonCheck(int score)
    {
        return (-score <= _summonY + 5);
    }

    private void ResetObject()
    {
        for(int i = 0; i < _lastMapObj.Count; i++)
        {
            PoolManager.Instance.Push(_lastMapObj[i]);
        }
        _lastMapObj.Clear();
    }

    public void ResetGame()
    {
        ResetObject();
        AddLastMapObjs();
        ResetObject();
        _summonY = -5;
    }

    private void SummonObject(SummonObj obj)
    {
        Vector2 summonPos = Vector2.zero;

        summonPos.x = Random.Range(obj.minXPos, obj.maxXPos);
        summonPos.y = _summonY;

        PoolableMono summonObject = PoolManager.Instance.Pop(obj.summonObj.name);
        summonObject.transform.position = summonPos;

        _mapObj.Add(summonObject);
    }
}
