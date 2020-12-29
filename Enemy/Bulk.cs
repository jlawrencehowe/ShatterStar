﻿using UnityEngine;
using System.Collections;

public class Bulk : EnemyBase {
	
	private float runDistance = 15f;
	private float closeDistance = 3f;
	private bool swap, running;
	private Vector3 targetLocation;
	public float maxRandom;
	private float randomRunningTimer;
	public GameObject bullet;
	
	// Use this for initialization
	public override void Start ()
	{
		base.Start ();
		moveSpeed = 3f;
		turnSpeed = 75;
		maxHealth = 300;
		health = maxHealth;
		maxShield = 100;
		shield = maxShield;
		fireRange = 100;
		damage = 100;
		suicideDamage = 150;
		expAmount = 75;
	}
	
	protected void EnemyShoot(float tarAngle, float curAngle){

		float distance = Vector2.Distance (gc.pc.transform.position, transform.position);
		if(running){
			tarAngle = -tarAngle;
		}
		float aprox = (tarAngle - curAngle);
		/*Debug.Log("Aprox = " + aprox);
		Debug.Log("Fire CD = " + fireCD);
		Debug.Log("Distance = " + distance);*/
		if(aprox < 5f && aprox > -5f && fireCD <= 0 && distance <= fireRange){
			GameObject tempBullet = Instantiate(bullet, this.transform.position, this.transform.rotation) as GameObject;
			tempBullet.GetComponent<EnemyBullet>().InitStats(7, damage, this.transform.rotation, 3, 1, false, false);
			fireCD = 2;
		}



		
	}
	
	protected override void EnemyMovement ()
	{
		//angle calculation
		float angle = FindTargetAngle();
		float enemyRot = transform.rotation.eulerAngles.z;
		EnemyShoot (angle, enemyRot);
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
			RotateRight ();
		} else if(Mathf.Abs(angle - enemyRot) > 5){
			RotateLeft ();
		}
		
		MoveForward();
		
	}
	
	private void MoveForward(){
		
		Vector3 tempPos = transform.position;
		tempPos = tempPos + (transform.up * moveSpeed * actualDeltaTime);
		transform.position = tempPos;
	}
	
	private void RotateRight ()
	{
		Vector3 tempRot = this.transform.rotation.eulerAngles;
		tempRot.z += turnSpeed * actualDeltaTime;
		this.transform.rotation = Quaternion.Euler (tempRot);
	}
	
	private void RotateLeft ()
	{
		Vector3 tempRot = this.transform.rotation.eulerAngles;
		tempRot.z -= turnSpeed * actualDeltaTime;
		this.transform.rotation = Quaternion.Euler (tempRot);
	}
	
	private float FindTargetAngle ()
	{
		
		//grab player location
		Vector3 target = gc.pc.transform.position;
		//make sure Z isn't an issue
		target.z = transform.position.z;
		//grab this location
		Vector3 objectPos = transform.position;
		
		//swap between running and attacking
		if (Mathf.Abs (Vector3.Distance (target, objectPos)) > runDistance && running) {
			running = false;
		} else if (Mathf.Abs (Vector3.Distance (target, objectPos)) < closeDistance && !running) {
			running = true;
			randomRunningTimer = 2f;
			//creating random run pattern
			targetLocation = gc.pc.transform.position + (((Random.value * maxRandom) - (maxRandom/2)) * transform.right);
		}
		
		if (running) {
			target = targetLocation;
		}
		
		//directional vector
		target.x = target.x - objectPos.x;
		target.y = target.y - objectPos.y;
		
		//makes enemy run from player
		if (running) {
			target = -target;
			//changes running target location
			randomRunningTimer -= Time.deltaTime;
			if(randomRunningTimer <= 0){
				targetLocation = gc.pc.transform.position + (((Random.value * maxRandom) - (maxRandom/2)) * transform.right);
				randomRunningTimer = 1f;
			}
			
		}
		
		//angle calculation
		float angle = Mathf.Atan2 (target.y, target.x) * Mathf.Rad2Deg - 90;
		if (angle < 0) {
			angle += 360;
		}
		return angle;
		
	}


}