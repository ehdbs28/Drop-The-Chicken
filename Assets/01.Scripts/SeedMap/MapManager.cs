using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

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
            maps[i].transform.position = new Vector2(0, -5f + (i * -10));
            maps[i].ResetMap();
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
