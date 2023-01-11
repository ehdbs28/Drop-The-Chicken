using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq.Expressions;

public class DragonBody : MonoBehaviour, IObstacle
{
    private float _dragonBodyTerm = 0.35f;
    private float _shakeValue = 0.05f;

    [SerializeField]
    private SpriteRenderer _sprite;
    public SpriteRenderer Sprite
    {
        get
        {
            if(_sprite == null)
                _sprite = GetComponent<SpriteRenderer>();

            return _sprite;
        }
    }

    public void EnterEvent(Collider2D col)
    {
        Player player = col.GetComponent<Player>();
        player.OnDamage();
    }

    public void StayEvent(Collider2D col)
    {
    }

    public void ExitEvent(Collider2D col)
    {
    }

    public void SetBody(int idx, Vector3 defaultPos)
    {
        Sprite.sortingOrder = -(idx+5);
        transform.position = defaultPos + Vector3.down * idx * _dragonBodyTerm;
    }
    public void ShakeBody()
    {
        float moveX = Random.Range(-_shakeValue, _shakeValue);
        transform.position = transform.position + Vector3.right * moveX;
        //StartCoroutine("DoShakeBody");
    }

    IEnumerator DoShakeBody()
    {
        while (true)
        {
            float defaultX = transform.position.x;
            float moveX = Random.Range(-_shakeValue, _shakeValue);
            transform.DOMoveX(transform.position.x + moveX, 0.1f);
            yield return new WaitForSeconds(0.11f);
            transform.DOMoveX(defaultX, 0.05f);
            yield return new WaitForSeconds(0.06f);
        }
    }
}
