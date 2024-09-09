using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBlinkFeedback : MonoBehaviour
{
    [SerializeField]
    private List<SpriteRenderer> _sprites;

    [SerializeField] private Material _paintWhite;
    [SerializeField] private Material _defaultMat;

    [SerializeField]
    private float _blinkTime = 0.1f;

    private Coroutine currentCo;
    private bool isBlink;

    public void Play(float blinkTime = 0.1f)
    {
        if (!isBlink)
        {

            currentCo = StartCoroutine(BlinkCo(blinkTime));

        }
        else
        {
            StopCoroutine(currentCo);
            currentCo = StartCoroutine(BlinkCo(blinkTime));

        }

    }

    private IEnumerator BlinkCo(float blinkTime)
    {

        isBlink = true;

        foreach (var sprite in _sprites)
        {
            sprite.material = _paintWhite;
        }

        yield return new WaitForSeconds(blinkTime);

        for (int i = 0; i < _sprites.Count; ++i)
        {
            var sprite = _sprites[i];

            sprite.material = _defaultMat;
        }

        isBlink = false;
    }

}
