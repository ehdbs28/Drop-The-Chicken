using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gimmick : PoolableMono
{
    public EGimmickType GimmickType;
    public float GimmickMinSpace;
    public float GimmickMaxSpace;
    public override void Reset()
    {
        
    }

    /// <summary>
    /// Spawn시 발행되는 함수.
    /// </summary>
    /// <param name="nextSummonY">이 변수는 다음 소환 위치를 전달해줌 </param>
    public virtual void Spawn(float nextSummonY)
    {
        
    }
}
