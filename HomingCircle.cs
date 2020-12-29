using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HomingCircle : MonoBehaviour {

	public List<GameObject> Enemies;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {


	
	}


	public void OnTriggerEnter2D(Collider2D col){

		if (col.gameObject.tag == "Enemy") {
			Enemies.Add(col.gameObject);
		}


	}

	public void OnTriggerExit2D(Collider2D col){

		if (col.gameObject.tag == "Enemy") {
			Enemies.Remove(col.gameObject);
		}

	}
}
