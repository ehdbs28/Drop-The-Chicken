using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingScreen : UIScreen
{
    [SerializeField] private Button tapToBack;
    [SerializeField] private Button tapToSoundPanel;
    [SerializeField] private Button tapToCreditPanel; 

    [SerializeField] private Image bgmIcon;
    [SerializeField] private Image sfxIcon;
    [SerializeField] private Image bgmBtnImg;
    [SerializeField] private Image sfxBtnImg;

    [SerializeField] private Sprite[] bgmIconSprites;
    [SerializeField] private Sprite[] sfxIconSprites;
    [SerializeField] private Sprite[] buttonSprites;
    // 0 -> unlock | 1 -> lock

    [SerializeField] private Button muteBGM;
    [SerializeField] private Button muteSFX;

    [SerializeField] private GameObject audioSettingPanel;
    [SerializeField] private GameObject creditPanel;

    [SerializeField] private TextMeshProUGUI settingTitle;

    public override void Init()
    {
        tapToBack.onClick.AddListener(() => {
            GameManager.Instance.GetManager<ESCManager>().IsOpenSetting = false;
        });

        tapToSoundPanel.onClick.AddListener(() => {
            PanelChange("AUDIO");
        });

        tapToCreditPanel.onClick.AddListener(() => {
            PanelChange("CREDIT");
        });

        muteBGM.onClick.AddListener(() => {
            GameManager.Instance.GetManager<AudioManager>().AudioMute(AudioType.BGM, !GameManager.Instance.GetManager<AudioManager>().IsMuteBGM);

            if(GameManager.Instance.GetManager<AudioManager>().IsMuteBGM){
                bgmIcon.sprite = bgmIconSprites[1];
                bgmBtnImg.sprite = buttonSprites[1];
            }
            else{
                bgmIcon.sprite = bgmIconSprites[0];
                bgmBtnImg.sprite = buttonSprites[0];
            }
        });

        muteSFX.onClick.AddListener(() => {
            GameManager.Instance.GetManager<AudioManager>().AudioMute(AudioType.SFX, !GameManager.Instance.GetManager<AudioManager>().IsMuteSFX);

            if(GameManager.Instance.GetManager<AudioManager>().IsMuteSFX){
                sfxIcon.sprite = sfxIconSprites[1];
                sfxBtnImg.sprite = buttonSprites[1];
            }
            else{
                sfxIcon.sprite = sfxIconSprites[0];
                sfxBtnImg.sprite = buttonSprites[0];
            }
        });

        base.Init();
    }

    private void PanelChange(string title){
        settingTitle.text = title;
        switch(title){
            case "AUDIO":
                audioSettingPanel.SetActive(true);
                creditPanel.SetActive(false);
                break;
            case "CREDIT":
                audioSettingPanel.SetActive(false);
                creditPanel.SetActive(true);
                break;
        }
    }
}
