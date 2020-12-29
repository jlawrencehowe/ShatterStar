using UnityEngine;
using System.Collections;

public class Dragger : EnemyBase {

	private float closeDistance = 1.25f;
	public bool isSlowing = false;
	private static int amountSlowers;
	public Vector3 playerFrozenPos;
	public Transform centerSpot;
	private float lerpTimer;
	public GameObject tractorBeam;
	
	// Use this for initialization
	public override void Start () {
		base.Start ();
		moveSpeed = 7f;
		turnSpeed = 150;
		maxHealth = 300;
		health = maxHealth;
		maxShield = 0;
		shield = maxShield;
		suicideDamage = 50;
		expAmount = 75;
	}

	protected override void EnemyMovement ()
	{

		float angle = FindTargetAngle();


		if (Mathf.Abs (Vector2.Distance (transform.position, gc.pc.transform.position)) < closeDistance && !gc.pc.isSlowed) {
			if(!isSlowing){
				isSlowing = true;
				amountSlowers++;
				gc.pc.isSlowed = true;
				gc.pc.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
				moveSpeed = 0.25f;
				playerFrozenPos = gc.pc.transform.position;
			}

		} 

		if (isSlowing) {
			thrusters[0].enabled = false;
			thrusters[1].enabled = true;
			thrusters[2].enabled = true;
			lerpTimer += Time.deltaTime;
			gc.pc.transform.position = Vector3.Lerp(playerFrozenPos, centerSpot.position, lerpTimer);
			tractorBeam.SetActive(true);

		}

		if (!isSlowing) {

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
				RotateRight ();
			} else if(Mathf.Abs(angle - enemyRot) > 5){
				RotateLeft ();
			}
		}
		
		MoveForward();

	}

	private float FindTargetAngle ()
	{
		
		Vector3 target = gc.pc.transform.position;
		target.z = 0f;
		
		Vector3 objectPos = transform.position;
		target.x = target.x - objectPos.x;
		target.y = target.y - objectPos.y;
		
		float angle = Mathf.Atan2 (target.y, target.x) * Mathf.Rad2Deg - 90;
		if (angle < 0) {
			angle += 360;
		}
		return angle;
		
	}

	private void MoveForward(){
		
		Vector3 tempPos = transform.position;
		Vector3 tempUp = transform.up;
		if (isSlowing) {
			tempUp = -tempUp;
		}
		tempPos = tempPos + (tempUp * moveSpeed * actualDeltaTime);
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

	public override void OnTriggerEnter2D(Collider2D col){
		
		if (col.gameObject.tag == "Player" && !isSlowing) {
			
			col.gameObject.GetComponent<PlayerController> ().TakeDamage (health);

			Death ();
			
		}
	}

	protected override void Death(){
		dead = true;
		miniMapSprite.enabled = false;
		GetComponent<BoxCollider2D> ().enabled = false;
		anim.SetTrigger ("Death");
		
		audioSource.clip = shipExplodeSFX;
		audioSource.volume = gc.pgd.sfxVol/50;
		audioSource.Play ();
		foreach (var thruster in thrusters) {
			thruster.enabled = false;
		}
		tractorBeam.SetActive(false);
		if (isSlowing) {
			amountSlowers--;
		}
		if(amountSlowers == 0){
			gc.pc.isSlowed = false;
		}
	}

}
