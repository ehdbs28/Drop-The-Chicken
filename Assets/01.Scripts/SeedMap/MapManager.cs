using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class MapManager : IManager
{
    private RandomMap[] maps;


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
                GameManager.Instance.GetManager<ScoreManager>().ScoreSubscribe(ScoreDifficult);
                ResetMap();
                break;
        }
    }
    public void Init()
    {
        maps = GameObject.Find("MapManager").GetComponentsInChildren<RandomMap>();
    }
    
    private void ResetMap()
    {
        for(int i = 0; i < maps.Length; i++)
        {
            maps[i].ResetMap();
            maps[i].transform.position = new Vector2(0, -5f + (i * -10));
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
