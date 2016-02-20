using UnityEngine;
using System.Collections;

public class Cover : MonoBehaviour {

    public SpriteRenderer sprite;

	// Use this for initialization
	void Start () {
	    sprite = this.GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        sprite.color = new Color(0f, 0f, 0f, 1f-(Time.timeScale<0.5f?0.5f:Time.timeScale));
	}
}
