using UnityEngine;
using System.Collections;

public enum WeaponType {
    Bullet,
    Rocket,
    Laser
}

public class WeaponModule : MonoBehaviour {

    public string name;
    public int hp;
    public WeaponType type;
    public int damage;
    public float fireRate;
    private float fireDelay;

    public bool coreModule;
  
	// Use this for initialization

    public void Shoot(BulletOwner owner, Quaternion shipRotation)
    {
        if(fireDelay<=0)
        {
            Debug.Log("Module " + name + " shoots bang bang");
            GameObject bulletObj = Instantiate(PrefabManager.GetBulletPrefab(type), this.transform.position, (coreModule?shipRotation:this.transform.localRotation)) as GameObject;
            fireDelay = fireRate;
        }
    }


	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        fireDelay -= Time.deltaTime;
	}
}
