using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormZone : PoolableMono
{
    [SerializeField] private float _stormForce;
    private ParticleSystem[] _stormParticles;

    private Rigidbody2D _playerRigid;
    private const string PLAYER_TAG = "Player";

    private void Awake() {
        _stormParticles = GetComponentsInChildren<ParticleSystem>();
    }

    private void Start() {
        foreach(var particle in _stormParticles){
            particle.Play();
        }
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

    public override void Reset()
    {
        //
    }
}
