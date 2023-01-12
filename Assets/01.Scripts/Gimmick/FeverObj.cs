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

public class FeverObj : PoolableMono, IObstacle
{
    [SerializeField] private FeverTxt _feverTxt;
    [SerializeField] private AudioClip _getObject;
    private bool _isDelete;
    private Canvas _canvas;
    private ParticleSystem _eatEffect;

    private void Awake()
    {
        _canvas = GetComponentInChildren<Canvas>();
        _eatEffect = GetComponentInChildren<ParticleSystem>();
    }

    public void EnterEvent(Collider2D col)
    {
        GameManager.Instance.GetManager<AudioManager>().PlayOneShot(_getObject);
        Player player = col.GetComponent<Player>();

        player.GetFeverObj(_feverTxt);
        StartCoroutine("DeleteObj");
    }

    public void StayEvent(Collider2D col)
    {

    }

    public void ExitEvent(Collider2D col)
    {
        
    }

    IEnumerator DeleteObj()
    {
        if (!_isDelete)
        {
            _isDelete = true;
            _eatEffect.Play();
            _canvas.gameObject.SetActive(false);
            yield return new WaitForSeconds(1f);
            _eatEffect.Stop();
            PoolManager.Instance.Push(this);
        }
        
    }

    public override void Reset()
    {
        _canvas.gameObject.SetActive(true);
        _isDelete = false;
    }
}
