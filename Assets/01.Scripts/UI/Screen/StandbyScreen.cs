using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StandbyScreen : UIScreen
{
    [SerializeField] private Button tapToStart;
    [SerializeField] private Button tapToSetting;
    [SerializeField] private Button tapToExit;

    public override void Init()
    {
        tapToStart.onClick.AddListener(() => {
            GameManager.Instance.UpdateState(GameState.INGAME);
        });

        tapToSetting.onClick.AddListener(() => {
            GameManager.Instance.GetManager<ESCManager>().IsOpenSetting = true;
        });

        tapToExit.onClick.AddListener(() => {
            Application.Quit();
        });

        base.Init();
    }
}
