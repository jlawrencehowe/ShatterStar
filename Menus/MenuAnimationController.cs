using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuAnimationController : MonoBehaviour {

	public Animator starAnim;
	public Image whiteLight;
	private bool triggerWhiteLight = false;
	public float speed, colorSpeed;

	public Canvas mainMenu;

	// Use this for initialization
	void Start () {



	}

	public void StartStar(){
		starAnim.SetTrigger("Start");
	}
	
	// Update is called once per frame
	void Update () {

		if(triggerWhiteLight){
			var tempScale =	whiteLight.rectTransform.localScale;
			tempScale.x += speed * Time.deltaTime;
			tempScale.y += speed * Time.deltaTime;
			whiteLight.rectTransform.localScale = tempScale;
		}

		if(whiteLight.rectTransform.localScale.x >= 1){
			Ending();
		}
	
	}

	public void TriggerWhiteLight(){
		triggerWhiteLight = true;
	}

	public void Ending(){

		mainMenu.enabled = true;
		triggerWhiteLight = false;
		var tempColor = whiteLight.color;
		tempColor.a -= Time.deltaTime * colorSpeed;
		starAnim.gameObject.SetActive(false);
		whiteLight.color = tempColor;
		if(tempColor.a <= 0){
			this.gameObject.SetActive(false);
			
		}
	}

	public void QuickEnd(){
		mainMenu.enabled = true;
		triggerWhiteLight = false;
		var tempColor = whiteLight.color;
		tempColor.a = 0;
		starAnim.gameObject.SetActive(false);
		whiteLight.color = tempColor;
		this.gameObject.SetActive(false);
			
	}
}
