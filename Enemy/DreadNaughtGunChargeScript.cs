using UnityEngine;
using System.Collections;

public class DreadNaughtGunChargeScript : MonoBehaviour {


	public Dreadnaught dread;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void CreateGunParticles(){
		dread.CreateGunParticles();
	}
}
