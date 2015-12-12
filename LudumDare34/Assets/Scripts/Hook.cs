using UnityEngine;
using System.Collections;
using Vectrosity;

public class Hook : MonoBehaviour {

    public static Hook instance;
    public Transform source;
    public float length;
    public bool extending;
    public float speed;
    public GameObject attachedObject;
    private VectorLine line;

    void Awake()
    {
        instance = this;
    }

    // Use this for initialization
	void Start () {
        line = VectorLine.SetLine(Color.white, new Vector2(this.transform.position.x,this.transform.position.y), new Vector2(source.position.x, source.position.y));
        line.SetWidth(0.15f);
        Canvas vectorCanvas = GameObject.Find("VectorCanvas").GetComponent<Canvas>();
        line.SetCanvas(vectorCanvas);
        vectorCanvas.renderMode = RenderMode.WorldSpace;        
	}
	
    void OnTriggerEnter(Collider coll)
    {
        WeaponModule module = coll.GetComponent<WeaponModule>();
        if (module !=null && attachedObject==null && module.attachedTo ==null)
        {
            attachedObject = coll.gameObject;
            attachedObject.transform.parent = this.transform;
            attachedObject.GetComponent<WeaponModule>().attachedTo = this.transform;
            extending = false;
        }
    }
    
	// Update is called once per frame
	void Update () {


        if(extending)
        {
            this.transform.position += transform.up * speed * Time.deltaTime;
            if (Vector3.Distance(this.transform.position, source.position)>length)
                extending = false;
        }
        else
        {
            if (Vector3.Distance(this.transform.position, source.position) > 0)
                this.transform.position = Vector3.MoveTowards(this.transform.position, source.position, (attachedObject!=null?speed*0.75f:speed) * Time.deltaTime);
        }


        line.points2.Clear();
        line.points2.Add(new Vector2(this.transform.position.x, this.transform.position.y));
        line.points2.Add(new Vector2(source.position.x, source.position.y));
        line.Draw();

	}
}
