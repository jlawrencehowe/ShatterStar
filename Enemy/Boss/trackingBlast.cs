using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class trackingBlast : EnemyBullet
{


	public float turnSpeed;
	private float actualDeltaTime;

	protected override void Start ()
	{
		base.Start ();
	}

	protected override void Update ()
	{
		base.Update ();
		actualDeltaTime = Time.deltaTime;
		if (timeSlow) {
			actualDeltaTime /= 4;
		}
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
			RotateRight (this.gameObject);
		} else {
			RotateLeft (this.gameObject);
		}
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






    
}
