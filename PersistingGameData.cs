using UnityEngine;
using System.Collections;
using GoogleMobileAds.Api;


public class PersistingGameData : MonoBehaviour {

	public float musicVol = 50;
	public float sfxVol = 50;
	public string shipType;
	private static PersistingGameData pgd;
    public BannerAd bannerAd;
    public int interAdCounter = 1;

	// Use this for initialization
	void Awake () {

		DontDestroyOnLoad (gameObject);
		if(PlayerPrefs.HasKey("music")){
			musicVol = PlayerPrefs.GetFloat("music");
		}
		if(PlayerPrefs.HasKey("sound")){
			sfxVol = PlayerPrefs.GetFloat("sound");
			AudioListener.volume = sfxVol/50f;
        }
        if (PlayerPrefs.HasKey("interAdCounter") && interAdCounter != null)
        {
            interAdCounter = PlayerPrefs.GetInt("interAdCounter");
        }
        if (pgd == null) {
			pgd = this;

        } else {
			Destroy(gameObject);
		}

        if(GameObject.Find("Banner") != null)
            bannerAd = GameObject.Find("Banner").GetComponent<BannerAd>();


    }

	public void ChangeMusic(float music){
		musicVol = music;
		PlayerPrefs.SetFloat("music", music);
	}

	public void ChangeSFX(float sfx){
		sfxVol = sfx;
		PlayerPrefs.SetFloat("sound", sfx);
		AudioListener.volume = sfxVol/50f;
	}
    public void IncrementInterAdCounter()
    {
        interAdCounter++;
        PlayerPrefs.SetFloat("interAdCounter", interAdCounter);

    }
    public void ResetInterAdCounter()
    {
        interAdCounter = 0;
        PlayerPrefs.SetFloat("interAdCounter", interAdCounter);

    }

   


}
