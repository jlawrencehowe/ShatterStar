using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class BossRailgun : EnemyBase
{

    private bool charginRailgun;
    private bool stopMovement;
    public List<Transform> railGunFireLocs;
    public  bool isActive = false;
    private bool firingRailgun = false;
    private bool downTime = false;
    private List<GameObject> hitObjects;
    private BoxCollider2D railgunHitBox;


    public override void Start()
    {
        gc = GameObject.Find("GameController").GetComponent<GameController>();
        hitObjects = new List<GameObject>();
        railgunHitBox = GetComponent<BoxCollider2D>();
        turnSpeed = 50;
        damage = 150;

    }

    protected override void FixedUpdate()
    {
        if (isActive)
        {
            base.FixedUpdate();
            if (!downTime && !charginRailgun && !firingRailgun)
            {
                downTime = true;
                Invoke("StartCharging", 3);
            }
            else
            {

            }
        }
    }

    protected override void EnemyMovement()
    {
      
            float angle = FindTargetAngle(this.gameObject, gc.pc.gameObject.transform.position, false);
            float enemyRot = transform.rotation.eulerAngles.z;
        Debug.DrawRay(this.transform.position, transform.up * 100, Color.red);
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

    private void StartCharging()
    {
        charginRailgun = true;
        downTime = false;
        Invoke("FireRailGun", 3);
        //setup redline effect to show player it is starting to fire
    }

    private void FireRailGun()
    {
        //actually fire the railgun
        firingRailgun = true;
        railgunHitBox.enabled = true;
        charginRailgun = false;
        InvokeRepeating("RailGunDamage", 0f, 1f);
        Invoke("StopRailGun", 3f);
    }

    private void RailGunDamage()
    {
        foreach (var hit in hitObjects)
        {
            if(hit != null && hit.tag == "Player")
            {
                hit.GetComponent<PlayerController>().TakeDamage(damage);
                Debug.Log("Player damage");
            }
            else if(hit != null && hit.tag == "Enemy")
            {
                hit.GetComponent<EnemyBase>().TakeDamage(damage);
                Debug.Log("Enemy damage");
            }
        }

    }

    private void StopRailGun()
    {
        railgunHitBox.enabled = false;
        firingRailgun = false;
        CancelInvoke();

    }

   

    public override void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player" ){
            if (!hitObjects.Contains(col.gameObject))
            {
                hitObjects.Add(col.gameObject);
                col.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
                Debug.Log("Player trigger");
                
            }
        }
        if((col.gameObject.tag == "Enemy" && col.gameObject.name != "Boss"))
        {
            hitObjects.Add(col.gameObject);
            col.gameObject.GetComponent<EnemyBase>().TakeDamage(damage);
            Debug.Log("Enemy trigger");
        }
    }

    public void ActivateRailGun()
    {
        isActive = true;
    }

    public void DeactivateRailGun()
    {
        StopRailGun();
        CancelInvoke();
        isActive = false;
    }

    public void OnTriggerExit2D(Collider2D col)
    {
        hitObjects.Remove(col.gameObject);
    }

    #region obsolete
    private void StopMovement()
    {
        stopMovement = true;
    }

    private void ResumeMovement()
    {
        stopMovement = false;
        charginRailgun = false;
    }
    #endregion


    #region Rotate Functions
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
