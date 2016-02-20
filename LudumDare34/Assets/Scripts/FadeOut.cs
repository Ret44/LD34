using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeOut : MonoBehaviour {

    public Image img;
    public Image img2;

    public float delay = 2f;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        delay -= Time.deltaTime;
        img.color = new Color(img.color.r, img.color.g, img.color.b, delay);
        img2.color = new Color(img2.color.r, img2.color.g, img2.color.b, delay);
	}
}
