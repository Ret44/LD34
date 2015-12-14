using UnityEngine;
using System.Collections;

public class MoveOnTrackBehaviour : EnemyBehaviour
{
    public float velocity;


    public float farRight; //A
    public float farDown; //B
    void Update()
    {
        if (GameStateManager.GetState() != GameState.GameOver)
        {
            if (Player.instance != null)
            {
                var offset = new Vector2(Player.instance.transform.position.x - this.transform.position.x, Player.instance.transform.position.y - this.transform.position.y);
                var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle - 90);
            }
        }
    }
}
