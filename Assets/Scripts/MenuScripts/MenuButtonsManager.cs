using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuButtonsManager : MonoBehaviour {

    private GameObject frenchSideButton;
    private GameObject germanSideButton;

    private GameObject easyDifficultyButton;
    private GameObject normalDifficultyButton;
    private GameObject hardDifficultyButton;

    private GameObject smallMapSizeButton;
    private GameObject mediumMapSizeButton;
    private GameObject largeMapSizeButton;

    private GameObject trenchNumSlider;

    private GameObject exitButton;
    private GameObject startGameButton;

    private InterSceneGameVariables gameVars;

	// Use this for initialization
	void Start () {
        frenchSideButton = GameObject.Find("Canvas/UIBasePlate/FrenchSideButton");
        germanSideButton = GameObject.Find("Canvas/UIBasePlate/GermanSideButton");

        easyDifficultyButton = GameObject.Find("Canvas/UIBasePlate/EasyDifficultyButton");
        normalDifficultyButton = GameObject.Find("Canvas/UIBasePlate/NormalDifficultyButton");
        hardDifficultyButton = GameObject.Find("Canvas/UIBasePlate/HardDifficultyButton");

        smallMapSizeButton = GameObject.Find("Canvas/UIBasePlate/SmallMapSizeButton");
        mediumMapSizeButton = GameObject.Find("Canvas/UIBasePlate/MediumMapSizeButton");
        largeMapSizeButton = GameObject.Find("Canvas/UIBasePlate/LargeMapSizeButton");

        trenchNumSlider = GameObject.Find("Canvas/UIBasePlate/TrenchNumSlider");

        exitButton = GameObject.Find("Canvas/UIBasePlate/ExitButton");
        startGameButton = GameObject.Find("Canvas/UIBasePlate/startGameButton");

        gameVars = GameObject.Find("GameVariables").GetComponent<InterSceneGameVariables>();
    }
	
	public void setFrenchSide() { gameVars.setSide(1); }
    public void setGermanSide()  { gameVars.setSide(2);  }
    public void setEasyDifficulty() { gameVars.setDifficulty(3.0f); }
    public void setNormalDifficulty() { gameVars.setDifficulty(2.0f); }
    public void setHardDifficulty() { gameVars.setDifficulty(1.5f); }
    public void setSmallMapSize() { gameVars.setMapSize(30); }
    public void setMediumMapSize() { gameVars.setMapSize(60); }
    public void setLargeMapSize() { gameVars.setMapSize(90); }
    public void setTrenchNum() {
        int value = (int)trenchNumSlider.GetComponent<Slider>().value;
        gameVars.setTrenchNum(value);
    }
    public void exitToDesktop() { Application.Quit(); }
    public void startGame() { SceneManager.LoadScene("Game"); }
}
