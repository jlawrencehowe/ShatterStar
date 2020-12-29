using UnityEngine;
using System.Collections;

public class MiniMapCam : MonoBehaviour {

	
	private GameController gc;

	// Use this for initialization
	void Start () {

		gc = GameObject.Find ("GameController").GetComponent<GameController> ();
	
	}
	
	// Update is called once per frame
	void Update () {

		Vector3 tempPosition = gc.pc.transform.position;
		tempPosition.z = -50;
		transform.position = tempPosition;
	
	}
}
