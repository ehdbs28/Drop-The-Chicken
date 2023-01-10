using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InGameScreen : UIScreen
{
    [SerializeField] private Button tapToSetting;
    [SerializeField] private TextMeshProUGUI scoreText;

    public override void Init()
    {
        GameManager.Instance.GetManager<ScoreManager>().ScoreSubscribe(UpScoreEvent);

        tapToSetting.onClick.AddListener(() => {
            GameManager.Instance.GetManager<ESCManager>().IsOpenSetting = true;
        });

        base.Init();
    }

    private void UpScoreEvent(int score){
        scoreText.text = score.ToString("D5");
    }
}
