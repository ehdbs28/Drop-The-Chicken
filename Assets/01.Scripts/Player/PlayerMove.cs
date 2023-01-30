// using System.Collections;
// using System.Collections.Generic;
// using UnityEditor;
// using UnityEngine;
// using UnityEngine.EventSystems;

// public class PlayerMove : MonoBehaviour
// {
//     Player player;
//     Touch touch;

//     private void Awake()
//     {
//         player = GetComponent<Player>();
//     }

//     private void Update()
//     {
//         MoveCheck();
//     }

//     private void MoveCheck()
//     {
//         if (!player.IsPlay) return;

//         PC_Move();
//         Moblie_Move();
//     }

//     private void PC_Move(){
//         float x = Input.GetAxisRaw("Horizontal");

//         if(GameManager.Instance.GetManager<PlayerManager>().State == PlayerState.LANDING){
//             if(x != 0) GameManager.Instance.GetManager<PlayerManager>().State = PlayerState.FALLING;
//         }
//         else{
//             // if(Input.GetKeyDown(KeyCode.D))
//             // {
//             //     player.PlayerMove(x > 0);
//             // }
//             // else 
//             // {
//             //     player.StopMove();
//             // }

//             // if(Input.GetKeyDown(KeyCode.A)){
//             //     player.PlayerMove(false);
//             // }
//             // else if(Input.GetKeyDown(KeyCode.D)){
//             //     player.PlayerMove(true);
//             // }
//         }
//     }

//     private void Moblie_Move(){
//         if(Input.touchCount > 0)
//         {
//             touch = Input.GetTouch(0);
//             Vector2 pos = Camera.main.ScreenToWorldPoint(touch.position);

//             switch (touch.phase)
//             {
//                 case TouchPhase.Moved:
//                 case TouchPhase.Stationary:
//                     {
//                         if (pos.x > 0)
//                         {
//                             player.PlayerMove(true);
//                         }
//                         else if (pos.x < 0)
//                         {
//                             player.PlayerMove(false);
//                         }
//                     };
//                     break;
//                 case TouchPhase.Ended:
//                     player.StopMove();
//                     break;
//             }
            
//         }
//     }
// }
