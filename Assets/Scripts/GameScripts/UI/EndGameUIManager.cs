using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndGameUIManager : MonoBehaviour {

    private GameObject wonBackground;
    private GameObject lostBackground;
    private GameObject menuButton;
    private GameObject desktopButton;

    private int playerTeam;
    private bool gameHasEnded;

    // Use this for initialization
    void Start () {
        wonBackground = GameObject.Find("Canvas/Finished Game Container/Won");
        lostBackground = GameObject.Find("Canvas/Finished Game Container/Lost");
        menuButton = GameObject.Find("Canvas/Finished Game Container/Menu Button");
        desktopButton = GameObject.Find("Canvas/Finished Game Container/Desktop Button");

        playerTeam = GameObject.Find("Canvas/Base").GetComponent<SpawnButtonManager>().playerTeam;
        gameHasEnded = false;
    }
	
	// Update is called once per frame
	void Update () {
        gameHasEnded = !(InterSceneGameVariables.gameVars.teamWon == 0);
        if (gameHasEnded)
        {
            showPopup();
        }
	}

    void showPopup()
    {
        if (InterSceneGameVariables.gameVars.teamWon == playerTeam)
        {
            wonBackground.GetComponent<Image>().enabled = true;
        }
        else
        {
            lostBackground.GetComponent<Image>().enabled = true;
        }
        menuButton.GetComponent<Button>().interactable = true;
        menuButton.GetComponent<Image>().enabled = true;
        desktopButton.GetComponent<Button>().interactable = true;
        desktopButton.GetComponent<Image>().enabled = true;
    }

    public void exitGameToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void exitGameToDesktop()
    {
        Application.Quit();
    }
}
