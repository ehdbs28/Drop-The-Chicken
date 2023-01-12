using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class MapManager : IManager
{
    private RandomMap[] maps;
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
                ResetMap();
                GameManager.Instance.GetManager<ScoreManager>().ScoreSubscribe(ScoreDifficult);
                break;
        }
    }
    public void Init()
    {
        maps = GameObject.Find("MapManager").GetComponentsInChildren<RandomMap>();
        _maxScorePos = GameObject.Find("MaxScorePos").transform;
    }
    
    private void ResetMap()
    {
        if(GameManager.Instance.GetManager<ScoreManager>().BestScore != 0)
            _maxScorePos.position = GameManager.Instance.GetManager<PlayerManager>().GetDefaultPlayerPos +
                (GameManager.Instance.GetManager<ScoreManager>().BestScore * Vector2.down);

        for(int i = 0; i < maps.Length; i++)
        {
            maps[i].transform.position = new Vector2(0, -5f + (i * -10));
            maps[i].ResetMap();
        }
        
    }

    private void ClearMap(){
        foreach(RandomMap map in maps){
            map.ClearMap();
        }
    }

    private void ScoreDifficult(int score)
    {
        foreach (RandomMap map in maps)
        {
            map.DifficultUp(score);
        }
    }
}
