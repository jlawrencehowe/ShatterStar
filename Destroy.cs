﻿using UnityEngine;
using System.Collections;

public class Destroy : MonoBehaviour {

	public float lifeTime;

	// Use this for initialization
	void Start () {
		
		Destroy(gameObject, lifeTime);
	}
	
	// Update is called once per frame
	void Update () {

	}
}
