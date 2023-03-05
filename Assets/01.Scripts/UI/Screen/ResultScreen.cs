using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ResultScreen : UIScreen
{
    [SerializeField] private Button tapToRestart;
    [SerializeField] private Button tapToMainmenu;
    [SerializeField] private Button tapToADReward;

    [SerializeField] private TextMeshProUGUI currentScore;
    [SerializeField] private TextMeshProUGUI bestScore;

    [SerializeField] private RectTransform screenPanel;

    public override void UpdateScreenState(bool open)
    {
        GameManager.Instance.GetManager<ScoreManager>().ScoreSubscribe(score => currentScore.text = score.ToString("D5"));
        bestScore.text = GameManager.Instance.GetManager<DataManager>().User.BestScore.ToString("D5");

        base.UpdateScreenState(open);

        if(open){
            screenPanel.DOAnchorPosY(0f, 1f).SetEase(Ease.OutBack).SetUpdate(true).OnComplete(() => {
                screenPanel.DOKill();
                isOpen = true;
            });
        }
    }

    public override void Init()
    {
        tapToRestart.onClick.AddListener(() => {
            if(!isOpen) return;

            isOpen = false;
            ButtonClickSound();
            screenPanel.DOAnchorPosY(1980f, 1f).SetEase(Ease.OutBack).SetUpdate(true)
            .OnComplete(() => {
                GameManager.Instance.UpdateState(GameState.INGAME);
                screenPanel.DOKill();
            });
        });

        tapToMainmenu.onClick.AddListener(() => {
            if(!isOpen) return;

            isOpen = false;
            ButtonClickSound();
            Sequence sq = DOTween.Sequence().SetUpdate(true);

            sq.Append(screenPanel.DOAnchorPosY(1980f, 1f).SetEase(Ease.InOutBack));

            sq.InsertCallback(0.3f, () => GameManager.Instance.GetManager<UIManager>().Transition(() => GameManager.Instance.UpdateState(GameState.STANDBY)));

            sq.OnComplete(() => {
                sq.Kill();
            });
        });

        tapToADReward.onClick.AddListener(() => {
            //광고재생
            if(!isOpen) return;

            isOpen = false;
            ButtonClickSound();
            Sequence sq = DOTween.Sequence().SetUpdate(true);

            sq.Append(screenPanel.DOAnchorPosY(1980f, 1f).SetEase(Ease.InOutBack));

            sq.InsertCallback(0.5f, () => {
                GameManager.Instance.IsRevibe = true;
                GameManager.Instance.UpdateState(GameState.INGAME);
            });

            sq.OnComplete(() => {
                sq.Kill();
            });
        });

        base.Init();
    }
}
