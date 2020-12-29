using UnityEngine;
using System.Collections;
using GoogleMobileAds.Api;

public class BannerAd : MonoBehaviour {

	
	private BannerView bannerView;
    public static BannerAd bannerAd;
    private InterstitialAd interAd;

	// Use this for initialization
	void Start () {
        /*
		#if UNITY_EDITOR
		string adUnitId = "unused";
		#else
		*/

        DontDestroyOnLoad(gameObject);
        if (bannerAd == null)
        {
            bannerAd = this;
            string adUnitId = "";
            MobileAds.Initialize(adUnitId);
            CreateBannerAd();
            CreateInterAd();

        }
        else
        {
            Destroy(gameObject);
        }



	}

  


    private void CreateBannerAd(){



		bannerView = new BannerView("", AdSize.Banner, AdPosition.Bottom);
		AdRequest request = new AdRequest.Builder().Build();
		bannerView.LoadAd(request);
		bannerView.Show();

	}

    public void DisableBannerAd()
    {

        if(bannerView != null)
        {

            bannerView.Hide();
        }

    }

    public void ShowBannerAd()
    {
        if (bannerView != null)
        {

            bannerView.Show();
        }
    }

    private void CreateInterAd()
    {
        interAd = new InterstitialAd("");
        interAd.LoadAd(new AdRequest.Builder().Build());
        interAd.OnAdClosed += LoadInterAd;
    }


    public void LoadInterAd(object sender, System.EventArgs e)
    {
        interAd.LoadAd(new AdRequest.Builder().Build());

    }


    public void ShowInterAd()
    {
        if (interAd.IsLoaded())
        {
            interAd.Show();
        }
    }




}
