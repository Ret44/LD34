using UnityEngine;
using System.Collections;

public class EnemyBehaviour : MonoBehaviour {
    
    protected Enemy ship;
	
    void Awake()
    {
        ship = this.GetComponent<Enemy>();
    }
    
    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}