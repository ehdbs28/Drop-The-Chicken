using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormZone : PoolableMono
{
    [SerializeField] private float _stormForce;
    private ParticleSystem[] _stormParticles;

    private Rigidbody2D _playerRigid;
    private const string PLAYER_TAG = "Player";

    private bool _isRight = false;

    private void Awake() {
        _stormParticles = GetComponentsInChildren<ParticleSystem>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag(PLAYER_TAG)){
            Player player = other.GetComponent<Player>();
            _playerRigid ??= player.GetComponent<Rigidbody2D>();
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if(_playerRigid == null) return;

        _playerRigid.AddForce(Vector2.left * _stormForce);
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag(PLAYER_TAG)){
            _playerRigid = null;
        }
    }

    public void SetDirection(bool isRight)
    {
        _isRight = isRight;
        transform.localScale = isRight ? new Vector3(14, -6, 1) : new Vector3(14, 6, 1);
        _stormForce = isRight ? -30f : 30f;
        foreach (var particle in _stormParticles)
        {
            particle.Play();
        }
    }

    public override void Reset()
    {
        //
        

    }
}
