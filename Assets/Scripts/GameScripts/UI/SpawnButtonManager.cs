using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class SpawnButtonManager : MonoBehaviour {

    public GameObject squadPrefab;

    protected GameObject rifleSpawnButton;
    protected GameObject assaultSpawnButton;
    protected GameObject machineGunSpawnButton;
    protected GameObject sniperSpawnButton;
    protected GameObject armourSpawnButton;

    protected GameObject rifleSpawnButtonOverlay;
    protected GameObject assaultSpawnButtonOverlay;
    protected GameObject machineGunSpawnButtonOverlay;
    protected GameObject sniperSpawnButtonOverlay;
    protected GameObject armourSpawnButtonOverlay;

    public GameObject soldierPrefabContainer;

    public int playerTeam;
    protected bool isHumanPlayer;
    public float mapWidth;

    // Soldier values
    protected int soldierNum;
    protected int healthPoints;
    protected float walkingSpeed;
    protected float spawnPosition;
    protected float weaponRange;
    protected float fireCooldownTime;
    protected float burstCooldownTime;
    protected int direction;
    protected int team;
    protected string squadType;
    protected GameObject soldierPrefab;
    protected float soldierSpawnTime;

    public float lastSpawnTime;

    // Weapon values
    private float accuracy; // Chance to hit, as a percentage

    void Start()
    {
        rifleSpawnButton = GameObject.Find("Canvas/RifleSpawnButton");
        assaultSpawnButton = GameObject.Find("Canvas/AssaultSpawnButton");
        machineGunSpawnButton = GameObject.Find("Canvas/MachineGunSpawnButton");
        sniperSpawnButton = GameObject.Find("Canvas/SniperSpawnButton");
        armourSpawnButton = GameObject.Find("Canvas/ArmourSpawnButton");
        
        rifleSpawnButtonOverlay = GameObject.Find("Canvas/RifleSpawnButtonLoadingOverlay");
        assaultSpawnButtonOverlay = GameObject.Find("Canvas/AssaultSpawnButtonLoadingOverlay");
        machineGunSpawnButtonOverlay = GameObject.Find("Canvas/MachineGunSpawnButtonLoadingOverlay");
        sniperSpawnButtonOverlay = GameObject.Find("Canvas/SniperSpawnButtonLoadingOverlay");
        armourSpawnButtonOverlay = GameObject.Find("Canvas/ArmourSpawnButtonLoadingOverlay");

        mapWidth = InterSceneGameVariables.gameVars.mapSize;
        lastSpawnTime = -100;
        isHumanPlayer = true;
        if (isHumanPlayer)
        {
            team = InterSceneGameVariables.gameVars.side;
        }
    }

    void resetAllOverlays()
    {
        rifleSpawnButtonOverlay.GetComponent<GUIOrderingManager>().resetAsActive();
        assaultSpawnButtonOverlay.GetComponent<GUIOrderingManager>().resetAsActive();
        machineGunSpawnButtonOverlay.GetComponent<GUIOrderingManager>().resetAsActive();
        sniperSpawnButtonOverlay.GetComponent<GUIOrderingManager>().resetAsActive();
        armourSpawnButtonOverlay.GetComponent<GUIOrderingManager>().resetAsActive();
    }

    void spawnSquad()
    {
        if (InterSceneGameVariables.gameVars.teamWon == 0)
        {
            lastSpawnTime = Time.time;
            if (team == 1) { direction = 1; }
            else if (team == 2) { direction = -1; }
            spawnPosition = -((mapWidth / 2.0f) + 3.0f) * direction;

            SquadManager squad = Instantiate(squadPrefab).GetComponent<SquadManager>();
            squad.soldierNum = soldierNum;
            squad.soldierTotalHealthPoints = healthPoints;
            squad.walkingSpeed = walkingSpeed;
            squad.spawnPosition = spawnPosition;
            squad.weaponRange = weaponRange;
            squad.fireCooldownTime = fireCooldownTime;
            squad.accuracy = accuracy;
            squad.direction = direction;
            squad.team = team;
            squad.mapWidth = mapWidth;
            squad.squadType = squadType;
            squad.soldierPrefab = soldierPrefab;
            if (squadType == "assault" || squadType == "machine gun")
            {
                squad.burstCooldownTime = burstCooldownTime;
            }
            if (isHumanPlayer) { resetAllOverlays(); }
        }
    }

    public void spawnRifleSquad ()
    {
        soldierSpawnTime = 6;
        if (lastSpawnTime + soldierSpawnTime < Time.time)
        {
            soldierNum = 6;
            healthPoints = 1;
            walkingSpeed = 0.023f;
            weaponRange = 10.0f;
            fireCooldownTime = 2;
            accuracy = 0.2f;
            squadType = "rifle";
            if (team == 1) { soldierPrefab = soldierPrefabContainer.GetComponent<SoldierPrefabContainer>().frenchRiflePrefab; }
            else { soldierPrefab = soldierPrefabContainer.GetComponent<SoldierPrefabContainer>().germanRiflePrefab; }
            spawnSquad();
        }
    }

    public void spawnAssaultSquad ()
    {
        soldierSpawnTime = 9;
        if (lastSpawnTime + soldierSpawnTime < Time.time)
        {
            soldierNum = 4;
            healthPoints = 1;
            walkingSpeed = 0.034f;
            weaponRange = 5.0f;
            fireCooldownTime = 0.2f;
            accuracy = 0.1f;
            burstCooldownTime = 0.5f;
            squadType = "assault";
            if (team == 1) { soldierPrefab = soldierPrefabContainer.GetComponent<SoldierPrefabContainer>().frenchAssaultPrefab; }
            else { soldierPrefab = soldierPrefabContainer.GetComponent<SoldierPrefabContainer>().germanAssaultPrefab; }
            spawnSquad();
        }
    }

    public void spawnMachinGunSquad()
    {
        soldierSpawnTime = 12;
        if (lastSpawnTime + soldierSpawnTime < Time.time)
        {
            soldierNum = 2;
            healthPoints = 1;
            walkingSpeed = 0.023f;
            weaponRange = 10.0f;
            fireCooldownTime = 0.15f;
            accuracy = 0.2f;
            burstCooldownTime = 0.5f;
            squadType = "machine gun";
            if (team == 1) { soldierPrefab = soldierPrefabContainer.GetComponent<SoldierPrefabContainer>().frenchMachineGunPrefab; }
            else { soldierPrefab = soldierPrefabContainer.GetComponent<SoldierPrefabContainer>().germanMachineGunPrefab; }
            spawnSquad();
        }
    }

    public void spawnSniperSquad()
    {
        soldierSpawnTime = 15;
        if (lastSpawnTime + soldierSpawnTime < Time.time)
        {
            soldierNum = 1;
            healthPoints = 1;
            walkingSpeed = 0.023f;
            weaponRange = 16.0f;
            fireCooldownTime = 2;
            accuracy = 1;
            squadType = "sniper";
            if (team == 1) { soldierPrefab = soldierPrefabContainer.GetComponent<SoldierPrefabContainer>().frenchSniperPrefab; }
            else { soldierPrefab = soldierPrefabContainer.GetComponent<SoldierPrefabContainer>().germanSniperPrefab; }
            spawnSquad();
        }
    }

    public void spawnArmourSquad()
    {
        soldierSpawnTime = 18;
        if (lastSpawnTime + soldierSpawnTime < Time.time)
        {
            soldierNum = 3;
            healthPoints = 10;
            walkingSpeed = 0.018f;
            weaponRange = 8.0f;
            fireCooldownTime = 3;
            accuracy = 0.15f;
            squadType = "armour";
            if (team == 1) { soldierPrefab = soldierPrefabContainer.GetComponent<SoldierPrefabContainer>().frenchArmourPrefab; }
            else { soldierPrefab = soldierPrefabContainer.GetComponent<SoldierPrefabContainer>().germanArmourPrefab; }
            spawnSquad();
        }
    }
}
