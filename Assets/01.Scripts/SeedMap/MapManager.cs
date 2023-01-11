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
        maps[0].transform.position = new Vector2(0, -5f);
        maps[0].ResetMap();

        maps[1].transform.position = new Vector2(0, -15f);
        maps[1].ResetMap();
    }
}
