using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;
using DG.Tweening;

public class InGameScreen : UIScreen
{
    [SerializeField] private Button tapToSetting;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI newText;

    [SerializeField] private List<TextMeshProUGUI> feverTexts = new List<TextMeshProUGUI>();
    [SerializeField] private Image feverImage;

    private float _feverRunTime = 0f;

    public override void Init()
    {
        newText.gameObject.SetActive(false);

        GameManager.Instance.GetManager<ScoreManager>().ScoreSubscribe(UpScoreEvent);
        GameManager.Instance.GetManager<PlayerManager>().PlayerFeverSubscribe(FeverTextEvent);
        GameManager.Instance.GetManager<PlayerManager>().PlayerFeverSubscribe(FeverImageEvent);

        tapToSetting.onClick.AddListener(() => {
            ButtonClickSound();
            GameManager.Instance.GetManager<ESCManager>().IsOpenSetting = true;
        });

        base.Init();
    }

    public override void UpdateScreenState(bool open)
    {


        base.UpdateScreenState(open);
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
        bool fever = (from value in feverList where value == false select value).Count() == 0;

        for(int index = 0; index < feverList.Count; index++){
            feverTexts[index].color = new Color(
                feverTexts[index].color.r,
                feverTexts[index].color.g,
                feverTexts[index].color.b,
                ((feverList[index] && !fever) ? 1f : 0.3f )
            );
        }
    }

    private void FeverImageEvent(List<bool> feverList){
        bool fever = (from value in feverList where value == false select value).Count() == 0;

        feverImage.enabled = fever;
        if(fever){
            _feverRunTime += Time.deltaTime;
            feverImage.fillAmount = Mathf.Lerp(1f, 0f, _feverRunTime / 5.7f);
        }
        else{
            _feverRunTime = 0;
            feverImage.fillAmount = 1;
        }
    }
}
