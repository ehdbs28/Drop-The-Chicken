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
            bool isObstacle = col.CompareTag("Obstacle");
            bool isItem = col.CompareTag("Item");
            if (!isObstacle && !isItem) return;

            if(player.IsUnbeatable){
                IBrokenObject brokenObject = col.GetComponent<IBrokenObject>();
                if(brokenObject != null){
                    brokenObject?.BrokenEvent();
                    GameManager.Instance.GetManager<AudioManager>().PlayOneShot(player.PlayerObjectBrokenClip);
                    int plusScore = (col.transform.GetComponent<DragonBody>() != null ? 1 : 10);
                    GameManager.Instance.GetManager<ScoreManager>().PlusScore(plusScore);
                }
            }
            else if(isObstacle)
            {
                IObstacle obstacle = col.GetComponent<IObstacle>();
                obstacle?.EnterEvent(playerCol);
            }
            else // Item
            {
                IItem item = col.GetComponent<IItem>();
                item?.EnterEvent(player);
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
