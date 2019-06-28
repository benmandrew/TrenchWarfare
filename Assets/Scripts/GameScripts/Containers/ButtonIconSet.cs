using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class ButtonIconSet : MonoBehaviour
{
    public Sprite def;
    public Sprite hover;
    public Sprite press;
    public Sprite disabled;

    public void changeAllSprites(GameObject gameObject)
    {
        if (def != null) { gameObject.GetComponent<Image>().sprite = def; }

        SpriteState tempSpriteState = gameObject.GetComponent<Button>().spriteState;
        if (hover != null) { tempSpriteState.highlightedSprite = hover; }
        if (press != null) { tempSpriteState.pressedSprite = press; }
        if (disabled != null) { tempSpriteState.disabledSprite = disabled; }
        gameObject.GetComponent<Button>().spriteState = tempSpriteState;
    }
}
