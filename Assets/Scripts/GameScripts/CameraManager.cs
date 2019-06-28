using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

    public float cameraScrollSpeed;
    private float mapWidth;
    private float worldSpaceWidth;

    private float mouseScrollBufferWidth;
    private float UIBarHeight;

    // Use this for initialization
    void Start () {
        mapWidth = GameObject.Find("Canvas/Base").GetComponent<SpawnButtonManager>().mapWidth;
        float worldSpaceHeight = Camera.main.orthographicSize * 2.0f;
        worldSpaceWidth = worldSpaceHeight * Camera.main.aspect;

        mouseScrollBufferWidth = Screen.width * 0.1f;
        UIBarHeight = Screen.height * 0.165f;
	}

	// Update is called once per frame
	void Update () {
        if (InterSceneGameVariables.gameVars.teamWon != 0) { return; }
        if (Input.GetKey(KeyCode.LeftArrow) || mouseInLeftScrollZone())
        {
            shiftX(-cameraScrollSpeed);
        }
        if (Input.GetKey(KeyCode.RightArrow) || mouseInRightScrollZone())
        {
            shiftX(cameraScrollSpeed);
        }
        // If camera is off left of map
        if (-(mapWidth / 2.0f) > transform.position.x - (worldSpaceWidth / 2.0f))
        {
            float newX = -(mapWidth / 2.0f) + (worldSpaceWidth / 2.0f);
            transform.position = new Vector3(newX, 0, -10);
        }
        // If camera is off right of map
        else if ((mapWidth / 2.0f) < transform.position.x + (worldSpaceWidth / 2.0f))
        {
            float newX = (mapWidth / 2.0f) - (worldSpaceWidth / 2.0f);
            transform.position = new Vector3(newX, 0, -10);
        }
    }

    void shiftX (float dx)
    {
        Vector3 currentPosition = transform.position;
        currentPosition.x += dx;
        transform.position = currentPosition;
    }

    bool mouseInLeftScrollZone()
    {
        return (Input.mousePosition.x < mouseScrollBufferWidth
            && Input.mousePosition.y > UIBarHeight);
    }

    bool mouseInRightScrollZone()
    {
        return (Input.mousePosition.x > Screen.width - mouseScrollBufferWidth
            && Input.mousePosition.y > UIBarHeight);
    }
}
