using UnityEngine;
using System.Collections;

public class TheSniper : PlayerController {

	// Use this for initialization
	public override void Start () {
		base.Start ();
		health = 750;
		maxHealth = 750;
		
		shield = 100;
		maxShield = 100;
		
		bulletDamage = 150;
		fireSpeed = 0.75f;
		bulletSpeed = 15;
		bulletDecay = 1.5f;
		
		acceleration = 300;
		maxSpeed = 4.0f;
		jsMovement = GameObject.Find ("JoyCircle").GetComponent<JoyStick> ();
		isShield = false;
		IncreasePierce();
	}
	public override void SetUpSecondGun ()
	{
		gun1.position = new Vector3(-0.061f, 0.375f, 0);
		gun2.position = new Vector3(0.061f, 0.375f, 0);
	}

}
