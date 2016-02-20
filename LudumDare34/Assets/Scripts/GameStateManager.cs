using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public static class ListExtenstions
{
    public static void AddMany<T>(this List<T> list, params T[] elements)
    {
        list.AddRange(elements);
    }
}

public enum GameState
{
    PreWave,
    Wave,
    GameOver
}


public struct WaveParam
{
    public string name;
    public string enemy;
    public int count;
    public int spawnerID;
    
    public WaveParam(string name, string enemy, int count, int spawnerID)
    {
        this.name = name; this.enemy = enemy; this.count = count; this.spawnerID = spawnerID;
    }
}

public class GameStateManager : MonoBehaviour {

    public static GameStateManager instance;
    public List<EnemySpawner> spawners;
    public GameState currentState;
    public int points;
    public int wave;
    public float multilpier;
    public List<List<WaveParam>> waves;
    public GameObject gameOverUI;

    public Text pts;
    
    public int enemyCount;

    public float timeBetweenWaves;
    private float timeDelay;

    public int loop;
    private float wavedelay = 2f;
    public List<Transform> enemies;
	// Use this for initialization

    public static GameState GetState()
    {
        return instance.currentState;
    }
    void Awake()
    {
        instance = this;

        timeDelay = timeBetweenWaves;

        loop = 0; 
        enemyCount = 0;

        waves = new List<List<WaveParam>>();

        List<WaveParam> tmpList = new List<WaveParam>();
        tmpList.AddMany(new WaveParam("OneFighter","Fighter", 3, 1));
        waves.Add(tmpList);

        tmpList = new List<WaveParam>();
        tmpList.AddMany(new WaveParam("2Cruiser1", "Cruiser", 1, 1), new WaveParam("2Cruiser2", "Cruiser", 1, 2), new WaveParam("2Cruiser3", "Cruiser", 1, 3));
        waves.Add(tmpList);

        tmpList = new List<WaveParam>();
        tmpList.AddMany(new WaveParam("3Shielder1", "Shielder", 1, 0), new WaveParam("3Shielder2", "Shielder", 1, 2),  new WaveParam("3Shielder3", "Shielder", 1, 3));
        waves.Add(tmpList);

        tmpList = new List<WaveParam>();
        tmpList.AddMany(new WaveParam("3Kamikaze10", "Kamikaze", 20, 1), new WaveParam("3Fighter2","Fighter", 2, 2));
        waves.Add(tmpList);
        
        tmpList = new List<WaveParam>();
        tmpList.AddMany(new WaveParam("4Fighter3", "Fighter", 3, 1), new WaveParam("4Cruiser1", "Cruiser", 1, 1), new WaveParam("4Cruiser2", "Cruiser", 1, 2));
        waves.Add(tmpList);

        tmpList = new List<WaveParam>();
        tmpList.AddMany(new WaveParam("5Laser", "Laser", 3, 0), new WaveParam("5Figther", "Fighter", 3, 2));
        waves.Add(tmpList);

        tmpList = new List<WaveParam>();
        tmpList.AddMany(new WaveParam("3Kamikaze10", "Kamikaze", 10, 1), new WaveParam("Laser1", "Laser", 2, 0));
        waves.Add(tmpList);


        tmpList = new List<WaveParam>();
        tmpList.AddMany(new WaveParam("3Shielder1", "Shielder", 1, 0), new WaveParam("3Shielder2", "Shielder", 1, 2), new WaveParam("3Shielder3", "Shielder", 1, 3));
        waves.Add(tmpList);


        tmpList = new List<WaveParam>();
        tmpList.AddMany(new WaveParam("5Laser", "Cruiser", 1, 1), new WaveParam("4Cruiser2", "Cruiser", 1, 2), new WaveParam("4Cruiser2", "Cruiser", 1, 3), new WaveParam("5Figther", "Kamikaze", 30, 1));
        waves.Add(tmpList);

        tmpList = new List<WaveParam>();
        tmpList.AddMany(new WaveParam("4Fighter3", "Fighter", 15, 1), new WaveParam("5Figther", "Kamikaze", 30, 1));
        waves.Add(tmpList);


    }
	void Start () {
        wave = -1;
        Time.timeScale = 0.4f;
	}
    
    public void StartNextWave()
    {

        this.wave++;
             //   Debug.Log("Starting new wave no."+this.wave.ToString() + " )
        if (this.wave >= waves.Count) { this.wave = 0; loop++; }
        List<WaveParam> wave = waves[this.wave];
            foreach (WaveParam param in wave)
            {
                if (!spawners[param.spawnerID].isSpawning)
                {
                  //  Debug.Log(param.name + "," + param.enemy);
                    spawners[param.spawnerID].spawnsLeft += param.count + (param.count*loop);
                    switch (param.enemy)
                    {
                        default: spawners[param.spawnerID].type = (EnemyType)Enum.Parse(typeof(EnemyType), param.enemy); break;
                        case "Fighter": spawners[param.spawnerID].type = EnemyType.Fighter; break;
                        case "Cruiser": spawners[param.spawnerID].type = EnemyType.Cruiser; break;
                        case "Kamikaze": spawners[param.spawnerID].type = EnemyType.Kamikaze; break;
                        case "Laser": spawners[param.spawnerID].type = EnemyType.Laser; break;
                        case "Shielder": spawners[param.spawnerID].type = EnemyType.Shielder; break;
                    }
                    spawners[param.spawnerID].type = (EnemyType)Enum.Parse(typeof(EnemyType), param.enemy);
                    spawners[param.spawnerID].isSpawning = true;
                }
            }
            this.currentState = GameState.Wave;
    }

	// Update is called once per frame
	void Update () {
        if (this.currentState == GameState.Wave)
        {
            wavedelay -= Time.deltaTime;
            timeDelay -= Time.deltaTime;
            if((wavedelay < 0 && enemyCount<=1 )||( wavedelay < 0 && timeDelay < 0))
            {
                wavedelay = 4f;
                StartNextWave();
                timeDelay = timeBetweenWaves;
            }           
        }
        if (Input.GetKey(KeyCode.Space))
        {
            Player.enemiesDestroyed = 0;
            Player.weaponsAttached = 0;
            Application.LoadLevel(1);
        }
        if (Input.GetKey(KeyCode.Escape)) Application.Quit();
        pts.text = points.ToString();
	}
}
