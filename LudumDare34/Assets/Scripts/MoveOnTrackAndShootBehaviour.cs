using UnityEngine;
using System.Collections;

public class MoveOnTrackAndShootBehaviour : EnemyBehaviour
{
    public float velocity;
    public float fireRate;
    public float fireDelay;

    public float farRight; //A
    public float farDown; //B
    void Update()
    {
        var offset = new Vector2(Player.instance.transform.position.x - this.transform.position.x, Player.instance.transform.position.y - this.transform.position.y);
        var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);

            
        fireDelay -= Time.deltaTime;
        if (fireDelay <= 0)
        {
            this.ship.Fire();
            fireDelay = fireRate;
        }
    }
}
