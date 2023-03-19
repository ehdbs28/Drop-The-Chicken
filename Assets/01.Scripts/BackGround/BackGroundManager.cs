using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundManager : MonoBehaviour, IManager
{
    [SerializeField] private GradientObject<Camera> _gradientCam;
    [SerializeField] private GradientObject<SpriteRenderer> _gradientSkyLight;

    public void UpdateState(GameState state)
    {
        switch(state){
            case GameState.INGAME:
                Init();
                break;
        }
    }

    private void Init(){
        _gradientCam.gradientObj = Camera.main;
        _gradientSkyLight.gradientObj = _gradientCam.gradientObj.transform.Find("SkyLight").GetComponent<SpriteRenderer>();

        StartCoroutine(_gradientCam.Gradient());
        StartCoroutine(_gradientSkyLight.Gradient());
    }
}
