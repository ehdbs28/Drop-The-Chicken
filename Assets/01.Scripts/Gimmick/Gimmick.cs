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
    /// Spawn�� ����Ǵ� �Լ�.
    /// </summary>
    /// <param name="nextSummonY">�� ������ ���� ��ȯ ��ġ�� �������� </param>
    public virtual void Spawn(float nextSummonY)
    {
        
    }
}
