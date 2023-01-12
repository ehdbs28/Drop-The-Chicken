using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkin : MonoBehaviour
{
    [SerializeField] RuntimeAnimatorController[] _animatorContorlloers;
    [SerializeField] Animator _playerAnimator;

    public void SkinSet(int idx)
    {
        _playerAnimator.runtimeAnimatorController = _animatorContorlloers[idx];
    }
}
