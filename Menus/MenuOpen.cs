using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuOpen : MonoBehaviour {

	public float speed;
	public bool isOpening, moving;
	public Image parentImage;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if(isOpening){
			Opening ();
		}
		else{
			Closing();
		}
	
	}

	public void Close(){
		parentImage.rectTransform.localScale = new Vector3(1, 1, 1);
		isOpening = false;
		moving = true;
	}

	public void Open(){
		parentImage.rectTransform.localScale = new Vector3(0, 1, 0);
		isOpening = true;
		moving = true;
	}

	private void Opening(){

		if(moving){
			float tempSpeed = speed;
			Vector3 tempScale = parentImage.rectTransform.localScale;
			tempScale.x += (Time.unscaledDeltaTime * tempSpeed);
			tempScale.y += (Time.unscaledDeltaTime * tempSpeed * 1.25f);
			if(tempScale.x > 1){
				tempScale.x = 1;
			}
			if(tempScale.y > 1){
				tempScale.y = 1;
			}
			if(tempScale.x == 1){
				moving = false;
			}
			parentImage.rectTransform.localScale = tempScale;
		}


	}


	private void Closing(){

		if(moving){
			float tempSpeed = -speed;
			Vector3 tempScale = parentImage.rectTransform.localScale;
			tempScale.x += (Time.unscaledDeltaTime * tempSpeed);
			//tempScale.y += (Time.unscaledDeltaTime * tempSpeed * 1.25f);
			if(tempScale.x < 0){
				tempScale.x = 0;
			}
			if(tempScale.y < 0){
				tempScale.y = 0;
			}
			if(tempScale.x <= 0){
				moving = false;
			}
			parentImage.rectTransform.localScale = tempScale;
		}
	}
}
