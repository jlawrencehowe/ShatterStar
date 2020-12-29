using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShieldGenerator : MonoBehaviour {

	public List<GameObject> Enemies;
	private float shieldIncreaseAmount = 5;
	private float totalShieldSaved;
	public Shield parentShield;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (parentShield.GetIsSlowed ()) {
			totalShieldSaved += Time.deltaTime/4 * shieldIncreaseAmount;
		} else {
			totalShieldSaved += Time.deltaTime * shieldIncreaseAmount;
		}

	
	}




	public void OnTriggerEnter2D(Collider2D col){

		if (col.gameObject.tag == "Enemy" && !Enemies.Contains(col.gameObject) && col.gameObject != parentShield.gameObject) {
			Enemies.Add(col.gameObject);
			col.gameObject.GetComponent<EnemyBase>().shieldChargers++;
		}
		
		
	}

	public void OnTriggerExit2D(Collider2D col){
		
		if (col.gameObject.tag == "Enemy" && Enemies.Contains(col.gameObject) && col.gameObject != parentShield.gameObject) {
			Enemies.Remove(col.gameObject);
			if(col.gameObject.GetComponent<EnemyBase>().shieldChargers != 0){
			col.gameObject.GetComponent<EnemyBase>().shieldChargers--;
			}
		}
		
		
	}

	public void Death(){
		foreach (var enemy in Enemies) {
            
			if(enemy != null && enemy.GetComponent<EnemyBase>().shieldChargers != 0){
				enemy.GetComponent<EnemyBase>().shieldChargers--;
			}
		}
	}
}
