using UnityEngine;
using System.Collections;

public class GameUI : MonoBehaviour {

	
	public GameObject healthBar;
	public GameObject shieldBar;

	public PlayerController pc;
	public GameController gc;

	// Use this for initialization
	void Start () {
		gc = GameObject.Find ("GameController").GetComponent<GameController> ();
		healthBar = GameObject.Find("HealthBar");
		shieldBar = GameObject.Find("ShieldBar");
	
	}
	
	// Update is called once per frame
	void Update () {

		
		UpdateHealthBar ();
		if (gc.pc.maxShield != 0)
			UpdateShieldBar ();
		else {
			Vector3 tempScale = shieldBar.transform.localScale;
			tempScale.x = 0;
			shieldBar.transform.localScale = tempScale;
		}
	
	}

	private void UpdateHealthBar(){
		Vector3 tempScale = healthBar.transform.localScale;
		tempScale.x = gc.pc.GetHealth() / gc.pc.GetMaxHealth();
		healthBar.transform.localScale = tempScale;
	}
	
	private void UpdateShieldBar(){
		Vector3 tempScale = shieldBar.transform.localScale;
		tempScale.x = gc.pc.GetShield() / gc.pc.GetMaxShield();
		shieldBar.transform.localScale = tempScale;
	}
}
