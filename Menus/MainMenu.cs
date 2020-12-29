using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour
{

	private PersistingGameData pgd;
	public Canvas shipSelectCanvas;
	public Canvas gameStartCanvas;
	public Canvas optionsCanvas;
	public Button backgroundCanvas;
	public Button pauseButton, pauseButton2, returnButton, tapButton, titleButton;
	public Slider musicSlider;
	public Slider sfxSlider;
	public Text tapAnywhereText;
	public Text musicText;
	public Text sfxText;
	public Image averageSelect, tankSelect, speedsterSelect, sniperSelect;
	private bool waitingForClose, waitingForOpen;
	private bool waitingForShipOpen, waitingForShipClose;
	private bool waitingForOptionsOpen, waitingForOptionsClose;
	private bool startingGame;
	private bool isOpaque;
	private MenuOpen menuOpen, shipMenu, optionsMenu;
	public AudioSource audio;
	public AudioClip playerSelectSFX, buttonPressSFX;

	private RectTransform selectedShip;
	public RectTransform averageRT, tankRT, sniperRT, speedsterRT;
	public LoadingScreen loadingScreen;

	// Use this for initialization
	void Start ()
	{
		
		audio = GetComponent<AudioSource> ();
		pgd = GameObject.Find ("PersistingGameData").GetComponent<PersistingGameData> ();
		shipSelectCanvas.enabled = false;
		optionsCanvas.enabled = false;
		menuOpen = GetComponent<MenuOpen> ();
		shipMenu = shipSelectCanvas.GetComponent<MenuOpen> ();
		optionsMenu = optionsCanvas.GetComponent<MenuOpen> ();
		menuOpen.Open ();
		waitingForOpen = true;
		backgroundCanvas.enabled = false;
		gameStartCanvas.enabled = false;
		ReappearText();
        pgd.bannerAd.ShowBannerAd();






    }
	
	// Update is called once per frame
	void Update ()
	{
	
		if (waitingForOpen) {
			if (!menuOpen.moving) {
				waitingForOpen = false;
				EnabledBackgroundCanvas (true);
				EnableButtons (true);
			}
		}
		
		if (waitingForClose) {
			if (!menuOpen.moving) {
				waitingForClose = false;
				gameStartCanvas.enabled = false;
				if (startingGame) {
					ShipSelectionOpen ();
				} else {
					OptionsOpen ();
				}
				EnabledBackgroundCanvas (false);
			}
		}
		if (waitingForShipOpen) {
			if (!shipMenu.moving) {
				waitingForShipOpen = false;
				EnableButtons (true);

			}
		}
		if (waitingForShipClose) {
			if (!shipMenu.moving) {
				waitingForShipClose = false;
				shipSelectCanvas.enabled = false;
				OptionsOpen ();
			}
		}
		if (waitingForOptionsOpen) {
			if (!optionsMenu.moving) {
				waitingForOptionsOpen = false;
				EnableButtons (true);
				
			}
		}
		if (waitingForOptionsClose) {
			if (!optionsMenu.moving) {
				waitingForOptionsClose = false;
				ReturnMainMenu ();
			}
		}

	}

	public void StartGame ()
	{
		audio.clip = buttonPressSFX;
		audio.volume = pgd.sfxVol / 50;
		audio.Play ();
		menuOpen.Close ();
		EnabledBackgroundCanvas (false);
		waitingForClose = true;
		EnableButtons (false);

    }

	public void StartShipSelection ()
	{
		
		audio.clip = buttonPressSFX;
		audio.volume = pgd.sfxVol / 50;
		audio.Play ();
		startingGame = true;
		StartGame ();
        pgd.bannerAd.DisableBannerAd();
    }

	public void ReturnMainMenu ()
	{
		gameStartCanvas.enabled = true;
		menuOpen.Open ();
		waitingForOpen = true;
		EnabledBackgroundCanvas (true);
		EnableButtons (false);
	}

	public void ShipSelectionOpen ()
	{
		shipSelectCanvas.enabled = true;
		shipSelectCanvas.GetComponent<MenuOpen> ().Open ();
		waitingForShipOpen = true;
		EnableButtons (false);
	}

	public void ShipSelectionClose ()
	{
		shipSelectCanvas.GetComponent<MenuOpen> ().Close ();
		waitingForShipClose = true;
		startingGame = false;
		EnableButtons (false);
	}

	public void OptionsOpen ()
	{

		optionsCanvas.enabled = true;
		waitingForOptionsOpen = true;
		optionsMenu.Open ();
		EnableButtons (false);
	}

	public void OptionsClose ()
	{
		waitingForOptionsClose = true;
		optionsMenu.Close ();
		EnableButtons (false);
	}

	public void Options ()
	{
		audio.clip = buttonPressSFX;
		audio.volume = pgd.sfxVol / 100;
		audio.Play ();
		startingGame = false;
		if (shipSelectCanvas.enabled == true) {
			ShipSelectionClose ();
		} else {
			StartGame ();
		}
		musicSlider.value = PlayerPrefs.GetFloat ("music");
		sfxSlider.value = PlayerPrefs.GetFloat ("sound");
		musicText.text = "Music: " + musicSlider.value;
		sfxText.text = "Sound: " + sfxSlider.value;
		MusicSlider ();
		SFXSlider ();

	}

	public void Return ()
	{
		audio.clip = buttonPressSFX;
		audio.volume = pgd.sfxVol / 100;
		audio.Play ();
		OptionsClose ();
	}

	private void EnableButtons (bool isEnabled)
	{
		pauseButton.enabled = isEnabled;
		returnButton.enabled = isEnabled;
		pauseButton2.enabled = isEnabled;
		tapButton.enabled = isEnabled;
		titleButton.enabled = isEnabled;
	}

	private void EnabledBackgroundCanvas (bool isEnabled)
	{
		
		backgroundCanvas.enabled = isEnabled;
	}

	public void SelectAverage ()
	{
		audio.clip = playerSelectSFX;
		audio.volume = pgd.sfxVol / 50;
		audio.Play ();
		pgd.shipType = "Average";
		selectedShip = averageRT;
		DisabledSelectButtons();
		DisabledShipSprites();
		averageRT.GetComponent<Image>().enabled = true;
		Invoke ("StartLevel", 3f);
		InvokeRepeating ("FlashPlayerSelect", 0f, 0.251f);
	}

	public void SelectTank ()
	{
		audio.clip = playerSelectSFX;
		audio.volume = pgd.sfxVol / 50;
		audio.Play ();
		pgd.shipType = "Tank";
		selectedShip = tankRT;
		DisabledSelectButtons();
		DisabledShipSprites();
		tankRT.GetComponent<Image>().enabled = true;
		Invoke ("StartLevel", 3f);
		InvokeRepeating ("FlashPlayerSelect", 0f, 0.25f);
	}

	public void SelectSpeedster ()
	{
		audio.clip = playerSelectSFX;
		audio.volume = pgd.sfxVol / 50;
		audio.Play ();
		pgd.shipType = "Speedster";
		selectedShip = speedsterRT;
		DisabledSelectButtons();
		DisabledShipSprites();
		speedsterRT.GetComponent<Image>().enabled = true;
		Invoke ("StartLevel", 3f);
		InvokeRepeating ("FlashPlayerSelect", 0f, 0.25f);
	}

	public void SelectSniper ()
	{
		audio.clip = playerSelectSFX;
		audio.volume = pgd.sfxVol / 50;
		audio.Play ();
		pgd.shipType = "Sniper";
		selectedShip = sniperRT;
		DisabledSelectButtons();
		DisabledShipSprites();
		sniperRT.GetComponent<Image>().enabled = true;
		Invoke ("StartLevel", 3f);
		InvokeRepeating ("FlashPlayerSelect", 0f, 0.25f);
	}

	private void DisabledShipSprites(){

		averageRT.GetComponent<Image>().enabled = false;
		sniperRT.GetComponent<Image>().enabled = false;
		tankRT.GetComponent<Image>().enabled = false;
		speedsterRT.GetComponent<Image>().enabled = false;


	}

	private void DisabledSelectButtons(){
		averageSelect.GetComponent<Button>().enabled = false;
		tankSelect.GetComponent<Button>().enabled = false;
		speedsterSelect.GetComponent<Button>().enabled = false;
		sniperSelect.GetComponent<Button>().enabled = false;
	}

	private void FlashPlayerSelect ()
	{
		Color color;
		if (pgd.shipType == "Average") {
			color = averageSelect.color;
		} else if (pgd.shipType == "Tank") {
			color = tankSelect.color;
		} else if (pgd.shipType == "Speedster") {
			color = speedsterSelect.color;
		} else {
			color = sniperSelect.color;
		}

		if (isOpaque) {
			isOpaque = !isOpaque;
			color.a = 0;
		} else {
			isOpaque = !isOpaque;
			color.a = 1;
		}

		if (pgd.shipType == "Average") {
			averageSelect.color = color;
		} else if (pgd.shipType == "Tank") {
			tankSelect.color = color;
		} else if (pgd.shipType == "Speedster") {
			speedsterSelect.color = color;
		} else {
			sniperSelect.color = color;
		}
	}

	private void StartLevel ()
	{
		//Application.LoadLevel ("Test");
		CancelInvoke();
		loadingScreen.enabled = true;
		loadingScreen.StartLoading(selectedShip);

	}

	public void MusicSlider ()
	{
		musicText.text = "Music: " + musicSlider.value;
		PlayerPrefs.SetFloat ("music", musicSlider.value);
		pgd.ChangeMusic (musicSlider.value);
		audio.clip = buttonPressSFX;
		audio.volume = pgd.sfxVol / 50;
		audio.Play ();
	}

	public void SFXSlider ()
	{
		sfxText.text = "Sound: " + sfxSlider.value;
		PlayerPrefs.SetFloat ("sound", sfxSlider.value);
		pgd.ChangeSFX (sfxSlider.value);
		audio.clip = buttonPressSFX;
		audio.volume = pgd.sfxVol / 50;
		audio.Play ();
	}

	private void DisappearText(){
		if(gameStartCanvas.enabled == true)
			tapAnywhereText.enabled = false;
		Invoke("ReappearText", 0.75f);
	}

	private void ReappearText(){
		if(gameStartCanvas.enabled == true)
			tapAnywhereText.enabled = true;
		Invoke("DisappearText", 1f);
	}


}
