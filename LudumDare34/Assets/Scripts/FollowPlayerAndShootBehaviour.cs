using UnityEngine;
using System.Collections;

public class FollowPlayerAndShootBehaviour : EnemyBehaviour
{
    public float velocity;
    public float safeDistance;
    public float fireRate;
    public float fireDelay;
    public bool slowRotation;
    void Update()
    {
        if (GameStateManager.GetState() != GameState.GameOver)
        {

            var offset = new Vector2(Player.instance.transform.position.x - this.transform.position.x, Player.instance.transform.position.y - this.transform.position.y);
            var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.Euler(0, 0, angle - 90), (slowRotation?0.01f:1f));
            if (Vector3.Distance(this.transform.position, Player.instance.transform.position) > safeDistance)
            {
                this.transform.position = Vector3.MoveTowards(this.transform.position, Player.instance.transform.position, velocity * Time.deltaTime);
            }
            else if (Vector3.Distance(this.transform.position, Player.instance.transform.position) < safeDistance / 2)
            {
                this.transform.position = Vector3.MoveTowards(this.transform.position, -Player.instance.transform.position, velocity * Time.deltaTime);
            }

            fireDelay -= Time.deltaTime;
            if (fireDelay <= 0)
            {
                this.ship.Fire();
                fireDelay = fireRate;
            }
        } 
    }

}