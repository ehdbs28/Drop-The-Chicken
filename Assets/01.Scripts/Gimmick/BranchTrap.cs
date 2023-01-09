using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchTrap : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")){
            IDamageable damageable = other.GetComponent<IDamageable>();
            damageable?.OnDamage();
        }
    }
}
