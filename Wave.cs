using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Wave : MonoBehaviour {
	
	public List<Group> Groups;
	public List<int> timers;
	public int currentGroup;
	public float groupTimer;

	public bool isActiveWave;
	public GameController gc; 

	// Use this for initialization
	void Start () {
		
		gc = GameObject.Find ("GameController").GetComponent<GameController> ();

	}
	
	// Update is called once per frame
	void Update () {


	
	}

	public bool WaveTick(){
		if (isActiveWave) {
			groupTimer -= Time.deltaTime;
			
			
			if ((groupTimer <= 0 || gc.LiveEnemies.Count == 0) && currentGroup != Groups.Count) {
				Groups [currentGroup].StartGroup ();
				if (Groups [currentGroup].NextEnemy ()) {
					currentGroup++;
					if(currentGroup != Groups.Count){
						groupTimer = timers [currentGroup];
					}
				}
			}
			
			if(currentGroup == Groups.Count && gc.LiveEnemies.Count == 0){
				isActiveWave = false;
				return true;
			}
			else{
				return false;
			}
			
		}

		return false;
	}
}
