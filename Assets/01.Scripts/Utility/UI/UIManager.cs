using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIManager : IManager
{   
    private List<UIScreen> uIScreens = new List<UIScreen>();
    
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
}
