using UnityEngine;
using System.Collections;

public enum Sound
{
    Cannon,
    Explosion1,
    Explosion2,
    Grab,
    Laser,
    Pew,
    Stack,
}


public class SoundPlayer : MonoBehaviour {

    public static SoundPlayer instance;

    public AudioSource cannon;
    public AudioSource explosion1;
    public AudioSource explosion2;
    public AudioSource grab;
    public AudioSource laser;
    public AudioSource pew;
    public AudioSource stack;


    public AudioSource musicTitle;
    public AudioSource musicGameplay;


    public static void PlaySound(Sound sound)
    {
        switch(sound)
        {
            case Sound.Cannon: instance.cannon.pitch = Random.Range(0.8f, 1.2f); instance.cannon.Play(); break;
            case Sound.Explosion1: instance.explosion1.pitch = Random.Range(0.8f, 1.2f); instance.explosion1.Play(); break;
            case Sound.Explosion2: instance.explosion2.pitch = Random.Range(0.8f, 1.2f); instance.explosion2.Play(); break;
            case Sound.Grab: instance.grab.pitch = Random.Range(0.8f, 1.2f); instance.grab.Play(); break;
            case Sound.Laser: instance.laser.pitch = Random.Range(0.8f, 1.2f); instance.laser.Play(); break;
            case Sound.Pew: instance.pew.pitch = Random.Range(0.8f, 1.2f); instance.pew.Play(); break;
            case Sound.Stack: instance.stack.pitch = Random.Range(0.8f, 1.2f); instance.stack.Play(); break;
            default: break;
        }
    }


    public static void PlayWeaponSound(WeaponType type)
    {
        switch(type)
        {
            case WeaponType.Bullet: SoundPlayer.PlaySound(Sound.Pew); break;
            case WeaponType.Plasma: SoundPlayer.PlaySound(Sound.Cannon); break;
            case WeaponType.Laser: SoundPlayer.PlaySound(Sound.Laser); break;
            //case WeaponType.Bullet: SoundPlayer.PlaySound(Sound.Cannon); break;
        }
    }
    void Awake()
    {
        instance = this;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
