using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GradientBackGroundColor : MonoBehaviour, IManager
{
    [SerializeField] private float _speed;
    [SerializeField] private float _waitTime;

    [SerializeField] private CircularQueue<Color> _colorPallete;

    private Camera _cam;

    public void UpdateState(GameState state)
    {
        switch(state){
            case GameState.INGAME:
                Init();
                break;
        }
    }

    private void Init(){
        _cam = Camera.main;
        StartCoroutine(ColorGradient());
    }

    private IEnumerator ColorGradient(){
        yield return new WaitForSeconds(_waitTime);

        float timer = 0f;
        float lerpTime = 0f;
        Color currentColor = _cam.backgroundColor;
        Color nextColor = _colorPallete.Dequeue();

        while (true)
        {
            lerpTime += Time.deltaTime * _speed;

            _cam.backgroundColor = Color.Lerp(currentColor, nextColor, lerpTime);

            if(_cam.backgroundColor.Equals(nextColor))
            {
                timer += Time.deltaTime;
            }

            if (timer > _waitTime)
            {
                timer = 0f;
                lerpTime = 0f;
                currentColor = _cam.backgroundColor;
                nextColor = _colorPallete.Dequeue();
            }

            yield return null;
        }
    }
}
