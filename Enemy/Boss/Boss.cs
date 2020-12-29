using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Boss : EnemyBase
{

    public enum State
    {
        Intro, Phase1, Phase2, Phase3, End
    }
    //triggers a one time transition for setting values between states
    public bool transitionPhase = true;
    //the starting pos of main
    public Vector3 startPos;
    //the ending position of intro
    public Vector3 endPos;
    //movement speed
    public float speed;
    private float spawnEnemyTimer;
    private GameObject currentSpawned;
    private float spawnedUnitSpeed = 1;
    public List<GameObject> spawnableEnemies;
    private List<GameObject> activeEnemies;
    public List<Transform> spawnableLocations;
    //private GameObject launchingEnemy;
    public GameObject waveBlast;
    public Transform waveBlastFireLoc;
    public GameObject trackingBlast;
    public List<Transform> trackingBlastFireLoc;
    public float trackingBlastDamage;
    public float trackingBlastSpeed;
    //private GameObject laser;
    //private List<Transform> laserFireLoc;
    private Animator animCont;
    public float knockBackDamage;
    public float knockBackForce;
    delegate void PhaseFunc();
    PhaseFunc phaseFunc;
    public State currentState = State.Intro;

    public float waveBlastSpeed;
    public float waveBlastDamage;

    public GameObject bossUIReference;
    public Canvas bossUI;
    public Image bossHealth;
    public float bossMaxHealth;
    public GameObject missileRef;
    public List<Transform> missileLauncherLocs;
    private List<GameObject> activeMissiles;
    bool waveBlastLoc1 = false;
    public BossRailgun bossRailGun;


    public override void Start()
    {
        base.Start();
        activeMissiles = new List<GameObject>();
        activeEnemies = new List<GameObject>();
        Vector3 tempPos = gc.pc.transform.position;
        tempPos.z = 8;
        tempPos.y += 14;
        this.transform.position = tempPos;
        this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
        startPos = tempPos;
        tempPos = startPos;
        tempPos.y -= 5.5f;
        endPos = tempPos;
        this.GetComponent<BoxCollider2D>().enabled = false;
        phaseFunc = IntroTransition;
        animCont = GetComponent<Animator>();
        GameObject tempUI = Instantiate(bossUIReference, this.transform) as GameObject;
        bossUI = tempUI.GetComponent<Canvas>();
        bossHealth = GameObject.Find("BossHealthBar").GetComponent<Image>();
        health = 600;
        bossMaxHealth = health;
        knockBackForce = 50;
        knockBackDamage = 100;
        waveBlastDamage = 100;
        waveBlastSpeed = 1;
        BossMissile.boss = this;
    }


    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (currentSpawned != null)
        {
            currentSpawned.transform.position = (currentSpawned.transform.position +
                (currentSpawned.transform.up * spawnedUnitSpeed * Time.deltaTime));
        }
    }


    protected override void EnemyMovement()
    {

        //intro slow move onto screen
        //wall appears to block user from leaving area around the main
        //maybe have weakpoints on the main
        //boss spawns enemies
        //has turrets surrounding main
        //turrets are children of main and have their own scripts
        //turrets have sight range only around them
        //main has actual collision, pushes user away on contact, dealing damage, but user cannot go through main

        phaseFunc();
    }

    public void IntroTransition()
    {
        speed = 0.5f;
        phaseFunc = Intro;
        gc.pc.GetComponent<BoxCollider2D>().enabled = false;
    }

    public void Intro()
    {



        Vector3 tempPos = this.transform.position;
        tempPos.y -= (Time.deltaTime * speed);
        this.transform.position = tempPos;
        if (this.transform.position.y <= endPos.y)
        {
            phaseFunc = Phase1Transition;
        }

    }


    public void Phase1Transition()
    {
        anim.SetTrigger("Phase1");
        phaseFunc = Phase1;
        speed = 0;
        //set active
        this.GetComponent<BoxCollider2D>().enabled = true;
        //static variable of the boss turret class, when true starts up the turrets
        BossTurret.activate = true;
        InvokeRepeating("SpawnEnemy", 5, 10);
        InvokeRepeating("FireMissile", 10, 10);
        gc.pc.GetComponent<BoxCollider2D>().enabled = true;


    }

    //main phase 1
    public void Phase1()
    {

        Debug.Log("Phase 1");

        if (health <= (0.66 * bossMaxHealth))
        {
            //phaseFunc = Phase2Transition;
            Debug.Log("Phase 2 start");
            anim.SetTrigger("Phase1End");
            RemoveAllMissiles();
            KillActiveEnemies();
            gc.pc.GetComponent<BoxCollider2D>().enabled = false;
            CancelInvoke();
        }

    }

    //phase 1 animation
    

    //end phase 1 and start phase 2
    public void Phase1Finish()
    {
        Debug.Log("Phase1Finish");
       
        phaseFunc = Phase2Transition;
    }

    public void SpawnEnemy()
    {
        //location closest spawnPoint to player
        if (currentSpawned == null)
        {
            Transform closestLocation = spawnableLocations[0];
            Vector2 playerLoc = gc.pc.transform.position;
            for (int i = 1; i < spawnableLocations.Count; i++)
            {
                if (Vector3.Distance(playerLoc, spawnableLocations[i].position) < Vector3.Distance(playerLoc, closestLocation.position))
                {

                    closestLocation = spawnableLocations[i];
                }

            }

            //randomly pick a number between 0 and the size of spawnableEnemies
            int random = Random.Range(0, 3);
            Vector3 tempPos = closestLocation.position;
            tempPos.z = 10;
            currentSpawned = Instantiate(spawnableEnemies[random], tempPos, closestLocation.rotation) as GameObject;
            currentSpawned.GetComponent<EnemyBase>().enabled = false;
            activeEnemies.Add(currentSpawned);
            Invoke("ActivateUnit", 1.5f);
            //grab code from dreadnaught for launching enemy

        }


        //instantiate new enemy using code similar to the dreadnaught
        //launch enemy
    }

    private void ActivateUnit()
    {
        if (currentSpawned != null)
        {
            currentSpawned.GetComponent<EnemyBase>().enabled = true;
            currentSpawned = null;
        }
    }

    private void KillActiveEnemies()
    {
        foreach (var enemy in activeEnemies)
        {
            if(enemy != null)
                Destroy(enemy);
        }
    }

    private void FireMissile()
    {

        foreach (Transform spawn in missileLauncherLocs)
        {
            var tempMissile = Instantiate(missileRef, spawn.position, spawn.rotation) as GameObject;

            activeMissiles.Add(tempMissile);
        }
    }

    public void RemoveMissile(GameObject missile)
    {
        activeMissiles.Remove(missile);
    }

    private void RemoveAllMissiles()
    {
        foreach (GameObject missile in activeMissiles)
        {
            Destroy(missile);
        }
        activeMissiles.Clear();
    }

    private void Phase2Transition()
    {
        Debug.Log("Phase2Transition");
        animCont.SetTrigger("Phase2");
        BossTurret.activate = false;
        BossTurret.deactivate = true;
        bossRailGun.ActivateRailGun();
        gc.pc.GetComponent<BoxCollider2D>().enabled = true;

        Invoke("FireWaveAttack", 12);
        InvokeRepeating("SpawnEnemy", 5, 5);
        phaseFunc = Phase2;
    }

    private void Phase2()
    {
        if (health <= (maxHealth * 0.33))
        {
            CancelInvoke();
            //phaseFunc = Phase3Transition;

            anim.SetTrigger("Phase2End");
            KillActiveEnemies();
            bossRailGun.DeactivateRailGun();
        }
    }
    

    private void FireTrackingBlast()
    {

        Transform closestLocation = trackingBlastFireLoc[0];
        Vector2 playerLoc = gc.pc.transform.position;
        for (int i = 1; i < trackingBlastFireLoc.Count; i++)
        {
            if (Vector3.Distance(playerLoc, trackingBlastFireLoc[i].position) > Vector3.Distance(playerLoc, closestLocation.position))
            {

                closestLocation = spawnableLocations[i];
            }

        }
        GameObject tempBullet = Instantiate(trackingBlast, closestLocation.position, closestLocation.rotation) as GameObject;
        tempBullet.GetComponent<trackingBlast>().InitStats(trackingBlastSpeed, trackingBlastDamage, closestLocation.rotation, 10, 1, false, true);


    }

    private void FireWaveAttack()
    {
        waveBlastLoc1 = !waveBlastLoc1;
        var startingRot = Quaternion.Euler(0, 0, 0);
        if (waveBlastLoc1)
        {
            startingRot = Quaternion.Euler(0, 0, 60);
            Invoke("FireWaveAttack", 5);
        }
        else
        {
            Invoke("FireWaveAttack", 12);
        }
        
        for (int i = 0; i < 3; i++)
        {
            var tempPos = this.transform.position;
            tempPos.z = 25;

            GameObject tempWaveBlast = Instantiate(waveBlast, tempPos, startingRot) as GameObject;
            tempWaveBlast.GetComponent<waveBlast>().InitStats(waveBlastSpeed, waveBlastDamage, startingRot, 10, 1, false, false);
            Vector3 tempAngle = startingRot.eulerAngles;
            tempAngle.z += 120;
            startingRot.eulerAngles = tempAngle;

        }


    }

    private void FireLasers()
    {
        //Instantiate(laser, laserFireLoc.position, laserFireLoc.rotation);

    }


    private void Phase3Transition()
    {
        animCont.SetTrigger("Phase3");
        phaseFunc = Phase3;
    }

    private void Phase3()
    {

        //phase 3?

        //movement
        //acts closer to normal enemies but faster and stronger


        if (health <= 0)
        {
            phaseFunc = End;
            anim.SetTrigger("Phase3End");
            bossRailGun.DeactivateRailGun();
        }




    }


    private void Phase3Finish()
    {
        //called by animation

        //set boxcollider size
        BoxCollider2D boxCollider2D = GetComponent<BoxCollider2D>();
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        boxCollider2D.offset = new Vector2(0, 0);
        boxCollider2D.size = new Vector3(sprite.bounds.size.x / transform.lossyScale.x,
                                         sprite.bounds.size.y / transform.lossyScale.y,
                                         sprite.bounds.size.z / transform.lossyScale.z);
        phaseFunc = Phase3;
    }

    protected override void Death()
    {
        
    }

   

  

    private void End()
    {
        animCont.SetTrigger("End");
        GetComponent<BoxCollider2D>().enabled = false;
        //end game

    }


    public override void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            coll.gameObject.GetComponent<PlayerController>().TakeDamage(knockBackDamage);
            KnockPlayerBack(coll.gameObject);

        }
    }

    private void KnockPlayerBack(GameObject player)
    {
        player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        Vector3 direction = (player.transform.position - this.transform.position);
        player.GetComponent<Rigidbody2D>().AddForce(direction * knockBackForce);
    }


    protected override void UpdateHealthBar()
    {
        Debug.Log("Health: " + health);
        bossHealth.fillAmount = health / bossMaxHealth;
    }


}
