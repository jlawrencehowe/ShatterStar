using UnityEngine;
using System.Collections;

public class TheSpeedster : PlayerController {

	// Use this for initialization
	public override void Start () {
		base.Start ();
		health = 300;
		maxHealth = 300;
		
		shield = 200;
		maxShield = 200;
		
		bulletDamage = 25;
		fireSpeed = 0.4f;
		bulletSpeed = 10;
		bulletDecay = 1f;
		
		acceleration = 400;
		maxSpeed = 4.5f;
		jsMovement = GameObject.Find ("JoyCircle").GetComponent<JoyStick> ();
		isShield = true;
		guns = 2;
	}

	public override void SetUpSecondGun ()
	{
		
	}


	

}
