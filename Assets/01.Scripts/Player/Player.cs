using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    [SerializeField] private float _fallingSpeed = 1f;
    [SerializeField] private float _moveSpeed = 4.5f;

    [SerializeField] private float _feverSpeed = 10f;

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

    #region Fever
    private bool _isFever = false;
    public bool IsFever => _isFever;

    private bool F;
    private bool E;
    private bool V;
    private bool E2;
    private bool R;

    private bool _isUnbeatable = false;
    #endregion

    private Animator _animator;
    private Rigidbody2D _rigid;
    private SpriteRenderer _spriteRenderer;

    private ParticleSystem[] _fastParticle;

    private void Awake() {
        _animator = GetComponent<Animator>();
        _rigid = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _fastParticle = transform.Find("FastParticle").GetComponentsInChildren<ParticleSystem>();
    }

    private void FixedUpdate() {
        if(!IsPlay) return;

        PlayerFall();
        PlayerMove();
        FeverStart();
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
        if (_isUnbeatable) return;
        GameManager.Instance.UpdateState(GameState.RESULT);
    }

    private void FeverStart()
    {
        bool fever = F && E && V && E2 && R;
        if(!_isFever && fever)
            StartCoroutine("DoFever");
    }
    public void GetFeverObj(FeverTxt txt)
    {
        if(IsFever) return;

        if(txt == FeverTxt.F && !F)
        {
            F = true;
        }
        else if(txt == FeverTxt.E && !E)
        {
            E = true;
        }
        else if(txt == FeverTxt.V && !V)
        {
            V = true;
        }
        else if(txt == FeverTxt.E && !E2)
        {
            E2 = true;
        }
        else if(txt == FeverTxt.R && !R)
        {
            R = true;
        }
    }
    private void ResetFever()
    {
        _isFever = false;
        _isUnbeatable = false;

        F = false;
        E = false;
        V = false;
        E2 = false;
        R = false;
    }

    IEnumerator DoFever()
    {
        _animator.SetTrigger("Fever");
        _isFever = true;
        _isUnbeatable = true;
        yield return new WaitForSeconds(5f);
        ResetFever();
    }

    private void OnTriggerEnter2D(Collider2D obj)
    {
        if(_isUnbeatable)
            Debug.Log(obj.name);
    }
}
