using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InGameScreen : UIScreen
{
    [SerializeField] private Button tapToSetting;
    [SerializeField] private TextMeshProUGUI scoreText;

    [SerializeField] private List<TextMeshProUGUI> feverTexts = new List<TextMeshProUGUI>();

    public override void Init()
    {
        GameManager.Instance.GetManager<ScoreManager>().ScoreSubscribe(UpScoreEvent);
        GameManager.Instance.GetManager<PlayerManager>().PlayerFeverSubscribe(FeverTextEvent);

        tapToSetting.onClick.AddListener(() => {
            GameManager.Instance.GetManager<ESCManager>().IsOpenSetting = true;
        });

        base.Init();
    }

    private void UpScoreEvent(int score){
        scoreText.text = score.ToString("D5");
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
