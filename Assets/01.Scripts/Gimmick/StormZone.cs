using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class StormZone : Gimmick, IObstacle
{
    [SerializeField] private float _stormForce;
    private float _stormForceMin = 75f;
    private float _stormForceMax = 225f;
    
    private float _force;
    
    private ParticleSystem[] _stormParticles;

    private Rigidbody2D _playerRigid;
    private const string PLAYER_TAG = "Player";

    private bool _isRight = false;

    private void Awake() {
        _stormParticles = GetComponentsInChildren<ParticleSystem>();
    }
    
    public void EnterEvent(Collider2D col)
    {
        GameManager.Instance.GetManager<ScoreManager>().ScoreSubscribe(ScoreToSpeed);
        Player player = col.GetComponent<Player>();
        _playerRigid ??= player.GetComponent<Rigidbody2D>();
    }

    public void StayEvent(Collider2D col)
    {
        if(_playerRigid == null) return;

        _playerRigid.AddForce(Vector2.left * _force);
    }

    public void ExitEvent(Collider2D col)
    {
        _playerRigid = null;
    }

    private void SetDirection(bool isRight)
    {
        _isRight = isRight;
        transform.localScale = isRight ? new Vector3(14, -6, 1) : new Vector3(14, 6, 1);

        _force = isRight ? -_stormForce : _stormForce;
        foreach (var particle in _stormParticles)
        {
            particle.Play();
        }
    }

    private void ScoreToSpeed(int score){
        if(_isRight){
            _force = Mathf.Lerp(-_stormForceMin, -_stormForceMax, score / _stormForceMax);
        }
        else{
            _force = Mathf.Lerp(_stormForceMin, _stormForceMax, score / _stormForceMax);
        }
    }

    public override void Reset()
    {
        SetDirection(Random.value > 0.5f);
        //
    }

    public override void Spawn(float nextSummonY)
    {
        transform.position = new Vector2(0, nextSummonY - 10);
    }
}
