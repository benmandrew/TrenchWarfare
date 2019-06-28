using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGunManager : SoldierBaseManager {

    // Update is called once per frame
    void Update()
    {
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
            if (!squad.isFighting) { isBursting = false; }
            readyToBurst();
            readyToFire();
        }
        updateAnimator();
    }

    void readyToFire()
    {
        if (isFighting && targetSoldier != null
            && shotsLeftInBurst > 0 && (lastFireTime + fireCooldownTime) < Time.time)
        {
            fireTrigger = true;
        }

        if (shotsLeftInBurst <= 0)
        {
            fireTrigger = false;
        }
    }

    void readyToBurst()
    {
        if (isFighting && targetSoldier != null
            && /*shotsLeftInBurst <= 0 &&*/ (lastFireTime + burstCooldownTime + burstCooldownTimeModifier) < Time.time)
        {
            shotsLeftInBurst = shotsInBurst + Random.Range(-1, 1);
            burstCooldownTimeModifier = Random.Range(0.0f, 1.0f);
            isBursting = true;
        }
    }

    public void singleFire()
    {
        if (targetSoldier != null)
        {
            shotsLeftInBurst -= 1;
            lastFireTime = Time.time;
            float hitChanceModifier = 1.0f;
            if (targetSoldier.inTrench)
            {
                hitChanceModifier = 0.5f;
            }
            bool shotHit = Random.Range(0.0f, 1.0f) < accuracy * hitChanceModifier;
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
}
