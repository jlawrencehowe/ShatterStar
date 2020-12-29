using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cluster : EnemyBase {

	public float bulletCharges;
	public List<AsteroidBullet> Bullets;
	public GameObject asteroidShot;
	private bool isRunning = false;
	
	// Use this for initialization
	public override void Start () {
		base.Start ();
		damage = 75;
		maxHealth = 175;
		health = 175;
		maxShield = 0;
		bulletCharges = 3;
		moveSpeed = 4f;
		turnSpeed = 100;
		fireRange = 5;
		fireCD = 2;
		suicideDamage = 50;
		expAmount = 75;

		for (int i = 0; i < 3; i++) {
			GameObject tempBullet = Instantiate(asteroidShot, this.transform.position, this.transform.rotation) as GameObject;
			Bullets.Add(tempBullet.GetComponent<AsteroidBullet>());
			tempBullet.GetComponent<AsteroidBullet>().InitStats(4f, damage, this, Bullets.Count);
		}
	}

	protected void EnemyShoot(){
		
		float distance = Vector2.Distance (gc.pc.transform.position, transform.position);
		//float aprox = Mathf.Abs (tarAngle - curAngle);
		if(bulletCharges > 0 && distance <= fireRange && Bullets.Count != 0){
			foreach(AsteroidBullet bullet in Bullets){
				Vector3 target = gc.pc.transform.position;
				target.z = 0f;
				
				Vector3 objectPos = transform.position;
				target.x = target.x - objectPos.x;
				target.y = target.y - objectPos.y;

				
				float angle = Mathf.Atan2 (target.y, target.x) * Mathf.Rad2Deg - 90;
				bullet.FireBullet(angle);
				bullet.isBullet = true;
			}
			bulletCharges = 0;
			Bullets.Clear();
		}
		
	}

	protected override void FixedUpdate(){
		base.FixedUpdate ();
		if (bulletCharges == 3) {
			fireCD = 0.5f;
		}
		if (fireCD <= 0 && bulletCharges < 3) {
			fireCD = 2;
			bulletCharges++;
			GameObject tempBullet = Instantiate(asteroidShot, this.transform.position, this.transform.rotation) as GameObject;
			Bullets.Add(tempBullet.GetComponent<AsteroidBullet>());
			Vector3 tempRot = this.transform.rotation.eulerAngles;
			if(Bullets.Count != 1){
				tempRot = new Vector3(transform.rotation.x, transform.rotation.y, 
				                                                  Bullets[Bullets.Count - 1].transform.rotation.z + 120);
			}
			tempBullet.transform.rotation = Quaternion.Euler(tempRot);
			tempBullet.GetComponent<AsteroidBullet>().InitStats(8f, damage, this, Bullets.Count);
		}

	}
	
	
	protected override void EnemyMovement ()
	{

			EnemyShoot ();
		
			Vector3 target = gc.pc.transform.position;
			target.z = 0f;
		
			Vector3 objectPos = transform.position;
			target.x = target.x - objectPos.x;
			target.y = target.y - objectPos.y;

			if (bulletCharges == 0) {
			isRunning = true;
		} else if (bulletCharges == 3) {
			isRunning = false;
		}
		if (isRunning) {
			target = -target;
		} 

			
		
			float angle = Mathf.Atan2 (target.y, target.x) * Mathf.Rad2Deg - 90;
			if (angle < 0) {
				angle += 360;
			}
			float enemyRot = transform.rotation.eulerAngles.z;
			float tempRotSpeed;
			if (timeSlowed) {
				tempRotSpeed = turnSpeed / 4; 
			} else {
				tempRotSpeed = turnSpeed;
			}
			if (enemyRot < angle) {
				float posTest = angle - enemyRot;
				float minTest = enemyRot + (360 - angle);
				if (posTest < minTest) {
					Vector3 tempRot = transform.rotation.eulerAngles;
					tempRot.z += tempRotSpeed * Time.deltaTime;
					transform.rotation = Quaternion.Euler (tempRot);
				} else {
					Vector3 tempRot = transform.rotation.eulerAngles;
					tempRot.z -= tempRotSpeed * Time.deltaTime;
					transform.rotation = Quaternion.Euler (tempRot);
				}
			
			} else {
				float minTest = enemyRot - angle;
				float posTest = angle + (360 - enemyRot);
				if (posTest < minTest) {
					Vector3 tempRot = transform.rotation.eulerAngles;
					tempRot.z += tempRotSpeed * Time.deltaTime;
					transform.rotation = Quaternion.Euler (tempRot);
				} else {
					Vector3 tempRot = transform.rotation.eulerAngles;
					tempRot.z -= tempRotSpeed * Time.deltaTime;
					transform.rotation = Quaternion.Euler (tempRot);
				}
			}
		
			Vector3 tempPos = transform.position;
			if (timeSlowed) {
				tempPos = tempPos + (transform.up * moveSpeed * (Time.deltaTime / 4));
			} else {
				tempPos = tempPos + (transform.up * moveSpeed * Time.deltaTime);
			}
			transform.position = tempPos;

	}


	protected override void Death ()
	{
		while(Bullets.Count != 0){
			var bullet = Bullets[0];
			Bullets.Remove(bullet);
			Destroy(bullet.gameObject);
		}
		base.Death ();
	}






}
