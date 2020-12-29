using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{

	//stats
	public float acceleration = 200;
	public float maxSpeed = 5;
	public float fireSpeed = 0.5f;
	//used for any buffs that alter stats temporarily
	protected float tempAcceleration;
	protected float tempMaxSpeed;
	protected float tempFireSpeed;
	protected float tempBulletDamage;
	protected float tempHealth;
	protected float tempMaxHealth;
	public float bulletSpeed = 20;
	public float bulletDamage = 1;
	protected float bulletDecay;
	protected float fireTimer;
	public float health;
	public float maxHealth;
	protected float shieldTimer = 0;
	public float maxShield = 0;
	public float shield = 0;
	protected float shieldRecoveryRate = 5;
	//used for possible keyboard controls
    protected float rotSpeed = 100;



	//abilities
	protected int firstAbility, secondAbility;
	public float abilityCD;
	public float maxCD;
	public bool isAbilityBoosted = false;
	protected bool isIronCurtained = false;
	public bool isTimeSlow = false;
	//flat CDs
	protected float abilityBoostCD = 60;
	protected float ironCurtainCD = 120;
	protected float railGunCD = 120;
	protected float timeSlowCD = 130;
	protected float timeSlowDuration = 10;
	protected bool isShield;
	protected bool isHealthRegen;
	protected int healthRegenLevel;
	//Amount of objects a bullet can hit before destroy
	public int pierceAmount = 1;
	protected bool isExplosiveShot;
	protected bool isHoming;
	public float guns = 1;

	//object references
	public GameObject bullet;
	public GameController gc;
	protected Animator anim;
	protected GameOverCanvas goc;
	protected JoyStick joy;
	public Button pauseButton;
	public JoyStick jsMovement;
	public Image expBar;
	public Transform gun1, gun2, gun3;
	public List<SpriteRenderer> shipBoosters;
	public GameObject lightning;
	public Transform railGunLoc;
	public GameObject railGun;
	public GameObject slowStartPart;
	public GameObject slowEndPart;
	public GameObject railGunCharge;
	public GameObject ironCurtain;
	private GameObject activeIronCurtain, activeLightning, activeRailGunCharge;
	private AudioSource audioSource;
	public AudioSource shieldAudio;
	public AudioClip levelUpAudio, playerDeathSFX;
	public AudioClip ironCurtainSFX, railgunSFX, timeSlowSFX, overChargeSFX;
	private Rigidbody2D playerRigBod;
    public Animator levelUpAnim, levelUpPartAnim;
    public Text levelUpText;
    private SpecialButton specialBut1, specialBut2;

	//experience
	protected float exp;
	public int level;
	protected int[] expNeeded = new int[] {
		250,
		500,
		950,
		2000,
		3750,
		7000,
		9500,
		16000,
		23000,
		30000,
		40000,
		50000,
		60000,
		70000,
		80000
	};

	//player state
	public bool isDead = false;
	public bool isSlowed = false;
	private bool chargingRailGun = false;



	public virtual void Start ()
	{
		audioSource = GetComponent<AudioSource>();
		playerRigBod = GetComponent<Rigidbody2D>();
		gc = GameObject.Find ("GameController").GetComponent<GameController> ();
		goc = GameObject.Find ("GameOverCanvas").GetComponent<GameOverCanvas> ();
        specialBut1 = GameObject.Find("SpecialButton").GetComponent<SpecialButton>();
        if(GameObject.Find("SpecialButton2") != null)
            specialBut2 = GameObject.Find("SpecialButton2").GetComponent<SpecialButton>();
        joy = GameObject.Find ("JoyCircle").GetComponent<JoyStick> ();
		expBar = GameObject.Find ("ExpBar").GetComponent<Image> ();
		pauseButton = GameObject.Find ("PauseButton").GetComponent<Button> ();
		anim = GetComponent<Animator> ();
        levelUpText = GameObject.Find("LevelUpText").GetComponent<Text>();
	
	}
	

	void Update ()
	{
		//debug
		if (Input.GetButtonDown ("TestButton")) {
            IncreaseEXP (expNeeded [level]);
            //Time.timeScale = 0.5f;
		}
	

		if (!isDead) {
		
			//reduce fire time
			if (fireTimer >= 0)
				fireTimer -= Time.deltaTime;

			//reduce shield time based on recovery rate
			if (shieldTimer <= 0) {
				shield += Time.deltaTime * shieldRecoveryRate;
				if (shield > maxShield) {
					shield = maxShield;
				}
				
				health += healthRegenLevel * Time.deltaTime;
				if (health > maxHealth) {
					health = maxHealth;
				}
			}



			if (ironCurtainCD <= 0 && isIronCurtained) {
				isIronCurtained = false;
			}



			shieldTimer -= Time.deltaTime;


		}


	}

	void FixedUpdate(){
		if (!isDead) {
			//grabs direction from virtual joystick script
			Vector3 direction = jsMovement.InputDirection;
			MovePlayer (direction);
			RotatePlayer (direction);

            //Fire
            if (Input.GetKey("j"))
            {
                //Debug.Log("fire");
                PlayerFire();
            }
            //Special
            if (Input.GetKeyDown("k"))
            {
                
                //Debug.Log("special 1");
                specialBut1.ButtonDown();
            }
            //Special 2
            if (Input.GetKeyDown("l"))
            {
                //Debug.Log("Special 2");

                if (specialBut2 != null)
                    specialBut2.ButtonDown();
                else if (GameObject.Find("SpecialButton2") != null)
                {
                    specialBut2 = GameObject.Find("SpecialButton2").GetComponent<SpecialButton>();
                    specialBut2.ButtonDown();
                }
            }
            //Move
            //MovePlayerKeyBoard(Input.GetAxisRaw("Vertical"));
            //Rotate
            //RotatePlayerKeyBoard(Input.GetAxisRaw("Horizontal"));
        
        }
	}

	//Handles applying force to the player and activating/deactivating booster objects
	public void MovePlayer (Vector3 direction)
	{ 
		//Check if the opening is finished
		if(gc.finishedOp){
		AudioSource boosterAudio = shipBoosters [0].GetComponent<AudioSource>();
		if (direction.magnitude != 0 && !isSlowed) {
			//apply force up to a max on the ship
			playerRigBod.AddForce (transform.up * acceleration * Time.deltaTime);
			playerRigBod.velocity = Vector3.ClampMagnitude(playerRigBod.velocity, maxSpeed);

			if (shipBoosters [0].enabled == false) {
				boosterAudio.volume = gc.pgd.sfxVol/300;
				if(!boosterAudio.isPlaying){
					boosterAudio.Play();
				}
				foreach (var booster in shipBoosters) {
					booster.enabled = true;
				}
			}
		} else if (shipBoosters [0].enabled == true) {
			foreach (var booster in shipBoosters) {
				booster.enabled = false;
			}
		}
		else{
			boosterAudio.volume = boosterAudio.volume - Time.deltaTime;
			if(boosterAudio.volume <= 0){
				boosterAudio.Stop();
			}
		}

		}

	}
    
	//handles player rotation
	private void RotatePlayer (Vector3 direction)
	{
	
		
		if(gc.finishedOp){
		float heading = Mathf.Atan2 (direction.x, direction.y) * Mathf.Rad2Deg;
		
		if (direction.x != 0 || direction.y != 0) {
			transform.rotation = Quaternion.Euler (0, 0, -heading);
			
		}
		}
	}
	//depreciated
	//handles keyboard movement controls for player
    private void MovePlayerKeyBoard(float direction)
    {
        if (gc.finishedOp)
        {
            AudioSource boosterAudio = shipBoosters[0].GetComponent<AudioSource>();
            if (direction != 0 && !isSlowed)
            {
                playerRigBod.AddForce(transform.up * acceleration * Time.deltaTime);
                playerRigBod.velocity = Vector3.ClampMagnitude(playerRigBod.velocity, maxSpeed);
                if (shipBoosters[0].enabled == false)
                {
                    boosterAudio.volume = gc.pgd.sfxVol / 300;
                    if (!boosterAudio.isPlaying)
                    {
                        boosterAudio.Play();
                    }
                    foreach (var booster in shipBoosters)
                    {
                        booster.enabled = true;
                    }
                }
            }
            else if (shipBoosters[0].enabled == true)
            {
                foreach (var booster in shipBoosters)
                {
                    booster.enabled = false;
                }
            }
            else
            {
                boosterAudio.volume = boosterAudio.volume - Time.deltaTime;
                if (boosterAudio.volume <= 0)
                {
                    boosterAudio.Stop();
                }
            }

        }
    }

	//depreciated
	//handles keyboard controls for player rotation
    private void RotatePlayerKeyBoard(float direction)
    {
        if (gc.finishedOp)
        {
            Vector3 tempRot = transform.rotation.eulerAngles;
            tempRot.z += (Time.deltaTime * rotSpeed * -direction);
            transform.rotation = Quaternion.Euler(tempRot);
        }
    }

	//fires main ship weapon on call
    public void PlayerFire ()
	{
		
		if (!chargingRailGun && gc.finishedOp) {
			if (fireTimer <= 0) {
				//check the active amount of guns on the ship, depends on ship and skills, then spawn X amount of bullets.
				if (guns == 1) {
					GameObject tempBullet = Instantiate (bullet, gun1.position, transform.rotation) as GameObject;
					tempBullet.GetComponent<Bullet> ().InitStats (bulletSpeed, bulletDamage, transform.rotation, bulletDecay, pierceAmount, isExplosiveShot, isHoming);
					fireTimer = fireSpeed;
			
				} else if (guns == 2) {

					GameObject tempBullet = Instantiate (bullet, gun1.position, transform.rotation) as GameObject;
					tempBullet.GetComponent<Bullet> ().InitStats (bulletSpeed, bulletDamage, transform.rotation, bulletDecay, pierceAmount, isExplosiveShot, isHoming);
					fireTimer = fireSpeed;

					tempBullet = Instantiate (bullet, gun2.position, transform.rotation) as GameObject;
					tempBullet.GetComponent<Bullet> ().InitStats (bulletSpeed, bulletDamage, transform.rotation, bulletDecay, pierceAmount, isExplosiveShot, isHoming);
					fireTimer = fireSpeed;
			
				} else {
					Vector3 tempTransform = transform.position;
					tempTransform.x += 0.1f;
					GameObject tempBullet = Instantiate (bullet, gun1.position, transform.rotation) as GameObject;
					tempBullet.GetComponent<Bullet> ().InitStats (bulletSpeed, bulletDamage, transform.rotation, bulletDecay, pierceAmount, isExplosiveShot, isHoming);
					fireTimer = fireSpeed;
						
					tempTransform = transform.position;
					tempTransform.x -= 0.1f;
					tempBullet = Instantiate (bullet, gun3.position, transform.rotation) as GameObject;
					tempBullet.GetComponent<Bullet> ().InitStats (bulletSpeed, bulletDamage, transform.rotation, bulletDecay, pierceAmount, isExplosiveShot, isHoming);
					fireTimer = fireSpeed;

					tempBullet = Instantiate (bullet, gun2.position, transform.rotation) as GameObject;
					tempBullet.GetComponent<Bullet> ().InitStats (bulletSpeed, bulletDamage, transform.rotation, bulletDecay, pierceAmount, isExplosiveShot, isHoming);
					fireTimer = fireSpeed;
			
				}
			}
		}
		

	}

	//Activates first special move based on ship type. 1 = Ability boost, 2 = Iron Curtain, 3 = Railgun, 4 = Chrono Slow
	public void SpecialMove (int abilityNum)
	{
		if(!isDead && gc.finishedOp){
		audioSource.volume = gc.pgd.sfxVol/100;
		//Ability boost
		if (abilityNum == 1) {
			AbilityBoost ();
			audioSource.loop = true;
			audioSource.clip = overChargeSFX;
		}
		//Iron Curtain
		else if (abilityNum == 2) {
			IronCurtain();
			audioSource.loop = false;
			audioSource.clip = ironCurtainSFX;
		} 
		//Railgun
			else if (abilityNum == 3) {
			ChargeRailGun ();
			audioSource.loop = false;
			audioSource.clip = railgunSFX;

		} 
		//Chrono Slow	
			else if (abilityNum == 4) {
			isTimeSlow = true;
			TimeSlow ();
			audioSource.loop = false;
			audioSource.clip = timeSlowSFX;
		}
		
		audioSource.Play();
		}
	}

	//Shuts off any active abilities
	public void DeactivateAbility ()
	{
	
		isAbilityBoosted = false;
		isIronCurtained = false;
		AbilityBoostEnd ();

	}

	//Activates Ability Boost skill which temporarily alters player temp stats
	public void AbilityBoost ()
	{
		//special effects for ability boost
		activeLightning = Instantiate (lightning, this.transform.position, this.transform.rotation) as GameObject;
		Destroy (activeLightning, 15);
		activeLightning.transform.parent = this.transform;
		anim.SetTrigger ("Overcharge");
		//updates all temp stats before increasing base stats
		tempAcceleration = acceleration;
		tempMaxSpeed = maxSpeed;
		tempFireSpeed = fireSpeed;
		tempBulletDamage = bulletDamage;
		tempHealth = health;
		tempMaxHealth = maxHealth;
		acceleration *= 1.5f;
		maxSpeed *= 1.5f;
		fireSpeed /= 1.5f;
		bulletDamage *= 1.5f;
		maxHealth *= 1.5f;
		health += (maxHealth - tempMaxHealth);
		if (health > maxHealth) {
			health = maxHealth;
		}
		
		//reverts ability boost after 15 seconds
		Invoke ("AbilityBoostEnd", 15);
		
	}

	//Reverts number changes from ability boost and removes special effects
	public void AbilityBoostEnd ()
	{
		isAbilityBoosted = false;
		anim.SetTrigger ("FinishOvercharge");
		acceleration = tempAcceleration;
		maxSpeed = tempMaxSpeed;
		fireSpeed = tempFireSpeed;
		bulletDamage = tempBulletDamage;
		maxHealth = tempMaxHealth;
		if (health > maxHealth) {
			health = maxHealth;
		}
		audioSource.Pause();
	}

	//activates iron curtain ability which reduces all incoming damage in the TakeDamage function
	public void IronCurtain ()
	{
		isIronCurtained = true;
		activeIronCurtain = Instantiate (ironCurtain, Vector3.zero, this.transform.rotation) as GameObject;
		activeIronCurtain.transform.SetParent (this.transform);
		//sets different center spots and scale for each ship due to different sizes
		if (gc.pgd.shipType == "Average") {
			activeIronCurtain.transform.localPosition = new Vector3 (-0.02f, 0.131f, -2);
			activeIronCurtain.transform.localScale = new Vector3 (1, 1, 1);
		} else if (gc.pgd.shipType == "Tank") {
			activeIronCurtain.transform.localPosition = new Vector3 (-0.025f, 0.237f, -2);
			activeIronCurtain.transform.localScale = new Vector3 (1.3f, 1.3f, 1);
		} else if (gc.pgd.shipType == "Sniper") {
			activeIronCurtain.transform.localPosition = new Vector3 (-0.015f, 0.155f, -2);
			activeIronCurtain.transform.localScale = new Vector3 (1.2f, 1.2f, 1);
		} else if (gc.pgd.shipType == "Speedster") {
			activeIronCurtain.transform.localPosition = new Vector3 (-0.013f, 0.21f, -2);
			activeIronCurtain.transform.localScale = new Vector3 (1.2f, 1.2f, 1);
		}
		Destroy (activeIronCurtain, 15);
		Invoke ("TriggerFade", 11);
		Invoke ("TriggerEnd", 14);
		Invoke ("IronCurtainEnd", 15);
	}

	//Starts end animation for iron curtain
	public void TriggerFade ()
	{
		activeIronCurtain.GetComponent<Animator> ().SetTrigger ("Fading");
	}

	//finishes end animation for iron curtain
	public void TriggerEnd ()
	{
		activeIronCurtain.GetComponent<Animator> ().SetTrigger ("End");
	}

	//final trigger for end of iron curtain
	public void IronCurtainEnd ()
	{
		isIronCurtained = false;
	}

	//Activates Chrono Slow ability
	public void TimeSlow ()
	{
		//special effects
		GameObject temp = Instantiate (slowStartPart, this.transform.position, this.transform.rotation) as GameObject;
		temp.transform.SetParent (this.transform);
		//static bool on base enemy class that reduces speed
		EnemyBase.timeSlowed = true;
		EnemyBullet.timeSlow = true;
		Invoke ("CreateTimePulseEnd", 8.75f);
		Invoke ("TimeSlowEnd", 10);
	}

	//Creates special effect for end of Chrono Slow
	private void CreateTimePulseEnd(){
		GameObject temp = Instantiate (slowEndPart, this.transform.position, this.transform.rotation) as GameObject;
		temp.transform.SetParent (this.transform);
	}

	//Reverts static bools on base enemy class
	public void TimeSlowEnd ()
	{
		EnemyBase.timeSlowed = false;
		EnemyBullet.timeSlow = false;
		isTimeSlow = false;

	}

	//Activates Railgun ability
	private void ChargeRailGun ()
	{
		//starts to charge the railgun
		activeRailGunCharge = Instantiate (railGunCharge, railGunLoc.position, this.transform.rotation) as GameObject;
		Destroy (activeRailGunCharge, 2);
		activeRailGunCharge.transform.parent = this.transform;
		//prevents player from firing normal bullets
		chargingRailGun = true;
		Invoke ("FireRailGun", 2);
	}

	//Activates Railgun
	private void FireRailGun ()
	{
		//Finds all targets within a set range of different raycasts spawning from the player in a large rectangle applying large amounts of damage
		chargingRailGun = false;
		playerRigBod.AddForce (-1 * transform.up * 500);
		List<GameObject> HitEnemies = new List<GameObject> ();
		Vector3 startPos = this.transform.position;
		startPos = startPos - (transform.right * 0.6f);
		for (int i = 0; i < 13; i++) {
			RaycastHit2D[] ray = Physics2D.RaycastAll (startPos, transform.up, Mathf.Infinity);
			Debug.DrawRay (startPos, transform.up * 100, Color.red, 10);
			for (int x = 0; x < ray.Length; x++) {
				if (ray [x].rigidbody.gameObject.tag == "Enemy" && !HitEnemies.Contains (ray [x].rigidbody.gameObject)) {
					HitEnemies.Add (ray [x].rigidbody.gameObject);
					ray [x].rigidbody.gameObject.GetComponent<EnemyBase> ().TakeDamage (500);
					Debug.Log ("hit");
				}
			}
			startPos += (transform.right * 0.1f);
		}


		/*
		RaycastHit2D[] ray = Physics2D.RaycastAll (this.transform.position, transform.up, Mathf.Infinity);
		for (int i = 0; i < ray.Length; i++) {
			if (ray [i].rigidbody.gameObject.tag == "Enemy") {
				HitEnemies.Add(ray [i].rigidbody.gameObject);
				ray [i].rigidbody.gameObject.GetComponent<EnemyBase> ().TakeDamage (5000);
				Debug.Log ("hit");
			}
		}
		Vector3 tempPos = this.transform.position;
		tempPos += transform.right * 0.1f;
		Debug.DrawRay(tempPos, transform.up * 100, Color.red, 10);
		ray = Physics2D.RaycastAll (tempPos, transform.up, Mathf.Infinity);
		for (int i = 0; i < ray.Length; i++) {
			if (ray [i].rigidbody.gameObject.tag == "Enemy" && !HitEnemies.Contains(ray [i].rigidbody.gameObject)) {
				HitEnemies.Add(ray [i].rigidbody.gameObject);
				ray [i].rigidbody.gameObject.GetComponent<EnemyBase> ().TakeDamage (5000);
				Debug.Log ("hit");
			}
		}
		tempPos = this.transform.position;
		tempPos -= transform.right * 0.1f;
		Debug.DrawRay(tempPos, transform.up * 100, Color.red, 10);
		ray = Physics2D.RaycastAll (tempPos, transform.up, Mathf.Infinity);
		for (int i = 0; i < ray.Length; i++) {
			if (ray [i].rigidbody.gameObject.tag == "Enemy" && !HitEnemies.Contains(ray [i].rigidbody.gameObject)) {
				ray [i].rigidbody.gameObject.GetComponent<EnemyBase> ().TakeDamage (5000);
				Debug.Log ("hit");
			}
		}
		Debug.DrawRay(this.transform.position, transform.up * 100, Color.red, 10);
		*/
		Instantiate (railGun, railGunLoc.position, transform.rotation);

	}

	//Reduce player health based on damage
	public void TakeDamage (float damage)
	{


		//Reduce damage beforehand if Iron Curtain is active
		if (isIronCurtained) {
			damage = damage * 0.3f;
			damage = Mathf.Ceil (damage);
		}
		//Reset shield timer for shield regen
		shieldTimer = 3;
		//remove value from damage based on shield first
		if(shield > 0 && ((shield - damage) <= 0)){
			shieldAudio.volume = gc.pgd.sfxVol/25;
			shieldAudio.Play();
		}
		shield -= damage;
		//then remove left over value from health
		if (shield < 0) {
			health += shield;
			shield = 0;
		}
		//call gameover if health is at or below 0
		if (health <= 0) {
			GameOver ();
		}

	}

    
	//Ends game
	private void GameOver ()
	{

		//pause all enemy audio
		foreach (var enemy in gc.LiveEnemies) {
			var tempAudio = enemy.GetComponent<AudioSource>();
			if(tempAudio != null){
				tempAudio.Pause();
			}
			enemy.enabled = false;
		}
		//remove ability special effect
		if(activeLightning != null){
			Destroy(activeLightning);
		}
		if(activeIronCurtain != null){
			Destroy(activeIronCurtain);
		}
		if(activeRailGunCharge != null){
			Destroy(activeRailGunCharge);
			chargingRailGun = false;
		}
		//revert status effects and cancel invokes
		EnemyBase.timeSlowed = false;
		EnemyBullet.timeSlow = false;
		isTimeSlow = false;
		CancelInvoke();
		//stop boosters
		shipBoosters [0].GetComponent<AudioSource>().enabled = false;
		audioSource.loop = false;
		audioSource.clip = playerDeathSFX;
		audioSource.volume = gc.pgd.sfxVol/50;
		audioSource.Play();
		//prevents player from doing anything
		isDead = true;
		//start death animation
		anim.SetTrigger ("Death");
		foreach (var booster in shipBoosters) {
			booster.enabled = false;
		}
		this.GetComponent<BoxCollider2D> ().enabled = false;
		playerRigBod.velocity = Vector3.zero;
		GameObject.Find ("PauseButton").GetComponent<PauseButton> ().enabled = false;
		joy.enabled = false;
	}

	public void FinishDeath ()
	{
		//Destroy (this);
		GetComponent<SpriteRenderer>().enabled = false;
		goc.GetComponent<GameOverCanvas> ().StartGameOver ();
	}

	public int GetPlayerLevel ()
	{
		return level;
	}

	public float GetHealth ()
	{
		return health;
	}

	public float GetMaxHealth ()
	{
		return maxHealth;
	}

	public float GetShield ()
	{
		return shield;
	}

	public float GetMaxShield ()
	{
		return maxShield;
	}

	public void IncreaseBulletDamage (float damage)
	{
		bulletDamage += damage;
	}

	public void IncreaseAcceleration (int speed)
	{
		acceleration += speed;
	}

	public void IncreaseMaxSpeed (float speed)
	{
		maxSpeed += speed;
	}

	public void IncreaseFireSpeed (float speed)
	{
		fireSpeed -= speed;
	}

	public void IncreaseBulletSpeed (float speed)
	{
		bulletSpeed += speed;
	}

	public void IncreaseBulletDecay (float decay)
	{
		bulletDecay += decay;
	}

	public void IncreaseMaxHealth (float health)
	{
		maxHealth += health;
		this.health = maxHealth;
	}

	public void IncreaseMaxShield (float shield)
	{
		maxShield += shield;
	}

	public void IncreaseHealthRegen ()
	{
		healthRegenLevel++;
	}

	public void IncreasePierce ()
	{
		pierceAmount++;
	}

	public void SetExplosive ()
	{
		isExplosiveShot = true;
	}

	public void IncreaseGuns ()
	{
		anim.SetTrigger ("Double");
		SetUpSecondGun ();
		guns++;
	}

	public void SetHoming ()
	{
		isHoming = true;
	}

	public void IncreaseEXP (float exp)
	{

		this.exp += exp;
		//Debug.Log (exp);
		CheckLevelUp ();

	}
	//Check player level and exp to see if level up is possible and then call update exp bar
	void CheckLevelUp ()
	{
		if (level >= expNeeded.Length) {
			int tempLevel = expNeeded.Length - 1;
			if (exp >= expNeeded [tempLevel]) {


				//audioSource.clip = levelUpAudio;
			//	audioSource.volume = gc.pgd.sfxVol/100;
				//audioSource.Play ();
				exp -= expNeeded [tempLevel];
				level++;
				gc.skills.LevelUp ();
                levelUpAnim.SetTrigger("LevelUp");
                levelUpPartAnim.SetTrigger(gc.pgd.shipType);
                levelUpText.enabled = true;
				health = maxHealth;
				shield = maxShield;
				shipBoosters [0].GetComponent<AudioSource>().Stop();

			}
		} else if (exp >= expNeeded [level]) {
			//audioSource.clip = levelUpAudio;
			//audioSource.volume = gc.pgd.sfxVol/100;
			//audioSource.Play ();
			exp -= expNeeded [level];
			level++;
			gc.skills.LevelUp ();
            levelUpAnim.SetTrigger("LevelUp");
            levelUpPartAnim.SetTrigger(gc.pgd.shipType);
            levelUpText.enabled = true;
            health = maxHealth;
			shield = maxShield;
			shipBoosters [0].GetComponent<AudioSource>().Stop();
		}

		UpdateExpBar ();
	}

	//Update the visual of the exp bar to match the actual amount
	void UpdateExpBar ()
	{

		expBar.fillAmount = exp / expNeeded [level];

	}

	//Gun positions depending on each ship
	public virtual void SetUpSecondGun ()
	{

	}


    public void FinishLevelUpEffect()
    {


        levelUpAnim.SetTrigger("Finish");
        levelUpPartAnim.SetTrigger("Finish");
        levelUpText.enabled = false;

    }

}
