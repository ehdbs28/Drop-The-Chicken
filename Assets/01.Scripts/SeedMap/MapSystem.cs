using System.Collections;
using System.Collections.Generic;
using UniRx.Triggers;
using UnityEngine;

[System.Serializable]
public class SummonObj
{
    public PoolableMono summonObj;
    public float minXPos = -2;
    public float maxXPos = 2;
    //확률 제작하기
    public int itemCount = 0;
}

[System.Serializable]
public class GimmickInfo
{
    public EGimmickType Type;
    public PoolableMono LastSpawnObj;
    public float NextSummonY { get; set; }  
}

public class MapSystem : MonoBehaviour
{
    private List<PoolableMono> _mapObj = new List<PoolableMono>();
    private List<PoolableMono> _lastMapObj = new List<PoolableMono>();

    private Player _player;
    private int _summonY = 0;

    #region gimmicks
    // �ٶ��� �� ���� ����
    // ���� ���� ���� ����

    [SerializeField]
    private SpawnGimmickListSO _gimmickListSO;
    private List<GimmickInfo> _spawnGimmickInfoList;
    
    [SerializeField]
    private PoolableMono _dragon;
    [SerializeField]
    private PoolableMono _wind;

    private PoolableMono _lastSpawnDragon = null;
    private PoolableMono _lastSpawnWind = null;

    private float _dragonNextSummonY = 0;
    private float _windNextSummonY = 0;

    private int _gimmickMinSpace = 80;
    private int _gimmickMaxSpace = 150;

    #endregion

    #region mapObjs

    List<SummonObj> _mapSeed = new List<SummonObj>();

    [SerializeField]
    private FeverObj[] _feverObjs;
    private float _feverSpawnPer = 0.2f; // 20��

    [SerializeField]
    private SummonObj[] _objs;

    private int _objMinSpace = 2;
    private int _objMaxSpace = 4;

    private int _summonSpace = 50;
    #endregion

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void Start()
    {
        FillSpawnDictionary();
    }

    private void FillSpawnDictionary()
    {
        _spawnGimmickInfoList = new List<GimmickInfo>();
        _gimmickListSO.GimmickList.ForEach((gimmick) =>
        {
            GimmickInfo info = new GimmickInfo
            {
                Type = gimmick.GimmickType,
                LastSpawnObj = null,
                NextSummonY = 0
            };

            _spawnGimmickInfoList.Add(info);
        });
    }

    public void ResetMap()
    {
        ResetGame();
        AddObj();
    }

    public void ResetGimmick()
    {
        
        foreach(var gimmickInfo in _spawnGimmickInfoList)
        {
            if(gimmickInfo.LastSpawnObj != null)
            {
                PoolManager.Instance.Push(gimmickInfo.LastSpawnObj);
                gimmickInfo.NextSummonY = -Random.Range(_gimmickMinSpace, _gimmickMaxSpace);
            }
        }
        
        //if (_lastSpawnDragon != null)
        //    PoolManager.Instance.Push(_lastSpawnDragon);
        //if(_lastSpawnWind != null)
        //    PoolManager.Instance.Push(_lastSpawnWind);

        //_dragonNextSummonY = -Random.Range(_gimmickMinSpace, _gimmickMaxSpace);
    }

    public void GimmickSpawn(Vector3 playerPos)
    {
        foreach (var gimmickInfo in _spawnGimmickInfoList)
        {
            if(playerPos.y <= gimmickInfo.NextSummonY)
            {
                if (gimmickInfo.LastSpawnObj != null)
                    PoolManager.Instance.Push(gimmickInfo.LastSpawnObj);

                string gimmickName = _gimmickListSO.GetGimmickObjectName(gimmickInfo.Type);
                if(gimmickName != null)
                {
                    Gimmick gimmick = PoolManager.Instance.Pop(gimmickName) as Gimmick;
                    gimmickInfo.LastSpawnObj = gimmick;
                    gimmick.Spawn(gimmickInfo.NextSummonY); // 스폰해줌.
                    
                    int gimmickSpace = Random.Range(_gimmickMinSpace, _gimmickMaxSpace);
                    float NextSummonY = playerPos.y - gimmickSpace;
                    gimmickInfo.NextSummonY = NextSummonY; // 다음 생성 위치 재설정
                }
                else
                {
                    Debug.LogWarning($"{gimmickInfo.Type}가 SO에 들어있지 않습니다.");
                }

            }
                
            //if (gimmickInfo.LastSpawnObj != null)
            //{
            //    PoolManager.Instance.Push(gimmickInfo.LastSpawnObj);
            //    gimmickInfo.NextSummonY = -Random.Range(_gimmickMinSpace, _gimmickMaxSpace);
            //}
        }

        if (playerPos.y <= _dragonNextSummonY)
        {
            //delete lastSpawnDragon
            if (_lastSpawnDragon != null)
                PoolManager.Instance.Push(_lastSpawnDragon);

            PoolableMono dragonObj = PoolManager.Instance.Pop(_dragon.name);
            _lastSpawnDragon = dragonObj;

            float x = Random.Range(-2f, 2f);
            dragonObj.transform.position = new Vector2(x, _dragonNextSummonY - 100);

            int dragonSpace = Random.Range(_gimmickMinSpace, _gimmickMaxSpace);
            _dragonNextSummonY = playerPos.y - dragonSpace;
        }
        
        if(playerPos.y <= _windNextSummonY - 10)
        {
            //delete lastSpawnWind
            if (_lastSpawnWind != null)
                PoolManager.Instance.Push(_lastSpawnWind);

            int windSpace = Random.Range(_gimmickMinSpace, _gimmickMaxSpace);
            _windNextSummonY = playerPos.y - windSpace;

            PoolableMono windObj = PoolManager.Instance.Pop(_wind.name);
            _lastSpawnWind = windObj;
            windObj.transform.position = new Vector2(0, _windNextSummonY);

        }

    }

    private void AddFever()
    {
        //player isFever check
        if (_player.IsFever) return;

        //spawnPer check
        float spawnPer = Random.Range(0f, 1f);
        if (spawnPer > _feverSpawnPer) return;

        float minX = -2;
        float maxX = 2;

        int randomIndex = Random.Range(0, _feverObjs.Length);
        // FeverObj true, false check
        while(FeverObjCheck(_player.Fevers, randomIndex))
        {
            randomIndex = Random.Range(0, _feverObjs.Length);
        }
        // Fever Spawn
        Vector2 summonPos = new Vector2(Random.Range(minX, maxX), _summonY);
        PoolableMono summonFeverObject = PoolManager.Instance.Pop(_feverObjs[randomIndex].name);

        summonFeverObject.transform.position = summonPos;
        _mapObj.Add(summonFeverObject);

        _summonY -= _objMinSpace;
    }

    private void AddEffect(){
        Vector2 summonPos = new Vector2(0, _summonY);
        PoolableMono skyBG = PoolManager.Instance.Pop("CloudBackGround");
        PoolingParticle lightParticle = PoolManager.Instance.Pop("LightParticle") as PoolingParticle;

        skyBG.transform.position = summonPos;
        lightParticle.SetPosition(summonPos);
        lightParticle.Play();

        _mapObj.Add(skyBG);

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
            AddEffect();

            SummonObject(MapObjsPop());
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
        ResetGimmick();

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

    private SummonObj MapObjsPop()
    {
        if (_mapSeed.Count == 0) MapObjsSetting();

        SummonObj summonObj = _mapSeed[0];
        _mapSeed.RemoveAt(0);
        return summonObj;
    }

    private void MapObjsSetting()
    {
        _mapSeed.Clear();

        foreach(SummonObj obj in _objs)
        {
            for(int i = 0; i < obj.itemCount; i++)
            {
                _mapSeed.Add(obj);
            }
        }

        //Shuffle
        for(int i = 0; i < _mapSeed.Count; i++) {
            int randomIndex = Random.Range(i, this._mapSeed.Count);

            SummonObj temp = _mapSeed[i];
            _mapSeed[i] = _mapSeed[randomIndex];
            _mapSeed[randomIndex] = temp;
        }
    }
}
