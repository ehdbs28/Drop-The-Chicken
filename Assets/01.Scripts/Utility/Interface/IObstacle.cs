using UnityEngine;

public interface IObstacle
{
    void EnterEvent(Collider2D col);
    void StayEvent(Collider2D col);
    void ExitEvent(Collider2D col);
}