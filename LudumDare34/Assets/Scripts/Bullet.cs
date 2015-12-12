using UnityEngine;
using System.Collections;

public enum ProjectileOwner {
    Player,
    Enemy
}

public class Projectile : MonoBehaviour
{
    public float speed;
    public int damage;
    public ProjectileOwner owner;
    public float lifeTimer;

    public SpriteRenderer sprite;

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Module")
        {
            WeaponModule module = col.gameObject.GetComponent<WeaponModule>();
            if (module.attachedTo != null)
                if (module.attachedTo.tag == "EnemyShip" && this.owner == ProjectileOwner.Player)
                {
                    PrefabManager.DeployHitParticles(this.transform.position);
                    module.Hit(damage);
                    Destroy(this.gameObject);                  
                }
                else if (module.attachedTo.tag == "PlayerShip" && this.owner == ProjectileOwner.Enemy)
                {
                    PrefabManager.DeployHitParticles(this.transform.position);
                    module.Hit(damage);
                    Destroy(this.gameObject);
                }
        }
        if (col.gameObject.tag == "EnemyShip" && this.owner == ProjectileOwner.Player)
        {
            col.gameObject.GetComponent<Enemy>().Hit(damage);
            PrefabManager.DeployHitParticles(this.transform.position);
            Destroy(this.gameObject);
        }
        if(col.gameObject.tag=="Hook")
        {
            PrefabManager.DeployHitParticles(this.transform.position);
            Destroy(this.gameObject);
        }
    }
    public void Update () {
        this.transform.position += transform.up * speed * Time.deltaTime;
        this.lifeTimer -= Time.deltaTime;
        if (this.lifeTimer < 0f) Destroy(this.gameObject);
	}
}

public class Bullet : Projectile {


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        base.Update();
	}
}
