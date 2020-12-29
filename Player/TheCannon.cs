using UnityEngine;
using System.Collections;

public class TheCannon : PlayerController {

	// Use this for initialization
	public override void Start () {
		base.Start ();
		acceleration = 150;
		maxSpeed = 3.5f;
		fireSpeed = 0.5f;
		bulletSpeed = 10;
		bulletDamage = 20;
		health = 250;
		jsMovement = GameObject.Find ("JoyCircle").GetComponent<JoyStick> ();
		isShield = false;
		bulletDecay = 1.5f;
		isExplosiveShot = true;
	}

}
