using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FeverTxt
{
    F,
    E,
    V,
    R,
}

public class FeverObj : PoolableMono
{
    [SerializeField] private FeverTxt _feverTxt;

    private void OnTriggerEnter2D(Collider2D obj)
    {
        if (obj.CompareTag("Player"))
        {
            Player player = obj.GetComponent<Player>();

            player.GetFeverObj(_feverTxt);
            PoolManager.Instance.Push(this);
        }
    }

    public override void Reset()
    {
        
    }
}
