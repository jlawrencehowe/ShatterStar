using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMissile : EnemyBase
{

    public GameObject explosionReference;
    float moveTimer = 0.5f;
    public static Boss boss;
    private bool exploded = false;
    // Use this for initialization
    public override void Start()
    {
        base.Start();
        moveSpeed = 5f;
        turnSpeed = 125;
        maxHealth = 120;
        health = 1;
        suicideDamage = 25;
        expAmount = 0;
        Invoke("Explode", 300);
    }

    protected override void FixedUpdate()
    {
        if (moveTimer >= 0)
        {
            Vector3 tempPos = this.transform.position;
            tempPos += (Time.deltaTime * this.transform.up * moveSpeed);
            this.transform.position = tempPos;
            moveTimer -= Time.deltaTime;
        }
        else
        {
            base.FixedUpdate();
        }

    }

    protected override void EnemyMovement()
    {

        float angle = FindTargetAngle(this.gameObject, gc.pc.gameObject.transform.position, false);
        float enemyRot = transform.rotation.eulerAngles.z;

        float posTest;
        float minTest;
        if (enemyRot < angle)
        {
            posTest = angle - enemyRot;
            minTest = enemyRot + (360 - angle);
        }
        else
        {
            minTest = enemyRot - angle;
            posTest = angle + (360 - enemyRot);
        }
        if (posTest < minTest && (Mathf.Abs(angle - enemyRot) > 5))
        {
            RotateRight(this.gameObject);
        }
        else if (Mathf.Abs(angle - enemyRot) > 5)
        {
            RotateLeft(this.gameObject);
        }

        MoveForward();


    }

    private void MoveForward()
    {

        Vector3 tempPos = transform.position;
        tempPos = tempPos + (transform.up * moveSpeed * actualDeltaTime);
        transform.position = tempPos;
    }

    private void RotateRight(GameObject rotatedObject)
    {
        Vector3 tempRot = rotatedObject.transform.rotation.eulerAngles;
        tempRot.z += turnSpeed * actualDeltaTime;
        rotatedObject.transform.rotation = Quaternion.Euler(tempRot);
    }

    private void RotateLeft(GameObject rotatedObject)
    {
        Vector3 tempRot = rotatedObject.transform.rotation.eulerAngles;
        tempRot.z -= turnSpeed * actualDeltaTime;
        rotatedObject.transform.rotation = Quaternion.Euler(tempRot);
    }

    private float FindTargetAngle(GameObject rotatedObject, Vector3 targetLoc, bool rightAngle)
    {

        targetLoc.z = 0f;

        Vector3 objectPos = rotatedObject.transform.position;
        targetLoc.x = targetLoc.x - objectPos.x;
        targetLoc.y = targetLoc.y - objectPos.y;
        float angle;
        if (!rightAngle)
        {
            angle = Mathf.Atan2(targetLoc.y, targetLoc.x) * Mathf.Rad2Deg - 90;
        }
        else
        {
            angle = Mathf.Atan2(targetLoc.y, targetLoc.x) * Mathf.Rad2Deg;
        }
        if (angle < 0)
        {
            angle += 360;
        }

        return angle;

    }

    public void InitRot(Vector3 rot)
    {
        this.transform.rotation = Quaternion.Euler(rot);
    }


    public override void OnTriggerEnter2D(Collider2D col)
    {

        //instantiate explosion
        if (col.gameObject.tag == "Player" || col.gameObject.name == "BossMissileExplosion")
        {
            Explode();

        }


    }

    private void Explode()
    {
        if (!exploded)
        {
            exploded = true;
            Instantiate(explosionReference, this.transform.position, this.transform.rotation);
            if (boss != null)
                boss.RemoveMissile(this.gameObject);
            Destroy(gameObject);
        }



    }

    protected override void Death()
    {
        dead = true;
        Explode();
        miniMapSprite.enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        foreach (var thruster in thrusters)
        {
            thruster.enabled = false;
        }
        audioSource.clip = shipExplodeSFX;
        audioSource.volume = gc.pgd.sfxVol / 50;
        audioSource.Play();
        gc.LiveEnemies.Remove(this);
        GetComponent<BoxCollider2D>().enabled = false;
        Invoke("FinishDeath", 3);
    }


}
