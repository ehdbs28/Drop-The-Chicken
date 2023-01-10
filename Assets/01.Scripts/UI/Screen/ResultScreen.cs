using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultScreen : UIScreen
{
    [SerializeField] private Button tapToRestart;
    [SerializeField] private Button tapToMainmenu;

    public override void Init()
    {
        tapToRestart.onClick.AddListener(() => {
            GameManager.Instance.UpdateState(GameState.INGAME);
        });

        tapToMainmenu.onClick.AddListener(() => {
            GameManager.Instance.UpdateState(GameState.INIT);
        });

        base.Init();
    }
}
