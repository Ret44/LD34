using UnityEngine;
using System.Collections;

public class Popup : MonoBehaviour {

    public TextMesh mesh;
    public float delay;
    	// Use this for initialization
	void Start () {
        mesh = this.GetComponent<TextMesh>();
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.position += Vector3.up * 1f * Time.deltaTime;
        delay -= Time.deltaTime;
        if (delay < 0.5f)
        {
            if(this.mesh.color == new Color(0f,0f,0f))
               this.mesh.color = new Color(1f,1f,1f);
            else if (this.mesh.color == new Color(1f, 1f, 1f))
                this.mesh.color = new Color(0f, 0f, 0f);
        }
        if (delay <= 0) Destroy(this.gameObject);
	}
}
