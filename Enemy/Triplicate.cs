using UnityEngine;
using System.Collections;

public class Triplicate : EnemyBase {
	
	private float runDistance = 7.5f;
	private float closeDistance = 2.5f;
	private bool swap, running;
	private Vector3 targetLocation;
	public float maxRandom;
	private float randomRunningTimer;
	public GameObject bullet;
	private int bulletsFired = 0;
	public GameObject singlet;
	
	// Use this for initialization
	public override void Start () {
		base.Start ();
		moveSpeed = 3f;
		turnSpeed = 100;
		maxHealth = 175;
		health = maxHealth;
		maxShield = 0;
		shield = maxShield;
		fireRange = 100;
		damage = 50;
		suicideDamage = 100;
		expAmount = 25;
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
		if(aprox < 3f && aprox > -3f && fireCD <= 0 && distance <= fireRange){
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

	public override void OnTriggerEnter2D(Collider2D col){
		
		if (col.gameObject.tag == "Player") {
			
			col.gameObject.GetComponent<PlayerController>().TakeDamage(health);
			SingleDeath();
			
		}
		
		
	}

	private void SingleDeath(){
		dead = true;
		audioSource.clip = shipExplodeSFX;
		audioSource.volume = gc.pgd.sfxVol/50;
		audioSource.Play ();
		miniMapSprite.enabled = false;
		GetComponent<BoxCollider2D> ().enabled = false;
		anim.SetTrigger ("Death");
	}

	protected override void Death(){
		for (int i = 0; i < 3; i++) {
			GameObject tempSinglet = Instantiate(singlet, this.transform.position,transform.rotation) as GameObject;
			Vector3 tempRot = transform.rotation.eulerAngles;
			tempRot.z += (120 * i);
			tempSinglet.GetComponent<Singlet>().InitRot(tempRot);
		}
		foreach (var thruster in thrusters) {
			thruster.enabled = false;
		}
		
		gc.LiveEnemies.Remove (this);
		miniMapSprite.enabled = false;
		audioSource.clip = shipExplodeSFX;
		audioSource.volume = gc.pgd.sfxVol/50;
		audioSource.Play ();
		dead = true;
		GetComponent<BoxCollider2D> ().enabled = false;
		anim.SetTrigger ("Death");
	}


}
