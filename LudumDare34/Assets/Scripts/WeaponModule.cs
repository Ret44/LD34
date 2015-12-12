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

    public SpriteRenderer sprite;
    public Transform tip;

    public Transform attachedTo;

    public bool coreModule;
  
	// Use this for initialization

    public void Shoot(BulletOwner owner, Quaternion shipRotation)
    {
        if(fireDelay<=0)
        {
            Debug.Log("Module " + name + " shoots bang bang");
            GameObject bulletObj = Instantiate(PrefabManager.GetBulletPrefab(type), tip.position, (coreModule?shipRotation:this.transform.rotation)) as GameObject;
            fireDelay = fireRate;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "PlayerShip")
        {
          //  WeaponModule newModule = col.GetComponent<Collider>().gameObject.GetComponent<WeaponModule>();
            if (Hook.instance.attachedObject == this.gameObject) Hook.instance.attachedObject = null;
            this.transform.parent = Player.instance.modulesRoot;
            this.attachedTo = Player.instance.transform;
            Player.instance.modules.Add(this);
            GameObject newPopup = Instantiate(PrefabManager.instance.textPopup, new Vector3(this.transform.position.x, this.transform.position.y, -5f), Quaternion.identity) as GameObject;
            newPopup.GetComponent<Popup>().mesh.text = this.name;
        }

        if(col.gameObject.tag == "Module")
        {
            WeaponModule other = col.GetComponent<WeaponModule>();
            if(other != null)
            if(other.attachedTo.tag=="PlayerShip")
            {
                if (Hook.instance.attachedObject == this.gameObject) Hook.instance.attachedObject = null;
                this.transform.parent = Player.instance.modulesRoot;
                this.attachedTo = Player.instance.transform;
                Player.instance.modules.Add(this);
                GameObject newPopup = Instantiate(PrefabManager.instance.textPopup, new Vector3(this.transform.position.x, this.transform.position.y, -5f), Quaternion.identity) as GameObject;
                newPopup.GetComponent<Popup>().mesh.text = this.name;
            }
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
