using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Dreadnaught : EnemyBase
{


	public GameObject Trooper, Charger, Triplicate;
	public GameObject spawnLocation;
	private GameObject currentSpawned = null;
	private float spawnedUnitSpeed = 1;

	//area the ship circles around when close to the player
	public Vector3 centerLocation;
	//the size around the center point the ship flies around
	private float radius = 5;
	//checks if they are close enough to spawn an enemy
	private bool inRange;
	//the two turret guns attached to the ship
	public GameObject gun, gun2;
	public Animator gunAnim, gun2Anim;
	public GameObject gunPart, gunPart2;
	public GameObject chargeParticles;
	//the points on the barrel where the bullets fire from
	public Transform gunpoint, gunpoint2;
	//the rotate speed of the guns
	private float gunRotSpeed = 50;
	//the ships bullet
	public GameObject bullet;
	private bool isCharging = false;
	private bool stopRot = false;
	private float firingWeaponCountDown = 0;

	//The time the fireCD is set to after firing a gun
	private float fireCDDefaultTime = 3;

	public GameObject railGun;

	public AudioClip railgunChargeSFX;

	//sets reference to the Game Controller object
	public void Awake ()
	{
		gc = GameObject.Find ("GameController").GetComponent<GameController> ();
	}
	
	// Use this for initialization
	public override void Start ()
	{
		base.Start ();
		maxHealth = 2000;
		health = maxHealth;
		maxShield = 0;
		shield = maxShield;
		moveSpeed = 0.5f;
		//moveSpeed = 0f;
		turnSpeed = 1;
		fireRange = 10;
		damage = 250;
		suicideDamage = 100;
		fireCD = fireCDDefaultTime;
		InvokeRepeating ("SpawnUnit", 5, 10);
		FindCenterTarget ();
		expAmount = 200;
	}

	protected override void FixedUpdate ()
	{
		base.FixedUpdate ();
		if (currentSpawned != null) {
			currentSpawned.transform.position = (currentSpawned.transform.position + 
				(currentSpawned.transform.up * spawnedUnitSpeed * Time.deltaTime));
		}
		if (WithinFiringDistance () && !stopRot) {
			RotateTowardsTarget (gun, gc.pc.transform.position, false, gunRotSpeed);
			RotateTowardsTarget (gun2, gc.pc.transform.position, false, gunRotSpeed);
			UpdateGuns (gun, gunpoint);
			UpdateGuns (gun2, gunpoint2);
		}
		firingWeaponCountDown -= actualDeltaTime;
		if(isCharging){
			if(firingWeaponCountDown <= 0.5f){
				StopRotatingGuns();
			}
			if(firingWeaponCountDown <= 0){
				FireGuns();
			}
		}
	}

	protected override void EnemyMovement ()
	{
		float distance = Vector2.Distance (this.transform.position, centerLocation);
		if (distance <= radius) {
			inRange = true;
			RotateTowardsTarget (this.gameObject, centerLocation, true, turnSpeed);
			this.transform.position += (transform.up * actualDeltaTime * moveSpeed);
			
		} else {
			inRange = false;
			//move towards center location
			RotateTowardsTarget (this.gameObject, centerLocation, false, turnSpeed);
			this.transform.position = (this.transform.position + (this.transform.up * moveSpeed * actualDeltaTime));
			float playerDistance = Vector3.Distance (gc.pc.transform.position, centerLocation);
			if (playerDistance > radius) {
				FindCenterTarget ();
			}
		}
		
		
	}

	private void ActivateUnit ()
	{
		currentSpawned.GetComponent<EnemyBase> ().enabled = true;
		currentSpawned.GetComponent<BoxCollider2D>().enabled = true;
		currentSpawned = null;
	}

	protected void SpawnUnit ()
	{
			int unitChoice = Random.Range (0, 3);
			GameObject tempUnit;
			Vector3 tempRot = this.transform.rotation.eulerAngles;
			tempRot.z = tempRot.z - 180;
			Quaternion tempQuat = Quaternion.Euler (tempRot);
			if (unitChoice == 0) {
				tempUnit = Instantiate (Trooper, spawnLocation.transform.position, tempQuat) as GameObject;
			} else if (unitChoice == 1) {
				tempUnit = Instantiate (Charger, spawnLocation.transform.position, tempQuat) as GameObject;
			} else {
				tempUnit = Instantiate (Triplicate, spawnLocation.transform.position, tempQuat) as GameObject;
			}
		gc.LiveEnemies.Add(tempUnit.GetComponent<EnemyBase>());
			currentSpawned = tempUnit;
			Vector3 tempTransform = currentSpawned.transform.position;
			tempTransform.z++;
			currentSpawned.transform.position = tempTransform;
			tempUnit.GetComponent<EnemyBase> ().enabled = false;
			tempUnit.GetComponent<BoxCollider2D>().enabled = false;
			currentSpawned.transform.parent = this.transform.parent;
			Invoke ("ActivateUnit", 1.5f);

	}

	private void FindCenterTarget ()
	{
		if (gc.pc != null)
			centerLocation = gc.pc.transform.position;
		else if (centerLocation == null) {
			centerLocation = GameObject.FindGameObjectWithTag ("Player").transform.position;
		}
	}

	private void RotateTowardsTarget (GameObject rotatedObject, Vector3 target, bool rightAngle, float rotSpeed)
	{
		float angle = FindTargetAngle (rotatedObject, target, rightAngle);
		float enemyRot = rotatedObject.transform.rotation.eulerAngles.z;
		float tempRotSpeed;
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
			RotateRight (rotatedObject, rotSpeed);
		} else {
			RotateLeft (rotatedObject, rotSpeed);
		}
		if((enemyRot - angle) > -1 && (enemyRot - angle < 1)){
			Vector3 tempRot = rotatedObject.transform.rotation.eulerAngles;
			tempRot.z = angle;
			rotatedObject.transform.rotation = Quaternion.Euler (tempRot);
		}


	}

	private void RotateRight (GameObject rotatedObject, float rotSpeed)
	{
		Vector3 tempRot = rotatedObject.transform.rotation.eulerAngles;
		tempRot.z += rotSpeed * actualDeltaTime;
		rotatedObject.transform.rotation = Quaternion.Euler (tempRot);
	}

	private void RotateLeft (GameObject rotatedObject, float rotSpeed)
	{
		Vector3 tempRot = rotatedObject.transform.rotation.eulerAngles;
		tempRot.z -= rotSpeed * actualDeltaTime;
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

	private void UpdateGuns (GameObject gun, Transform gunPoint)
	{

		if (fireCD <= 0) {
			//grab player location
			Vector3 target = gc.pc.transform.position;
			//directional vector
			target.x = target.x - gun.transform.position.x;
			target.y = target.y - gun.transform.position.y;
			float tarAngle = Mathf.Atan2 (target.y, target.x) * Mathf.Rad2Deg - 90;


			if (tarAngle < 0) {
				tarAngle += 360;
			}
			if (Mathf.Abs (tarAngle - gun.transform.rotation.eulerAngles.z) <= 1f) {
				//spawn electricity on guns
				if(!audioSource.isPlaying){
					audioSource.clip = railgunChargeSFX;
					audioSource.volume = gc.pgd.sfxVol/100;
					audioSource.Play();
				}
				isCharging = true;
				//Invoke("StopRotatingGuns", 2.5f);
				firingWeaponCountDown = 3;
				gunAnim.SetTrigger("Charging");
				gun2Anim.SetTrigger("Charging");
				//Invoke("FireGuns", 3);
				fireCD = 8;
			}

		}	
	}

	public void CreateGunParticles(){
		if(gunPart == null){
			gunPart = Instantiate(chargeParticles, new Vector3(0, 1.017f, -7), Quaternion.Euler(Vector3.zero)) as GameObject;
			gunPart.transform.SetParent(gun.transform);
			gunPart.transform.localPosition = new Vector3(0, 1.017f, -7);
			gunPart.transform.localEulerAngles = Vector3.zero;
			gunPart2 = Instantiate(chargeParticles, new Vector3(0, 1.017f, -7), Quaternion.Euler(Vector3.zero)) as GameObject;
			gunPart2.transform.SetParent(gun2.transform);
			gunPart2.transform.localPosition = new Vector3(0, 1.017f, -7);
			gunPart2.transform.localEulerAngles = Vector3.zero;
		}
	}

	private void StopRotatingGuns(){
		stopRot = true;
	}

	private void FireGuns(){
		if(!dead){
			stopRot = false;
			isCharging = false;
			fireCD = fireCDDefaultTime;
			Destroy(gunPart);
			Destroy(gunPart2);
			gunPart = null;
			gunPart2 = null;
			gunAnim.SetTrigger("Finish");
			gun2Anim.SetTrigger("Finish");
			CreateRailGunHitRay(gunpoint, gun);
			CreateRailGunHitRay(gunpoint2, gun2);
		}


	}


	private void CreateRailGunHitRay(Transform gunPoint, GameObject gun){
		List<GameObject> HitEnemies = new List<GameObject>();
		Vector3 startPos = gunPoint.position;
		startPos = (startPos - (gunPoint.right * 0.1f));
		for(int i = 0; i < 3; i++){
			RaycastHit2D[] ray = Physics2D.RaycastAll (startPos, gun.transform.up, 100);
			Debug.DrawRay(startPos, gun.transform.up * 100, Color.red, 10);
			for (int x = 0; x < ray.Length; x++) {
				if (ray [x].rigidbody.gameObject.tag == "Player" && !HitEnemies.Contains(ray [x].rigidbody.gameObject)) {
					HitEnemies.Add(ray [x].rigidbody.gameObject);
					ray [x].rigidbody.gameObject.GetComponent<PlayerController> ().TakeDamage (damage);
				}
			}
			startPos = (startPos + (gunPoint.transform.right * 0.1f));
		}
		
		
		Instantiate(railGun, gunPoint.position, gun.transform.rotation);
	}

	private bool WithinFiringDistance ()
	{
		float distance = Vector3.Distance (this.transform.position, gc.pc.transform.position);
		if (fireRange >= distance) {
			return true;
		}
		return false;
	}

	public virtual void OnTriggerEnter2D(Collider2D col){
		
		if (col.gameObject.tag == "Player") {
			
			col.gameObject.GetComponent<PlayerController>().TakeDamage(suicideDamage);
			gc.pc.GetComponent<Rigidbody2D>().velocity = (-gc.pc.GetComponent<Rigidbody2D>().velocity) * 50;
			
		}
		
		
	}

	protected override void Death ()
	{
		base.Death ();
		if(gunPart != null)
			Destroy(gunPart);
		if(gunPart != null)
			Destroy(gunPart2);
		if(currentSpawned != null)
			ActivateUnit();
		CancelInvoke();
		gunAnim.SetTrigger("Death");
		gun2Anim.SetTrigger("Death");
	}


}
