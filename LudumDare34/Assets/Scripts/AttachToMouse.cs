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
	}
}
