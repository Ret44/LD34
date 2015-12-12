using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour {

    public int hp;
    public float velocity;
    public bool isShooting;
    public List<WeaponModule> modules;
    public Transform spriteTransform;
    public Transform modulesRoot;	// Use this for initialization
	void Start () {
	
	}

    public void Hit(int dmg)
    {
        this.hp -= dmg;
        if (this.hp < 0)
        {
            for (int i = 0; i < modules.Count;i++ )
            {
                if (modules[i] != null)
                {
                    modules[i].attachedTo = null;
                    modules[i].transform.parent = null;
                }
            }
            PrefabManager.DeployExplosionParticles(this.transform.position, 1f);
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
	
	}
}
