using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormZone : PoolableMono, IObstacle
{
    [SerializeField] private float _stormForce;
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

    public override void Reset()
    {
        SetDirection(Random.value > 0.5f);
        //
    }
}
