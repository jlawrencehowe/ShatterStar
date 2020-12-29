using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossTurret : EnemyBase {

	public static bool activate;
    private bool playerInRange = false;
    public GameObject bullet;
    public GameObject firePoint;
    public static bool deactivate;
    

    // Use this for initialization
    public override void Start () {
        base.Start();
        turnSpeed = 100;
        maxHealth = 75;
        health = 75;
        maxShield = 0;
        damage = 100;
    }

    // Update is called once per frame
    protected override void EnemyMovement()
    {
        if (playerInRange && activate)
        {
            RotateTowardsPlayer();
        }
        if (deactivate)
        {
            anim.SetTrigger("Deactivate");
        }
        
    }

    protected void EnemyShoot(float tarAngle, float curAngle)
    {

       
        float aprox = Mathf.Abs(tarAngle - curAngle);
        if (aprox <= 0.5f && fireCD <= 0 && playerInRange)
        {
            GameObject tempBullet = Instantiate(bullet, firePoint.transform.position, this.transform.rotation) as GameObject;
            tempBullet.GetComponent<EnemyBullet>().InitStats(5, damage, this.transform.rotation, 3, 1, false, false);
           
            fireCD = 0.8f;
            
        }

    }

    private void RotateTowardsPlayer()
    {
        float angle = FindTargetAngle(this.gameObject, gc.pc.gameObject.transform.position, false);
        float enemyRot = transform.rotation.eulerAngles.z;

        EnemyShoot(angle, enemyRot);
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
        if (posTest < minTest)
        {
            RotateRight(this.gameObject);
        }
        else
        {
            RotateLeft(this.gameObject);
        }
    }
    #region Trigger Functions

    public override void OnTriggerEnter2D(Collider2D col)
    {
        //base.OnTriggerEnter2D(col);
        if(col.gameObject.tag == "Player")
        {
            playerInRange = true;
        }
    }

    public void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            playerInRange = false;
        }
    }

    #endregion
    #region Rotation Functions
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
    #endregion
}
