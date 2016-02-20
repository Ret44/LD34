using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextBlink : MonoBehaviour {

    public Text ui;
    public Text shadow;
    public bool notContinue;
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
       if(shadow!=null) shadow.color = new Color(shadow.color.r, shadow.color.g, shadow.color.b, Mathf.Abs(Mathf.Sin(x)));
        x += Time.deltaTime;
        if (!notContinue && Input.GetKey(KeyCode.Space)) Application.LoadLevel(1);
	}
}
