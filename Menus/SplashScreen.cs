using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SplashScreen : MonoBehaviour
{

	public Image splashScreen;
	public float speed;
	bool fadeOut;
	public float waitTimer = 6;

	public MenuAnimationController menuAnim;

	// Use this for initialization
	void Start ()
	{
		splashScreen.color = Color.black;
		fadeOut = false;
		menuAnim.gameObject.SetActive(false);
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		waitTimer -= Time.deltaTime;
		Color tempColor = splashScreen.color;
		float tempSpeed = speed;
		if (fadeOut) {
			tempSpeed = -tempSpeed;
		}
		
		tempColor.r += (Time.deltaTime * tempSpeed);
		tempColor.b += (Time.deltaTime * tempSpeed);
		tempColor.g += (Time.deltaTime * tempSpeed);
		if (tempColor.r >= 1 && waitTimer <= 2) {
			tempColor.r = 1;
			tempColor.b = 1;
			tempColor.g = 1;
			fadeOut = true;
		}
		if(waitTimer <= 0){
			//this.GetComponent<Canvas>().enabled = false;
			tempColor.a += (Time.deltaTime * tempSpeed * 2);

		}
		
		splashScreen.color = tempColor;
		if(splashScreen.color.a <= 0){
			FinishSplash();
		}


	}

	public void FinishSplash(){
		this.GetComponent<Canvas>().enabled = false;
		menuAnim.gameObject.SetActive(true);
		menuAnim.StartStar();
		this.GetComponent<SplashScreen>().enabled = false;
	}
}
