using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class PlayerColliderComponent : IPlayerComponent
{
    public PlayerColliderComponent(Player player) : base(player)
    {

    }

    public override void UpdateState(GameState state)
    {
        switch(state){
            case GameState.INIT:
                Init();
                break;
        }
    }

    private void Init(){
        Collider2D playerCol = player.GetComponent<Collider2D>();

        player.OnTriggerEnter2DAsObservable()
        .Where(condition => GameManager.Instance.State == GameState.INGAME && !GameManager.Instance.Stop && !player.IsDie)
        .Subscribe(col => {
            if(!col.CompareTag("Obstacle")) return;

            if(player.IsUnbeatable){
                IBrokenObject brokenObject = col.GetComponent<IBrokenObject>();
                brokenObject?.BrokenEvent();
            }
            else{
                IObstacle obstacle = col.GetComponent<IObstacle>();
                obstacle?.EnterEvent(playerCol);
            }
        });

        player.OnTriggerStay2DAsObservable()
        .Where(condition => GameManager.Instance.State == GameState.INGAME && !GameManager.Instance.Stop && !player.IsDie)
        .Subscribe(col => {
            if(!col.CompareTag("Obstacle")) return;

            IObstacle obstacle = col.GetComponent<IObstacle>();

            obstacle?.StayEvent(playerCol);
        });

        player.OnTriggerExit2DAsObservable()
        .Where(condition => GameManager.Instance.State == GameState.INGAME && !GameManager.Instance.Stop && !player.IsDie)
        .Subscribe(col => {
            if(!col.CompareTag("Obstacle")) return;

            IObstacle obstacle = col.GetComponent<IObstacle>();

            obstacle?.ExitEvent(playerCol);
        });
    }
}
