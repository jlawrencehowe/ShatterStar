using UnityEngine;
using System.Collections;

public class Singlet : EnemyBase {

	float moveTimer = 0.5f;
	
	// Use this for initialization
	public override void Start () {
		base.Start ();
		moveSpeed = 5f;
		turnSpeed = 125;
		maxHealth = 120;
		health = 50;
		suicideDamage = 25;
		expAmount = 25;
	}

	protected override void FixedUpdate ()
	{
		if (moveTimer >= 0) {
			Vector3 tempPos = this.transform.position;
			tempPos += (Time.deltaTime * this.transform.up * moveSpeed);
			this.transform.position = tempPos;
			moveTimer -= Time.deltaTime;
		} 
		else {
			base.FixedUpdate ();
		}

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
		if (posTest < minTest && (Mathf.Abs(angle - enemyRot) > 5)) {
			RotateRight (this.gameObject);
		} else if(Mathf.Abs(angle - enemyRot) > 5){
			RotateLeft (this.gameObject);
		}
		
		MoveForward();
		
		
	}
	
	private void MoveForward(){
		
		Vector3 tempPos = transform.position;
		tempPos = tempPos + (transform.up * moveSpeed * actualDeltaTime);
		transform.position = tempPos;
	}
	
	private void RotateRight (GameObject rotatedObject)
	{
		Vector3 tempRot = rotatedObject.transform.rotation.eulerAngles;
		tempRot.z += turnSpeed * actualDeltaTime;
		rotatedObject.transform.rotation = Quaternion.Euler (tempRot);
	}
	
	private void RotateLeft (GameObject rotatedObject)
	{
		Vector3 tempRot = rotatedObject.transform.rotation.eulerAngles;
		tempRot.z -= turnSpeed * actualDeltaTime;
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

	public void InitRot(Vector3 rot){
		this.transform.rotation = Quaternion.Euler (rot);
	}


}
