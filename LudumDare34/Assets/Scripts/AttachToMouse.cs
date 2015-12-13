using UnityEngine;
using System.Collections;

public class AttachToMouse : MonoBehaviour {


    void Awake()
    {
#if !UNITY_EDITOR
        Cursor.visible = false;
#endif
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
 //       Camera.main.transform.position = new Vector3((this.transform.position.x + Player.instance.transform.position.x) / 2, (this.transform.position.y + Player.instance.transform.position.y) / 2, -10f);
	}
}
