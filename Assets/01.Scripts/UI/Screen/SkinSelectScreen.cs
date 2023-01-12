using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Linq;

public class SkinSelectScreen : UIScreen
{
    public Skin[] skins;

    [SerializeField] private Button tapToBack;
    [SerializeField] private Button backIndex;
    [SerializeField] private Button frontIndex;

    [SerializeField] private RectTransform screenPanel;

    private int index = 0;

    public override void Init()
    {
        for(int i = 0; i < skins.Length; i++){
            if(GameManager.Instance.GetManager<DataManager>().User.SkinUnlock > i){
                skins[i].achive = true;
            }
        }

        tapToBack.onClick.AddListener(() => {
            ButtonClickSound();
            screenPanel.DOAnchorPosY(1980f, 1f).SetEase(Ease.InOutBack).SetUpdate(true)
            .OnComplete(() => {
                base.UpdateScreenState(false);
                screenPanel.DOKill();
            });
        });
        backIndex.onClick.AddListener(() => {
            ButtonClickSound();
            index--;
            index = Mathf.Clamp(index, 0, skins.Length - 1);

            SkinSelect(index);
        });
        frontIndex.onClick.AddListener(() => {
            ButtonClickSound();
            index++;
            index = Mathf.Clamp(index, 0, skins.Length - 1);

            SkinSelect(index);
        });
    }

    private void SkinSelect(int index){
        foreach(var skin in skins){
            if(skin.index == index){
                skin.gameObject.SetActive(true);
                if(skin.achive)
                    GameManager.Instance.GetManager<PlayerManager>().SkinChange(index);
            }
            else{
                skin.gameObject.SetActive(false);
            }
        }
    }

    public override void UpdateScreenState(bool open)
    {
        base.UpdateScreenState(open);

        if(open){
            screenPanel.DOAnchorPosY(0f, 1f).SetEase(Ease.OutBack).SetUpdate(true).OnComplete(() => screenPanel.DOKill());
        }
    }
}
