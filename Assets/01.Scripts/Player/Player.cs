using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Player : MonoBehaviour, IDamageable
{
    [SerializeField] private float _fallingSpeed = 1f;
    public float FallingSpeed
    {
        get { return _fallingSpeed; }
        set { _fallingSpeed = value; }
    }
    [SerializeField] private float _fastSpeed;

    [SerializeField] private float _moveSpeed = 4.5f;
    [SerializeField] private float _feverSpeed = 10f;

    [SerializeField] private Material _paintWhite;
    [SerializeField] private Material _defaultMat;

    [SerializeField] private Transform _fallPos;

    [SerializeField] private ParticleSystem[] _feverParticles;

    private Vector2 _defaultPlayerPos = new Vector2(0, 4.35f);
    public Vector2 DefaultPlayerPos => _defaultPlayerPos;

    public bool IsPlay {get; set;}
    public bool IsDie{
        get => GameManager.Instance.Stop;
        set => GameManager.Instance.Stop = value;
    }
    private bool _isFast;
    public bool IsFast {get => _isFast; set{
        _isFast = value;

        if(_isFast){
            fastStack++;
            _fastSpeed = _fallingSpeed + (1f * fastStack);

            foreach(ParticleSystem particle in _fastParticle)
                particle.Play();

            StopCoroutine("SlowDown");
            StartCoroutine("SlowDown");
        }
        else{
            foreach(ParticleSystem particle in _fastParticle)
                particle.Stop();

            fastStack = 0;
        }
    }}

    #region Fever
    private bool _isFever = false;
    public bool IsFever => _isFever;

    private bool _isUnbeatable = false;
    public bool IsUnbeatable => _isUnbeatable;

    public List<bool> Fevers;
    #endregion

    private int fastStack = 0;

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

        if (IsDie){
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
        _rigid.velocity = new Vector2(_rigid.velocity.x, -((_isFast) ? _fastSpeed: _fallingSpeed));
    }

    private IEnumerator SlowDown(){
        yield return new WaitForSeconds(3f);
        IsFast = false;
    }

    public void OnDamage()
    {
        StartCoroutine("Die");
    }

    public void ResetPlayer()
    {
        transform.position = _defaultPlayerPos;
        IsFast = false;
        StopCoroutine("Die");
        ResetFever();
    }

    private void FeverStart()
    {
        bool fever = (from value in Fevers where value == false select value).Count() == 0; 

        if(!_isFever && fever)
            StartCoroutine("DoFever");
    }

    public void GetFeverObj(FeverTxt txt)
    {
        if(IsFever || IsDie) return;

        if(txt == FeverTxt.F && !Fevers[0])
        {
            Fevers[0] = true;
        }
        else if(txt == FeverTxt.E && !Fevers[1])
        {
            Fevers[1] = true;
        }
        else if(txt == FeverTxt.V && !Fevers[2])
        {
            Fevers[2] = true;
        }
        else if(txt == FeverTxt.E && !Fevers[3])
        {
            Fevers[3] = true;
        }
        else if(txt == FeverTxt.R && !Fevers[4])
        {
            Fevers[4] = true;
        }
    }

    private void ResetFever()
    {
        _animator.SetBool("Fever", false);
        _isFever = false;
        _isUnbeatable = false;

        for(int i = 0; i < Fevers.Count; i++){
            Fevers[i] = false;
        }

        foreach(var particle in _feverParticles)
            particle.Play();
    }

    IEnumerator DoFever()
    {
        _animator.SetBool("Fever", true);
        yield return new WaitForSeconds(0.7f);

        foreach(var particle in _feverParticles)
            particle.Play();

        IsFast = false;
        _isFever = true;
        _isUnbeatable = true;
        float saveSpd = _fallingSpeed;
        _fallingSpeed = _feverSpeed;
        yield return new WaitForSeconds(5f);

        foreach(var particle in _feverParticles)
            particle.Stop();
        
        _animator.SetBool("Fever", false);
        _fallingSpeed = saveSpd;
        ResetFever();
    }

    IEnumerator Die()
    {
        IsFast = false;
        IsDie = true;
        _animator.SetBool("Die", true);
        _spriteRenderer.material = _paintWhite;
        yield return new WaitForSeconds(0.3f);
        _spriteRenderer.material = _defaultMat;

        
        float m_HeightArc = 5;
        Vector3 m_StartPosition = transform.position;
        float x = (transform.position.x > 0) ? -2f : 2f;
        float m_Speed = Mathf.Abs(transform.position.x - x);
        _fallPos.position = new Vector3(x, transform.position.y - 8, 0);

        Vector3 nextPosition = Vector3.zero;

        while (!((int)nextPosition.y == (int)_fallPos.position.y))
        {
            float x0 = m_StartPosition.x;
            float x1 = _fallPos.position.x;
            float distance = x1 - x0;
            float nextX = Mathf.MoveTowards(transform.position.x, x1, m_Speed * Time.fixedDeltaTime);
            float baseY = Mathf.Lerp(m_StartPosition.y, _fallPos.position.y, (nextX - x0) / distance);
            float arc = m_HeightArc * (nextX - x0) * (nextX - x1) / (-0.25f * distance * distance);
            nextPosition = new Vector3(nextX, baseY + arc, transform.position.z);

            //transform.rotation = LookAt2D(nextPosition - transform.position);
            transform.position = nextPosition;

            yield return new WaitForFixedUpdate();
        }
        //yield return new WaitForSeconds(1f);
        _animator.SetBool("Die", false);
        GameManager.Instance.UpdateState(GameState.RESULT);
    }

    Quaternion LookAt2D(Vector2 forward)
    {
        return Quaternion.Euler(0, 0, Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg);
    }
}
