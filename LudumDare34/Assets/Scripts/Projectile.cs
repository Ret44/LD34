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
    [HideInInspector]
    public float timer;

    public WeaponType type;
    public SpriteRenderer sprite;

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Shield")
        {
            Debug.Log("KOLIZJA");
            Vector3 newUp = Vector3.Reflect(this.transform.up, col.transform.up);
            this.transform.rotation = Quaternion.LookRotation(this.transform.forward, newUp);
            //this.transform.rotation = Quaternion.LookRotation();
            //this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, this.transform.rotation.eulerAngles.z));
            this.sprite.color = Color.red;
            this.owner = ProjectileOwner.Enemy;
        }
        if (col.gameObject.tag == "Module")
        {
            WeaponModule module = col.gameObject.GetComponent<WeaponModule>();
            if (module.attachedTo != null)
                if (module.attachedTo.tag == "EnemyShip" && this.owner == ProjectileOwner.Player)
                {
                    PrefabManager.DeployHitParticles(this.transform.position);
                    module.Hit(damage);
                    BulletPoolManager.ReturnBullet(this);
                }
                else if (module.attachedTo.tag == "PlayerShip" && this.owner == ProjectileOwner.Enemy)
                {
                    PrefabManager.DeployHitParticles(this.transform.position);
                    module.Hit(damage);
                    BulletPoolManager.ReturnBullet(this);
                }
        }
        if (col.gameObject.tag == "EnemyShip" && this.owner == ProjectileOwner.Player)
        {
            col.gameObject.GetComponent<Enemy>().Hit(damage);
            PrefabManager.DeployHitParticles(this.transform.position);
            BulletPoolManager.ReturnBullet(this);
        }
        if (col.gameObject.tag == "PlayerShip" && this.owner == ProjectileOwner.Enemy)
        {
            Player.Hit(damage);
            PrefabManager.DeployHitParticles(this.transform.position);
            BulletPoolManager.ReturnBullet(this);
        }
        if (col.gameObject.tag == "Kamikaze" && this.owner == ProjectileOwner.Player)
        {
            col.transform.parent.gameObject.GetComponent<Enemy>().Hit(damage);
            PrefabManager.DeployHitParticles(this.transform.position);
            BulletPoolManager.ReturnBullet(this);
        }
        if(col.gameObject.tag=="Hook")
        {
            PrefabManager.DeployHitParticles(this.transform.position);
            BulletPoolManager.ReturnBullet(this);
        }
    }
    public void Update () {
        this.transform.position += transform.up * speed * Time.deltaTime;
        this.timer -= Time.deltaTime;
        if (this.timer < 0f) BulletPoolManager.ReturnBullet(this);
	}
}

//public class Bullet : Projectile {


//    // Use this for initialization
//    void Start () {
	
//    }
	
//    // Update is called once per frame
//    void Update () {
//        base.Update();
//    }
//}
