using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    [SerializeField] private float _fallingSpeed = 1f;
    [SerializeField] private float _moveSpeed = 4.5f;

    public bool IsPlay {get; set;}
    private bool _isFast;
    public bool IsFast {get => _isFast; set{
        _isFast = value;

        if(value){
            foreach(ParticleSystem particle in _fastParticle)
                particle.Play();

            Invoke("SlowDown", 3f);
        }
        else{
            foreach(ParticleSystem particle in _fastParticle)
                particle.Stop();
        }
    }}

    private Rigidbody2D _rigid;
    private SpriteRenderer _spriteRenderer;

    private ParticleSystem[] _fastParticle;

    private void Awake() {
        _rigid = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _fastParticle = transform.Find("FastParticle").GetComponentsInChildren<ParticleSystem>();
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
        _rigid.velocity = new Vector2(_rigid.velocity.x, -((_isFast) ? _fallingSpeed * 1.5f : _fallingSpeed));
    }

    private void SlowDown(){
        _isFast = false;
    }

    public void OnDamage()
    {
        GameManager.Instance.UpdateState(GameState.RESULT);
    }
}
