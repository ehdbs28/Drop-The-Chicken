using UnityEngine;
using DG.Tweening;

public class Pigeon : PoolableMono, IObstacle
{
    [SerializeField] private float minSpeed; // �̵��ӵ�
    [SerializeField] private float maxSpeed;
    private float speed;

    [SerializeField] private float maxX;
    [SerializeField] private float minX;

    private bool moveCheck = false;

    private void FixedUpdate()
    {
        PigeonMove();
    }

    private void PigeonMove()
    {
        if (GameManager.Instance.Stop) return;

        transform.position = transform.position + transform.right * speed * Time.fixedDeltaTime;
        bool checkMin = transform.position.x <= minX && moveCheck;
        bool checkMax = transform.position.x >= maxX && !moveCheck;
        if (checkMin || checkMax)
            ChangeMoveDir();
        
    }

    private void ChangeMoveDir()
    {
        transform.rotation = ( moveCheck ) ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0, 180, 0);
        moveCheck = (moveCheck) ? false : true;
    }


    public override void Reset()
    {
        speed = Random.Range(minSpeed, maxSpeed);
    }

    public void EnterEvent(Collider2D col)
    {
        Player player = col.GetComponent<Player>();
        player.OnDamage();
    }

    public void StayEvent(Collider2D col)
    {
    }

    public void ExitEvent(Collider2D col)
    {
    }
}
