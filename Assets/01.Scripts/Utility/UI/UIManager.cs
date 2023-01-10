using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class UIManager : IManager
{   
    private List<UIScreen> uIScreens = new List<UIScreen>();

    private IObservable<bool> _inputESCKeyStream;
    
    public UIManager(){
        uIScreens.Add(GameObject.Find("Standby Screen").GetComponent<UIScreen>());
        uIScreens.Add(GameObject.Find("InGame Screen").GetComponent<UIScreen>());
        uIScreens.Add(GameObject.Find("Result Screen").GetComponent<UIScreen>());
        //uIScreens.Add(GameObject.Find("Setting Screen").GetComponent<UIScreen>());
    }

    public void UpdateState(GameState state)
    {
        switch(state){
            case GameState.INIT:
                Init();
                CloseAllScreen();
                InitAllScreen();
                break;
            default:
                ActiveScreen(state);
                break;
        }
    }

    public void Init(){
        _inputESCKeyStream = Observable.EveryUpdate().Select(input => Input.GetKeyDown(KeyCode.Escape));
    }

    private void ActiveScreen(GameState state){
        CloseAllScreen();

        foreach(var screen in uIScreens.Where(screen => screen.state == state)){
            screen.UpdateScreenState(true);
        }
    }

    private void CloseAllScreen(){
        foreach(var screen in uIScreens){
            screen.UpdateScreenState(false);
        }
    }

    private void InitAllScreen(){
        foreach(var screen in uIScreens){
            screen.Init();
        }
    }

    public void ESCSubscribe(Action<bool> action){
        _inputESCKeyStream.Subscribe(action).AddTo(GameManager.Instance);
    }
}
