using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AbilityIncrease : MonoBehaviour {

	public SkillUps skillUps;
	public Text skillInfoText;
	public string abilityName;
	public string skillInfo;

	public delegate void AbilityFunc();
	public AbilityFunc abilityFunc;
	private AudioSource audio;
	private PersistingGameData pgd;

	// Use this for initialization
	void Start () {

		pgd = GameObject.Find ("PersistingGameData").GetComponent<PersistingGameData> ();
		audio = GetComponent<AudioSource>();
		audio.ignoreListenerPause = true;
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ClickedButton(){
		//updateText
		audio.volume = pgd.sfxVol;
		audio.Play();
		skillInfoText.text = "Skill: " + skillInfo;
		//set this as active button
		skillUps.ActiveButton = this.gameObject;
	}

	//called when confirm button is pressed on the skills up menu
	public void ConfirmButtonPressed(){
		abilityFunc();
	}
}
