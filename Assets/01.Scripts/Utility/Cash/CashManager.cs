using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashManager : IManager
{
    public int Cash { get; set; }
    public void UpdateState(GameState state)
    {
        switch (state)
        {
            case GameState.INIT:
                Init();
                break;
            case GameState.RESULT:
                SaveCash();
                break;
        }
    }


    private void Init()
    {
        Cash = GameManager.Instance.GetManager<DataManager>().User.Cash;
    }

    private void SaveCash()
    {
        GameManager.Instance.GetManager<DataManager>().User.Cash = Cash;
    }

    public void AddCash(int cash) => Cash += cash;
    public bool UseCash(int use)
    {
        if(Cash - use < 0)
        {
            Debug.Log("Cash가 부족합니다.");
            return false;
        }
        else
        {
            Cash -= use;
            return true;
        }
    }
}
