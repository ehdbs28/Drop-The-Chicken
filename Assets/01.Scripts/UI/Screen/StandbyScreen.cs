using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StandbyScreen : UIScreen
{
    [SerializeField] private Button tapToStart;
    [SerializeField] private Button tapToSetting;
    [SerializeField] private Button tapToSkinSelect;

    [SerializeField] private CanvasGroup[] fadeObjects;
    [SerializeField] private RectTransform[] moveObjects;

    public override void Init()
    {
        tapToStart.onClick.AddListener(() => {
            ButtonClickSound();
            Sequence sq = DOTween.Sequence().SetUpdate(true);

            foreach(var fadeObj in fadeObjects){
                sq.Join(fadeObj.DOFade(0f, 1f));
            }

            foreach(var moveObj in moveObjects){
                sq.Join(moveObj.DOAnchorPosX(moveObj.anchoredPosition.x > 0 ? 873f : -839f, 1f));
            }

            sq.InsertCallback(0.3f, () => GameManager.Instance.GetManager<UIManager>().Transition(() => GameManager.Instance.UpdateState(GameState.INGAME)));

            sq.OnComplete(() => {
                sq.Kill();
            });
        });

        tapToSetting.onClick.AddListener(() => {
            ButtonClickSound();
            GameManager.Instance.GetManager<ESCManager>().IsOpenSetting = true;
        });

        tapToSkinSelect.onClick.AddListener(() => {
            ButtonClickSound();
            GameManager.Instance.GetManager<UIManager>().GetScreen<SkinSelectScreen>().UpdateScreenState(true);
        });

        base.Init();
    }

    public override void UpdateScreenState(bool open)
    {
        base.UpdateScreenState(open);

        if(open){
            Sequence sq = DOTween.Sequence().SetUpdate(true);

            foreach(var fadeObj in fadeObjects){
                sq.Join(fadeObj.DOFade(1f, 1f));
            }

            foreach(var moveObj in moveObjects){
                sq.Join(moveObj.DOAnchorPosX(moveObj.anchoredPosition.x > 0 ? 473f : -439f, 1f));
            }

            sq.OnComplete(() => sq.Kill());
        }
    }
}
