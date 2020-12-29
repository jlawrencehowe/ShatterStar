using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SkillUps : MonoBehaviour {

	private int damageUp;
	private int attackRateUp;
	private int healthUp;
	private int shieldUp;
	private int healthRegen;
	private int tankHealthBoost;
	private int tankExplosive;
	private int speedsterExtraCannon;
	private int speedsterAttackRateUp;
	private int sniperPierce;
	private int sniperDamageBoost;
	private int allStatUp;
	private int homingShot;

	public Text damageUpText;
	public Text attackRateUpText;
	public Text healthUpText;
	public Text shieldUpText;
	public Text healthRegenText;
	public Text tankHealthBoostText;
	public Text tankExplosiveText;
	public Text speedsterExtraCannonText;
	public Text speedsterAttackRateUpText;
	public Text sniperPierceText;
	public Text sniperDamageBoostText;
	public Text allStatUpText;
	public Text homingShotText;
	public Text averageClassText;
	public Text tankClassText;
	public Text sniperClassText;
	public Text speedsterClassText;

	public int averageClass;
	public int tankClass;
	public int sniperClass;
	public int speedsterClass;

	public Button damageUpButton;
	public Button attackRateUpButton;
	public Button healthUpButton;
	public Button shieldUpButton;
	public Button healthRegenButton;
	public Button tankHealthBoostButton;
	public Button tankExplosiveButton;
	public Button speedsterExtraCannonButton;
	public Button speedsterAttackRateUpButton;
	public Button sniperPierceButton;
	public Button sniperDamageBoostButton;
	public Button allStatUpButton;
	public Button averageHomingButton;

	public Button averageClassButton;
	public Button tankClassButton;
	public Button sniperClassButton;
	public Button speedsterClassButton;

	private bool secondClass;

	private float slowTimer;
	private bool slowing;
	private bool resuming;

	GameController gc;
	PersistingGameData pgd;

	public Canvas skillUpCanvas;
	public Text AbilityText;
	public Text playerLevelText;
	public GameObject ActiveButton;
	public JoyStick joyStick;
	public FireButton fireButton;
	public SpecialButton specialButton, specialButton2;

	
	public AudioSource audio;
	public AudioClip levelUpSFX, confirmButton;



	// Use this for initialization
	void Awake () {
		gc = GameObject.Find ("GameController").GetComponent<GameController> ();
		pgd = GameObject.Find ("PersistingGameData").GetComponent<PersistingGameData> ();

		damageUpButton.GetComponent<AbilityIncrease>().abilityFunc = DamageUp;
		attackRateUpButton.GetComponent<AbilityIncrease>().abilityFunc = AttackRateUp;
		healthUpButton.GetComponent<AbilityIncrease>().abilityFunc = HealthUp;
		shieldUpButton.GetComponent<AbilityIncrease>().abilityFunc = ShieldUp;
		healthRegenButton.GetComponent<AbilityIncrease>().abilityFunc = HealthRegen;
		tankHealthBoostButton.GetComponent<AbilityIncrease>().abilityFunc = TankHealthBoost;
		tankExplosiveButton.GetComponent<AbilityIncrease>().abilityFunc = Explosive;
		speedsterExtraCannonButton.GetComponent<AbilityIncrease>().abilityFunc = SpeedsterExtraCannon;
		speedsterAttackRateUpButton.GetComponent<AbilityIncrease>().abilityFunc = SpeedsterAttackRateBoost;
		sniperPierceButton.GetComponent<AbilityIncrease>().abilityFunc = SniperPierceUp;
		sniperDamageBoostButton.GetComponent<AbilityIncrease>().abilityFunc = SniperDamageBoost;
		allStatUpButton.GetComponent<AbilityIncrease>().abilityFunc = AllStatUp;
		averageHomingButton.GetComponent<AbilityIncrease>().abilityFunc = HomingShot;
		averageClassButton.GetComponent<AbilityIncrease>().abilityFunc = AverageClass;
		tankClassButton.GetComponent<AbilityIncrease>().abilityFunc = TankClass;
		sniperClassButton.GetComponent<AbilityIncrease>().abilityFunc = SniperClass;
		speedsterClassButton.GetComponent<AbilityIncrease>().abilityFunc = SpeedsterClass;
		StartingClass();
		audio.ignoreListenerPause = true;

	}
	
	// Update is called once per frame
	void Update () {

		if (slowing) {
			slowTimer -= Time.unscaledDeltaTime;
			if(slowTimer <= 0){
				Time.timeScale = 0;
				slowing = false;
				NextStepLevelUp();
			}
		}
		if (resuming) {
			slowTimer -= Time.unscaledDeltaTime;
			if(slowTimer <= 0){
				Time.timeScale = 1;
				resuming = false;
				gc.pc.IncreaseEXP(0);
			}
		}
	
	}

	public void LevelUp(){
		Time.timeScale = 0.25f;
		slowTimer = 1.5f;
		slowing = true;
		resuming = false;
		audio.clip = levelUpSFX;
		audio.Play();

	}

	public void NextStepLevelUp(){
		Time.timeScale = 0;
		if (gc.pc.isAbilityBoosted) {
			gc.pc.AbilityBoostEnd ();
		}
		TurnOnMenu ();

	}

	private void EnableUIButtons(bool isEnabled){
		
		joyStick.enabled = isEnabled;
		fireButton.enabled = isEnabled;
		specialButton.enabled = isEnabled;
		specialButton.triggerEnabled = isEnabled;

		specialButton2.enabled = isEnabled;
		specialButton2.triggerEnabled = isEnabled;
	}

	public void DamageUp(){
		damageUp++;
		gc.pc.IncreaseBulletDamage (5);
		TurnOffMenu ();
	}

	public void AttackRateUp(){
		attackRateUp++;
		gc.pc.IncreaseFireSpeed (0.1f);
		TurnOffMenu ();
	}

	public void HealthUp(){
		healthUp++;
		gc.pc.IncreaseMaxHealth (20);
		TurnOffMenu ();
	}

	public void ShieldUp(){
		shieldUp++;
		gc.pc.IncreaseMaxShield (20);
		TurnOffMenu ();
	}

	public void HealthRegen(){
		healthRegen++;
		gc.pc.IncreaseHealthRegen ();
		TurnOffMenu ();
	}

	public void TankHealthBoost(){
		tankHealthBoost++;
		gc.pc.IncreaseMaxHealth (200);
		TurnOffMenu ();
	}

	public void Explosive(){
		tankExplosive++;
		gc.pc.SetExplosive ();
		TurnOffMenu ();
	}


	public void SpeedsterExtraCannon(){
		speedsterExtraCannon++;
		gc.pc.IncreaseGuns ();
		TurnOffMenu ();
	}

	public void SpeedsterAttackRateBoost(){
		speedsterAttackRateUp++;
		gc.pc.IncreaseFireSpeed (0.2f);
		TurnOffMenu ();
	}

	public void SniperPierceUp(){
		sniperPierce++;
		gc.pc.IncreasePierce ();
		TurnOffMenu ();
	}

	public void SniperDamageBoost(){
		sniperDamageBoost++;
		gc.pc.IncreaseBulletDamage (10);
		TurnOffMenu ();
	}
	
	public void AllStatUp(){
		allStatUp++;
		gc.pc.IncreaseBulletDamage (2);
		gc.pc.IncreaseFireSpeed (0.05f);
		gc.pc.IncreaseMaxHealth (20);
		TurnOffMenu ();
	}

	public void HomingShot(){
		homingShot++;
		gc.pc.SetHoming ();
		TurnOffMenu ();
	}




	public void TankClass(){

		tankClass++;
		secondClass = true;
		gc.pc.IncreaseMaxHealth (tankHealthBoost);
		gc.pc.IncreaseBulletDamage (damageUp);
		specialButton2.SetAbility(2);
		specialButton2.gameObject.SetActive(true);
		specialButton2.triggerEnabled = true;
		TurnOffMenu ();
	}

	public void SpeedsterClass(){
		
		speedsterClass++;
		gc.pc.IncreaseFireSpeed (speedsterAttackRateUp);
		gc.pc.IncreaseMaxSpeed (2.5f);
		gc.pc.IncreaseAcceleration (50);
		secondClass = true;
		specialButton2.SetAbility(4);
		specialButton2.gameObject.SetActive(true);
		specialButton2.triggerEnabled = true;
		TurnOffMenu ();
	}

	public void SniperClass(){
		
		sniperClass++;
		gc.pc.IncreaseBulletDamage (20);
		gc.pc.IncreaseBulletSpeed (10);
		secondClass = true;
		specialButton2.SetAbility(3);
		specialButton2.gameObject.SetActive(true);
		specialButton2.triggerEnabled = true;
		TurnOffMenu ();
	}

	public void AverageClass(){
		
		averageClass++;
		gc.pc.IncreaseBulletDamage (damageUp);
		gc.pc.IncreaseFireSpeed (0.5f);
		gc.pc.IncreaseMaxHealth (healthUp);
		secondClass = true;
		specialButton2.SetAbility(1);
		specialButton2.gameObject.SetActive(true);
		specialButton2.triggerEnabled = true;
		TurnOffMenu ();
	}

	public void StartingClass(){
		if (pgd.shipType == "Average") {
			averageClass++;
		}
		else if(pgd.shipType == "Tank") {
			tankClass++;
		}
		else if(pgd.shipType == "Sniper") {
			sniperClass++;
		}
		else if(pgd.shipType == "Speedster") {
			speedsterClass++;
		}
	}

	public void TurnOnMenu(){
		
		EnableUIButtons(false);

		//Base Skills
		if (damageUp < 5)
			damageUpButton.interactable = true;
		else
			damageUpButton.interactable = false;
		if (attackRateUp < 5)
			attackRateUpButton.interactable = true;
		else
			attackRateUpButton.interactable = false;
		if (healthUp < 5)
				healthUpButton.interactable = true;
		else
			healthUpButton.interactable = false;

		if (shieldUp < 5)
				shieldUpButton.interactable = true;
		else
			shieldUpButton.interactable = false;
		if (healthRegen < 5)
				healthRegenButton.interactable = true;
		else
			healthRegenButton.interactable = false;



		//tank skills
		if (tankHealthBoost < 5 && gc.pc.GetPlayerLevel() >= 5 && tankClass == 1)
				tankHealthBoostButton.interactable = true;
		else
			tankHealthBoostButton.interactable = false;
		if (tankExplosive < 1 && gc.pc.GetPlayerLevel() >= 5 && tankClass == 1)
				tankExplosiveButton.interactable = true;
		else
			tankExplosiveButton.interactable = false;


		//speedster skills
		if (speedsterExtraCannon < 1 && gc.pc.GetPlayerLevel() >= 5 && speedsterClass == 1)
				speedsterExtraCannonButton.interactable = true;
		else
			speedsterExtraCannonButton.interactable = false;
		if (speedsterAttackRateUp < 5 && gc.pc.GetPlayerLevel() >= 5 && speedsterClass == 1)
				speedsterAttackRateUpButton.interactable = true;
		else
			speedsterAttackRateUpButton.interactable = false;



		//sniper skills
		if (sniperPierce < 1 && gc.pc.GetPlayerLevel() >= 5 && sniperClass == 1)
				sniperPierceButton.interactable = true;
		else
			sniperPierceButton.interactable = false;
		if (sniperDamageBoost < 5 && gc.pc.GetPlayerLevel() >= 5 && sniperClass == 1)
				sniperDamageBoostButton.interactable = true;
		else
			sniperDamageBoostButton.interactable = false;



		//average class skills
		if (allStatUp < 5 && gc.pc.GetPlayerLevel() >= 5 && averageClass == 1)
				allStatUpButton.interactable = true;
		else
			allStatUpButton.interactable = false;

		if (homingShot < 1 && gc.pc.GetPlayerLevel() >= 5 && averageClass == 1)
			averageHomingButton.interactable = true;
		else
			averageHomingButton.interactable = false;



	



		//class picks
		if(averageClass == 0 && !secondClass && gc.pc.GetPlayerLevel() >= 5)
			averageClassButton.interactable = true;
		else
			averageClassButton.interactable = false;
		if(tankClass == 0 && !secondClass && gc.pc.GetPlayerLevel() >= 5)
			tankClassButton.interactable = true;
		else
			tankClassButton.interactable = false;
		if(sniperClass == 0 && !secondClass && gc.pc.GetPlayerLevel() >= 5)
			sniperClassButton.interactable = true;
		else
			sniperClassButton.interactable = false;
		if(speedsterClass == 0 && !secondClass && gc.pc.GetPlayerLevel() >= 5)
			speedsterClassButton.interactable = true;
		else
			speedsterClassButton.interactable = false;

		playerLevelText.text = "Level: " + (gc.pc.GetPlayerLevel());
		skillUpCanvas.enabled = true;
		damageUpText.text = damageUp + "/5";
		attackRateUpText.text = attackRateUp + "/5";
		healthUpText.text = healthUp + "/5";
		shieldUpText.text = shieldUp + "/5";
		healthRegenText.text = healthRegen + "/5";
		tankHealthBoostText.text = tankHealthBoost + "/5";
		tankExplosiveText.text = tankExplosive + "/1";
		speedsterExtraCannonText.text = speedsterExtraCannon + "/1";
		speedsterAttackRateUpText.text = speedsterAttackRateUp + "/5";
		sniperPierceText.text = sniperPierce + "/1";
		sniperDamageBoostText.text = sniperDamageBoost + "/5";
		allStatUpText.text = allStatUp + "/5";
		homingShotText.text = homingShot + "/1";
		averageClassText.text = averageClass + "/1";
		tankClassText.text = tankClass + "/1";
		sniperClassText.text = sniperClass + "/1";
		speedsterClassText.text = speedsterClass + "/1";

	}


	private void TurnOffMenu(){
		if (gc.pc.isAbilityBoosted) {
			gc.pc.AbilityBoost();
		}
		ActiveButton = null;
		AbilityText.text = "";
		skillUpCanvas.enabled = false;
		Time.timeScale = 0.25f;
		resuming = true;
		slowTimer = 1.5f;
		DelayedEnabledUIButtons(true);

	}

	private void DelayedEnabledUIButtons(bool isEnabled){
		Invoke("EnableUIButtons", 0.1f);
	}

	private void EnableUIButtons(){
		joyStick.enabled = true;
		fireButton.enabled = true;
		specialButton.enabled = true;
		specialButton.triggerEnabled = true;
		
		specialButton2.enabled = true;
		specialButton2.triggerEnabled = true;
	}

	public void ConfirmButton(){

		if(ActiveButton != null){
			audio.volume = gc.pgd.sfxVol;
			audio.clip = confirmButton;
			audio.Play();
			ActiveButton.GetComponent<AbilityIncrease>().ConfirmButtonPressed();
            gc.pc.FinishLevelUpEffect();
        }

	}




}
