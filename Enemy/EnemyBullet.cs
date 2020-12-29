using UnityEngine;
using System.Collections;

public class EnemyBullet : MonoBehaviour {

	public BulletExplosion bulletExplosion;
	
	protected float speed;
	protected float damage;
	
	protected bool pierce;
	private int pierceAmount = 1;
	protected bool explosive;
	protected bool homing;
	private int hitCounter = 0;
	protected float bulletDecay;
	
	public GameObject hitParticle;
	public static bool timeSlow;
	protected SpriteRenderer sr;
	protected AudioSource audio;
	public AudioClip blasterSFX, blasterHitSFX;
	protected GameController gc;
	
	// Use this for initialization
	protected virtual void Start () {
		sr = GetComponent<SpriteRenderer>();
		gc = GameObject.Find ("GameController").GetComponent<GameController> ();
		audio = GetComponent<AudioSource>();
		if(timeSlow == true){
			audio.pitch = 0.25f;
		}
		audio.volume = gc.pgd.sfxVol/50;
		audio.Play();
		
	}
	
	// Update is called once per frame
	protected virtual void Update () {

		if (timeSlow) {
			audio.pitch = 0.25f;
			transform.position = transform.position + (speed * transform.up * Time.deltaTime/4);
		} else {
			audio.pitch = 1;
			transform.position = transform.position + (speed * transform.up * Time.deltaTime);
		}
		
		
	}
	
	public void InitStats(float speed, float damage, Quaternion rot, float bulletdecay, int pierceAmount, bool isExplosive, bool isHoming){
		SetSpeed (speed);
		SetDamage (damage);
		SetRotation (rot);
		SetBulletDecay (bulletdecay);
		SetExplosive (explosive);
		InvokeRepeating ("FadeOut", bulletdecay - 0.3f, 0.1f);
		SetPierceAmount (pierceAmount);
		homing = isHoming;
		Destroy (gameObject, bulletDecay);
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

	private void FadeOut(){
		
		var tempColor = sr.color;
		tempColor.a -= 0.3f;
		sr.color = tempColor;
		if(tempColor.a <= 0.2f){
			GetComponent<BoxCollider2D>().enabled = false;
		}
		
	}
	
	public virtual void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "Player") {
			hitCounter++;

			if(explosive){
				GameObject tempEnemy = col.gameObject;
				Explode(tempEnemy);
			}
			col.GetComponent<PlayerController>().TakeDamage(damage);
			if(hitCounter >= pierceAmount){
				Vector3 tempRot;
				tempRot.x = -this.transform.rotation.eulerAngles.z - 90;
				tempRot.y = 90;
				tempRot.z = 0;
				var tempPart = Instantiate(hitParticle, this.transform.position, Quaternion.Euler(Vector3.zero)) as GameObject;
				tempPart.transform.eulerAngles = tempRot;
				tempPart.GetComponent<ParticleSystem>().startRotation = Mathf.Deg2Rad * (tempRot.x + 90);
				audio.clip = blasterHitSFX;
				audio.volume = gc.pgd.sfxVol/50;
				audio.Play();
				gameObject.GetComponent<BoxCollider2D>().enabled = false;
				gameObject.GetComponent<SpriteRenderer>().enabled = false;
				Destroy(gameObject, 3f);
			}
		}
		
	}
	
	private void Explode(GameObject enemy){
		BulletExplosion tempExp = Instantiate (bulletExplosion, this.transform.position, this.transform.rotation) as BulletExplosion;
		tempExp.InitStats (enemy, damage);
		
	}
}
