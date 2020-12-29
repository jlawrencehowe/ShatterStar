using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bullet : MonoBehaviour {

	public BulletExplosion bulletExplosion;

	private float speed;
	private float damage;

	private bool pierce;
	private int pierceAmount = 1;
	public bool explosive;
	private bool homing;
	private int hitCounter = 0;
	private float bulletDecay;
	private float turnSpeed = 300f;
	public List<GameObject> hitEnemies = new List<GameObject>();
	public GameObject targetedEnemy = null;
	public GameObject hitParticle;
	private SpriteRenderer sr;
	public HomingCircle homingCircle;
	protected AudioSource audio;
	public AudioClip blasterSFX, blasterExplosionSFX, blasterHitSFX;
	protected GameController gc;

	// Use this for initialization
	void Start () {
		sr = GetComponent<SpriteRenderer>();
		gc = GameObject.Find ("GameController").GetComponent<GameController> ();
		audio = GetComponent<AudioSource>();
		audio.volume = gc.pgd.sfxVol/50;
		audio.Play();
	}
	
	// Update is called once per frame
	void Update () {
	

		if(targetedEnemy != null && homing){
			HomingMovement();
		}
		else{
			transform.position = transform.position + (speed * transform.up * Time.deltaTime);
		}
		if(homingCircle.Enemies.Count != 0) {
			targetedEnemy = FindNewTarget ();
		}

		bulletDecay -= Time.deltaTime;
		if (bulletDecay <= 0) {
			if(explosive){
				Explode(null);
			}
			Destroy(gameObject);
		}


	}

	public void InitStats(float speed, float damage, Quaternion rot, float bulletdecay, int pierceAmount, bool isExplosive, bool isHoming){
		SetSpeed (speed);
		SetDamage (damage);
		SetRotation (rot);
		SetBulletDecay (bulletdecay);
		InvokeRepeating ("FadeOut", bulletdecay - 0.3f, 0.1f);
		SetExplosive (isExplosive);
		SetPierceAmount (pierceAmount);
		homing = isHoming;
	}

	public void SetSpeed(float speed){
		this.speed = speed;
	}

	public void SetDamage(float damage){
		this.damage = damage;
	}

	public void SetRotation(Quaternion rot){
		transform.rotation = rot;
	}

	public void SetPierce(bool pierce){
		this.pierce = pierce;
	}

	public void SetPierceAmount(int amount){
		pierceAmount = amount;
	}

	public void SetExplosive(bool explosive){
		this.explosive = explosive;
	}

	public void SetBulletDecay(float bulletDecay){
		this.bulletDecay = bulletDecay;
	}

	public void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "Enemy") {
			hitCounter++;
			if(explosive){
				GameObject tempEnemy = col.gameObject;
				Explode(tempEnemy);
			}
			else{
				audio.clip = blasterHitSFX;
				audio.volume = gc.pgd.sfxVol/50;
				audio.Play();
			}
			col.GetComponent<EnemyBase>().TakeDamage(damage);
			hitEnemies.Add(col.gameObject);
			targetedEnemy = FindNewTarget ();
			if(hitCounter >= pierceAmount){
				homing = false;
				Vector3 tempRot;
				tempRot.x = -this.transform.rotation.eulerAngles.z - 90;
				tempRot.y = 90;
				tempRot.z = 0;
				//Vector3 tempSpawn = this.transform.position + (this.transform.up * 0.3f);
				var tempPart = Instantiate(hitParticle, this.transform.position, Quaternion.Euler(Vector3.zero)) as GameObject;
				tempPart.transform.eulerAngles = tempRot;
				tempPart.GetComponent<ParticleSystem>().startRotation = Mathf.Deg2Rad * (tempRot.x + 90);

				gameObject.GetComponent<BoxCollider2D>().enabled = false;
				gameObject.GetComponent<SpriteRenderer>().enabled = false;
				Destroy(gameObject, 3f);
			}
		}

	}

	private void FadeOut(){

		var tempColor = sr.color;
		tempColor.a -= 0.3f;
		sr.color = tempColor;

	}

	private void Explode(GameObject enemy){
		BulletExplosion tempExp = Instantiate (bulletExplosion, this.transform.position, this.transform.rotation) as BulletExplosion;
		tempExp.InitStats (enemy, damage);
		audio.clip = blasterExplosionSFX;
		audio.volume = gc.pgd.sfxVol/50;
		audio.Play();

	}

	private void HomingMovement ()
	{

		if (targetedEnemy == null) {
			targetedEnemy = FindNewTarget ();
		}
		if (targetedEnemy == null) {
			return;
		}
		Vector3 target = targetedEnemy.transform.position;
		target.z = 0f;
		
		Vector3 objectPos = transform.position;
		target.x = target.x - objectPos.x;
		target.y = target.y - objectPos.y;
		
		float angle = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg - 90;
		if (angle < 0) {
			angle += 360;
		}
		float enemyRot = transform.rotation.eulerAngles.z;
		if (enemyRot < angle) {
			float posTest = angle - enemyRot;
			float minTest = enemyRot + (360 - angle);
			if (posTest < minTest) {
				Vector3 tempRot = transform.rotation.eulerAngles;
				tempRot.z += turnSpeed * Time.deltaTime;
				transform.rotation = Quaternion.Euler(tempRot);
			} else {
				Vector3 tempRot = transform.rotation.eulerAngles;
				tempRot.z -= turnSpeed * Time.deltaTime;
				transform.rotation = Quaternion.Euler(tempRot);
			}
			
		} else {
			float minTest = enemyRot - angle;
			float posTest = angle + (360 - enemyRot);
			if (posTest < minTest) {
				Vector3 tempRot = transform.rotation.eulerAngles;
				tempRot.z += turnSpeed * Time.deltaTime;
				transform.rotation = Quaternion.Euler(tempRot);
			} else {
				Vector3 tempRot = transform.rotation.eulerAngles;
				tempRot.z -= turnSpeed * Time.deltaTime;
				transform.rotation = Quaternion.Euler(tempRot);
			}
		}
		
		Vector3 tempPos = transform.position;
		tempPos = tempPos + (transform.up * speed * Time.deltaTime);
		transform.position = tempPos;

		
		
	}

	private GameObject FindNewTarget(){
		/*
		GameObject closestTarget = targetedEnemy;
		for (int x = 0; x < hitEnemies.Count; x++) {
			if(closestTarget == hitEnemies[x]){
				closestTarget = null;
			}
		}
		for (int i = 0; i < homingCircle.Enemies.Count; i++) {
			bool skip = false;
			for (int x = 0; x < hitEnemies.Count; x++) {
				if(homingCircle.Enemies[i] == hitEnemies[x]){
					skip = true;
					break;
				}
			}
			if(skip){
				continue;
			}
			if(closestTarget != null && Vector2.Distance(this.transform.position, homingCircle.Enemies[i].transform.position) < Vector2.Distance(this.transform.position, closestTarget.transform.position)){
				closestTarget = homingCircle.Enemies[i];
			}
		}
		Debug.Log(closestTarget);
		return closestTarget;	
*/
		GameObject closestTarget = null;
		for(int i = 0; i < homingCircle.Enemies.Count; i++){
			bool skip = false;
			for(int x = 0; x < hitEnemies.Count; x++){
				if(homingCircle.Enemies[i] == hitEnemies[x]){
					skip = true;
					break;
				}
			}
			if(closestTarget == null && !skip){
				closestTarget = homingCircle.Enemies[i];
				break;
			}
			if(!skip && closestTarget != null && 
			   Vector2.Distance(this.transform.position, homingCircle.Enemies[i].transform.position) < 
			   Vector2.Distance(this.transform.position, closestTarget.transform.position)){
				closestTarget = homingCircle.Enemies[i];
			}

		}
		return closestTarget;
	}
}
