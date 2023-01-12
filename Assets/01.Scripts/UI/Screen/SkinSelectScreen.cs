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

    [SerializeField] private int index = 0;

    public override void Init()
    {
        tapToBack.onClick.AddListener(() => {
            ButtonClickSound();
            screenPanel.DOAnchorPosY(1980f, 1f).SetEase(Ease.InOutBack).SetUpdate(true)
            .OnComplete(() => {
                base.UpdateScreenState(false);
                screenPanel.DOKill();
                isOpen = false;
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
            if(skin.achive){
                skin.GetComponent<Image>().sprite = skin.skinImg[1];
                skin.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = skin.skinName[1];
            }

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


        if(open && !isOpen){
            skins[1].achive = GameManager.Instance.GetManager<DataManager>().User.KingUnlock;
            skins[2].achive = GameManager.Instance.GetManager<DataManager>().User.RobotUnlock;
            screenPanel.DOAnchorPosY(0f, 1f).SetEase(Ease.OutBack).SetUpdate(true).OnComplete(() => {
                screenPanel.DOKill();
                isOpen = true;
            });
        }
    }
}
