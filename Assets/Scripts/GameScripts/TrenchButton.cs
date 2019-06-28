using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrenchButton : MonoBehaviour {

    public Button buttonComponent;
    private Vector3 buttonPosition;
    public float buttonScalarScale;
    private Vector3 buttonScale;
    public TrenchManager parentTrench;
    private float buttonOffset;

    private Vector3 trenchScreenPosition;
    public RectTransform rectTransform;

    public GameObject cam;

    public int id;
    private bool previousTrenchFilledState;
    public float heightMultiplier;

	// Use this for initialization
	void Start () {
        rectTransform = GetComponent<RectTransform>();
        cam = Camera.main.gameObject;

        // Set button scale to equal that of the trench
        buttonScale = new Vector3(buttonScalarScale, buttonScalarScale, buttonScalarScale);
        rectTransform.localScale = buttonScale;

        previousTrenchFilledState = true;
    }
	
    public void setOffset (int maxSquadCapacity, float buttonSpacer)
    {
        buttonOffset = (id - (((float)maxSquadCapacity - 1) / 2.0f)) * buttonSpacer;
    }

    // OnClick function
    public void evacuateSquad()
    {
        if (parentTrench.storedSquads[id] != null)
        {
            if (!parentTrench.storedSquads[id].isEnteringTrench())
            {
                parentTrench.storedSquads[id].exitTrench();
            }
        }
    }

	// Update is called once per frame
	void Update () {
        trenchScreenPosition = cam.GetComponent<Camera>().WorldToScreenPoint(parentTrench.transform.position);
        buttonPosition = rectTransform.position;
        buttonPosition.x = trenchScreenPosition.x + buttonOffset;
        buttonPosition.y = Screen.height * heightMultiplier;
        rectTransform.position = buttonPosition;

        if (previousTrenchFilledState != (parentTrench.storedSquads[id] != null))
        {
            previousTrenchFilledState = parentTrench.storedSquads[id] != null;
            ButtonIconSet buttonIconSet;
            int setIndex = 0; ;
            if (parentTrench.storedSquads[id] != null)
            {
                switch (parentTrench.storedSquads[id].squadType)
                {
                    case "rifle":
                        setIndex = 1;
                        break;
                    case "assault":
                        setIndex = 2;
                        break;
                    case "machine gun":
                        setIndex = 3;
                        break;
                    case "sniper":
                        setIndex = 4;
                        break;
                    case "armour":
                        setIndex = 5;
                        break;
                }
            }
            buttonIconSet = GameObject.Find("TrenchButtonIconSets").transform.GetChild(setIndex).gameObject.GetComponent<ButtonIconSet>();
            buttonIconSet.changeAllSprites(gameObject);
        }
	}

    public void setup (TrenchManager trench, int newId, Canvas canvas)
    {
        parentTrench = trench;
        transform.SetParent(canvas.transform);
        id = newId;
    }
}
