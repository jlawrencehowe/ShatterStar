using UnityEngine;
using System.Collections;

public class TheTank : PlayerController {

	// Use this for initialization
	public override void Start () {
		base.Start ();
		health = 1000;
		maxHealth = 1000;
		
		shield = 0;
		maxShield = 0;
		
		bulletDamage = 200;
		fireSpeed = 1f;
		bulletSpeed = 8;
		bulletDecay = 1f;
		
		acceleration = 200;
		maxSpeed = 3.0f;
		jsMovement = GameObject.Find ("JoyCircle").GetComponent<JoyStick> ();
		isShield = false;


	}
	public override void SetUpSecondGun ()
	{
		gun1.position = new Vector3(0.124f, 0.386f, 0);
		gun2.position = new Vector3(-0.124f, 0.386f, 0);
		
	}

}
