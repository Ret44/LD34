using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletPoolManager : MonoBehaviour {

    public static BulletPoolManager instance;
   // public List<Projectile> shot;
    public List<Projectile> bulletPool;
    public List<Projectile> plasmaPool;
    public List<Projectile> laserPool;

    public int bulletPoolSize;
    public int plasmaPoolSize;
    public int laserPoolSize;

    [Header("Counter")]
    public int bulletsLive;
    public int plasmaLive;
    public int laserLive;

    // Use this for initialization
	void Awake () {
        instance = this;
        GameObject projTmp;
        for (int i = 0; i < bulletPoolSize; i++)
        {
            projTmp = Instantiate(PrefabManager.GetBulletPrefab(WeaponType.Bullet), Vector3.zero, Quaternion.identity) as GameObject;
            projTmp.transform.parent = this.transform;
            projTmp.name += "Bullet" + i.ToString();        
            projTmp.SetActive(false);
            bulletPool.Add(projTmp.GetComponent<Projectile>());
        }

        for (int i = 0; i < laserPoolSize; i++)
        {
            projTmp = Instantiate(PrefabManager.GetBulletPrefab(WeaponType.Laser), Vector3.zero, Quaternion.identity) as GameObject;
            projTmp.transform.parent = this.transform;
            projTmp.name += "Laser" + i.ToString();
            projTmp.SetActive(false);
            laserPool.Add(projTmp.GetComponent<Projectile>());
        }
        for (int i = 0; i < plasmaPoolSize; i++)
        {
            projTmp = Instantiate(PrefabManager.GetBulletPrefab(WeaponType.Plasma), Vector3.zero, Quaternion.identity) as GameObject;
            projTmp.transform.parent = this.transform;
            projTmp.name += "Plasma" + i.ToString();
            projTmp.SetActive(false);
            plasmaPool.Add(projTmp.GetComponent<Projectile>());
        }
	}

    public static Projectile DispatchProjectile(WeaponType type, Vector3 position, Quaternion rotation)
    {
        if (type == WeaponType.Bullet) return BulletPoolManager.DispatchBullet(position, rotation);
        else if (type == WeaponType.Laser) return BulletPoolManager.DispatchLaser(position, rotation);
        else return BulletPoolManager.DispatchPlasma(position, rotation);
    }

    public static Projectile DispatchBullet(Vector3 position, Quaternion rotation)
    {
        if (instance.bulletPool.Count == 0)
        {
            GameObject bulletTmp = Instantiate(PrefabManager.GetBulletPrefab(WeaponType.Bullet), Vector3.zero, Quaternion.identity) as GameObject;
            bulletTmp.transform.parent = instance.transform;
            bulletTmp.name += "BulletExtra" + instance.bulletPool.Count.ToString();
            bulletTmp.SetActive(false);
            Projectile proj = bulletTmp.GetComponent<Projectile>();
            proj.timer = proj.lifeTimer;
            return proj;
        }
        Projectile bullet = instance.bulletPool[0];
        instance.bulletPool.Remove(bullet);
        //instance.shot.Add(bullet);
        bullet.transform.position = position;
        bullet.transform.rotation = rotation;
        bullet.timer = bullet.lifeTimer;
        bullet.gameObject.SetActive(true);
        instance.bulletsLive++;
        return bullet;
    }
    public static Projectile DispatchLaser(Vector3 position, Quaternion rotation)
    {
        if (instance.laserPool.Count == 0)
        {
            GameObject bulletTmp = Instantiate(PrefabManager.GetBulletPrefab(WeaponType.Laser), Vector3.zero, Quaternion.identity) as GameObject;
            bulletTmp.transform.parent = instance.transform;
            bulletTmp.name += "BulletExtra" + instance.laserPool.Count.ToString();
            bulletTmp.SetActive(false);
            Projectile proj = bulletTmp.GetComponent<Projectile>();
            proj.timer = proj.lifeTimer;
            return proj;
        }
        Projectile bullet = instance.laserPool[0];
        instance.laserPool.Remove(bullet);
        //instance.shot.Add(bullet);
        bullet.transform.position = position;
        bullet.transform.rotation = rotation;
        bullet.timer = bullet.lifeTimer;
        bullet.gameObject.SetActive(true);
        instance.laserLive++;
        return bullet;
    }
    public static Projectile DispatchPlasma(Vector3 position, Quaternion rotation)
    {
        if (instance.plasmaPool.Count == 0)
        {
            GameObject bulletTmp = Instantiate(PrefabManager.GetBulletPrefab(WeaponType.Plasma), Vector3.zero, Quaternion.identity) as GameObject;
            bulletTmp.transform.parent = instance.transform;
            bulletTmp.name += "BulletExtra" + instance.plasmaPool.Count.ToString();
            bulletTmp.SetActive(false);
            Projectile proj = bulletTmp.GetComponent<Projectile>();
            proj.timer = proj.lifeTimer;
            return proj;
        }
        Projectile bullet = instance.plasmaPool[0];
        instance.plasmaPool.Remove(bullet);
        //instance.shot.Add(bullet);
        bullet.transform.position = position;
        bullet.transform.rotation = rotation;
        bullet.timer = bullet.lifeTimer;
        bullet.gameObject.SetActive(true);
        instance.plasmaLive++;
        return bullet;
    }
    public static void ReturnBullet(Projectile bullet)
    {
        bullet.gameObject.SetActive(false);
        if (bullet.type == WeaponType.Bullet)
        {
            instance.bulletsLive--;
            instance.bulletPool.Add(bullet);
        }
        if (bullet.type == WeaponType.Laser)
        {
            instance.laserLive--;
            instance.laserPool.Add(bullet);
        }
        if (bullet.type == WeaponType.Plasma)
        {
            instance.plasmaLive--;
            instance.plasmaPool.Add(bullet);
        }
    }

	// Update is called once per frame
	void Update () {
	
	}
}
