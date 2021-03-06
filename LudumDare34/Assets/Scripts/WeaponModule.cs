﻿using UnityEngine;
using System.Collections;

public enum WeaponType {
    Bullet,
    Rocket,
    Plasma,
    Laser,
    Shield
}

public class WeaponModule : MonoBehaviour {

    public string name;
    public int hp;
    public WeaponType type;
    public int damage;
    public float fireRate;
    private float fireDelay;

    public Vector3 direction;
    public float velocity;

    public SpriteRenderer sprite;
    private float blink;
    public Transform tip;


    public Transform attachedTo;
    public WeaponModule connectedWith;
    
    public bool coreModule;
    public bool attachedToHook;

    public float deathTimer;

    private ProjectileOwner tmpOwner;
    private Quaternion tmpRotation;

	// Use this for initialization


    public void DispatchBullet()
    {
        //Debug.Log("Module " + name + " shoots bang bang");
        //GameObject bulletObj = Instantiate(PrefabManager.GetBulletPrefab(type), tip.position, (coreModule ? tmpRotation : this.transform.rotation)) as GameObject;
        //Projectile proj = bulletObj.GetComponent<Projectile>();
        Projectile proj = BulletPoolManager.DispatchProjectile(type, tip.position, (coreModule ? tmpRotation : this.transform.rotation));
        proj.type = type;        
        proj.damage = this.damage;
        if (tmpOwner == ProjectileOwner.Enemy)
        {
            proj.sprite.color = new Color32(223, 74, 49, 255);

        }
        else
        {
            proj.sprite.color = Color.white;
        }
        proj.owner = tmpOwner;
        if (type == WeaponType.Laser)
        {
            proj.transform.parent = this.transform;
        }
        fireDelay = fireRate;

        SoundPlayer.PlayWeaponSound(this.type);
    }

    public void Shoot(ProjectileOwner owner, Quaternion shipRotation)
    {
        if(fireDelay<=0)
        {
            if (tip != null)
            {
                tmpOwner = owner;
                tmpRotation = shipRotation;
                if(type==WeaponType.Laser) this.GetComponent<Animator>().SetTrigger("Fire");
                else DispatchBullet();

            }
        }
    }

    public void Disconnect()
    {
        if (connectedWith != null)
        {
            WeaponModule tmp = connectedWith;
            this.connectedWith = null;
            tmp.Disconnect();
        }
        this.attachedTo = null;
        this.transform.parent = null;
        this.attachedToHook = false;
        Player ply = (attachedTo != null ? attachedTo.GetComponent<Player>() : null);
        if (ply != null) ply.modules.Remove(this);
        else
        {
            Enemy eny = (attachedTo != null ? attachedTo.GetComponent<Enemy>() : null);
            if (eny != null) eny.modules.Remove(this);
        }

        direction = new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), 0f);
        velocity = Random.Range(2f, 3.5f);
        deathTimer = 5f;
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -2f);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "PlayerShip" && attachedToHook)
        {
          //  WeaponModule newModule = col.GetComponent<Collider>().gameObject.GetComponent<WeaponModule>();
            if (Hook.instance.attachedObject == this.gameObject) Hook.instance.attachedObject = null;
            this.transform.parent = Player.instance.modulesRoot;
            this.attachedTo = Player.instance.transform;
            attachedToHook = false;
            Player.AddModule(this);
        }

        if (col.gameObject.tag == "Module" && attachedToHook)
        {
            WeaponModule other = col.GetComponent<WeaponModule>();
            if(other != null)
            if(other.attachedTo!=null)
            if(other.attachedTo.tag=="PlayerShip")
            {
                if (Hook.instance.attachedObject == this.gameObject) Hook.instance.attachedObject = null;
                this.transform.parent = Player.instance.modulesRoot;
                this.attachedTo = Player.instance.transform;
                other.connectedWith = this;
                attachedToHook = false;
                Player.AddModule(this);
            }
        }
        if (col.gameObject.tag == "Module" && this.attachedTo!=null)
        {
            WeaponModule other = col.GetComponent<WeaponModule>();
            if(other != null)
            if(other.attachedTo!=null)
                if(other.attachedTo.tag=="EnemyShip" && this.attachedTo.tag=="PlayerShip")
                {
                    Player.Hit(other.hp);
                    other.Hit(30);                    
                }

        }

        if ((col.gameObject.tag == "EnemyShip" || col.gameObject.tag == "Kamikaze") && this.attachedTo != null)
        {
            if (this.attachedTo.tag == "PlayerShip")
            {
                Enemy enemy = col.gameObject.GetComponent<Enemy>();
                if (enemy != null)
                {
                    Player.Hit(50);
                    enemy.Hit(30);
                }
            }
        }
    }

    public void Hit(int dmg)
    {
        if(this.attachedTo!=null)
            if (this.attachedTo.tag == "PlayerShip")
            {
                Player.Hit(dmg);
            }
            else if (this.attachedTo.tag == "EnemyShip")
            {
                this.attachedTo.GetComponent<Enemy>().Hit(dmg);
            }
        //this.hp -= dmg;
        //blink = 0.5f;
        //if (this.hp < 0)
        //{
        //    PrefabManager.DeployExplosionParticles(this.transform.position, 0.5f);
        //    if (this.connectedWith != null) this.connectedWith.Disconnect();

        //    Destroy(this.gameObject);
        //}
    }



	void Start () {
        blink = 1f;
	}
	
	// Update is called once per frame
	void Update () {
        fireDelay -= Time.deltaTime;

        if (blink < 1f)
        {
            blink += Time.deltaTime * 2;
        }
        else blink = 1f;

      sprite.color = new Color(1f, blink, blink);

        if(attachedTo==null)
        {
            this.transform.Translate(direction * Time.deltaTime * velocity);
            this.transform.Rotate(Vector3.forward * Time.deltaTime * velocity);
            deathTimer -= Time.deltaTime;
            if(deathTimer<=0)
            {
                PrefabManager.DeployExplosionParticles(this.transform.position, 0.5f);
                SoundPlayer.PlaySound(Sound.Explosion1);
                Destroy(this.gameObject);
            }
        }

        if(attachedTo!=null)
        if(attachedTo.tag == "PlayerShip")
        {
            var offset = new Vector2(this.transform.position.x - attachedTo.position.x, this.transform.position.y - attachedTo.position.y);
            var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle - 90);
        }
	}
}
