using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;
using DG.Tweening;

public class PlayerManager : IManager
{
    private Player _player;
    private IObservable<Vector3> playerPosStream;
    private IObservable<List<bool>> playerFeverStream;

    float _minSpeed = 2f;
    float _maxSpeed = 7f;
    float _maxSpeedScore = 500;

    private readonly List<IPlayerComponent> components = new List<IPlayerComponent>();

    public void UpdateState(GameState state)
    {
        switch(state){
            case GameState.INIT:
                Init();
                break;
            case GameState.STANDBY:
                _player.ResetPlayer();
                _player.transform.position = _player.DefaultPlayerPos + Vector2.up * 3f;
                _player.transform.DOMove(_player.DefaultPlayerPos, 1f).SetEase(Ease.InOutQuad).SetUpdate(true);
                break;
            case GameState.INGAME:
                Observable.Timer(TimeSpan.FromSeconds(0.3f)).Subscribe(timer => {
                    _player.IsPlay = true;
                });
                _player.ResetPlayer();
                GameManager.Instance.Stop = false;
                GameManager.Instance.GetManager<CameraManager>().CamSizeSubscribe(PlayerMoveLimit);
                GameManager.Instance.GetManager<ScoreManager>().ScoreSubscribe(ScoreSetSpeed);
                break;
        }

        foreach(var component in components){
            component.UpdateState(state);
        }
    }

    public void Init(){
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        playerFeverStream = Observable.EveryUpdate().Where(condition => GameManager.Instance.State == GameState.INGAME).Select(list => _player.Fevers);
        playerPosStream = Observable.EveryUpdate().Where(condition => GameManager.Instance.State == GameState.INGAME && !_player.IsDie && _player.IsPlay).Select(position => _player.transform.position);
        
        _player.IsPlay = false;
        _player.Fevers = Enumerable.Repeat(false, 5).ToList();

        components.Add(new PlayerColliderComponent(_player));
    }


    public Vector2 GetDefaultPlayerPos => _player.DefaultPlayerPos;
    private void PlayerMoveLimit(float camSize){
        _player.transform.position = new Vector3(Mathf.Clamp(_player.transform.position.x, -camSize + .2f, camSize - .2f), _player.transform.position.y);
    }

    public void PlayerFeverSubscribe(Action<List<bool>> action){
        playerFeverStream.Subscribe(action).AddTo(GameManager.Instance);
    }

    private void ScoreSetSpeed(int score)
    {
        if (_player.IsFever) return;

        if (score == 0)
        {
            _player.FallingSpeed = _minSpeed;
            return;
        }
    
        _player.FallingSpeed = Mathf.Lerp(_minSpeed, _maxSpeed, score / _maxSpeedScore);
    }

    public void PlayerPosSubscribe(Action<Vector3> action){
        playerPosStream.Subscribe(action).AddTo(GameManager.Instance);
    }
}
