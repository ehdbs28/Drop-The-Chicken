using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using DG.Tweening;

public class UIManager : IManager
{   
    private List<UIScreen> uIScreens = new List<UIScreen>();
    private RectTransform screenTransition;
    
    public UIManager(){
        uIScreens.Add(GameObject.Find("Standby Screen").GetComponent<UIScreen>());
        uIScreens.Add(GameObject.Find("InGame Screen").GetComponent<UIScreen>());
        uIScreens.Add(GameObject.Find("Result Screen").GetComponent<UIScreen>());
        //uIScreens.Add(GameObject.Find("Setting Screen").GetComponent<UIScreen>());

        screenTransition = GameObject.Find("Transition Panel").GetComponent<RectTransform>();
    }

    public void UpdateState(GameState state)
    {
        switch(state){
            case GameState.INIT:
                CloseAllScreen();
                InitAllScreen();
                break;
            default:
                ActiveScreen(state);
                break;
        }
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

    public void Transition(Action screenChange){
        Sequence sq = DOTween.Sequence().SetUpdate(true);

        sq.Append(screenTransition.DOAnchorPosY(0, 1f).SetEase(Ease.OutQuad));
        sq.AppendInterval(0.5f);
        sq.AppendCallback(() => screenChange?.Invoke());
        sq.Append(screenTransition.DOAnchorPosY(1980, 1f).SetEase(Ease.OutQuad));
        sq.OnComplete(() => {
            sq.Kill();
        });
    }
}
