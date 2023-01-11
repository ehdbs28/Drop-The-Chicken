using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ESCManager : IManager
{
    private UIScreen _settingScreen;
    private bool _isOpenSetting = false;
    public bool IsOpenSetting { get => _isOpenSetting; set {
        _isOpenSetting = value;
        _settingScreen.UpdateScreenState(_isOpenSetting);
        GameManager.Instance.GameStop = _isOpenSetting;
    }}

    public void UpdateState(GameState state)
    {
        switch(state){
            case GameState.INIT:
                Init();
                break;
        }
    }

    public void Init(){
        _settingScreen = GameObject.Find("Setting Screen").GetComponent<UIScreen>();
        _settingScreen.Init();
    }
}
