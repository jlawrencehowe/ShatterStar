using UnityEngine;
using System.Collections;

public class Shield : EnemyBase {

	public GameObject closestTarget;
	public Vector3 targetLocation;
	private const float SLOWDOWNDISTANCE = 2f;
	private const float DRIFTDISTANCE = 0.05f;
	private float acceleration = 1;
	private float deacceleration = 1;
	private float maxSpeed = 5.5f;
	private float slowSpeed = 1;
	private Vector3 driftDirection;
	private float shieldChargeAmount = 25;
	private float shieldPulseTimer;
	private enum MoveState
	{
		SpeedUp,
		SlowDown,
		Drift
	};
	private MoveState moveState = MoveState.SpeedUp;

	public ShieldGenerator shieldG;

	
	// Use this for initialization
	public override void Start () {
		base.Start ();
		maxHealth = 50;
		health = 50;
		maxShield = 0;
		moveSpeed = 5.5f;
		turnSpeed = 100;
		CalculateClosestEnemy ();
		suicideDamage = 50;
		expAmount = 75;
	}

	protected override void FixedUpdate(){
		base.FixedUpdate();
		shieldPulseTimer -= actualDeltaTime;
		if(shieldPulseTimer <= 0){
			ShieldPulse();
			shieldPulseTimer = 3;
		}

	}

	protected override void EnemyMovement ()
	{
		if (closestTarget != null) {
			Debug.Log("Move");
			Move();
		} 
		else {
			if(gc.LiveEnemies.Count - gc.LiveShields.Count == 0){
				Debug.Log("ChargePlayer");
				ChargePlayer();
			}
			else{
				Debug.Log("Calculating");
				CalculateClosestEnemy();
			}
		}


	}

	private void ShieldPulse(){

		foreach (var enemy in shieldG.Enemies) {
			if(enemy.name != this.name)
				enemy.GetComponent<EnemyBase>().ShieldPulse(shieldChargeAmount);
		}

	}


	private void CalculateClosestEnemy(){

		float minDistance = 10000f;
		float secondaryDistance = 10000f;
		closestTarget = null;
		GameObject secondaryTarget = null;
		foreach (var enemy in gc.LiveEnemies) {
			float curDistance = Mathf.Abs(Vector2.Distance(transform.position, enemy.transform.position));
			if(enemy.name == "Launcher" || enemy.name == "Safety" || enemy.name == "Bulk" 
			   || enemy.name == "Dreadnaught" || enemy.name == "Artilery"){
				if(curDistance < minDistance && enemy.gameObject.name != this.gameObject.name){
					closestTarget = enemy.gameObject;
					minDistance = curDistance;
				}
			}
			else{
				if(curDistance < secondaryDistance && enemy.gameObject.name != this.gameObject.name){
					secondaryTarget = enemy.gameObject;
					secondaryDistance = curDistance;
				}
			}
			if(closestTarget == null && secondaryTarget != null){
				closestTarget = secondaryTarget;
			}
			

		}



	}

	protected void Move(){

		float distance = Mathf.Abs(Vector2.Distance(targetLocation, this.transform.position));

		if (distance > SLOWDOWNDISTANCE && moveState != MoveState.Drift) {
			Debug.Log("SpeedUP");
			MoveTowardsAlliedTargetSpeedUp ();
			moveState = MoveState.SpeedUp;
		} else if (distance <= SLOWDOWNDISTANCE && distance > DRIFTDISTANCE && moveState != MoveState.Drift) {
			Debug.Log("Slowdown");
			MoveTowardsAlliedTargetSlowDown ();
			moveState = MoveState.SlowDown;
			driftDirection = targetLocation - transform.position;
			CalculateNewLocation();
		} else {
			Debug.Log("Start drift");
			Drift();
			moveState = MoveState.Drift;
		}
		if (moveState == MoveState.Drift && distance > SLOWDOWNDISTANCE) {
			
			Debug.Log("new location");
			CalculateNewLocation();
		}
		if (moveState != MoveState.Drift) {
			
			Debug.Log("not drift move");
			this.transform.position += (moveSpeed * transform.up * actualDeltaTime);
		} else {
			Debug.Log("drift move");
			this.transform.position += (moveSpeed * driftDirection * actualDeltaTime);
		}

	}

	private void MoveTowardsAlliedTargetSpeedUp(){
		RotateTowardsTarget (CalculateDirectionalVectorTowardsTarget());
		moveSpeed += acceleration * actualDeltaTime;
		if (moveSpeed >= maxSpeed) {
			moveSpeed = maxSpeed;
		}
	}

	private void MoveTowardsAlliedTargetSlowDown(){
		RotateTowardsTarget (CalculateDirectionalVectorTowardsTarget());
		if (moveSpeed > slowSpeed - 0.5 && moveSpeed < slowSpeed + 0.5) {
			moveSpeed = slowSpeed;
		}
		if (moveSpeed >= slowSpeed + 0.5) {
			moveSpeed -= deacceleration;
		} else if (moveSpeed <= slowSpeed - 0.5) {
			moveSpeed += deacceleration;
		}

	}

	private void Drift(){
		RotateTowardsTarget (CalculateDirectionalVectorTowardsTarget());
		moveSpeed = slowSpeed;
	}

	//Random circular target around target ally
	private void CalculateNewLocation(){

		targetLocation = (Vector2)closestTarget.transform.position + Random.insideUnitCircle * DRIFTDISTANCE;

	}

	private void ChargePlayer(){

		RotateTowardsTarget (CalculateDirectionalVectorTowardsPlayer());
		Vector3 tempPos = transform.position;
		if (timeSlowed) {
			tempPos = tempPos + (transform.up * moveSpeed * (actualDeltaTime/4));
		} else {
			tempPos = tempPos + (transform.up * moveSpeed * actualDeltaTime);
		}
		transform.position = tempPos;
	}

	private Vector3 CalculateDirectionalVectorTowardsPlayer(){
	
		Vector3 target = gc.pc.transform.position;
		target.z = 0f;
		
		Vector3 objectPos = transform.position;
		target.x = target.x - objectPos.x;
		target.y = target.y - objectPos.y;
		return target;
	}

	private Vector3 CalculateDirectionalVectorTowardsTarget(){
		
		Vector3 target = targetLocation;
		target.z = 0f;
		
		Vector3 objectPos = transform.position;
		target.x = target.x - objectPos.x;
		target.y = target.y - objectPos.y;
		return target;
	}



	private void RotateTowardsTarget(Vector3 target){

		float angle = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg - 90;
		if (angle < 0) {
			angle += 360;
		}
		float enemyRot = transform.rotation.eulerAngles.z;
		float tempRotSpeed;
		if(timeSlowed){
			tempRotSpeed = turnSpeed/4; 
		}
		else{
			tempRotSpeed = turnSpeed;
		}
		if (enemyRot < angle) {
			float posTest = angle - enemyRot;
			float minTest = enemyRot + (360 - angle);
			if (posTest < minTest) {
				Vector3 tempRot = transform.rotation.eulerAngles;
				tempRot.z += tempRotSpeed * actualDeltaTime;
				transform.rotation = Quaternion.Euler(tempRot);
			} else {
				Vector3 tempRot = transform.rotation.eulerAngles;
				tempRot.z -= tempRotSpeed * actualDeltaTime;
				transform.rotation = Quaternion.Euler(tempRot);
			}
			
		} else {
			float minTest = enemyRot - angle;
			float posTest = angle + (360 - enemyRot);
			if (posTest < minTest) {
				Vector3 tempRot = transform.rotation.eulerAngles;
				tempRot.z += tempRotSpeed * actualDeltaTime;
				transform.rotation = Quaternion.Euler(tempRot);
			} else {
				Vector3 tempRot = transform.rotation.eulerAngles;
				tempRot.z -= tempRotSpeed * actualDeltaTime;
				transform.rotation = Quaternion.Euler(tempRot);
			}
		}
	}

	protected override void Death(){
		dead = true;
		miniMapSprite.enabled = false;
		gc.LiveEnemies.Remove (this);
		gc.LiveShields.Remove (this);
		
		audioSource.clip = shipExplodeSFX;
		audioSource.volume = gc.pgd.sfxVol/50;
		audioSource.Play ();
		GetComponent<BoxCollider2D> ().enabled = false;
		shieldG.Death ();
		anim.SetTrigger ("Death");
	}

}
