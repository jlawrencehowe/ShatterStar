using UnityEngine;
using System.Collections;

public class Safety : EnemyBase
{

	private float runDistance = 10f;
	private float enemyCloseDistance = 3f;
	private float fireDistance = 8f;
	private bool swap, running;
	private Vector3 targetLocation;
	public float maxRandom;
	private float randomRunningTimer;
	public GameObject bullet;
	public float actualSpeed;
	public float actualTurnSpeed;

	private enum State {running, firing, returning};

	private State currentState = State.returning;

	public GameObject turret;
	public GameObject gunPoint;
	
	// Use this for initialization
	public override void Start ()
	{
		base.Start ();
		moveSpeed = 2.5f;
		turnSpeed = 100;
		maxShield = 50;
		fireRange = 100;
	}

	protected void EnemyShoot(float tarAngle, float curAngle){
		
		float distance = Vector2.Distance (gc.pc.transform.position, transform.position);
		float aprox = Mathf.Abs (tarAngle - curAngle);
		if(distance <= fireDistance && aprox <= 0.5f && fireCD <= 0){
			GameObject tempBullet = Instantiate(bullet, gunPoint.transform.position, gunPoint.transform.rotation) as GameObject;
			tempBullet.GetComponent<EnemyBullet>().InitStats(5, 5, turret.transform.rotation, 1, 1, false, false);
			fireCD = 1;
		}
		
	}

	protected override void EnemyMovement ()
	{
		actualSpeed = moveSpeed;
		actualTurnSpeed = turnSpeed;

		//grab player location
		Vector3 target = gc.pc.transform.position;
		Vector3 gunTarget = gc.pc.transform.position;
		//make sure Z isn't an issue
		target.z = transform.position.z;
		gunTarget.z = turret.transform.position.z;
		//grab this location
		Vector3 objectPos = transform.position;
		float distance = Mathf.Abs (Vector3.Distance (target, objectPos));
		//swap between running and attacking
		if (distance > runDistance && currentState == State.running) {
			currentState = State.returning;
		} else if (distance < enemyCloseDistance && currentState != State.running) {
			currentState = State.running;
			randomRunningTimer = 2f;
			//creating random run pattern
			targetLocation = gc.pc.transform.position + (((Random.value * maxRandom) - (maxRandom/2)) * transform.right);
		}
		else if(distance < fireDistance && currentState == State.returning){
			currentState = State.firing;
		}



		//makes enemy run from player
		if (currentState == State.running) {
			target = targetLocation;
			//changes running target location
			randomRunningTimer -= Time.deltaTime;
			if(randomRunningTimer <= 0){
				targetLocation = gc.pc.transform.position + (((Random.value * maxRandom) - (maxRandom/2)) * transform.right);
				randomRunningTimer = 1f;
			}

		}

		//directional vector
		target.x = target.x - objectPos.x;
		target.y = target.y - objectPos.y;
		gunTarget.x = gunTarget.x - objectPos.x;
		gunTarget.y = gunTarget.y - objectPos.y;

		if (currentState == State.running) {
			target = -target;
		}

		if (currentState == State.firing) {
			actualSpeed = (distance - 2.5f)/2 * actualSpeed;
			actualTurnSpeed = (turnSpeed * actualSpeed/moveSpeed);
			//actualTurnSpeed = 0;
			if(actualSpeed < 0.25f){
				actualSpeed = 0.25f;
			}
		}

		//angle calculation
		float angle = Mathf.Atan2 (target.y, target.x) * Mathf.Rad2Deg - 90;
		float gunAngle = Mathf.Atan2 (gunTarget.y, gunTarget.x) * Mathf.Rad2Deg - 90;
		if (angle < 0) {
			angle += 360;
		}
		if (gunAngle < 0) {
			gunAngle += 360;
		}
		GunRotationAndShoot (gunAngle);
		float enemyRot = transform.rotation.eulerAngles.z;

		if (timeSlowed) {
			actualTurnSpeed /= 4;
		}
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

		//move object up based on speed, up transform, and time
		Vector3 tempPos = transform.position;
		if (timeSlowed) {
			tempPos = tempPos + (transform.up * actualSpeed * (Time.deltaTime / 4));
		} else {
			tempPos = tempPos + (transform.up * actualSpeed * Time.deltaTime);
		}
		transform.position = tempPos;



		
	}

	private void GunRotationAndShoot(float angle){

		float tempTurnSpeed = turnSpeed;
		if (timeSlowed) {
			tempTurnSpeed /= 4; 
		}

		float enemyRot = turret.transform.rotation.eulerAngles.z;

		if (enemyRot < angle) {
			float posTest = angle - enemyRot;
			float minTest = enemyRot + (360 - angle);
			if (posTest < minTest) {
				Vector3 tempRot = turret.transform.rotation.eulerAngles;
				tempRot.z += turnSpeed * Time.deltaTime;
				turret.transform.rotation = Quaternion.Euler (tempRot);
			} else {
				Vector3 tempRot = turret.transform.rotation.eulerAngles;
				tempRot.z -= turnSpeed * Time.deltaTime;
				turret.transform.rotation = Quaternion.Euler (tempRot);
			}
			
		} else {
			float minTest = enemyRot - angle;
			float posTest = angle + (360 - enemyRot);
			if (posTest < minTest) {
				Vector3 tempRot = turret.transform.rotation.eulerAngles;
				tempRot.z += turnSpeed * Time.deltaTime;
				turret.transform.rotation = Quaternion.Euler (tempRot);
			} else {
				Vector3 tempRot = turret.transform.rotation.eulerAngles;
				tempRot.z -= turnSpeed * Time.deltaTime;
				turret.transform.rotation = Quaternion.Euler (tempRot);
			}
		}

		
		EnemyShoot (angle, enemyRot);
	}


}
