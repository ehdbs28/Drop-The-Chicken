using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class ScoreManager : IManager
{
    private int _currentScore = 0;
    private int _plusScore = 0;
    private int _bestScore {
        get => GameManager.Instance.GetManager<DataManager>().User.BestScore; 
        set => GameManager.Instance.GetManager<DataManager>().User.BestScore = value;
    }
    public bool IsCurrentScoreBest => (_bestScore <= _currentScore);
    public int BestScore => _bestScore;

    private Vector3 playerStartPos = Vector3.zero;

    private IObservable<int> upScoreStream;

    public void UpdateState(GameState state)
    {
        if(_bestScore >= 300) GameManager.Instance.GetManager<DataManager>().User.KingUnlock = true;
        if(_bestScore >= 500) GameManager.Instance.GetManager<DataManager>().User.RobotUnlock = true;

        switch(state){
            case GameState.INIT:
                Init();
                break;
            case GameState.INGAME:
                if(!GameManager.Instance.IsRevibe) _plusScore = 0;
                Debug.Log(_plusScore);
                break;
        }
    }

    private void Init(){
        upScoreStream = Observable.EveryUpdate().Where(condition => GameManager.Instance.State == GameState.INGAME).Select(score => _currentScore);

        GameManager.Instance.GetManager<PlayerManager>().PlayerPosSubscribe(PlayerMoveEvent);
    }

    private void PlayerMoveEvent(Vector3 playerPos){
        if(playerStartPos == Vector3.zero) playerStartPos = playerPos;

        _currentScore = (int)Mathf.Abs(playerStartPos.y - playerPos.y) + _plusScore;

        if(_currentScore > _bestScore){
            _bestScore = _currentScore;
        }
    }

    public void ScoreSubscribe(Action<int> action){
        upScoreStream.Subscribe(action);
    }

    public void PlusScore(int plus)
    {
        _plusScore += plus;
        GameManager.Instance.GetManager<MapManager>().MaxScorePos.position += Vector3.up * plus;
    }
}
