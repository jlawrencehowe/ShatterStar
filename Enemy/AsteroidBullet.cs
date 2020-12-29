using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AsteroidBullet : EnemyBullet {

	public Cluster parentCluster;
	private float rotationSpeed;
	public bool isBullet = false;
	private float height, width;
	protected float orbitDistance = 0.5f;
	private float orderNumber;
	public SpriteRenderer bulletSprite;


	// Use this for initialization
	void Start () {
		audio = GetComponent<AudioSource>();
		rotationSpeed = 100;
		gc = GameObject.Find ("GameController").GetComponent<GameController> ();
		height = 2*gc.miniMapCam.orthographicSize + 5;
		width = height*gc.miniMapCam.aspect + 5;

	}
	
	// Update is called once per frame
	void Update () {


		if (isBullet) {
			if (timeSlow) {
				transform.position += transform.up * speed * Time.deltaTime/4;
			}
			else {
				transform.position += transform.up * speed * Time.deltaTime;
			}
			OffScreenDeath ();
		} else {
			Vector3 tempRot = transform.rotation.eulerAngles;
			if (timeSlow) {
				tempRot.z += rotationSpeed * Time.deltaTime/4;
			}
			else {
				tempRot.z += rotationSpeed * Time.deltaTime;
			}
			transform.rotation = Quaternion.Euler(tempRot);
			Vector3 orbitRot = Quaternion.AngleAxis(100 * Time.time + (120 * orderNumber), Vector3.forward) * new Vector3(orbitDistance,0,0);
			if(parentCluster != null)
				transform.position = parentCluster.transform.position + orbitRot;

		}


	
	}

	public void FireBullet(float angle){
		isBullet = true;
/*
		target.z = 0f;
		
		Vector3 objectPos = transform.position;
		target.x = target.x - objectPos.x;
		target.y = target.y - objectPos.y;
		
		float angle = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg - 90;
		Vector3 tempRot = transform.rotation.eulerAngles;
		tempRot.z = angle;
		*/
		Vector3 tempRot = transform.rotation.eulerAngles;
		tempRot.z = angle;
		transform.rotation = Quaternion.Euler (tempRot);

	}

	public void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "Player") {
			col.GetComponent<PlayerController>().TakeDamage(damage);
			if(parentCluster != null)
				parentCluster.Bullets.Remove(this.GetComponent<AsteroidBullet>());
			Vector3 tempRot;
			tempRot.x = -this.transform.rotation.eulerAngles.z - 90;
			tempRot.y = 90;
			tempRot.z = 0;
			var tempPart = Instantiate(hitParticle, this.transform.position, Quaternion.Euler(Vector3.zero)) as GameObject;
			tempPart.transform.eulerAngles = tempRot;
			tempPart.GetComponent<ParticleSystem>().startRotation = Mathf.Deg2Rad * (tempRot.x + 90);
			audio.volume = gc.pgd.sfxVol/50;
			if(timeSlow == true){
				audio.pitch = 0.25f;
			}
			audio.Play();
			gameObject.GetComponent<BoxCollider2D>().enabled = false;
			bulletSprite.enabled = false;
			Destroy(gameObject, 3f);
			
		}
		if (col.gameObject.tag == "PlayerBullet") {
			Destroy(col.gameObject);
			if(parentCluster != null)
				parentCluster.Bullets.Remove(this.GetComponent<AsteroidBullet>());
			Vector3 tempRot;
			tempRot.x = -this.transform.rotation.eulerAngles.z - 90;
			tempRot.y = 90;
			tempRot.z = 0;
			var tempPart = Instantiate(hitParticle, this.transform.position, Quaternion.Euler(Vector3.zero)) as GameObject;
			tempPart.transform.eulerAngles = tempRot;
			tempPart.GetComponent<ParticleSystem>().startRotation = Mathf.Deg2Rad * (tempRot.x + 90);
			audio.volume = gc.pgd.sfxVol/50;
			if(timeSlow == true){
				audio.pitch = 0.25f;
			}
			audio.Play();
			gameObject.GetComponent<BoxCollider2D>().enabled = false;
			bulletSprite.enabled = false;
			Destroy(gameObject, 3f);
			
		}
	}

	public void InitStats(float speed, float damage, Cluster parentCluster, int orderNumber){
		this.orderNumber = orderNumber;
		this.parentCluster = parentCluster;
		SetSpeed (speed);
		SetDamage (damage);
		Vector3 orbitRot = Quaternion.AngleAxis(100 * Time.time + (120 * orderNumber), Vector3.forward) * new Vector3(orbitDistance,0,0);
		transform.position = parentCluster.transform.position + orbitRot;

	}

	private void OffScreenDeath(){

		float xDistance = Mathf.Abs(transform.position.x - gc.pc.transform.position.x);
		float yDistance = Mathf.Abs(transform.position.y - gc.pc.transform.position.y);

		if (xDistance > width) {
			Destroy(gameObject);
		}
		if (yDistance > height) {
			Destroy(gameObject);
		}



	}
}
