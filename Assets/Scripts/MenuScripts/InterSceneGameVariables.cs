using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterSceneGameVariables : MonoBehaviour {

    public int side;
    public float difficulty; // Simple multiplier for spawn rates of enemy
    public int mapSize;
    public int trenchNum;
    public int teamWon;

    public static InterSceneGameVariables gameVars;

    // Use this for initialization
    void Start () {
        if (gameVars == null)
        {
            DontDestroyOnLoad(gameObject);
            gameVars = this;
            side = 1;
            difficulty = 2.0f;
            mapSize = 60;
            trenchNum = 0;
            teamWon = 0;
        }
        else if (gameVars != this)
        {
            Destroy(gameObject);
        }
    }

    public void setSide(int newSide)
    {
        side = newSide;
    }

    public void setDifficulty(float newDifficulty)
    {
        difficulty = newDifficulty;
    }

    public void setMapSize(int newMapSize)
    {
        mapSize = newMapSize;
    }

    public void setTrenchNum(int newTrenchNum)
    {
        trenchNum = newTrenchNum;
    }

    public void setTeamWon(int newTeamWon)
    {
        teamWon = newTeamWon;
    }
}
