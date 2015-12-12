using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

    public static Player instance;

    public GameObject crosshair;
    public float velocity;
    public float shootVelocity;
    public bool isShooting;
    public List<WeaponModule> modules;
    public Transform spriteTransform;

    public Transform modulesRoot;

    public Hook hook;
    
    void Awake()
    {
        instance = this;
        this.crosshair = GameObject.Find("crosshair") as GameObject;
    }

    //void OnTriggerEnter(Collider col)
    //{
    //    Debug.Log("Kolizja");
    //    if (col.gameObject.tag == "Module")
    //    {
    //        WeaponModule newModule = col.gameObject.GetComponent<WeaponModule>();
    //        if(newModule.attachedTo!=null) newModule.attachedTo.GetComponent<Hook>().attachedObject = null;
    //        newModule.transform.parent = modulesRoot;
    //        newModule.attachedTo = this.transform;
    //        modules.Add(newModule);
                        
    //    }
    //}

    public static bool AddModule(WeaponModule newModule)
    {

        for(int i=0;i<instance.modules.Count;i++)
        {
            if (instance.modules[i] == newModule) return false;
        }
        instance.modules.Add(newModule);
        GameObject newPopup = Instantiate(PrefabManager.instance.textPopup, new Vector3(instance.transform.position.x, instance.transform.position.y, -5f), Quaternion.identity) as GameObject;
        newPopup.GetComponent<Popup>().mesh.text = newModule.name;
        return true;
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKey(KeyCode.R)) Application.LoadLevel("mainscene");


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
