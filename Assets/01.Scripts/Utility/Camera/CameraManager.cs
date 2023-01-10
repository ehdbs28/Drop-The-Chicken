using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class CameraManager : IManager
{
    private Camera mainCam;
    
    private Vector2 camLimit; 
    private Vector2 camOffset;

    private IObservable<float> cameraSizeStream;

    public void UpdateState(GameState state)
    {
        switch(state){
            case GameState.INIT:
                Init();
                break;
        }
    }

    private void Init(){
        mainCam = Camera.main;
        camLimit = new Vector2(-0.2f, 0.2f);
        camOffset = new Vector2(0, -2.5f);

        cameraSizeStream = Observable.EveryUpdate().Where(condition => GameManager.Instance.State == GameState.INGAME).Select(size => mainCam.orthographicSize * mainCam.aspect);

        GameManager.Instance.GetManager<PlayerManager>().PlayerPosSubscribe(PlayerMoveEvent);
    }

    private void PlayerMoveEvent(Vector3 playerPosition){
        float x = playerPosition.x + camOffset.x;
        float y = playerPosition.y + camOffset.y;

        x = Mathf.Clamp(x, camLimit.x, camLimit.y);

        Vector3 movePos = new Vector3(x, y, -10f);

        mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, movePos, Time.deltaTime);
    }

    public void CamSizeSubscribe(Action<float> action){
        cameraSizeStream.Subscribe(action);
    }
}
