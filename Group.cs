using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Group : MonoBehaviour {


	public List<GameObject> Spawners;
	private int currentEnemy;
	public GameController gc; 
	public bool isActiveGroup;
	private float height, width;
	private EnemyReferences enemyReference;

	public enum EnemyType{
		chargerE, trooperE, bulkE, artilleryE, clusterE, draggerE, dreadnaughtE, shielderE, triplicateE, BossE
	}
	public List<EnemyType> enemies;


	// Use this for initialization
	void Start () {
		enemyReference = GameObject.Find("EnemyReference").GetComponent<EnemyReferences>();
		gc = GameObject.Find ("GameController").GetComponent<GameController> ();
		height = 2*gc.miniMapCam.orthographicSize + 15;
		width = height*gc.miniMapCam.aspect + 15;

	}
	


	public void StartGroup(){
		isActiveGroup = true;
	}


	public bool NextEnemy(){

		if (currentEnemy != enemies.Count) {
			GameObject tempEnemy = Instantiate(ExchangeEnemy(enemies[currentEnemy]), gc.pc.transform.position + SpawnLocation(), this.transform.rotation) as GameObject;
			
			Debug.Log(tempEnemy.transform.position);
			if(ExchangeEnemy(enemies[currentEnemy]).name == "Shielder"){
				gc.LiveShields.Add(tempEnemy.GetComponent<EnemyBase>());
			}
			gc.LiveEnemies.Add(tempEnemy.GetComponent<EnemyBase>());
			currentEnemy++;
			NextEnemy();
		}

		if (currentEnemy == enemies.Count) {
			isActiveGroup = false;
			return true;
		} else {
			return false;
		}

	}

	private Vector3 SpawnLocation(){

		Vector3 spawnLocation = new Vector3 ();
		int spawnType = Random.Range(0, 3);

		if (spawnType == 0) {
			spawnLocation.y = height/2;
			spawnLocation.x = (Random.value * width) - (width/2);
		} else if (spawnType == 1) {
			spawnLocation.y = -height/2;
			spawnLocation.x = (Random.value * width) - (width/2);
		} else if (spawnType == 2) {
			spawnLocation.x = width/2;
			spawnLocation.y = (Random.value * height) - (height/2);
		} else {
			spawnLocation.x = -width/2;
			spawnLocation.y = (Random.value * height) - (height/2);
		}

		return spawnLocation;


	}

	private GameObject ExchangeEnemy(EnemyType enemyType){
		if (enemyType == EnemyType.chargerE) {
			return enemyReference.charger;
		} 
		else if (enemyType == EnemyType.trooperE) {
			return enemyReference.trooper;
		}
		else if (enemyType == EnemyType.triplicateE) {
			return enemyReference.triplicate;
		}
		else if (enemyType == EnemyType.artilleryE) {
			return enemyReference.artillery;
		}
		else if (enemyType == EnemyType.bulkE) {
			return enemyReference.bulk;
		}
		else if (enemyType == EnemyType.clusterE) {
			return enemyReference.cluster;
		}
		else if (enemyType == EnemyType.draggerE) {
			return enemyReference.dragger;
		}
		else if (enemyType == EnemyType.shielderE) {
			return enemyReference.shielder;
		}
		else if(enemyType == EnemyType.dreadnaughtE){
			return enemyReference.dreadnaught;
		}
        else
        {
            return enemyReference.boss;
        }
	}

}
