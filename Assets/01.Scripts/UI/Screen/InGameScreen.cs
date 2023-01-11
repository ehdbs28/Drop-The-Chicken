using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class InGameScreen : UIScreen
{
    [SerializeField] private Button tapToSetting;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI newText;

    [SerializeField] private List<TextMeshProUGUI> feverTexts = new List<TextMeshProUGUI>();

    public override void Init()
    {
        newText.gameObject.SetActive(false);

        GameManager.Instance.GetManager<ScoreManager>().ScoreSubscribe(UpScoreEvent);
        GameManager.Instance.GetManager<PlayerManager>().PlayerFeverSubscribe(FeverTextEvent);

        tapToSetting.onClick.AddListener(() => {
            GameManager.Instance.GetManager<ESCManager>().IsOpenSetting = true;
        });

        base.Init();
    }

    private void UpScoreEvent(int score){
        scoreText.text = score.ToString("D5");

        BestScoreCheck();
    }

    private void BestScoreCheck()
    {
        bool check = (GameManager.Instance.GetManager<ScoreManager>().IsCurrentScoreBest);
        newText.gameObject.SetActive(check);
    }

    private void FeverTextEvent(List<bool> feverList){
        for(int index = 0; index < feverList.Count; index++){
            feverTexts[index].color = new Color(
                feverTexts[index].color.r,
                feverTexts[index].color.g,
                feverTexts[index].color.b,
                (feverList[index] ? 1f : 0.3f )
            );
        }
    }
}
