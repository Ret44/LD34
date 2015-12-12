using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

    public GameObject crosshair;
    public float velocity;
    public float shootVelocity;
    public bool isShooting;
    public List<WeaponModule> modules;
    private Transform spriteTransform;
    void Awake()
    {
        this.crosshair = GameObject.Find("crosshair") as GameObject;
        this.spriteTransform = this.transform.GetChild(0);
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        var move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        transform.position += move * (isShooting?shootVelocity:velocity) * Time.deltaTime;
     
        if(Input.GetMouseButton(0))
        {
            for (int i = 0; i < modules.Count; i++)
                modules[i].Shoot(BulletOwner.Player, spriteTransform.rotation);
        }
        
        isShooting = !(Input.GetMouseButtonDown(0));        
    }

}
