using UnityEngine;
using System.Collections;

public class Charger : EnemyBase {

	private float speedMulti;
	private float forwardSpeedDirection;
	private float forwardSpeedTimer;
	public GameObject thrusterHolder;

	// Use this for initialization
	public override void Start () {
		base.Start ();
		speedMulti = 1;
		moveSpeed = 4.25f;
		turnSpeed = 75;
		maxHealth = 45;
		health = 45;
		suicideDamage = 50;
		forwardSpeedDirection = transform.rotation.eulerAngles.z;
	}


	protected override void EnemyMovement ()
	{
		
	
		float angle = FindTargetAngle (this.gameObject, gc.pc.gameObject.transform.position, false);
		float enemyRot = transform.rotation.eulerAngles.z;

		float posTest;
		float minTest;
		if (enemyRot < angle) {
			posTest = angle - enemyRot;
			minTest = enemyRot + (360 - angle);
		} else {
			minTest = enemyRot - angle;
			posTest = angle + (360 - enemyRot);
		}
		if (posTest < minTest) {
			RotateRight (this.gameObject, posTest, angle);
		} else{
			RotateLeft (this.gameObject, minTest, angle);
		}
		forwardSpeedTimer += actualDeltaTime;
		if(forwardSpeedTimer >= 1){
			speedMulti += actualDeltaTime * 0.5f;
			if(speedMulti > 2f){
				speedMulti = 2f;
			}
		}
		else{
			speedMulti -= actualDeltaTime * 0.25f;
			if(speedMulti <= 1){
				speedMulti = 1;
			}
		}
		var tempScale = thrusterHolder.transform.localScale;
		tempScale.x = speedMulti;
		tempScale.y = speedMulti;
		thrusterHolder.transform.localScale = tempScale;
		//Debug.Log(speedMulti);
		MoveForward();



		/*
		  if (posTest < minTest && (Mathf.Abs(angle - enemyRot) > 5)) {
			RotateRight (this.gameObject);
		} else if(Mathf.Abs(angle - enemyRot) > 5){
			RotateLeft (this.gameObject);
		}
		else{
			forwardSpeedTimer += actualDeltaTime;
		}
		if(forwardSpeedTimer >= 1){
			speedMulti += actualDeltaTime * 0.5f;
			if(speedMulti > 1.5f){
				speedMulti = 1.5f;
			}
		}
		else{
			speedMulti -= actualDeltaTime;
			if(speedMulti <= 1){
				speedMulti = 1;
			}
		}
		Debug.Log(speedMulti);
		MoveForward();
		 */


	}

	private void MoveForward(){

		Vector3 tempPos = transform.position;
		tempPos = tempPos + (transform.up * moveSpeed * speedMulti * actualDeltaTime);
		transform.position = tempPos;
	}

	private void RotateRight (GameObject rotatedObject, float rotAmount, float targetAngle)
	{
		Vector3 tempRot = rotatedObject.transform.rotation.eulerAngles;
		if(rotAmount < 0.1f){
			tempRot.z = targetAngle;
		}
		else{
			tempRot.z += turnSpeed * actualDeltaTime;
			forwardSpeedTimer = 0;
		}
		rotatedObject.transform.rotation = Quaternion.Euler (tempRot);
	}
	
	private void RotateLeft (GameObject rotatedObject, float rotAmount, float targetAngle)
	{

		Vector3 tempRot = rotatedObject.transform.rotation.eulerAngles;
		if(rotAmount < 0.1f){
			tempRot.z = targetAngle;
		}
		else{
			tempRot.z -= turnSpeed * actualDeltaTime;
			forwardSpeedTimer = 0;
		}
		rotatedObject.transform.rotation = Quaternion.Euler (tempRot);
	}
	
	private float FindTargetAngle (GameObject rotatedObject, Vector3 targetLoc, bool rightAngle)
	{
		
		targetLoc.z = 0f;
		
		Vector3 objectPos = rotatedObject.transform.position;
		targetLoc.x = targetLoc.x - objectPos.x;
		targetLoc.y = targetLoc.y - objectPos.y;
		float angle;
		if (!rightAngle) {
			angle = Mathf.Atan2 (targetLoc.y, targetLoc.x) * Mathf.Rad2Deg - 90;
		} else {
			angle = Mathf.Atan2 (targetLoc.y, targetLoc.x) * Mathf.Rad2Deg;
		}
		if (angle < 0) {
			angle += 360;
		}
		
		return angle;
		
	}
	

}
