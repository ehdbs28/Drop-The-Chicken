using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FeverTxt
{
    F,
    E,
    V,
    R,
}

public class FeverObj : PoolableMono
{
    [SerializeField] private FeverTxt _feverTxt;
    private Canvas _canvas;
    private ParticleSystem _eatEffect;

    private void Awake()
    {
        _canvas = GetComponentInChildren<Canvas>();
        _eatEffect = GetComponentInChildren<ParticleSystem>();
    }

    private void OnTriggerEnter2D(Collider2D obj)
    {
        if (obj.CompareTag("Player"))
        {
            Player player = obj.GetComponent<Player>();

            player.GetFeverObj(_feverTxt);
            StartCoroutine("DeleteObj");
        }
    }

    IEnumerator DeleteObj()
    {
        _eatEffect.Play();
        _canvas.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);
        _eatEffect.Stop();
        PoolManager.Instance.Push(this);
    }

    public override void Reset()
    {
        _canvas.gameObject.SetActive(true);
    }
}
