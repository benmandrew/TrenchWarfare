using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadManager : MonoBehaviour {

    public List<SoldierBaseManager> soldierList;
    public bool inTrench;
    public bool isAdvancing;
    public bool isFighting;
    public float forwardPosition; // X coordinate of the forwardmost soldier
    public TrenchManager currentTrench;
    public int trenchIndex;
    public bool isStored;
    public HashSet<SquadManager> engagedSquads = new HashSet<SquadManager>();

    // Initialisation variables
    public int soldierNum;
    public int soldierTotalHealthPoints;
    public float walkingSpeed;
    public float spawnPosition;
    public float topYSpawn;
    public float bottomYSpawn;
    public GameObject soldierPrefab;
    private float variableSpawnPosition;
    public string squadType;
    public float fireCooldownTime;
    public float accuracy;
    // ONLY APPLICABLE FOR AUTOMATIC WEAPONS
    public float burstCooldownTime;

    public float weaponRange;
    public int direction;
    public int team;
    public float mapWidth;

    // Stores the trench depth modifier once a trench
    // has been entered and from then on, to stop
    // the squad relying on the trench object itself
    public float yShift;

    // Use this for initialization
    void Start () {

        forwardPosition = spawnPosition;
        isAdvancing = true;
        inTrench = false;
        isFighting = false;
        currentTrench = null;
        addSoldiers(soldierNum, spawnPosition);

        BoxCollider2D boxCollider = transform.GetChild(0).GetComponent<BoxCollider2D>();
        Vector3 colliderPosition = boxCollider.transform.position;
        colliderPosition.x = spawnPosition;
        boxCollider.transform.position = colliderPosition;
    }

    void addSoldiers (int soldierNum, float spawnPosition)
    {
        for (int i = 0; i < soldierNum; i++)
        {
            SoldierBaseManager newSoldier;
            switch(squadType)
            {
                case "assault":
                    newSoldier = Instantiate(soldierPrefab).GetComponent<AssaultManager>();
                    break;
                case "machine gun":
                    newSoldier = Instantiate(soldierPrefab).GetComponent<MachineGunManager>();
                    break;
                case "sniper":
                    newSoldier = Instantiate(soldierPrefab).GetComponent<SniperManager>();
                    break;
                case "armour":
                    newSoldier = Instantiate(soldierPrefab).GetComponent<ArmourManager>();
                    break;
                default: // RIFLE (or errors :[ )
                    /* Basically make rifles the default choice, more error prone but C# doesn't
                     * like it when variables are defined only in conditional statements */
                    newSoldier = Instantiate(soldierPrefab).GetComponent<RifleManager>();
                    break;
            }
            newSoldier.setPosition(spawnPosition, topYSpawn, bottomYSpawn);
            newSoldier.healthPoints = soldierTotalHealthPoints;
            newSoldier.walkingSpeed = walkingSpeed;
            newSoldier.fireCooldownTime = fireCooldownTime;
            newSoldier.accuracy = accuracy;
            newSoldier.transform.parent = transform;
            newSoldier.squad = this;
            if (direction == -1)
            {
                // Flip the soldier on the X axis
                Vector3 scale = newSoldier.transform.localScale;
                scale.x *= -1;
                newSoldier.transform.localScale = scale;
            }
            soldierList.Add(newSoldier);
        }
    }

    // Update is called once per frame
    void Update() {
        if (soldierList.Count == 0)
        {
            Destroy(gameObject);
        }
        SoldierBaseManager soldier;
        for (int i = 0; i < soldierList.Count; i++)
        {
            soldier = soldierList[i];
            soldier.isFighting = isFighting;
            // Push the forwardmost position
            if (isFurtherForward(soldier.transform.position.x, forwardPosition))
            {
                forwardPosition = soldier.transform.position.x;
            }
        }
        // Concatenate the already engaged squads with the newly in range squads
        //engagedSquads.UnionWith(getEnemySquadsInRange());
        engagedSquads = new HashSet<SquadManager>(getEnemySquadsInRange());
        if (engagedSquads.Count >= 1)
        {
            if (isFighting == false)
            {
                for (int i = 0; i < soldierList.Count; i++)
                {
                    soldierList[i].lastFireTime = Time.time - soldierList[i].fireCooldownTime;
                }
            }
            isFighting = true;
        }
        else { isFighting = false; }
        List<SquadManager> removeSquadList = new List<SquadManager>();
        foreach (SquadManager enemySquad in engagedSquads)
        {
            if (enemySquad.soldierList.Count == 0)
            {
                removeSquadList.Add(enemySquad);
            }
        }
        foreach (SquadManager removedEnemySquad in removeSquadList)
        {
            engagedSquads.Remove(removedEnemySquad);
        }
        // Changing the position of the box collider
        BoxCollider2D boxCollider = transform.GetChild(0).GetComponent<BoxCollider2D>();
        Vector3 colliderPosition = boxCollider.transform.position;
        colliderPosition.x = forwardPosition;
        boxCollider.transform.position = colliderPosition;
    }

    public void enterTrench (TrenchManager trench)
    {
        currentTrench = trench;
        inTrench = true;
        isAdvancing = false;
        if (yShift == 0)
        {
            yShift = currentTrench.trenchDepth;
        }
        SoldierBaseManager soldier;
        for (int i = 0; i < soldierList.Count; i++)
        {
            // Set the Soldier states to those of the squad
            soldier = soldierList[i];
            soldier.enteringTrench = true;
            soldier.currentTrench = trench;
            soldier.updateAnimator();
            soldier.xModifier = Random.Range(0.0f, 0.2f);
        }
    }

    public void exitTrench ()
    {
        if (isStored)
        {
            currentTrench.storedSquads[trenchIndex] = null;
        }
        currentTrench = null;
        inTrench = false;
        isAdvancing = true;
        SoldierBaseManager soldier;
        for (int i = 0; i < soldierList.Count; i++)
        {
            // Set the Soldier states to those of the squad
            soldier = soldierList[i];
            soldier.inTrench = false;
            soldier.exitingTrench = true;
            soldier.isAdvancing = isAdvancing;
            soldier.updateAnimator();
        }
    }

    public bool isFurtherForward(float x1, float x2)
    {
        bool returnBool = true;
        if (direction == 1)
        {
            returnBool = x1 > x2;
        }
        else if (direction == -1)
        {
            returnBool = x1 < x2;
        }
        return returnBool;
    }

    List<SquadManager> getEnemySquadsInRange ()
    {
        List<SquadManager> enemySquads = new List<SquadManager>();
        SquadManager squad;

        float range = weaponRange;
        if (inTrench) { range *= 1.2f; }

        Vector2 rayOrigin = transform.GetChild(0).GetComponent<BoxCollider2D>().transform.position;
        Vector2 rayDirection = new Vector2(direction, 0.0f);
        RaycastHit2D[] results = Physics2D.RaycastAll(rayOrigin, rayDirection, range);
        Debug.DrawRay(rayOrigin, rayDirection * weaponRange);

        for (int i = 0; i < results.Length; i++)
        {
            Collider2D collider = results[i].collider;
            squad = collider.transform.parent.GetComponent<SquadManager>();
            if (squad != null && squad.GetType() == typeof(SquadManager))
            {
                if (squad.team != team)
                {
                    enemySquads.Add(squad);
                }
            }
        }
        return enemySquads;
    }

    bool enemySquadsAlive ()
    {
        foreach (SquadManager enemySquad in engagedSquads)
        {
            if (enemySquad.soldierList.Count == 0)
            {
                return true;
            }
        }
        return false;
    }

    public bool isEnteringTrench ()
    {
        for (int i = 0; i < soldierList.Count; i++)
        {
            if (soldierList[i].enteringTrench)
            {
                return true;
            }
        }
        return false;
    }

    public bool isExitingTrench()
    {
        for (int i = 0; i < soldierList.Count; i++)
        {
            if (soldierList[i].exitingTrench)
            {
                return true;
            }
        }
        return false;
    }
}
