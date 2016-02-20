using UnityEngine;
using System.Collections;

public enum EnemyType
{
    Fighter,
    Cruiser,
    Laser,
    Laser2,
    Kamikaze,
    Shielder
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
                    if (type == EnemyType.Shielder)
                    {
                        var offset = new Vector2(Player.instance.transform.position.x - enemy.transform.position.x, Player.instance.transform.position.y - enemy.transform.position.y);
                        var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
                        enemy.transform.rotation = Quaternion.Euler(0, 0, angle - 90);
                    }
                    enemy.GetComponent<Enemy>().hp += (GameStateManager.instance.loop * 10);
                    GameStateManager.instance.enemies.Add(enemy.transform);
                    GameStateManager.instance.enemyCount++;
                }
                else
                {
                    GameObject kami = Instantiate(PrefabManager.GetEnemyPrefab(type), Player.instance.transform.position, Quaternion.identity) as GameObject;
                    kami.transform.parent = Player.instance.transform;
                    GameStateManager.instance.enemies.Add(kami.transform);
                    GameStateManager.instance.enemyCount++;
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
