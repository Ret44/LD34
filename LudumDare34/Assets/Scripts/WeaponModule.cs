using UnityEngine;
using System.Collections;

public enum WeaponType {
    Bullet,
    Rocket,
    Plasma,
    Laser
}

public class WeaponModule : MonoBehaviour {

    public string name;
    public int hp;
    public WeaponType type;
    public int damage;
    public float fireRate;
    private float fireDelay;

    public SpriteRenderer sprite;
    public Transform tip;

    public Transform attachedTo;
    public WeaponModule connectedWith;
    
    public bool coreModule;
    public bool attachedToHook;

    private ProjectileOwner tmpOwner;
    private Quaternion tmpRotation;

	// Use this for initialization


    public void DispatchBullet()
    {
        Debug.Log("Module " + name + " shoots bang bang");
        GameObject bulletObj = Instantiate(PrefabManager.GetBulletPrefab(type), tip.position, (coreModule ? tmpRotation : this.transform.rotation)) as GameObject;
        Projectile proj = bulletObj.GetComponent<Projectile>();
        proj.damage = this.damage;
        if (tmpOwner == ProjectileOwner.Enemy)
        {
            proj.sprite.color = Color.red;
        }
        else
        {
            proj.sprite.color = Color.yellow;
        }
        proj.owner = tmpOwner;
        if (type == WeaponType.Laser)
        {
            proj.transform.parent = this.transform;
        }
        fireDelay = fireRate;
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
    }

    public void Hit(int dmg)
    {
        this.hp -= dmg;
        if (this.hp < 0)
        {
            PrefabManager.DeployExplosionParticles(this.transform.position, 0.5f);
            if (this.connectedWith != null) this.connectedWith.Disconnect();

            Destroy(this.gameObject);
        }
    }



	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        fireDelay -= Time.deltaTime;

        if(attachedTo!=null)
        if(attachedTo.tag == "PlayerShip")
        {
            var offset = new Vector2(this.transform.position.x - attachedTo.position.x, this.transform.position.y - attachedTo.position.y);
            var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle - 90);
        }
	}
}
