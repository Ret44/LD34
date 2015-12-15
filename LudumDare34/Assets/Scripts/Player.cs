using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public enum ShipState
{
    Running,
    Dying
}

public class Player : MonoBehaviour {

    public static Player instance;

    public GameObject crosshair;
    public int HP;
    public int MaxHP;
    private Text hullUI;
    public float velocity;
    public float shootVelocity;
    public bool isShooting;
    public List<WeaponModule> modules;
    public Transform spriteTransform;

    public Transform modulesRoot;

    public Hook hook;
    public
    void Awake()
    {
        instance = this;
        this.crosshair = GameObject.Find("crosshair") as GameObject;
        hullUI = GameObject.Find("hullUI").GetComponent<Text>();
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag=="Kamikaze")
        {
            Enemy enemy = col.transform.parent.GetComponent<Enemy>();
            int playerDmg = 50;
            int enemyDmg = Player.instance.HP;
            enemy.Hit(enemyDmg);
            Player.Hit(playerDmg);
        }
        if (col.gameObject.tag == "EnemyShip")
        {
            Enemy enemy = col.GetComponent<Enemy>();
            int playerDmg = enemy.hp;
            int enemyDmg = Player.instance.HP;
            enemy.Hit(enemyDmg);
            Player.Hit(playerDmg);
        }
        if (col.gameObject.tag == "Module")
        {
            WeaponModule module = col.gameObject.GetComponent<WeaponModule>();
            if(module.attachedTo!=null)
                if(module.attachedTo.tag=="EnemyShip")
                {
                    Player.Hit(Mathf.Abs(module.hp));
                    module.Hit(Player.instance.HP);                    
                }
        }
    }

    public static bool AddModule(WeaponModule newModule)
    {

        for(int i=0;i<instance.modules.Count;i++)
        {
            if (instance.modules[i] == newModule) return false;
        }
        instance.modules.Add(newModule);
        GameObject newPopup = Instantiate(PrefabManager.instance.textPopup, new Vector3(instance.transform.position.x, instance.transform.position.y, -5f), Quaternion.identity) as GameObject;
        newPopup.GetComponent<Popup>().mesh.text = newModule.name;
        SoundPlayer.PlaySound(Sound.Stack);
        return true;
    }

    public static void Hit(int dmg)
    {
        instance.HP -= dmg;

        if (instance.HP <= 0)
        {
            foreach(WeaponModule module in instance.modules)
            {
                module.transform.parent = null;
                module.deathTimer = Random.Range(0f, 1f);
                module.attachedTo = null;
            }
            PrefabManager.DeployExplosionParticles(instance.transform.position,1.2f);
            GameObject[] kamikazis = GameObject.FindGameObjectsWithTag("Kamikaze");
            foreach(GameObject kamikaze in kamikazis)
            {
                kamikaze.transform.parent.parent = null;
                kamikaze.transform.parent.GetComponent<Enemy>().Hit(9999);
            }
            Destroy(instance.gameObject);
        }
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        
        hullUI.text = (this.HP * 100 / MaxHP).ToString() + "%";

        var move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        transform.position += move * (isShooting?shootVelocity:velocity) * Time.deltaTime;
     
        if(Input.GetMouseButton(0))
        {
            for (int i = 0; i < modules.Count; i++)
                modules[i].Shoot(ProjectileOwner.Player, spriteTransform.rotation);
        }
        
        isShooting = !(Input.GetMouseButtonDown(0));        

        if(Input.GetMouseButton(1))
        {
            Hook.Fire(spriteTransform.rotation);
        }
    }

}
