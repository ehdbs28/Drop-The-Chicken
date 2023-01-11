using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SummonObj
{
    public PoolableMono summonObj;
    public Transform minPos;
    public Transform maxPos;
    public int SummonCount;
    public int CountRandomRange;
}

public class RandomMap : MonoBehaviour
{
    private float _stormZonePer = 0.1f; // 10�ۼ�Ʈ

    [SerializeField]
    private StormZone storm;
    private bool onStorm;

    [SerializeField]
    private FeverObj[] feverObjs;
    private Player _player;

    private int _spawnFeverObjTime = 3;
    private int _feverCount = 0;

    [SerializeField]
    private SummonObj[] seeds;
    private List<int> yThereIsObj = new List<int>();

    private List<PoolableMono> mapObj = new List<PoolableMono>();

    private int _resetWorldMoveY = -30;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public void ResetMap()
    {
        gameObject.SetActive(true);
        ResetDifficult();
        ResetObject();
        ResetSeeds();
    }

    public void ClearMap(){
        ResetObject();
        gameObject.SetActive(false);
    }
    
    private void AddMap()
    {
        transform.position = transform.position + new Vector3(0, _resetWorldMoveY, 0);
        ResetSeeds();
        AddFever();
    }

    private void AddFever()
    {
        if(_feverCount >= _spawnFeverObjTime)
        {
            int maxX = 3;
            int minX = -3;
            int X = Random.Range(minX, maxX);

            int maxY = 5;
            int minY = -5;
            int Y = 0;
            while (true)
            {
                if (yThereIsObj.Count > 10) break;

                Y = Random.Range(minY, maxY);
                if (!yThereIsObj.Contains((int)Y)) break;
            }
            _feverCount = 0;
            for (int i = 0; i < _player.Fevers.Count; i++)
            {
                if (!_player.Fevers[i])
                {
                    PoolableMono fever = PoolManager.Instance.Pop(feverObjs[i].name);
                    fever.transform.position = transform.position + new Vector3(X, Y, 0);
                    mapObj.Add(fever);
                    break;
                }
            }
        }
        _feverCount++;
    }

    private void ResetSeeds()
    {
        yThereIsObj.Clear();
        
        for (int i = 0; i < seeds.Length; i++)
        {
            if (seeds[i].CountRandomRange <= 0) continue;

            seeds[i].SummonCount = Random.Range(0, seeds[i].CountRandomRange + 1);
        }
        SetStormZone();
        SettingMap();
    }


    private void SetStormZone()
    {
        onStorm = (Random.Range(0.0f, 1.0f) <= _stormZonePer);
        storm.gameObject.SetActive(onStorm);
        storm.SetDirection((Random.Range(0.0f, 1.0f) <= 0.5f));

    }
    // seed��� ���� �����մϴ�.
    private void SettingMap()
    {
        for (int i = 0; i < seeds.Length; i++)
        {
            if (seeds[i].SummonCount <= 0) continue;
            for (int j = 0; j < seeds[i].SummonCount; j++)
            {
                SummonObject(seeds[i]);
            }
        }
    }
    private void ResetObject()
    {
        for(int i = 0; i < mapObj.Count; i++)
        {
            PoolManager.Instance.Push(mapObj[i]);
        }
        mapObj.Clear();
    }
    private void SummonObject(SummonObj obj)
    {
        Vector2 summonPos = Vector2.zero;

        summonPos.x = Random.Range(obj.minPos.position.x, obj.maxPos.position.x);
        while (true)
        {
            if (yThereIsObj.Count > 10) break;
            if (obj.minPos.position.y == obj.maxPos.position.y)
            {
                summonPos.y = obj.minPos.position.y;
                break;
            }

            summonPos.y = Random.Range((int)obj.minPos.position.y, (int)obj.maxPos.position.y);
            if (!yThereIsObj.Contains((int)summonPos.y)) break;

        }
        yThereIsObj.Add((int)summonPos.y);

        PoolableMono summonObject = PoolManager.Instance.Pop(obj.summonObj.name);
        summonObject.transform.position = summonPos;

        mapObj.Add(summonObject);
    }
    private void ResetDifficult()
    {
        seeds[0].CountRandomRange = 0;
        seeds[1].CountRandomRange = 1;
        seeds[4].CountRandomRange = 2;
    }
    public void DifficultUp(int score)
    {
        if (score >= 100)
        {
            seeds[0].CountRandomRange = 1;
        }
        if (score >= 200)
        {
            seeds[1].CountRandomRange = 2;
            seeds[4].CountRandomRange = 3;
        }
      

    }
    
    private void OnTriggerExit2D(Collider2D obj)
    {
        if (obj.CompareTag("Player"))
        {
            AddMap();
        }
    }
}
