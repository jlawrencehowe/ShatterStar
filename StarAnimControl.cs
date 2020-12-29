using UnityEngine;
using System.Collections;

public class StarAnimControl : MonoBehaviour {

	public MenuAnimationController menuAnimCont;
	private AudioSource audioSource;
	public AudioClip starCrack, starTwinkle;
	private PersistingGameData pgd;

	// Use this for initialization
	void Start () {
		pgd = GameObject.Find("PersistingGameData").GetComponent<PersistingGameData>();
		audioSource = GetComponent<AudioSource>();
	
	}
	
	// Update is called once per frame
	void Update () {
	}


	public void TriggerWhiteLight(){
		menuAnimCont.TriggerWhiteLight();
		this.transform.position = new Vector3(-16.7f, 47.5f, 0);
	}

	public void StarTwinkle(){
		audioSource.volume = pgd.sfxVol;
		audioSource.clip = starTwinkle;
		audioSource.Play();
	}

	public void StarCrack(){
		
		audioSource.volume = pgd.sfxVol;
		audioSource.clip = starCrack;
		audioSource.Play();
	}


}
