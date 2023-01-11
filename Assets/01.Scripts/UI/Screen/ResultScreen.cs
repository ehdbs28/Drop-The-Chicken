using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultScreen : UIScreen
{
    [SerializeField] private Button tapToRestart;
    [SerializeField] private Button tapToMainmenu;

    [SerializeField] private TextMeshProUGUI currentScore;
    [SerializeField] private TextMeshProUGUI bestScore;

    public override void UpdateScreenState(bool open)
    {
        GameManager.Instance.GetManager<ScoreManager>().ScoreSubscribe(score => currentScore.text = score.ToString("D5"));
        bestScore.text = GameManager.Instance.GetManager<DataManager>().User.BestScore.ToString("D5");

        base.UpdateScreenState(open);
    }

    public override void Init()
    {
        tapToRestart.onClick.AddListener(() => {
            GameManager.Instance.UpdateState(GameState.INGAME);
        });

        tapToMainmenu.onClick.AddListener(() => {
            GameManager.Instance.UpdateState(GameState.STANDBY);
        });

        base.Init();
    }
}
