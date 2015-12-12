using UnityEngine;
using System.Collections;

public class PrefabManager : MonoBehaviour {

    public static PrefabManager instance;
    [Header("Bullets")]
    public GameObject cannonBullet;
    public GameObject rocket;
    [Header("Modules")]
    public GameObject standardModule;
    [Header("Popups")]
    public GameObject textPopup;
//[Header("Enemies")]



    void Awake()
    {
        instance = this;
    }

    public static GameObject GetBulletPrefab(WeaponType type)
    {
        switch(type)
        {
            case WeaponType.Bullet: return instance.cannonBullet;
            case WeaponType.Rocket: return instance.rocket;
            default: return instance.cannonBullet;
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
