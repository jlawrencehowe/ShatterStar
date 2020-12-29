using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Artillery : EnemyBase
{

	private float runDistance = 10f;
	private float enemyCloseDistance = 4f;
	private float fireDistance = 6f;
	private bool swap, running;
	private Vector3 targetLocation;
	public float maxRandom;
	private float randomRunningTimer;
	public bool chargingLaser;
	
	public enum State
	{
		running,
		returning}
	;
	
	public State currentState = State.returning;
	public List<GameObject> firePoints;
	public List<GameObject> heldBullets;
	public GameObject homingLaser;
	public AudioClip laserChargeSFX;
	
	// Use this for initialization
	public override void Start ()
	{
		base.Start ();
		maxHealth = 250;
		health = 250;
		maxShield = 0;
		moveSpeed = 4f;
		turnSpeed = 100;
		fireRange = 100;
		damage = 100;
		anim.SetBool ("DelayFire", true);
		suicideDamage = 100;
		expAmount = 100;
	}
	
	protected void EnemyShoot (float tarAngle, float curAngle)
	{

		chargingLaser = false;
		currentState = State.running;
		anim.SetBool ("DelayFire", true);
		for (int i = 0; i < heldBullets.Count; i++) {
			heldBullets [i].GetComponent<EnemyHomingLaser> ().FinishCharging ();
			heldBullets [i].GetComponent<EnemyHomingLaser> ().InitStats (7, damage, 6, gc);
		}
		heldBullets.Clear ();
		audioSource.Stop ();
		
	}

	public void CreateBullets ()
	{
		if (!gc.pc.isDead) {
			Vector3 currentRot = this.transform.rotation.eulerAngles;
			currentRot.z -= 180;
			if (!audioSource.isPlaying) {
				audioSource.clip = laserChargeSFX;
				audioSource.Play ();
			}
			for (int i = 0; i < firePoints.Count; i++) {
				GameObject tempLaser = Instantiate (homingLaser, firePoints [i].transform.position, Quaternion.Euler (currentRot)) as GameObject;
				heldBullets.Add (tempLaser);
			
			}
		}
	}
	
	protected override void EnemyMovement ()
	{
		
		if (!gc.pc.isDead) {
			//grab base move speed for altering
			float actualSpeed = moveSpeed;
			//grab base turn speed for altering
			float actualTurnSpeed = turnSpeed;
			//grab player location
			Vector3 target = gc.pc.transform.position;
			//make sure Z isn't an issue
			target.z = transform.position.z;
			//grab this location
			Vector3 objectPos = transform.position;
			//get distance between player and enemy
			float distance = Mathf.Abs (Vector3.Distance (target, objectPos));


			//swap between running and attacking

			//once far enough away and running, will start to return towards the player
			if (distance > runDistance && currentState == State.running) {
				currentState = State.returning;

			} 
		//if not already runnning and player is too close, start running
		else if (distance < enemyCloseDistance && currentState != State.running && !chargingLaser) {
				currentState = State.running;
				randomRunningTimer = 2f;
				//creating random run pattern
				targetLocation = gc.pc.transform.position + (((Random.value * maxRandom) - (maxRandom / 2)) * transform.right);
			}
//		//if close enough, start firing
//		else if(distance < fireDistance && currentState == State.returning){
//			currentState = State.firing;
//		}
			//start slowing down when getting close to firing range
			if ((currentState == State.returning && distance <= fireDistance + 2) || chargingLaser) {
				if (!chargingLaser) {
					fireCD = 3f;
				}
				float tempPercentSpeed = (2 - (3f - fireCD)) / 2;
				if (tempPercentSpeed > 1 || fireCD == 5) {
					tempPercentSpeed = 1;
				}
				if (tempPercentSpeed <= 0 || fireCD <= 2.5f) {
					anim.SetBool ("DelayFire", false);
					tempPercentSpeed = 0;

				}
				actualSpeed = tempPercentSpeed * moveSpeed;
				foreach (var thruster in thrusters) {
					thruster.enabled = false;
				}
				chargingLaser = true;
			}
			Debug.Log (fireCD);
			//Debug.Log(actualSpeed);
		
		
			//makes enemy run from player
			if (currentState == State.running) {
				target = targetLocation;
				//changes running target location
				randomRunningTimer -= Time.deltaTime;
				if (randomRunningTimer <= 0) {
					targetLocation = gc.pc.transform.position + (((Random.value * maxRandom) - (maxRandom / 2)) * transform.right);
					randomRunningTimer = 1f;
				}
			
			}
		
			//directional vector towards player
			target.x = target.x - objectPos.x;
			target.y = target.y - objectPos.y;

			//get vector away from player if running
			if (currentState == State.running) {
				target = -target;
			}
		
	
		
			//angle calculation
			float angle = Mathf.Atan2 (target.y, target.x) * Mathf.Rad2Deg - 90;
			if (angle < 0) {
				angle += 360;
			}
			float enemyRot = transform.rotation.eulerAngles.z;
			if (chargingLaser && fireCD <= 0) {
				foreach (var thruster in thrusters) {
					thruster.enabled = true;
				}
				EnemyShoot (angle, enemyRot);
			}
		
			if (timeSlowed) {
				actualTurnSpeed /= 4;
				actualSpeed /= 4;
			}
			if (!chargingLaser) {
				RotateEnemy (enemyRot, angle, actualTurnSpeed);
			}
			MoveEnemy (actualSpeed);
		
		}
		else{
			anim.enabled = false;
		}
		
		
	}

	private void MoveEnemy (float actualSpeed)
	{
		//move object up based on speed, up transform, and time
		Vector3 tempPos = transform.position;
		tempPos = tempPos + (transform.up * actualSpeed * Time.deltaTime);
		transform.position = tempPos;
	}

	private void RotateEnemy (float enemyRot, float angle, float actualTurnSpeed)
	{
		if (enemyRot < angle) {
			float posTest = angle - enemyRot;
			float minTest = enemyRot + (360 - angle);
			if (posTest < minTest) {
				Vector3 tempRot = transform.rotation.eulerAngles;
				tempRot.z += actualTurnSpeed * Time.deltaTime;
				transform.rotation = Quaternion.Euler (tempRot);
			} else {
				Vector3 tempRot = transform.rotation.eulerAngles;
				tempRot.z -= actualTurnSpeed * Time.deltaTime;
				transform.rotation = Quaternion.Euler (tempRot);
			}
				
		} else {
			float minTest = enemyRot - angle;
			float posTest = angle + (360 - enemyRot);
			if (posTest < minTest) {
				Vector3 tempRot = transform.rotation.eulerAngles;
				tempRot.z += actualTurnSpeed * Time.deltaTime;
				transform.rotation = Quaternion.Euler (tempRot);
			} else {
				Vector3 tempRot = transform.rotation.eulerAngles;
				tempRot.z -= actualTurnSpeed * Time.deltaTime;
				transform.rotation = Quaternion.Euler (tempRot);
			}
		}

	}

	protected override void Death ()
	{
		foreach (var bullet in heldBullets) {
			Destroy (bullet);
		}

		base.Death ();
	}
	

}
