using UnityEngine;
using System.Collections;

public enum EnemyType
{
    Fighter,
    Cruiser,
    Laser,
    Kamikaze
}

public class EnemySpawner : MonoBehaviour {
    
    public Transform spawnerLocation;

    public bool isSpawning;
    public int spawnCount;
    public float mainDelay;
    public float spawnDelay;
    public int spawnsLeft;
    private float delay;

    public EnemyType type;

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
            if (delay <= 0 && mainDelay <= 0)
            {
                if (type != EnemyType.Kamikaze)
                {
                    GameObject enemy = Instantiate(PrefabManager.GetEnemyPrefab(type), spawnerLocation.position, Quaternion.identity) as GameObject;
                    GameStateManager.instance.enemies.Add(enemy.transform);
                }
                else
                {
                    GameObject kami = Instantiate(PrefabManager.GetEnemyPrefab(type), Player.instance.transform.position, Quaternion.identity) as GameObject;
                    kami.transform.parent = Player.instance.transform;
                    GameStateManager.instance.enemies.Add(kami.transform);
                }
                delay = spawnDelay;
                spawnsLeft--;
            }

            delay -= Time.deltaTime;
            mainDelay -= Time.deltaTime;

            if (spawnsLeft == 0)
                isSpawning = false;
        }
	}
}
