using UnityEngine;
using System.Collections;

public class FighterSpawner : MonoBehaviour {
    
    public Transform spawnerLocation;

    public bool isSpawning;
    public int fightersCount;
    public float spawnDelay;
    public int spawnsLeft;
    private float delay;

    public void Spawn(int count)
    {
        spawnsLeft = count;
        isSpawning = true;
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if(isSpawning)
        {
            if (delay <= 0)
            {
                Instantiate(PrefabManager.instance.fighter, spawnerLocation.position, Quaternion.identity);
                delay = spawnDelay;
                spawnsLeft--;
            }

            delay -= Time.deltaTime;

            if (spawnsLeft == 0)
                isSpawning = false;
        }
	}
}
