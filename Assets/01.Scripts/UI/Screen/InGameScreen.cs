using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameScreen : UIScreen
{
    [SerializeField] private Button tapToSetting;

    public override void Init()
    {
        tapToSetting.onClick.AddListener(() => {
            GameManager.Instance.GetManager<ESCManager>().IsOpenSetting = true;
        });

        base.Init();
    }
}
