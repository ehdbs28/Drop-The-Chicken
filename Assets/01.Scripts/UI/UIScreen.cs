using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScreen : MonoBehaviour
{
    [SerializeField] internal GameState state;
    [SerializeField] protected CanvasGroup canvasGroup;

    protected bool isOpen = false;

    public virtual void UpdateScreenState(bool open){
        canvasGroup.alpha = open ? 1 : 0;
        canvasGroup.interactable = open;
        canvasGroup.blocksRaycasts = open;
    }

    public void ButtonClickSound(){
        GameManager.Instance.GetManager<UIManager>().ButtonClickSound();
    }

    public virtual void Init(){
        
    }
}
