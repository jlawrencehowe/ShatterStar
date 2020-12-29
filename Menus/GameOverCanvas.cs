using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameOverCanvas : MonoBehaviour {

	public Canvas shipSelection;
	private PersistingGameData pgd;
	private bool waitingForFinishOpen, waitingForFinishClose;
	private bool selectionCanvasOpen;
	public Button yes, no;
	private MenuOpen menuOpen, shipSelectionMenuOpen;
	public AudioSource audio;


	// Use this for initialization
	void Start () {
		audio = GetComponent<AudioSource>();
		menuOpen = GetComponent<MenuOpen>();
		pgd = GameObject.Find ("PersistingGameData").GetComponent<PersistingGameData> ();
		shipSelectionMenuOpen = GameObject.Find("CharacterSelection").GetComponent<MenuOpen>();
		audio.ignoreListenerPause = true;
	}
	
	// Update is called once per frame
	void Update () {

		if (waitingForFinishOpen) {
			if (!menuOpen.moving) {
				waitingForFinishOpen = false;
				ActivateButtons();
				}
		}
		if(waitingForFinishClose){
			if (!menuOpen.moving) {
				waitingForFinishClose = false;
				shipSelection.GetComponent<MenuOpen>().Open();
				shipSelection.enabled = true;
				selectionCanvasOpen = true;
			}
		}
		if(selectionCanvasOpen){
			if (!shipSelectionMenuOpen.moving) {
				selectionCanvasOpen = false;

			}
		}

	}

	public void Yes(){
		audio.volume = pgd.sfxVol/50;
		audio.Play();
		menuOpen.Close();
		waitingForFinishClose = true;
	}

	public void No(){
		audio.volume = pgd.sfxVol/50;
		audio.Play();
		Time.timeScale = 1;
		Application.LoadLevel ("MainMenu");
	}

	public void SelectAverage(){
		audio.volume = pgd.sfxVol/50;
		audio.Play();
		pgd.shipType = "Average";
		Application.LoadLevel ("Test");
	}
	
	public void SelectTank(){
		audio.volume = pgd.sfxVol/50;
		audio.Play();
		pgd.shipType = "Tank";
		Application.LoadLevel ("Test");
	}
	
	public void SelectSpeedster(){
		audio.volume = pgd.sfxVol/50;
		audio.Play();
		pgd.shipType = "Speedster";
		Application.LoadLevel ("Test");
	}
	
	public void SelectSniper(){
		audio.volume = pgd.sfxVol/50;
		audio.Play();
		pgd.shipType = "Sniper";
		Application.LoadLevel ("Test");
	}

	public void StartGameOver(){
		this.GetComponent<Canvas>().enabled = true;
		menuOpen.Open();
		waitingForFinishOpen = true;
        if(pgd.interAdCounter >= 3)
        {
            pgd.ResetInterAdCounter();
            BannerAd.bannerAd.ShowInterAd();

        }
        pgd.IncrementInterAdCounter();
	}

	private void ActivateButtons(){
		yes.enabled = true;
		no.enabled = true;
	}

}
