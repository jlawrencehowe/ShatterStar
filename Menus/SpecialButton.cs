using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpecialButton : MonoBehaviour {

	private GameController gc;
	public int abilityNum;
	public Image cdImage;
	public Image buttonImage;
	public bool triggerEnabled = true;
	public float maxCD;
	public float abilityCD;
	public Sprite overCharge, ironCurtain, railGun, timeSlow;


	// Use this for initialization
	void Start () {

		gc = GameObject.Find ("GameController").GetComponent<GameController> ();
		if (gc.pgd.shipType == "Average") {
			SetAbility(1);
		}
		else if(gc.pgd.shipType == "Tank") {
			SetAbility(2);
		}
		else if(gc.pgd.shipType == "Sniper") {
			SetAbility(3);
		}
		else if(gc.pgd.shipType == "Speedster") {
			SetAbility(4);
		}
	
	}
	
	// Update is called once per frame
	void Update () {
		/*Vector3 tempScale = cdImage.rectTransform.localScale;
		if (gc.pc.abilityCD != 0 && gc.pc.maxCD != 0) {
			tempScale.y = gc.pc.abilityCD / gc.pc.maxCD;
		}
		else {
			tempScale.y = 0;
		}
		if (tempScale.y < 0) {
			tempScale.y = 0;
		}

		cdImage.rectTransform.localScale = tempScale;*/
		abilityCD -= Time.deltaTime;

		cdImage.fillAmount = abilityCD/maxCD;
	
	}

	public void ButtonDown(){
		if (abilityCD <= 0 && triggerEnabled && gc.finishedOp) {
			gc.pc.SpecialMove(abilityNum);
			abilityCD = maxCD;
		}
	}

	public void SetAbility(int ability){
		abilityNum = ability;
		if(abilityNum == 1){
			buttonImage.sprite = overCharge;
			maxCD = 120;
		}
		else if(abilityNum == 2){
			maxCD = 180;
			buttonImage.sprite = ironCurtain;
		}
		else if(abilityNum == 3){
			maxCD = 90;
			buttonImage.sprite = railGun;
		}
		else if(abilityNum == 4){
			maxCD = 180;
			buttonImage.sprite = timeSlow;
		}
	}
}
