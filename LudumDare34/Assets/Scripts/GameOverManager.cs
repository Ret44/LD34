using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public enum GameOverPhase
{
    CountEnemy,
    CountWeapons,
    CountPoints,
    WaitToConnect,
    InputName,
    HighScores
}

public class GameOverManager : MonoBehaviour {


    public static GameOverManager instance;
    public bool scoresLoaded;
    private float awaitTime = 10f;

    public bool unlockControls;
    // Use this for initialization

    public GameOverPhase currentPhase;

    public Text leaderboardPositions;
    public Text leaderboardNames;
    public Text leaderboardEnemies;
    public Text leaderboardWeapons;
    public Text leaderboardScore;

    public List<string> names;
    public List<string> enemies;
    public List<string> weapons;
    public List<string> scores;

    [Header("Root objects")]
    public GameObject Scores;
    public GameObject Connection;
    public GameObject Leaderboard;

    [Header("ScorePhase")]
    public GameObject enemiesDestroyedLabel;
    public Text enemiesDestroyed;
    private int enemyCounter;
    public GameObject weaponsAttachedLabel;
    public Text weaponsAttached;
    private int weaponCounter;
    public GameObject pointsGainedLabel;
    public Text points;
    private int pointsCounter;


    [Header("ConnectionPhase")]
    public GameObject connectingLabel;
    public GameObject inputField;

    public string playerPos;

    void Awake()
    {
        instance = this;
    }
	void Start () {
       // dreamloLeaderBoard.GetSceneDreamloLeaderboard().LoadScores();
	}
    
    [ContextMenu ("Write scores")]    
    void GetLeaderboards()
    {
     //   dreamloLeaderBoard.GetSceneDreamloLeaderboard().LoadScores();
        string[] scores = dreamloLeaderBoard.GetSceneDreamloLeaderboard().ToStringArray();
        foreach (string score in scores)
        {
            string[] data = score.Split('|');
            Debug.Log(score);
            if(Convert.ToInt32(data[5])<10)
            {
                this.names.Add(data[0]);
                this.scores.Add(data[1]);
                this.enemies.Add(data[2]);
                this.weapons.Add(data[3]);
            }
            if(data[0]==inputField.GetComponent<InputField>().text)
            {
                playerPos = (Convert.ToInt32(data[5])+1).ToString();
            }
        }

        leaderboardNames.text = leaderboardEnemies.text = leaderboardPositions.text = leaderboardWeapons.text = leaderboardScore.text = "";
        foreach (string name in names) leaderboardNames.text += name + "\n";
        foreach (string enemy in enemies) leaderboardEnemies.text += enemy + "\n";
        foreach (string weapon in weapons) leaderboardWeapons.text += weapon + "\n";
        foreach (string score in this.scores) leaderboardScore.text += score + "\n";
        for (int i = 1; i < names.Count + 1; i++) leaderboardPositions.text += i.ToString() + "\n";

        if (Convert.ToInt32(playerPos) > 10)
        {
            leaderboardPositions.text += "...\n" + playerPos;
            leaderboardNames.text += "\n" + inputField.GetComponent<InputField>().text;
            leaderboardEnemies.text += "\n" + Player.enemiesDestroyed;
            leaderboardWeapons.text += "\n" + Player.weaponsAttached;
            leaderboardScore.text += "\n" + GameStateManager.instance.points;
        }

        leaderboardNames.gameObject.SetActive(true);
        leaderboardEnemies.gameObject.SetActive(true);
        leaderboardPositions.gameObject.SetActive(true);
        leaderboardWeapons.gameObject.SetActive(true);
        leaderboardScore.gameObject.SetActive(true);
        connectingLabel.SetActive(false);
    }
    
    public void AddScore()
    {
        Debug.Log("ADDIN");
        string playerName = inputField.GetComponent<InputField>().text;
        if (playerName == "") { playerName = "Unknown Hero"; inputField.GetComponent<InputField>().text = playerName; }
        Scores.SetActive(false);
        Connection.SetActive(false);
        Leaderboard.SetActive(true);
        currentPhase = GameOverPhase.WaitToConnect;
        dreamloLeaderBoard.GetSceneDreamloLeaderboard().AddScore(inputField.GetComponent<InputField>().text, GameStateManager.instance.points, Player.enemiesDestroyed, Player.weaponsAttached.ToString());
    }

	// Update is called once per frame
	void Update () {
	    if(currentPhase==GameOverPhase.CountEnemy)
        {
            enemiesDestroyedLabel.SetActive(true);       
            enemiesDestroyed.text = (enemyCounter).ToString();
            enemyCounter += 4;
            SoundPlayer.PlaySound(Sound.Pew);
            Debug.Log(Player.enemiesDestroyed.ToString());
            if (enemyCounter >= Player.enemiesDestroyed)
            {
                enemiesDestroyed.text = Player.enemiesDestroyed.ToString();
                currentPhase = GameOverPhase.CountWeapons;
            }
        }
        if (currentPhase == GameOverPhase.CountWeapons)
        {
            weaponsAttachedLabel.SetActive(true);
            weaponsAttached.text = (weaponCounter).ToString();
            weaponCounter += 4;
            Debug.Log(Player.weaponsAttached.ToString());
            if (weaponCounter >= Player.weaponsAttached)
            {
                weaponsAttached.text = Player.weaponsAttached.ToString();
                currentPhase = GameOverPhase.CountPoints;
            }
        }
        if (currentPhase == GameOverPhase.CountPoints)
        {
            pointsGainedLabel.SetActive(true);
            points.text = (pointsCounter).ToString();
            pointsCounter += 55;
            Debug.Log(GameStateManager.instance.points.ToString());
            if (pointsCounter >= GameStateManager.instance.points)
            {
                points.text = GameStateManager.instance.points.ToString();
                Connection.SetActive(true);
                currentPhase = GameOverPhase.InputName;
            }
        }

        if (currentPhase == GameOverPhase.WaitToConnect && scoresLoaded)
        {
            GetLeaderboards();
            currentPhase = GameOverPhase.HighScores;
        }
        //if (currentPhase == GameOverPhase.WaitToConnect && !scoresLoaded)
        //{
        //    awaitTime -= Time.deltaTime;
        //    if (awaitTime <= 0)
        //    {
        //        dreamloLeaderBoard.GetSceneDreamloLeaderboard().LoadScores();
        //        connectingLabel.GetComponent<Text>().text = "Retrying connection...";
        //    }
        //}
        //if (currentPhase == GameOverPhase.WaitToConnect && !scoresLoaded)
        //{
        //    currentPhase = GameOverPhase.InputName;
        //}

        
        
	}
}
