using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    [SerializeField] private float _fallingSpeed = 1f;
    [SerializeField] private float _moveSpeed = 4.5f;

    public bool IsPlay {get; set;}

    private Rigidbody2D _rigid;
    private SpriteRenderer _spriteRenderer;

    private void Awake() {
        _rigid = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update() {

    }

    private void FixedUpdate() {
        if(!IsPlay) return;

        PlayerFall();
        PlayerMove();
    }

    private void PlayerMove(){
        float input = Input.GetAxisRaw("Horizontal");

        if(input == 1)
            _spriteRenderer.flipX = true;
        else if(input == -1)
            _spriteRenderer.flipX = false;

        _rigid.velocity = new Vector2(input * _moveSpeed, _rigid.velocity.y);
    }

    private void PlayerFall(){
        _rigid.velocity = new Vector2(_rigid.velocity.x, -_fallingSpeed);
    }

    public void OnDamage()
    {
        GameManager.Instance.UpdateState(GameState.RESULT);
    }
}
