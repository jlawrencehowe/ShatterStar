using UnityEngine;
using System.Collections;

public class TheAverage : PlayerController {

	// Use this for initialization
	public override void Start () {
		base.Start ();
		health = 500;
		maxHealth = 500;

		shield = 200;
		maxShield = 200;
		
		bulletDamage = 100;
		fireSpeed = 0.65f;
		bulletSpeed = 10;
		bulletDecay = 1f;

		acceleration = 350;
		maxSpeed = 3.5f;

		jsMovement = GameObject.Find ("JoyCircle").GetComponent<JoyStick> ();
		isShield = true;
		isHoming = true;
	}

	public override void SetUpSecondGun ()
	{
		gun1.position = new Vector3(-0.114f, -0.007f, 0);
		gun2.position = new Vector3(0.114f, -0.007f, 0);
	}
	

}
