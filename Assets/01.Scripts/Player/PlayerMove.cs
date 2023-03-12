using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMove : MonoBehaviour
{
    Player player;
    Touch touch;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Update()
    {
        MoveCheck();
    }

    private void MoveCheck()
    {
        if (!player.IsPlay) return;

        float x = Input.GetAxisRaw("Horizontal");
        x *= (player.IsMirror) ? -1 : 1;
        if(x != 0)
        {
            if(x > 0)
            {
                player.PlayerMove(true);
            }
            else
            {
                player.PlayerMove(false);
            }
        }
        else
        {
            player.StopMove();
        }
        

        if(Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            Vector2 pos = Camera.main.ScreenToWorldPoint(touch.position);

            switch (touch.phase)
            {
                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    {
                        if (pos.x > 0)
                        {
                            player.PlayerMove(true);
                        }
                        else if (pos.x < 0)
                        {
                            player.PlayerMove(false);
                        }
                    };
                    break;
                case TouchPhase.Ended:
                    player.StopMove();
                    break;
            }
            
        }
    }
}
