using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    [SerializeField] private float _fallingSpeed = 1f;
    public float FallingSpeed
    {
        get { return _fallingSpeed; }
        set { _fallingSpeed = value; }
    }
    [SerializeField] private float _moveSpeed = 4.5f;
    

    [SerializeField] private float _feverSpeed = 10f;

    [SerializeField] private Material _paintWhite;
    [SerializeField] private Material _defaultMat;

    [SerializeField] private Transform _fallPos;

    public bool IsPlay {get; set;}
    private bool _isDie;
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
        if (GameManager.Instance.Stop)
        {
            _rigid.velocity = Vector3.zero;
            return;
        };

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
        if (_isUnbeatable || _isDie) return;

        StartCoroutine("Die");
        
    }

    private void FeverStart()
    {
        bool fever = F && E && V && E2 && R;
        if(!_isFever && fever)
            StartCoroutine("DoFever");
    }
    public void GetFeverObj(FeverTxt txt)
    {
        if(IsFever || _isDie) return;

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
    private void OnTriggerEnter2D(Collider2D obj)
    {
        if(_isUnbeatable)
            Debug.Log(obj.name);

        if (!_isDie && obj.CompareTag("DamageAbleObj"))
            OnDamage();
    }
    
    IEnumerator DoFever()
    {
        _animator.SetTrigger("Fever");
        float saveSpd = _fallingSpeed;
        _fallingSpeed = _feverSpeed;
        _isFever = true;
        _isUnbeatable = true;
        yield return new WaitForSeconds(5f);
        _fallingSpeed = saveSpd;
        ResetFever();
    }
    IEnumerator Die()
    {
        _isDie = true;
        _animator.SetTrigger("Die");
        _spriteRenderer.material = _paintWhite;
        GameManager.Instance.Stop = true;
        yield return new WaitForSeconds(0.3f);
        _spriteRenderer.material = _defaultMat;

        
        float m_Speed = 0.5f;
        float m_HeightArc = 5;
        Vector3 m_StartPosition = transform.position;
        _fallPos.position = new Vector3(transform.position.x + 2, transform.position.y - 8, 0);

        while (true)
        {
            float x0 = m_StartPosition.x;
            float x1 = _fallPos.position.x;
            float distance = x1 - x0;
            float nextX = Mathf.MoveTowards(transform.position.x, x1, m_Speed * Time.fixedDeltaTime);
            float baseY = Mathf.Lerp(m_StartPosition.y, _fallPos.position.y, (nextX - x0) / distance);
            float arc = m_HeightArc * (nextX - x0) * (nextX - x1) / (-0.25f * distance * distance);
            Vector3 nextPosition = new Vector3(nextX, baseY + arc, transform.position.z);

            //transform.rotation = LookAt2D(nextPosition - transform.position);
            transform.position = nextPosition;


            if (nextPosition == _fallPos.position)
                break;

            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForSeconds(1.5f);
        GameManager.Instance.Stop = false;
        GameManager.Instance.UpdateState(GameState.RESULT);
    }
    Quaternion LookAt2D(Vector2 forward)
    {
        return Quaternion.Euler(0, 0, Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg);
    }



}
