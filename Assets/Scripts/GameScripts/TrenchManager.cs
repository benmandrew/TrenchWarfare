using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrenchManager : MonoBehaviour {

    public static int maxSquadCapacity = 3;
    public List<SquadManager> storedSquads = new List<SquadManager>(); // Squads stored in the trench, including those just passing through it
    public bool isLocked;
    public float edgeModifier;

    private GameObject canvas;
    public GameObject trenchButtonPrefab;
    public List<TrenchButton> buttonList;

    // Percieved depth of trench to modify y
    // coordinate of soldiers in the trench
    public float trenchDepth;
    // Allowed variation in x coordinates within the trench
    public float leftDx;
    public float rightDx;

    private float buttonSpacer;

    // Use this for initialization
    void Start()
    {
        isLocked = false;
        canvas = GameObject.Find("Canvas");
        TrenchButton button;
        for (int i = 0; i < maxSquadCapacity; i++)
        {
            button = Instantiate(trenchButtonPrefab).GetComponent<TrenchButton>();
            button.setup(this, i, canvas.GetComponent<Canvas>());
            setButtonSpacer();
            button.setOffset(maxSquadCapacity, buttonSpacer);
            buttonList.Add(button);
        }
    }

    // Generate the scalar for the quadratic equation that changes the
    // y coordinate of a soldier when "jumping" into or out of a trench
    // http://jwilson.coe.uga.edu/emt668/emat6680.f99/jones/instructional%20unit/writingquads.html
    public float getJumpEquation (SoldierBaseManager soldier, int side)
    {
        // Side is either -1 or 1, used as a multiplier to decide
        // which side of the trench to use as the apex of the curve
        float apexX = transform.position.x + (edgeModifier * side);
        float apexY = soldier.startingY;
        float pointX = transform.position.x + (soldier.xModifier + soldier.centerOffset) * soldier.squad.direction;
        float pointY = apexY - trenchDepth;
        float scalar = (pointY - apexY) / Mathf.Pow(pointX - apexX, 2);
        return scalar;
    }

	// Update is called once per frame
	void Update () {
        for (int i = 3; i < storedSquads.Count; i++)
        {
            if (storedSquads[i] != null)
            {
                if (!storedSquads[i].isEnteringTrench())
                {
                    storedSquads[i].exitTrench();
                }
            }
        }
        setButtonSpacer();
        foreach (TrenchButton button in buttonList)
        {
            button.setOffset(3, buttonSpacer);
        }
    }

    void OnTriggerEnter2D (Collider2D squadCollider)
    {
        bool squadStored = false;
        SquadManager squad = squadCollider.gameObject.transform.parent.GetComponent<SquadManager>();
        for (int i = 0; i < storedSquads.Count; i++)
        {
            if (storedSquads[i] == null)
            {
                storedSquads[i] = squad;
                squad.trenchIndex = i;
                squad.isStored = true;
                squadStored = true;
                break;
            }
        }
        if (!squadStored)
        {
            storedSquads.Add(squad);
            squad.trenchIndex = storedSquads.Count - 1;
            squad.isStored = true;
            squadStored = true;
        }
        squad.enterTrench(this);
    }

    void setButtonSpacer()
    {
        buttonSpacer = (int)(Camera.main.pixelWidth * 0.05);
    }
}
