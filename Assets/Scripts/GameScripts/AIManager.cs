using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : SpawnButtonManager {

    private int side;
    private float difficulty; // Multiplier for spawn times

    private float rifleSpawnTime;
    private float assaultSpawnTime;
    private float machineGunSpawnTime;
    private float sniperSpawnTime;
    private float armourSpawnTime;

    private string nextSquadToSpawn;
    private float currentSquadSpawnTime;

    private float spawnTimeMultiplier;

    // Use this for initialization
    void Start () {
        side = InterSceneGameVariables.gameVars.side;
        if (side == 1) { side = 2; direction = -1; }
        else if (side == 2) { side = 1; direction = 1; }
        difficulty = InterSceneGameVariables.gameVars.difficulty;
        mapWidth = InterSceneGameVariables.gameVars.mapSize;
        lastSpawnTime = -3; // Some large negative value to prevent overlap

        rifleSpawnTime = 6 * difficulty;
        assaultSpawnTime = 9 * difficulty;
        machineGunSpawnTime = 12 * difficulty;
        sniperSpawnTime = 15 * difficulty;
        armourSpawnTime = 18 * difficulty;

        isHumanPlayer = false;
        team = side;
    }

	// Update is called once per frame
	void Update ()
    {
        getSquadSpawnTime();
        newSquadSpawn();
	}

    void getSquadSpawnTime()
    {
        if (nextSquadToSpawn == null) { calculateNextSquad(); }
        switch (nextSquadToSpawn)
        {
            case "rifle":
                currentSquadSpawnTime = rifleSpawnTime;
                break;
            case "assault":
                currentSquadSpawnTime = assaultSpawnTime;
                break;
            case "machine gun":
                currentSquadSpawnTime = machineGunSpawnTime;
                break;
            case "sniper":
                currentSquadSpawnTime = sniperSpawnTime;
                break;
            case "armour":
                currentSquadSpawnTime = armourSpawnTime;
                break;
        }
    }

    void newSquadSpawn()
    {
        if (lastSpawnTime + currentSquadSpawnTime < Time.time)
        {
            switch (nextSquadToSpawn)
            {
                case "rifle":
                    spawnRifleSquad();
                    break;
                case "assault":
                    spawnAssaultSquad();
                    break;
                case "machine gun":
                    spawnMachinGunSquad();
                    break;
                case "sniper":
                    spawnSniperSquad();
                    break;
                case "armour":
                    spawnArmourSquad();
                    break;
            }
            lastSpawnTime = Time.time;
            calculateNextSquad();
        }
    }

    void calculateNextSquad()
    {
        // Revolutionary AI
        int randSquadType = Random.Range(1, 5);
        switch (randSquadType)
        {
            case 1:
                nextSquadToSpawn = "rifle";
                break;
            case 2:
                nextSquadToSpawn = "assault";
                break;
            case 3:
                nextSquadToSpawn = "machine gun";
                break;
            case 4:
                nextSquadToSpawn = "sniper";
                break;
            case 5:
                nextSquadToSpawn = "armour";
                break;
        }
    }

    void spawnSquad()
    {
        float spawnPosition = -((mapWidth / 2.0f) + 3.0f) * direction;
        spawnPosition = -((mapWidth / 2.0f) + 3.0f) * direction;

        SquadManager squad = Instantiate(squadPrefab).GetComponent<SquadManager>();
        squad.soldierNum = soldierNum;
        squad.soldierTotalHealthPoints = healthPoints;
        squad.walkingSpeed = walkingSpeed;
        squad.spawnPosition = spawnPosition;
        squad.weaponRange = weaponRange;
        squad.fireCooldownTime = fireCooldownTime;
        squad.direction = direction;
        squad.team = team;
        squad.mapWidth = mapWidth;
        squad.squadType = squadType;
        squad.soldierPrefab = soldierPrefab;
        if (squadType == "assault" || squadType == "machine gun")
        {
            squad.burstCooldownTime = burstCooldownTime;
        }
    }


}
