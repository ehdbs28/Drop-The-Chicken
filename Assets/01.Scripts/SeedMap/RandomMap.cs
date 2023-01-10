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
    private float _stormZonePer = 0.1f; // 10퍼센트

    [SerializeField]
    private StormZone storm;
    private bool onStorm;

    [SerializeField]
    private SummonObj[] seeds;
    private List<int> yThereIsObj = new List<int>();

    private int _resetWorldMoveY = -20;

    private void Start()
    {
        ResetSeeds();
    }

    private void ResetMap()
    {
        transform.position = transform.position + new Vector3(0, _resetWorldMoveY, 0);
        ResetSeeds();
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
    // seed대로 맵을 생성합니다.
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
    }
    private void OnTriggerExit2D(Collider2D obj)
    {
        if (obj.CompareTag("Player"))
        {
            Debug.Log("감진됨");
            ResetMap();
        }
    }
}
