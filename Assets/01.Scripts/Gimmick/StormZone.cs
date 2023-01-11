using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormZone : MonoBehaviour
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

    private void OnTriggerEnter2D(Collider2D other) {
        if (GameManager.Instance.Stop) return;

        if(other.CompareTag(PLAYER_TAG)){
            Player player = other.GetComponent<Player>();
            _playerRigid ??= player.GetComponent<Rigidbody2D>();
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if(_playerRigid == null || GameManager.Instance.Stop) return;

        _playerRigid.AddForce(Vector2.left * _force);
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
        _force = isRight ? -_stormForce : _stormForce;
        foreach (var particle in _stormParticles)
        {
            particle.Play();
        }
    }
}
