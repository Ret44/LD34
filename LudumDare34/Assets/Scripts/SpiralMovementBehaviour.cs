using UnityEngine;
using System.Collections;

public class SpiralMovementBehaviour : MonoBehaviour {

    public Enemy main;
    public float velocity;
  //  public Enemy
        
	// Use this for initialization
	void Start () {
        main = GetComponent<Enemy>();
	}
	
	// Update is called once per frame
    void Update()
    {
        Transform ship = main.spriteTransform;
        //main.transform.rotation = new Quaternion(main.transform.rotation.x, main.transform.rotation.y, main.transform.rotation.z + (Time.deltaTime * velocity), main.transform.rotation.w);
        //main.transform.RotateAround(this.transform.position, Vector3.forward, velocity * Time.deltaTime);
        //ship.position = new Vector3(ship.position.x, ship.position.y - (Time.deltaTime * velocity/2), ship.position.z);
        ship.position = new Vector3(ship.position.y * Mathf.Cos(Time.deltaTime * velocity),ship.position.y * Mathf.Sin(Time.deltaTime * velocity));
        //unit += Time.deltaTime;
    }
}
