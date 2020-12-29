using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PauseButton : MonoBehaviour
{

	public Canvas PauseCanvas;
	public Slider soundBar;
	public Text soundText;
	public Slider musicBar;
	public Text musicText;
	public Button resumeButton;
	public Canvas QuitCanvas;
	public Button quitButton;
	private JoyStick joyCircle;
	private FireButton actionButton;
	private SpecialButton specialButton;
	private PersistingGameData pgd;
	private bool waitingForResumeClose, startTime;
	private bool waitingForQuitClose;
	public AudioSource audio;

	// Use this for initialization
	void Start ()
	{
		QuitCanvas.enabled = false;
		PauseCanvas.enabled = false;
		joyCircle = GameObject.Find ("JoyCircle").GetComponent<JoyStick> ();
		actionButton = GameObject.Find ("ActionButton").GetComponent<FireButton> ();
		specialButton = GameObject.Find ("SpecialButton").GetComponent<SpecialButton> ();
		pgd = GameObject.Find ("PersistingGameData").GetComponent<PersistingGameData> ();
		audio.ignoreListenerPause = true;
	
	}
	
	// Update is called once per frame
	void Update ()
	{

		if (waitingForResumeClose) {
			startTime = !PauseCanvas.GetComponent<MenuOpen> ().moving;
			if (startTime) {
				FinishResume ();
				waitingForResumeClose = false;
			}
		}

		if (waitingForQuitClose) {
			startTime = !QuitCanvas.GetComponent<MenuOpen> ().moving;
			if (startTime) {
				FinishQuit ();
				waitingForQuitClose = false;
			}
		}
	
	}

	public void PauseButtonPress ()
	{

		if (Time.timeScale != 0) {
			AudioListener.pause = true;
			audio.volume = pgd.sfxVol/50;
			audio.Play();
			EnableDisablePauseMenu(true);
			joyCircle.enabled = false;
			actionButton.enabled = false;
			specialButton.enabled = false;
			Time.timeScale = 0;
			PauseCanvas.GetComponent<MenuOpen> ().Open ();
			PauseCanvas.enabled = true;
			soundBar.value = pgd.sfxVol;
			musicBar.value = pgd.musicVol;
			UpdateSFXVol ();
			UpdateMusicVol ();
		}

	}

	public void UpdateMusicVol ()
	{
		pgd.ChangeMusic (musicBar.value);
		musicText.text = "Music: " + musicBar.value;
		audio.volume = pgd.sfxVol/50;
		audio.Play();
	}

	public void UpdateSFXVol ()
	{
		pgd.ChangeSFX (soundBar.value);
		soundText.text = "Sound: " + soundBar.value;
		audio.volume = pgd.sfxVol/50;
		audio.Play();
	}

	private void FinishResume ()
	{
		specialButton.enabled = true;
		actionButton.enabled = true;
		joyCircle.enabled = true;
		Time.timeScale = 1;
		PauseCanvas.enabled = false;
		AudioListener.pause = false;
	}

	public void ResumeButton ()
	{
		
		audio.volume = pgd.sfxVol/50;
		audio.Play();
		PauseCanvas.GetComponent<MenuOpen> ().Close ();
		EnableDisablePauseMenu(false);
		waitingForResumeClose = true;
	}

	private void EnableDisablePauseMenu (bool enable)
	{
		musicBar.enabled = enable;
		soundBar.enabled = enable;
		quitButton.enabled = enable;
		resumeButton.enabled = enable;

	}

	private void FinishQuit ()
	{
		
		QuitCanvas.enabled = false;
		EnableDisablePauseMenu (true);
	}

	public void QuitButton ()
	{
		
		audio.volume = pgd.sfxVol/50;
		audio.Play();
		QuitCanvas.enabled = true;
		EnableDisablePauseMenu (false);
		QuitCanvas.GetComponent<MenuOpen> ().Open ();
	}

	public void YesButton ()
	{
		audio.volume = pgd.sfxVol/50;
		audio.Play();
		Time.timeScale = 1;
		AudioListener.pause = false;
		Application.LoadLevel ("MainMenu");
	}

	public void NoButton ()
	{
		
		audio.volume = pgd.sfxVol/50;
		audio.Play();
		QuitCanvas.GetComponent<MenuOpen> ().Close ();
		waitingForQuitClose = true;
	}

   
}
