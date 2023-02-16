using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SummonObj
{
    public PoolableMono summonObj;
    public Transform minPos;
    public Transform maxPos;
}

public class RandomMap : MonoBehaviour
{
    [SerializeField]
    private GameObject _platform;

    private int _platformMaxPosX = 2;
    private int _platformMinPosX = -2;

    [SerializeField]
    private FeverObj[] feverObjs;
    private Player _player;

    [SerializeField]
    private bool thirdMap;

    private List<int> yThereIsObj = new List<int>(); // 이 y값에 오브젝트가 있는가

    [SerializeField]
    private SummonObj[] objs;
    private List<PoolableMono> mapObj = new List<PoolableMono>(); // 생성된 오브젝트들

    private int _resetWorldMoveY = -30;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public void ClearMap(){
        ResetObject();
    } // 맵 지우기
    public void ResetMap()
    {
        ResetObject();
        SetMap();
    } // 맵 지우고 다시 생성
    private void AddMap()
    {
        transform.position = transform.position + new Vector3(0, _resetWorldMoveY, 0);
        ResetMap();
        AddFever();
    } // 맵 위치 옮기고 지우고 다시 생성

    private void AddFever()
    {
        if(thirdMap)
        {
            int maxX = 2;
            int minX = -2;
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
    } // FeverObj생성

    private void ResetObject() // 전에 생성된 오브젝트를 지워준다.
    {
        for(int i = 0; i < mapObj.Count; i++)
        {
            PoolManager.Instance.Push(mapObj[i]);
        }
        mapObj.Clear();
        yThereIsObj.Clear();
    }
    private void SetMap() // 맵을 생성한다.
    {
        // 플랫폼 위치를 좌우에서 옮겨준다.

        // 플랫폼과 플랫폼이 다시 생성되는 틈 사이를 2~3개에 오브젝트로 채워준다.
        int objCount = Random.Range(2, 4);

        for(int i=0; i<objCount; i++)
        {
            int randomIndex = Random.Range(0, objs.Length);
            SummonObject(objs[randomIndex]);
        }
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

            summonPos.y = Random.Range((int)obj.minPos.position.y, (int)obj.maxPos.position.y+1);
            if (!yThereIsObj.Contains((int)summonPos.y)) break;
        }
        yThereIsObj.Add((int)summonPos.y);

        PoolableMono summonObject = PoolManager.Instance.Pop(obj.summonObj.name);
        summonObject.transform.position = summonPos;

        mapObj.Add(summonObject);
    }
    
    private void OnTriggerExit2D(Collider2D obj)
    {
        if (obj.CompareTag("Player"))
        {
            AddMap();
        }
    }
}
