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
    public bool ready;

    public float timeScaleController;

    public Transform midPoint;

    void Awake()
    {
        instance = this;
        ready = true;
    }

    // Use this for initialization
	void Start () {
        line = VectorLine.SetLine(Color.white, new Vector2(this.transform.position.x,this.transform.position.y), new Vector2(source.position.x, source.position.y));
        line.SetWidth(0.15f);
        line.SetColor(new Color32(134,134,134,255));
        Canvas vectorCanvas = GameObject.Find("VectorCanvas").GetComponent<Canvas>();
        line.SetCanvas(vectorCanvas);
        vectorCanvas.renderMode = RenderMode.WorldSpace;        
	}
	
    void OnTriggerEnter(Collider coll)
    {
        WeaponModule module = coll.GetComponent<WeaponModule>();
        if (module !=null && attachedObject==null && module.attachedTo ==null && !ready)
        {
            timeScaleController = 0.1f;
            attachedObject = coll.gameObject;
            attachedObject.transform.parent = this.transform;
            module.attachedToHook = true;
            attachedObject.GetComponent<WeaponModule>().attachedTo = this.transform;
            extending = false;
            SoundPlayer.PlaySound(Sound.Grab);
        }
    }

    public static void Fire(Quaternion targetRotation)
    {
        if(instance.ready)
        {
            instance.transform.rotation = targetRotation;
            instance.extending = true;
            instance.midPoint.position = instance.transform.position;
        }
    }
    
	// Update is called once per frame
	void Update () {
        if (GameStateManager.GetState() != GameState.GameOver)
        {
            if (timeScaleController < 1f) timeScaleController += Time.deltaTime * 3;
            else timeScaleController = 1f;

            ///if()

            if(GameStateManager.GetState() != GameState.PreWave) Time.timeScale = timeScaleController;
            if (Player.instance != null)
            {
                if (extending)
                {
                    ready = false;
                    this.transform.position += transform.up * speed * Time.deltaTime;
                    if (Vector3.Distance(this.transform.position, source.position) > length)
                        extending = false;
                }
                else
                {
                    if (Vector3.Distance(this.transform.position, source.position) > 0f)
                    {
                        this.transform.position = Vector3.MoveTowards(this.transform.position, source.position, (attachedObject != null ? speed * 0.75f : speed) * Time.deltaTime);
                        midPoint.position = Vector3.MoveTowards(this.transform.position, source.position, Time.deltaTime * speed * 2f);
                    }

                    if (Vector3.Distance(this.transform.position, source.position) < 0.25f) ready = true;

                }


                line.points2.Clear();
                line.points2.Add(new Vector2(this.transform.position.x, this.transform.position.y));
                line.points2.Add(new Vector2(source.position.x, source.position.y));
                line.Draw();
            }
        }
	}
}
