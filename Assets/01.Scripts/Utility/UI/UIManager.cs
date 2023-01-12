using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using DG.Tweening;

public class UIManager : MonoBehaviour, IManager
{   
    [SerializeField] private AudioClip _btnClickClip;
    private List<UIScreen> uIScreens = new List<UIScreen>();
    private RectTransform screenTransition;
    
    public void Init(){
        uIScreens.Add(GameObject.Find("Canvas/SkinSelect Panel").GetComponent<UIScreen>());
        uIScreens.Add(GameObject.Find("Canvas/Standby Screen").GetComponent<UIScreen>());
        uIScreens.Add(GameObject.Find("Canvas/InGame Screen").GetComponent<UIScreen>());
        uIScreens.Add(GameObject.Find("Canvas/Result Screen").GetComponent<UIScreen>());
        //uIScreens.Add(GameObject.Find("Setting Screen").GetComponent<UIScreen>());

        screenTransition = GameObject.Find("Canvas/Transition Panel").GetComponent<RectTransform>();
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

    public void ButtonClickSound(){
        GameManager.Instance.GetManager<AudioManager>().PlayOneShot(_btnClickClip);
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

    public T GetScreen<T>() where T : UIScreen{
        T value = default(T);

        foreach(var screen in uIScreens.OfType<T>()){
            value = screen;
        }

        return value;
    }
}
