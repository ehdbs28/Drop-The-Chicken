using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class MapManager : IManager
{
    private MapSystem map;
    private Transform _maxScorePos;
    public Transform MaxScorePos
    {
        get { return _maxScorePos; }
        set { _maxScorePos = value; }
    }

    public void UpdateState(GameState state)
    {
        switch (state)
        {
            case GameState.INIT:
                Init();
                break;
            case GameState.STANDBY:
                ClearMap();
                break;
            case GameState.INGAME:
                GameManager.Instance.GetManager<ScoreManager>().ScoreSubscribe(MapUpdate);
                ResetMap();
                break;
        }
    }
    public void Init()
    {
        map = GameObject.Find("MapManager").GetComponent<MapSystem>();
        _maxScorePos = GameObject.Find("MaxScorePos").transform;
    }
    
    private void ResetMap()
    {
        if(GameManager.Instance.GetManager<ScoreManager>().BestScore != 0)
            _maxScorePos.position = GameManager.Instance.GetManager<PlayerManager>().GetDefaultPlayerPos +
                (GameManager.Instance.GetManager<ScoreManager>().BestScore * Vector2.down);

        map.ResetMap();
    }

    private void ClearMap(){
        map.ResetGame();
    }

    private void MapUpdate(int score)
    {
        if(map.ObjSummonCheck(score))
        {
            map.AddMap();
        }
    }

}
