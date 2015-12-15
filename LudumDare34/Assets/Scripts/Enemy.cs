using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour {

    public int hp;
    private int maxHP;
    public float velocity;
    public bool isShooting;
    public List<WeaponModule> modules;
    public Transform spriteTransform;
    private SpriteRenderer sprite;
    private float blink;
    public Transform modulesRoot;	// Use this for initialization

    void Awake()
    {
        blink = 1f;
        maxHP = hp;
        sprite = spriteTransform.GetComponent<SpriteRenderer>();
    }
	void Start () {
	
	}

    public void Hit(int dmg)
    {
        this.hp -= dmg;
        blink = 0.5f;
        if (this.hp < 0)
        {
            for (int i = 0; i < modules.Count;i++ )
            {
                if (modules[i] != null)
                {
                 //   modules[i].attachedTo = null;
                  //  modules[i].transform.parent = null;
                    modules[i].Disconnect();
                }
            }
            PrefabManager.DeployExplosionParticles((this.tag == "EnemyShip" ? this.transform.position : this.spriteTransform.position), 1f);
            GameStateManager.instance.enemies.Remove(this.transform);
            SoundPlayer.PlaySound(Sound.Explosion1);
            Destroy(this.gameObject);
        }
    }

    public void Fire()
    {
        for(int i =0; i<modules.Count;i++)
        {
           if(modules[i]!=null) modules[i].Shoot(ProjectileOwner.Enemy, spriteTransform.rotation);
        }
    }

	// Update is called once per frame
	void Update () {
        if(blink<1f) {       
            blink += Time.deltaTime * 2;
        } else  blink = 1f;

        sprite.color = new Color(1f, blink, blink);
	    //spriteTransform.
	}
}
