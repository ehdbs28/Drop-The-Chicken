using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SummonObj
{
    public PoolableMono summonObj;
    public float minXPos = -2;
    public float maxXPos = 2;
}

public class MapSystem : MonoBehaviour
{
    private List<PoolableMono> _mapObj = new List<PoolableMono>();
    private List<PoolableMono> _lastMapObj = new List<PoolableMono>();

    private Player _player;
    private int _summonY = 0;

    #region �� ������Ʈ

    [SerializeField]
    private FeverObj[] _feverObjs;
    private float _feverSpawnPer = 0.2f; // 20��

    [SerializeField]
    private SummonObj[] _objs;

    private int _objMinSpace = 2;
    private int _objMaxSpace = 4;

    private int _summonSpace = 50;
    #endregion

    #region ��, �ٶ�
    // �ٶ��� �� ���� ����
    // ���� ���� ���� ����

    private int _dragonSpace = 0;
    private int _windSpace = 0;

    private int _gimmickMinSpace = 80;
    private int _gimmickMaxSpace = 150;

    #endregion

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
        //��� fever������Ʈ�� true�� ��� ���� �ȵǵ����ϱ�
        if (_player.IsFever) return;

        //��ȯ Ȯ�� üũ
        float spawnPer = Random.Range(0f, 1f);
        if (spawnPer > _feverSpawnPer) return;

        float minX = -2;
        float maxX = 2;

        int randomIndex = Random.Range(0, _feverObjs.Length);
        // Fever ��ġ�� �ʰ� üũ
        while(FeverObjCheck(_player.Fevers, randomIndex))
        {
            randomIndex = Random.Range(0, _feverObjs.Length);
        }
        // Fever �߿� ä������ ���� ���� �ϳ� ����
        Vector2 summonPos = new Vector2(Random.Range(minX, maxX), _summonY);
        PoolableMono summonFeverObject = PoolManager.Instance.Pop(_feverObjs[randomIndex].name);

        summonFeverObject.transform.position = summonPos;
        _mapObj.Add(summonFeverObject);

        _summonY -= _objMinSpace;
    }

    private bool FeverObjCheck(List<bool> fevers, int index)
    {
        return fevers[index];
    }

    public void AddMap()
    {
        ResetObject();
        AddObj();
    }

    private void AddObj()
    {
        int endSummonY = _summonY - _summonSpace;

        AddLastMapObjs();

        while (_summonY > endSummonY)
        {
            AddFever();

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

    public bool ObjSummonCheck(Vector3 pos)
    {
        return (pos.y <= _summonY + 10);
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
        _summonY = 0;
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
