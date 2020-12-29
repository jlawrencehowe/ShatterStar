using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyBase : MonoBehaviour {

	public float health = 100;
	protected float maxHealth;
	protected float shield;
	protected float maxShield;
	protected float shieldTimer = 3f;
	protected float shieldDecay;
	public float extraShield;
	public int shieldChargers;

	protected GameController gc;

	protected float turnSpeed;
	protected float moveSpeed;
	protected float damage;
	protected float suicideDamage;
	protected bool dead = false;
	protected Animator anim;
	protected float fireCD;
	protected float fireRange;
	public SpriteRenderer sr;
	public  List<SpriteRenderer> thrusters;

	public static bool timeSlowed;
	protected float actualDeltaTime;

	public Transform healthBar, shieldBar;
	public SpriteRenderer shieldSprite;
	public SpriteRenderer miniMapSprite;
	protected float expAmount = 50;
	protected AudioSource audioSource;
	public AudioClip shipExplodeSFX;

	// Use this for initialization
	public virtual void Start() {
		audioSource = GetComponent<AudioSource>();
		gc = GameObject.Find ("GameController").GetComponent<GameController> ();
		anim = GetComponent<Animator> ();
		UpdateShield();
	}
	


	protected virtual void FixedUpdate(){
		actualDeltaTime = Time.deltaTime;
		var tempComponent = GetComponent<AudioSource>();
		if(timeSlowed){
			actualDeltaTime /= 4;
			if(tempComponent != null){
				tempComponent.pitch = 0.25f;
			}
		}
		else{
			if(tempComponent != null){
				tempComponent.pitch = 1f;
			}
		}
		
		if (!dead) {
			fireCD -= actualDeltaTime;
			shieldTimer -= actualDeltaTime;
			shieldDecay -= actualDeltaTime;
			
			if(shieldTimer <= 0){
				ShieldRecharge();
			}
			if(shieldDecay <= 0){
				ShieldDecay();
			}
			EnemyMovement ();
			UpdateShield();
		}

		if(gc.pc.isDead){
			GetComponent<EnemyBase>().enabled = false;
		}
	}

	protected virtual void EnemyMovement(){

	}

	private void UpdateShield(){

        if(sr != null)
		    sr.color = new Color (1, 1, 1, (shield + extraShield) / (maxShield + (shieldChargers * 100)));

	}

	public void TakeDamage(float damage){
		//Debug.Log (damage);
		//Debug.Log ("Health: " + health);
		shieldTimer = 3;
		extraShield -= damage;
		if (extraShield < 0) {
			shield += extraShield;
			extraShield = 0;
		}
		if (shield < 0) {
			health += shield;
			shield = 0;
		}
		if (health <= 0) {
			gc.pc.IncreaseEXP(expAmount);
			Death();
		}

		UpdateHealthBar();
	}

	protected virtual void UpdateHealthBar(){
		
	}

	protected virtual void ShieldRecharge(){

		shield += actualDeltaTime;
		if (shield >= maxShield) {
			shield = maxShield;
		}
	}

	protected virtual void ShieldDecay(){

			extraShield -= actualDeltaTime;
		if (extraShield <= 0) {
			extraShield = 0;
		}
	}

	protected virtual void ExtraShieldPulse(float shield){

		extraShield += shield;
		if (extraShield >= shieldChargers * 100) {
			extraShield = (shieldChargers * 100);
		}
	}

	public virtual void OnTriggerEnter2D(Collider2D col){

		if (col.gameObject.tag == "Player") {

			col.gameObject.GetComponent<PlayerController>().TakeDamage(suicideDamage);
			gc.LiveEnemies.Remove (this);
			Death();

		}


	}

	protected virtual void Death(){
		dead = true;
		miniMapSprite.enabled = false;
		foreach (var thruster in thrusters) {
			thruster.enabled = false;
		}
		audioSource.clip = shipExplodeSFX;
		audioSource.volume = gc.pgd.sfxVol/50;
		audioSource.Play ();
		gc.LiveEnemies.Remove (this);
		GetComponent<BoxCollider2D> ().enabled = false;
		anim.SetTrigger ("Death");
	}

	public void FinishDeath(){	
		Destroy (gameObject, 2f);
	}

	public bool GetIsSlowed(){
		return timeSlowed;
	}

	public void ShieldPulse(float shield){
		shieldDecay = 1.5f;
		ExtraShieldPulse (shield);

	}
}
