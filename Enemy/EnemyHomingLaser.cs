using UnityEngine;
using System.Collections;

public class EnemyHomingLaser : EnemyBullet {


	protected float startSpeed;
	private float turnSpeed;
	public TrailRenderer tRenderer;
	private float initMove = 1f;
	public static bool timeSlow;
	private bool charging = true;

	public Animator anim;
	
	// Use this for initialization
	void Start () {
		sr = GetComponent<SpriteRenderer>();
		gc = GameObject.Find ("GameController").GetComponent<GameController> ();
		audio = GetComponent<AudioSource>();
		if(timeSlow == true){
			audio.pitch = 0.25f;
		}
		tRenderer.enabled = false;
		startSpeed = 0.5f;
		turnSpeed = 250;
		charging = true;

	}
	
	// Update is called once per frame
	void Update () {

		float time = Time.deltaTime;
		if (timeSlow) {
			time /= 4;
		}
		if (timeSlow) {
			audio.pitch = 0.25f;
		} else {
			audio.pitch = 1;
		}
		if(!charging){
			/*
		initMove -= time;
		if (initMove > 0) {
			transform.position = transform.position + (startSpeed * transform.up * time);
		} else {
*/
			HomingAttack();
		//}
		}
		
		
	}
	
	public void InitStats(float speed, float damage, float bulletdecay, GameController gc){
		SetSpeed (speed);
		SetDamage (damage);
		SetBulletDecay (bulletdecay);
		this.gc = gc;
		InvokeRepeating ("FadeOut", bulletdecay - 0.3f, 0.1f);
		Destroy (gameObject, bulletDecay);
	}
	
	public void FinishCharging(){
		charging = false;
		tRenderer.enabled = true;
		anim.SetTrigger("FinishCharging");
		audio.Play();
		Debug.Log("Finish Charging");
	}
	


	


	private void HomingAttack ()
	{
		
		Vector3 target = gc.pc.transform.position;
		target.z = 0f;
		
		Vector3 objectPos = transform.position;
		target.x = target.x - objectPos.x;
		target.y = target.y - objectPos.y;
		
		float angle = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg - 90;
		if (angle < 0) {
			angle += 360;
		}
		float enemyRot = transform.rotation.eulerAngles.z;
		float tempRotSpeed;
		if(timeSlow){
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
				tempRot.z += tempRotSpeed * Time.deltaTime;
				transform.rotation = Quaternion.Euler(tempRot);
			} else {
				Vector3 tempRot = transform.rotation.eulerAngles;
				tempRot.z -= tempRotSpeed * Time.deltaTime;
				transform.rotation = Quaternion.Euler(tempRot);
			}
			
		} else {
			float minTest = enemyRot - angle;
			float posTest = angle + (360 - enemyRot);
			if (posTest < minTest) {
				Vector3 tempRot = transform.rotation.eulerAngles;
				tempRot.z += tempRotSpeed * Time.deltaTime;
				transform.rotation = Quaternion.Euler(tempRot);
			} else {
				Vector3 tempRot = transform.rotation.eulerAngles;
				tempRot.z -= tempRotSpeed * Time.deltaTime;
				transform.rotation = Quaternion.Euler(tempRot);
			}
		}
		
		Vector3 tempPos = transform.position;
		if (timeSlow) {
			tempPos = tempPos + (transform.up * speed * (Time.deltaTime/4));
		} else {
			tempPos = tempPos + (transform.up * speed * Time.deltaTime);
		}
		transform.position = tempPos;
		
		
	}

	public override void OnTriggerEnter2D (Collider2D col)
	{
		if(col.gameObject.tag == "Player"){
			tRenderer.enabled = false;
		}
		base.OnTriggerEnter2D (col);

	}

}
