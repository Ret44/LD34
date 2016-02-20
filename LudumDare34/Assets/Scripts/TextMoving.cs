using UnityEngine;
using System.Collections;

public class TextMoving : MonoBehaviour {

    public float velocity;
    public float limit;
    public float start;

	// Use this for initialization
	void Start () {
        start = this.transform.position.x;
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.Translate(new Vector3(-Time.deltaTime * velocity, 0f, 0f));
        if (this.transform.position.x < limit) this.transform.position = new Vector3(start, this.transform.position.y, this.transform.position.z);
	}
}
