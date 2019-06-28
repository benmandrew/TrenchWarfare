using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrenchSpawner : MonoBehaviour {

    private int trenchNum;
    private int mapWidth;
    private float trenchGap;

    public GameObject trenchPrefab;

	// Use this for initialization
	void Start () {
        trenchNum = InterSceneGameVariables.gameVars.trenchNum;
        mapWidth = InterSceneGameVariables.gameVars.mapSize;
        if (trenchNum != 0)
        {
            trenchGap = mapWidth / (trenchNum + 2);
            spawnTrenches();
        }
	}
	
    void spawnTrenches()
    {
        float scale = 2.725f;
        Vector3 position = new Vector3();
        GameObject trench;
        GameObject trenchContainer = GameObject.Find("Trenches");
        float newX;
        for (float i = 0; i < trenchNum; i++)
        {
            newX = (i - ((trenchNum - 1) / 2.0f)) * trenchGap;
            position.x = newX;
            trench = Instantiate(trenchPrefab, position, transform.rotation);
            trench.transform.localScale = new Vector3(scale, scale, scale);
            trench.transform.SetParent(trenchContainer.transform);
        }
    }
}
