using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButtonAnimationManager : MonoBehaviour {

    private GameObject[] countryButtons = new GameObject[2];
    private Sprite[] countrySprites = new Sprite[4];
    
    private GameObject[] difficultyButtons = new GameObject[3];
    private Sprite[] difficultySprites = new Sprite[6];
    
    private GameObject[] mapSizeButtons = new GameObject[3];
    private Sprite[] mapSizeSprites = new Sprite[6];

    private GameObject trenchNumSlider;
    private GameObject trenchNumCounter;
    private Sprite[] trenchNumCounterSprites = new Sprite[6];

    private GameObject exitButton;
    private GameObject startGameButton;

    // Use this for initialization
    void Start () {
        loadButtons();
        loadSprites();
    }
	
    void loadButtons()
    {
        countryButtons[0] = GameObject.Find("Canvas/UIBasePlate/FrenchSideButton");
        countryButtons[1] = GameObject.Find("Canvas/UIBasePlate/GermanSideButton");

        difficultyButtons[0] = GameObject.Find("Canvas/UIBasePlate/EasyDifficultyButton");
        difficultyButtons[1] = GameObject.Find("Canvas/UIBasePlate/NormalDifficultyButton");
        difficultyButtons[2] = GameObject.Find("Canvas/UIBasePlate/HardDifficultyButton");

        mapSizeButtons[0] = GameObject.Find("Canvas/UIBasePlate/SmallMapSizeButton");
        mapSizeButtons[1] = GameObject.Find("Canvas/UIBasePlate/MediumMapSizeButton");
        mapSizeButtons[2] = GameObject.Find("Canvas/UIBasePlate/LargeMapSizeButton");

        trenchNumSlider = GameObject.Find("Canvas/UIBasePlate/TrenchNumSlider");
        trenchNumCounter = GameObject.Find("Canvas/UIBasePlate/TrenchNumCounter");

        exitButton = GameObject.Find("Canvas/ExitButton");
        startGameButton = GameObject.Find("Canvas/startGameButton");
    }

    void loadSprites()
    {
        Object[] spriteSheet = Resources.LoadAll("MenuSpriteSheet");
        int[] countrySpriteIndices = { 4, 5, 1, 2 };
        for (int i = 0; i < countrySpriteIndices.Length; i++)
        {
            countrySprites[i] = spriteSheet[countrySpriteIndices[i] + 1] as Sprite;
        }
        int[] difficultySpriteIndices = { 9, 10, 11, 13, 14, 15 };
        for (int i = 0; i < difficultySpriteIndices.Length; i++)
        {
            difficultySprites[i] = spriteSheet[difficultySpriteIndices[i] + 1] as Sprite;
        }
        int[] mapSizeSpriteIndices = { 19, 20, 21, 23, 24, 25 };
        for (int i = 0; i < mapSizeSpriteIndices.Length; i++)
        {
            mapSizeSprites[i] = spriteSheet[mapSizeSpriteIndices[i] + 1] as Sprite;
        }
        int[] trenchNumSpriteIndices = { 18, 16, 12, 8, 6, 3 };
        for (int i = 0; i < trenchNumSpriteIndices.Length; i++)
        {
            trenchNumCounterSprites[i] = spriteSheet[trenchNumSpriteIndices[i] + 1] as Sprite;
        }
    }

    void pressButtonInSet(GameObject[] buttons, Sprite[] sprites, int pressedIndex)
    {
        int spriteNum = sprites.Length;
        int buttonNum = buttons.Length;
        // Set pressed button's image to pressed version
        buttons[pressedIndex].GetComponent<Image>().sprite = sprites[pressedIndex + buttonNum];

        for (int i = 0; i < buttonNum; i++)
        {
            if (pressedIndex != i)
            {
                // Set other button's images to unpressed versions
                buttons[i].GetComponent<Image>().sprite = sprites[i];
            }
        }
    }
    
	public void pressFranceSideButton() { pressButtonInSet(countryButtons, countrySprites, 0); }
    public void pressGermanySideButton() { pressButtonInSet(countryButtons, countrySprites, 1); }
    public void pressEasyDifficultyButton() { pressButtonInSet(difficultyButtons, difficultySprites, 0); }
    public void pressNormalDifficultyButton() { pressButtonInSet(difficultyButtons, difficultySprites, 1); }
    public void pressHardDifficultyButton() { pressButtonInSet(difficultyButtons, difficultySprites, 2); }
    public void pressSmallMapSizeButton() { pressButtonInSet(mapSizeButtons, mapSizeSprites, 0); }
    public void pressMediumMapSizeButton() { pressButtonInSet(mapSizeButtons, mapSizeSprites, 1); }
    public void pressLargeMapSizeButton() { pressButtonInSet(mapSizeButtons, mapSizeSprites, 2); }
    
    public void changeTrenchNumCounter()
    {
        int value = (int)trenchNumSlider.GetComponent<Slider>().value;
        trenchNumCounter.GetComponent<Image>().sprite = trenchNumCounterSprites[value];
    }
}
