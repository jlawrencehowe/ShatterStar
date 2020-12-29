using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletExplosion : MonoBehaviour {

	private float damage;
	private List<GameObject> enemies = new List<GameObject>{};
	private GameObject enemy;


	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void InitStats(GameObject enemy, float damage){
		SetDamage (damage);
		GetComponent<CircleCollider2D>().enabled = true;
		this.enemy = enemy;
	}

	public void SetDamage(float damage){

		this.damage = damage;
	}

	public void AnimFinish(){
		Destroy(gameObject);
	}

	public void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "Enemy") {
			bool isNewEnemy = true;
			for (int i = 0; i < enemies.Count; i++) {
				if(enemies[i] == col.gameObject){
					isNewEnemy = false;
					break;
				}
			}
			if(isNewEnemy){
				if(col.gameObject == enemy){
					col.GetComponent<EnemyBase>().TakeDamage(damage/2);
				}
				else{
					col.GetComponent<EnemyBase>().TakeDamage(damage);
				}
			}
			enemies.Add(col.gameObject);

		}
	}
}
