using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextBlink : MonoBehaviour {

    public Text ui;
    public Text shadow;
    private float x;
	// Use this for initialization
	void Start () {
        x = 0;
   //     ui = GetComponent<Text>();
  //      shadow = GetComponentInChildren<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        ui.color = new Color(ui.color.r, ui.color.g, ui.color.b, Mathf.Abs(Mathf.Sin(x)));
        shadow.color = new Color(shadow.color.r, shadow.color.g, shadow.color.b, Mathf.Abs(Mathf.Sin(x)));
        x += Time.deltaTime;
        if (Input.GetKey(KeyCode.Space)) Application.LoadLevel(1);
	}
}
