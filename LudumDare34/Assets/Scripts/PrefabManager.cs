﻿using UnityEngine;
using System.Collections;

public class PrefabManager : MonoBehaviour {

    public static PrefabManager instance;

    [Header("Bullets")]
    public GameObject cannonBullet;
    public GameObject plasmaBullet;
    public GameObject rocket;
    public GameObject laserBeam;
    [Header("Modules")]
    public GameObject standardModule;
    [Header("Popups")]
    public GameObject textPopup;
    [Header("Particles")]
    public GameObject hitParticles;
    public GameObject explosionParticles;
    [Header("Enemies")]
    public GameObject fighter;
    public GameObject cruiser;
    

    void Awake()
    {
        instance = this;
    }

    public static void DeployHitParticles(Vector3 position)
    {
        GameObject particles = Instantiate(instance.hitParticles, new Vector3(position.x,position.y,-5f), Quaternion.identity) as GameObject;
        Destroy(particles, 1f);
    }

    public static void DeployExplosionParticles(Vector3 position, float scale = 1f)
    {
        GameObject particles = Instantiate(instance.explosionParticles, new Vector3(position.x, position.y, -5f), Quaternion.identity) as GameObject;
        particles.transform.localScale = new Vector3(scale, scale, scale);
        Destroy(particles, 5f);
    }
    public static GameObject GetBulletPrefab(WeaponType type)
    {
        switch(type)
        {
            case WeaponType.Bullet: return instance.cannonBullet;
            case WeaponType.Rocket: return instance.rocket;
            case WeaponType.Plasma: return instance.plasmaBullet;
            case WeaponType.Laser: return instance.laserBeam;
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
