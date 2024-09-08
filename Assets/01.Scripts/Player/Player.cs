using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine.Rendering;
using DG.Tweening;
using System;
//using static UnityEditor.Experimental.GraphView.GraphView;

public class Player : MonoBehaviour, IDamageable
{
    [SerializeField] private float _fallingSpeed = 1f;
    public float BaseFallingSpeed
    {
        get { return _fallingSpeed; }
        set { _fallingSpeed = value; }
    }
    private float risingSpeed;
    [SerializeField] private float _fastSpeed;

    [SerializeField] private float _moveSpeed = 4.5f;
    public float MoveSpeed{ get => _moveSpeed; set => _moveSpeed = value; }
    [SerializeField] private float _feverSpeed = 10f;

    [SerializeField] private Material _paintWhite;
    [SerializeField] private Material _defaultMat;

    [SerializeField] private ParticleSystem[] _feverParticles;
    private Vector2 _fallPos = Vector2.zero;

    [Header("Audio Clip")]
    public AudioClip PlayerDieClip;
    public AudioClip PlayerFeverStartClip;
    public AudioClip PlayerObjectBrokenClip;
    public AudioClip PlayerFeverWind;
    public AudioClip FeverBgm;

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

    private bool _isSlow;
    public bool IsSlow
    {
        get => _isSlow; set
        {
            _isSlow = value;

            if (_isSlow)
            {
                slowStack++;

                if(_isFast)
                {
                    _isFast = false;
                }

                StopCoroutine("FastDown");
                StartCoroutine("FastDown");
            }
            else
            {
                DetachBallon();
                slowStack = 0;
            }
        }
    }

   
    private bool _isMirror;
    public bool IsMirror { get => _isMirror; set{
        _isMirror = value;

        if(_isMirror){
            StopCoroutine("MirrorActiveFalse");
            StartCoroutine("MirrorActiveFalse");
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
    private int slowStack = 0;

    private Animator _animator;
    private Rigidbody2D _rigid;
    private SpriteRenderer _spriteRenderer;

    private ParticleSystem[] _fastParticle;

    private Vector2 _lastPosition;

    private bool isEffectedByObstacle = false; //장애물에게 영향을 받고 있으면 Etc : PunchTrap

    Coroutine addForceCoroutine;


    [Header("Rising")]
    [SerializeField] AnimationCurve risingCurve;

    Tween riseTween;

    [Header("Ballon")]
    [SerializeField] SpriteRenderer ballonRender;

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
        FeverStart();
    }

    public void PlayerMove(bool isRight){
        if(isRight)
            _spriteRenderer.flipX = true;
        else
            _spriteRenderer.flipX = false;
        float dirX = ((isRight) ? 1 : -1);
        _rigid.velocity = new Vector2(dirX * _moveSpeed, _rigid.velocity.y);
    }
    public void StopMove()
    {
        if(isEffectedByObstacle == false)
        {
            _rigid.velocity = new Vector2(0, _rigid.velocity.y);
        }
    }

    private void PlayerFall(){
        
        
        float curFallingSpeed = -(BaseFallingSpeed - risingSpeed);

        if (curFallingSpeed < 0) // 하강중
            curFallingSpeed = curFallingSpeed - (1 * fastStack) + (1 * slowStack);
        else// 상승중
            curFallingSpeed = curFallingSpeed + (1 * fastStack) - (1 * slowStack);

        _rigid.velocity = new Vector2(_rigid.velocity.x, curFallingSpeed);
    }

    // controlTime = 플레이어 제어를 놓는 시간
    public void AddForce(Vector2 force, float controlTime)
    {
        if(addForceCoroutine != null)
        {
            StopCoroutine(addForceCoroutine);
        }
        addForceCoroutine = StartCoroutine(AddForceByObstacle(force, controlTime));
    }

    private IEnumerator AddForceByObstacle(Vector2 force, float controlTime)
    {
        //isEffectedByObstacle = true;
        float t = 0f;
        while(t < controlTime)
        {
            Vector2 lerpForce = Vector2.Lerp(force, Vector2.zero, t);
            _rigid.AddForce(lerpForce, ForceMode2D.Impulse);

            t += Time.deltaTime;
            yield return null;
        }

        //isEffectedByObstacle = false;
    }

    public void Rising(float risingSpeed, float riseTime)
    {
        if(riseTween != null)
        {
            riseTween.Kill();
        }

        this.risingSpeed = risingSpeed;
        riseTween = DOTween.To(() => this.risingSpeed, speed => this.risingSpeed = speed, 0f, riseTime).SetEase(risingCurve);
    }


    private IEnumerator FastDown()
    {
        yield return new WaitForSeconds(3f);
        IsSlow = false;
    }

    private IEnumerator SlowDown(){
        yield return new WaitForSeconds(3f);
        IsFast = false;
        //IsSlow = false;
    }

    private IEnumerator MirrorActiveFalse(){
        yield return new WaitForSeconds(3f);
        IsMirror = false;
    }

    public void OnDamage()
    {
        StartCoroutine("Die");
    }

    public void ResetPlayer()
    {
        transform.position = (!GameManager.Instance.IsRevive) ? _defaultPlayerPos : _lastPosition;
        IsFast = false;
        StopCoroutine("Die");

        if(!GameManager.Instance.IsRevive){
            ResetFever();
        }
        else {
            FeverStart();
        }
    }

    private void FeverStart()
    {
        bool fever = (from value in Fevers where value == false select value).Count() == 0; 

        if(!_isFever && (fever || GameManager.Instance.IsRevive))
        {
            StartCoroutine("DoFever");
            return;
        }
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
        else if(txt == FeverTxt.E2 && !Fevers[3])
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
            particle.Stop();
    }

    IEnumerator DoFever()
    {
        _isFever = true;
        _isUnbeatable = true;
        GameManager.Instance.GetManager<AudioManager>().PlayOneShot(PlayerFeverStartClip);
        _animator.SetBool("Fever", true);
        yield return new WaitForSeconds(0.7f);

        GameManager.Instance.GetManager<AudioManager>().PlayOneShot(PlayerFeverWind);
        GameManager.Instance.GetManager<AudioManager>().PlayBGM(FeverBgm);

        foreach(var particle in _feverParticles)
            particle.Play();

        IsFast = false;
        float saveSpd = _fallingSpeed;
        _fallingSpeed = _feverSpeed;
        yield return new WaitForSeconds(5f);

        GameManager.Instance.GetManager<AudioManager>().PlayBGM(GameManager.Instance.GetManager<AudioManager>()._ingameBGM);

        foreach(var particle in _feverParticles)
            particle.Stop();

        Vector2 pos = transform.position;
        pos.x = 0f;
        pos.y -= 15f;
        LayerMask layer = LayerMask.GetMask("Breakable");
        Collider2D[] cols = Physics2D.OverlapBoxAll(pos, new Vector2(10f, 30f), 0f, layer);
        if(cols.Length > 0)
        {

            for(int i = 0; i < cols.Length; i++)
            {

                if(cols[i].TryGetComponent<IBrokenObject>(out IBrokenObject brokenObject))
                {
                    brokenObject.BrokenEvent();
                }

            }

        }

        _animator.SetBool("Fever", false);
        _fallingSpeed = saveSpd;
        ResetFever();
    }

    IEnumerator Die()
    {
        _lastPosition = transform.position;
        GameManager.Instance.GetManager<AudioManager>().PlayOneShot(PlayerDieClip);
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
        _fallPos = new Vector3(x, transform.position.y - 8, 0);
        Vector3 nextPosition = Vector3.zero;

        while (!((int)nextPosition.y == (int)_fallPos.y))
        {
            float x0 = m_StartPosition.x;
            float x1 = _fallPos.x;
            float distance = x1 - x0;
            float nextX = Mathf.MoveTowards(transform.position.x, x1, m_Speed * Time.fixedDeltaTime);
            float baseY = Mathf.Lerp(m_StartPosition.y, _fallPos.y, (nextX - x0) / distance);
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

    public void TakeMushRoom()
    {
        StopCoroutine("GoSmall");
        StartCoroutine("GoSmall");
    }

    IEnumerator GoSmall()
    {
        transform.DOScale(0.5f * Vector3.one, 0.5f);
        yield return new WaitForSeconds(3f);

        transform.DOScale(Vector3.one, 0.5f);
    }

    public void TakeBallon()
    {
        IsSlow = true;
        AttachBallon();
    }

    private void AttachBallon()
    {
        ballonRender.enabled = true;
    }

    private void DetachBallon()
    {
        ballonRender.enabled = false;
    }

}
