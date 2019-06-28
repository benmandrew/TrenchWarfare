using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionButtonsManager : MonoBehaviour {

    private GameObject exitButton;
    private GameObject exitWarningYesButton;
    private GameObject exitWarningNoButton;
    private GameObject exitWarningBackground;
    private bool showExitWarning;

    private GameObject soundButton;
    private Animator soundAnim;
    private bool soundOn;

	// Use this for initialization
	void Start () {
        exitButton = GameObject.Find("Canvas/Exit Button");
        exitWarningYesButton = GameObject.Find("Canvas/Exit Warning Container/Yes Button");
        exitWarningNoButton = GameObject.Find("Canvas/Exit Warning Container/No Button");
        exitWarningBackground = GameObject.Find("Canvas/Exit Warning Container/Background");
        showExitWarning = false;
        soundButton = GameObject.Find("Canvas/Sound Button");
        soundAnim = GameObject.Find("Canvas/Sound Button").GetComponent<Animator>();
        soundOn = true;
    }
	
	// Update is called once per frame
	void Update () {
		if (showExitWarning)
        {
            exitWarningYesButton.GetComponent<Button>().interactable = true;
            exitWarningYesButton.GetComponent<Image>().enabled = true;
            exitWarningNoButton.GetComponent<Button>().interactable = true;
            exitWarningNoButton.GetComponent<Image>().enabled = true;
            exitWarningBackground.GetComponent<Image>().enabled = true;
        }
        else
        {
            exitWarningYesButton.GetComponent<Button>().interactable = false;
            exitWarningYesButton.GetComponent<Image>().enabled = false;
            exitWarningNoButton.GetComponent<Button>().interactable = false;
            exitWarningNoButton.GetComponent<Image>().enabled = false;
            exitWarningBackground.GetComponent<Image>().enabled = false;
        }
	}
    
    public void switchSoundState()
    {
        soundOn = !soundOn;
        soundAnim.SetBool("Sound On", soundOn);
        AudioListener.volume = soundOn ? 1 : 0;
    }

    public void openExitWarning()
    {
        showExitWarning = true;
    }

    public void closeExitWarning()
    {
        showExitWarning = false;
    }

    public void exitGameToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
