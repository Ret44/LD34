using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using InControl;

public enum ShipState
{
    Running,
    Dying
}

public enum ControlMode
{
    Controller,
    Keyboard
}

public class Player : MonoBehaviour {

    public static Player instance;

    public GameObject crosshair;
    public SpriteRenderer crosshairSprite;
    public int HP;
    public int MaxHP;
    private Text hullUI;
    public float velocity;
    public float shootVelocity;
    public bool isShooting;
    public List<WeaponModule> modules;
    public Transform spriteTransform;

    public static int enemiesDestroyed;
    public static int weaponsAttached;

    public ControlMode controlMode;

    public Transform modulesRoot;

    public Hook hook;
    public
    void Awake()
    {
        instance = this;
        //this.crosshair = GameObject.Find("crosshair") as GameObject;        
        hullUI = GameObject.Find("hullUI").GetComponent<Text>();
        if (controlMode == ControlMode.Controller)
        {
            crosshairSprite.transform.position = new Vector3(0f, 3f, 0f);
            crosshair.GetComponent<AttachToMouse>().enabled = false;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag=="Kamikaze")
        {
            Enemy enemy = col.transform.parent.GetComponent<Enemy>();
            int playerDmg = 250;
            int enemyDmg = Player.instance.HP;
            enemy.Hit(enemyDmg);
            Player.Hit(playerDmg);
        }
        if (col.gameObject.tag == "EnemyShip")
        {
            Enemy enemy = col.GetComponent<Enemy>();
            int playerDmg = 100;//enemy.hp;
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
        if (GameStateManager.GetState() == GameState.PreWave) GameStateManager.instance.currentState = GameState.Wave;
        instance.HP += 5;
        instance.hullUI.text = (instance.HP * 100 / instance.MaxHP).ToString() + "%";
        GameStateManager.instance.points += 50;
        weaponsAttached++;
        return true;
    }

    public static void Hit(int dmg)
    {
        instance.HP -= dmg;
        instance.hullUI.text = (instance.HP * 100 / instance.MaxHP).ToString() + "%";

        CameraScreenShake.Shake((float)dmg/50);

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
            GameStateManager.instance.currentState = GameState.GameOver;
            Time.timeScale = 0.5f;
            GameStateManager.instance.gameOverUI.SetActive(true);
            instance.crosshair.transform.parent = null;
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
       // Debug.Log(move.ToString());
        Vector3 movementVec = transform.position + (move * (isShooting?shootVelocity:velocity) * Time.deltaTime);
        if (movementVec.x > -9.5f && movementVec.x < 9.5f && movementVec.y > -6.5f && movementVec.y < 6.5f) 
        transform.position = movementVec;
        
       if(controlMode == ControlMode.Controller) 
        {
            float x = InputManager.ActiveDevice.RightStick.Value.x;
            float y = InputManager.ActiveDevice.RightStick.Value.y;
            float aim_angle = 0.0f;
            bool aiming_right = false;
            bool aiming_up = false;

            float R_analog_threshold = 0.20f;

            if (Mathf.Abs(x) < R_analog_threshold) { x = 0.0f; }
            if (Mathf.Abs(y) < R_analog_threshold) { y = 0.0f; }

            if (x != 0.0f || y != 0.0f)
            {
                aim_angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
                this.transform.rotation = Quaternion.AngleAxis(aim_angle, Vector3.forward);
            }
        }

       if (Input.GetMouseButton(0) || InputManager.ActiveDevice.LeftTrigger.IsPressed ) 
        {
            for (int i = 0; i < modules.Count; i++)
                modules[i].Shoot(ProjectileOwner.Player, spriteTransform.rotation);
        }

       isShooting = !(Input.GetMouseButtonDown(0)) && !InputManager.ActiveDevice.LeftTrigger.IsPressed;

        if (Input.GetButton("Fire2") ||InputManager.ActiveDevice.RightBumper.IsPressed)
        {
            Hook.Fire(spriteTransform.rotation);
        }
    }

}
