using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// IF MORE STATES ARE ADDED CHANGE THE RANDOM CHOICE CODE IN SELECT TARGET
enum FiringState { Standing, Crouched, Prone };

public class SoldierBaseManager : MonoBehaviour {
    // At spawn time, the soldier is held back until
    // a random amount of time has passed to separate
    // the soldiers in the x axis
    public float spawnTime;
    public float holdTime;
    public bool isHeld;

    public int healthPoints;
    public float walkingSpeed;
    public SoldierBaseManager targetSoldier;

    public bool isAlive;
    public bool isAdvancing;
    public bool inTrench;
    public bool enteringTrench;
    public bool exitingTrench;
    public bool isFighting;

    public SquadManager squad;
    public TrenchManager currentTrench;
    private SpriteRenderer rend;
    private Animator anim;
    private FiringState firingState;
    // The starting value for the y coordinate, used as an anchor
    // point when depth modifiers for trenches are applied over time
    public float startingY;
    // Random modifier to create variation of 
    // soldiers on the trench floor within the x axis
    public float xModifier;
    public float centerOffset;

    // Weapon variables
    public float fireCooldownTime;
    public float fireCooldownTimeModifier;
    public float lastFireTime; // Last point in time the weapon was fired
    public float accuracy; // Chance to hit, as a percentage

    public bool fireTrigger;
    // ONLY APPLICABLE FOR AUTOMATIC WEAPONS
    public float burstCooldownTime;
    public float burstCooldownTimeModifier;
    public bool isBursting;
    public int shotsLeftInBurst;
    public int shotsInBurst;

    public AudioClip shotSound;
    private SpawnButtonManager gameController;

    // Use this for initialization
    void Start () {
        spawnTime = Time.time;
        holdTime = Random.Range(0.0f, 1.0f);
        isHeld = true;

        isAlive = true;
        isAdvancing = false;
        inTrench = false;
        enteringTrench = false;
        exitingTrench = false;
        isFighting = false;

        anim = GetComponent<Animator>();
        firingState = FiringState.Standing;
        startingY = transform.position.y;
        
        fireCooldownTimeModifier = Random.Range(0.0f, 1.0f);
        lastFireTime = Time.time - fireCooldownTime;

        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<SpawnButtonManager>();

        // ONLY APPLICABLE FOR AUTOMATIC WEAPONS
        shotsLeftInBurst = 0;
    }
	
    public void setPosition (float spawnPosition, float topY, float bottomY)
    {
        Vector3 currentPosition = transform.position;
        currentPosition.x = spawnPosition;
        currentPosition.y = Random.Range(bottomY, topY);
        transform.position = currentPosition;

        rend = GetComponent<SpriteRenderer>();
        rend.sortingOrder = -(int) (currentPosition.y * 1000.0f);
    }

    // Update is called once per frame
    void Update() {
        if (isAlive)
        {
            if (spawnTime + holdTime < Time.time && isHeld)
            {
                isAdvancing = true;
                isHeld = false;
            }
            if (isAdvancing && !isHeld && (!isFighting || enteringTrench || exitingTrench))
            {
                advance();
            }
            if (enteringTrench)
            {
                if (squad.isFurtherForward(transform.position.x, currentTrench.transform.position.x + (xModifier + centerOffset) * squad.direction))
                {
                    inTrench = true;
                    enteringTrench = false;
                    isAdvancing = false;
                }
            }
            if (enteringTrench || exitingTrench)
            {
                applyTrenchDepthModifier();
            }
            shiftPosition(-transform.parent.gameObject.transform.position.x * squad.direction);
            selectTarget();
            readyToFire();
            //if (targetSoldier == null) { fireTrigger = false; }
        }
        updateAnimator();
    }

    protected void advance ()
    {
        shiftPosition(x: walkingSpeed * squad.direction);
    }

    protected void selectTarget ()
    {
        if (Mathf.Abs(transform.position.x) < Mathf.Abs(squad.mapWidth / 2.0f))
        {
            bool selectNewTarget = false;
            if (isFighting)
            {
                if (targetSoldier != null)
                {
                    if (!targetSoldier.isAlive) { selectNewTarget = true; }
                }
                else { selectNewTarget = true; }
            }
            if (selectNewTarget && !(enteringTrench || exitingTrench))
            {
                if (squad.engagedSquads.Count >= 1)
                {
                    // Select random target (soldier) from the list of engaged squads
                    SquadManager[] engagedSquadsArray = new SquadManager[squad.engagedSquads.Count];
                    squad.engagedSquads.CopyTo(engagedSquadsArray);
                    SquadManager targetSquad = engagedSquadsArray[Random.Range(0, squad.engagedSquads.Count)];
                    targetSoldier = targetSquad.soldierList[Random.Range(0, targetSquad.soldierList.Count)];
                    firingState = 0;//(FiringState)Random.Range(0, 2);
                }
                else
                {
                    isFighting = false;
                    targetSoldier = null;
                }
            }
        }
        else
        {
            advance();
        }
    }

    void readyToFire ()
    {
        if (isFighting && targetSoldier != null
            && (lastFireTime + fireCooldownTime + fireCooldownTimeModifier) < Time.time)
        {
            lastFireTime = Time.time;
            fireCooldownTimeModifier = Random.Range(0.0f, 1.0f);
            fireTrigger = true;
        }
    }

    public void singleFire()
    {
        if (targetSoldier != null)
        {
            lastFireTime = Time.time;
            float hitChanceModifier = 1.0f;
            if (targetSoldier.inTrench && squad.squadType != "assault")
            {
                hitChanceModifier = 0.5f;
            }
            float randomHitChance = Random.Range(0.0f, 1.0f);
            bool shotHit = randomHitChance < accuracy * hitChanceModifier;
            if (shotHit)
            {
                targetSoldier.healthPoints -= 1;
                if (targetSoldier.healthPoints <= 0)
                {
                    targetSoldier.kill();
                    targetSoldier = null;
                }
            }
        }
        fireTrigger = false;
    }

    void playShotSound()
    {
        AudioSource.PlayClipAtPoint(shotSound, transform.position);
    }

    public void kill ()
    {
        isAlive = false;
        transform.parent = GameObject.Find("DeadSoldiers").transform;
        squad.soldierList.Remove(this);
        anim.SetTrigger("Death Trigger");
        
        isAdvancing = false;
        inTrench = false;
        enteringTrench = false;
        exitingTrench = false;
        isFighting = false;

        targetSoldier = null;
    }

    public void applyTrenchDepthModifier()
    {
        int side;
        float boundary;
        float relativeX;
        //float middleX = transform.position.x;
        if (squad.isFurtherForward(currentTrench.transform.position.x + (xModifier + centerOffset) * squad.direction, transform.position.x))
        {
            // Moving into the trench
            side = (-1) * squad.direction;
            boundary = currentTrench.transform.position.x + currentTrench.edgeModifier * side;
            relativeX = (boundary - transform.position.x) * -side;
            if (relativeX > 0)
            {
                relativeX = 0;
            }
        }
        else
        {
            // Moving out of the trench
            side = 1 * squad.direction;
            boundary = currentTrench.transform.position.x + currentTrench.edgeModifier * side;
            relativeX = (boundary - transform.position.x) * -side;
            if (relativeX > 0)
            {
                exitingTrench = false;
                currentTrench = null;
                relativeX = 0;
                updateAnimator();
                return;
            }
        }
        float newY = startingY + currentTrench.getJumpEquation(this, side) * Mathf.Pow(relativeX, 2);
        setY(newY);
    }
    
    bool hasWon()
    {
        // If the soldier has gotten to the enemy's side of the map, their team wins
        if (transform.position.x * squad.direction > ((squad.mapWidth / 2) - 2))
        {
            if (InterSceneGameVariables.gameVars.teamWon != squad.team)
            {
                InterSceneGameVariables.gameVars.setTeamWon(squad.team);
            }
            return true;
        }
        return false;
    }

    public void updateAnimator ()
    {
        if (anim != null)
        {
            anim.SetBool("Is Alive", isAlive);
            anim.SetBool("Is Advancing", isAdvancing);
            anim.SetBool("In Trench", inTrench);
            anim.SetBool("Entering Trench", enteringTrench);
            anim.SetBool("Exiting Trench", exitingTrench);
            anim.SetBool("Is Fighting", isFighting);
            anim.SetBool("Is Bursting", isBursting);
            anim.SetInteger("Firing State", (int)firingState + 1);
            if (fireTrigger) { anim.SetTrigger("Fire Trigger"); }
            else { anim.ResetTrigger("Fire Trigger"); }
            fireTrigger = false;

            if (hasWon() || InterSceneGameVariables.gameVars.teamWon == squad.team)
            {
                anim.SetBool("Has Won", true);
                isAdvancing = false;
            }
            if (InterSceneGameVariables.gameVars.teamWon != 0
                && InterSceneGameVariables.gameVars.teamWon != squad.team)
            {
                anim.SetBool("Has Lost", true);
                isAdvancing = false;
                if (isAlive)
                {
                    Object.Destroy(gameObject);
                }
            }
        }
    }

    public void shiftPosition (float x = 0, float y = 0, float z = 0)
    {
        Vector3 currentPosition = transform.position;
        currentPosition.x += x;
        currentPosition.y += y;
        currentPosition.z += z;
        transform.position = currentPosition;
    }

    public void setX(float x)
    {
        Vector3 currentPosition = transform.position;
        currentPosition.x = x;
        transform.position = currentPosition;
    }
    public void setY(float y)
    {
        Vector3 currentPosition = transform.position;
        currentPosition.y = y;
        transform.position = currentPosition;
    }
    public void setZ(float z)
    {
        Vector3 currentPosition = transform.position;
        currentPosition.z = z;
        transform.position = currentPosition;
    }
    public float getY()
    {
        float y = transform.position.y;
        if (inTrench)
        {
            y -= currentTrench.trenchDepth;
        }
        return y;
    }
}
