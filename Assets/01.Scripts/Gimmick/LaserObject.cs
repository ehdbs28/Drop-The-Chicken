using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserObject : PoolableMono, IObstacle, IBrokenObject
{
    [SerializeField] private Vector2 _startPos;
    [SerializeField] private Vector2 _endPos;

    [SerializeField] private LineRenderer _lineRenderer;

    public void SetPosition(Vector2 startPos, Vector2 endPos){
        _lineRenderer.SetPosition(0, startPos);
        _lineRenderer.SetPosition(1, endPos);

        _startPos = startPos;
        _endPos = endPos;
    }

    public void BrokenEvent()
    {
        
    }

    public void EnterEvent(Collider2D col)
    {
        Player player = col.GetComponent<Player>();
        player?.OnDamage();
    }

    public void StayEvent(Collider2D col)
    {
    }

    public void ExitEvent(Collider2D col)
    {
    }

    public override void Reset()
    {
    }
}
