using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class CameraManager : IManager
{
    private Camera mainCam;
    
    private Vector2 camOffset;

    private Vector3 defaultCamPos;

    private IObservable<float> cameraSizeStream;

    public void UpdateState(GameState state)
    {
        switch(state){
            case GameState.INIT:
                Init();
                break;
            case GameState.INGAME:
                ResetCam();
                break;
        }
    }

    private void ResetCam()
    {
        mainCam.transform.position = defaultCamPos;
    }

    private void Init(){
        mainCam = Camera.main;
        camOffset = new Vector2(0, -1.5f);
        defaultCamPos = new Vector3(0, 0, -10f);

        cameraSizeStream = Observable.EveryUpdate().Where(condition => GameManager.Instance.State == GameState.INGAME).Select(size => mainCam.orthographicSize * mainCam.aspect);

        GameManager.Instance.GetManager<PlayerManager>().PlayerPosSubscribe(PlayerMoveEvent);
    }

    private void PlayerMoveEvent(Vector3 playerPosition){
        float x = 0;
        float y = playerPosition.y + camOffset.y;

        Vector3 movePos = new Vector3(x, y, -10f);

        mainCam.transform.position = movePos;
    }

    public void CamSizeSubscribe(Action<float> action){
        cameraSizeStream.Subscribe(action);
    }
}
