using UnityEngine;
using System.Collections;

public enum BulletOwner {
    Player,
    Enemy
}

public class Bullet : MonoBehaviour {


    public float speed;
    public float damage;
    public BulletOwner owner;
    public float lifeTimer;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.position += transform.up * speed * Time.deltaTime;
        this.lifeTimer -= Time.deltaTime;
        if (this.lifeTimer < 0f) Destroy(this.gameObject);
	}
}
