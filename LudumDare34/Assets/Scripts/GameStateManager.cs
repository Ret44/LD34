using UnityEngine;
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
    public string enemy;
    public int count;
    public int spawnerID;
    
    public WaveParam(string enemy, int count, int spawnerID)
    {
        this.enemy = enemy; this.count = count; this.spawnerID = spawnerID;
    }
}

public class GameStateManager : MonoBehaviour {

    public static GameStateManager instance;
    public List<EnemySpawner> spawners;
    public GameState currentState;
    public float points;
    public int wave;
    public float multilpier;
    public List<List<WaveParam>> waves;

    public List<Transform> enemies;
	// Use this for initialization

    public static GameState GetState()
    {
        return instance.currentState;
    }
    void Awake()
    {
        instance = this;


        waves = new List<List<WaveParam>>();

        List<WaveParam> tmpList = new List<WaveParam>();
        tmpList.AddMany(new WaveParam("Fighter", 3, 1));
        waves.Add(tmpList);

        tmpList = new List<WaveParam>();
        tmpList.AddMany(new WaveParam("Cruiser", 1, 1), new WaveParam("Cruiser",1,2));
        waves.Add(tmpList);

        tmpList = new List<WaveParam>();
        tmpList.AddMany(new WaveParam("Kamikaze", 10, 1), new WaveParam("Fighter", 2, 1));
        waves.Add(tmpList);


        //tmpList = new List<WaveParam>();
        //tmpList.AddMany(new WaveParam("Fighter", 3, 1), new WaveParam("Cruiser", 1, 1), new WaveParam("Cruiser", 1, 2));
        //waves.Add(tmpList);

        //tmpList = new List<WaveParam>();
        //tmpList.AddMany(new WaveParam("Laser", 3, 0), new WaveParam("Fighter", 3, 2));
        //waves.Add(tmpList);

     

    }
	void Start () {
        wave = -1;
	}
    
    public void StartNextWave()
    {
        this.wave++;
        if (this.wave > waves.Count) this.wave = 0;
        List<WaveParam> wave = waves[this.wave];
            foreach (WaveParam param in wave)
            {
                spawners[param.spawnerID].spawnsLeft += param.count;
                spawners[param.spawnerID].type = (EnemyType)Enum.Parse(typeof(EnemyType), param.enemy);
                spawners[param.spawnerID].isSpawning = true;
            }
            this.currentState = GameState.Wave;
    }

	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.N) && enemies.Count == 0) StartNextWave();
        if (Input.GetKey(KeyCode.R)) Application.LoadLevel("mainscene");
	}
}
