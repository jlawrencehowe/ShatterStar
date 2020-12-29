using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FireButton : MonoBehaviour {

	public PlayerController pc;
	private GameController gc;
	private bool isTouched = false;
	private Image button;

	// Use this for initialization
	void Start () {
		
		gc = GameObject.Find ("GameController").GetComponent<GameController> ();
		button = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {

		if(isTouched && gc.finishedOp){
			gc.pc.PlayerFire();
		}
	
	}

	public void TouhcExit(){
		isTouched = false;
		button.color = new Color32(255,255,255,214);

	}
	
	
	public void TouchEnter(){
		isTouched = true;
		button.color = new Color32(255,255,255,150);
	}

}
