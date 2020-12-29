using UnityEngine;
using System.Collections;

public class SpawnLocation : MonoBehaviour {

	public Camera miniMapCam;
	private Vector2 location;
	private GameController gc;
	public int locationNumber;
	//1 top left, 2 top right, 3 bottom left, 4 bottom right, 5 top center, 6 bottom center

	// Use this for initialization
	void Start () {
		
		float vertExtent = miniMapCam.orthographicSize;    
		float horzExtent = vertExtent * Screen.width / Screen.height;

		gc = GameObject.Find ("GameController").GetComponent<GameController> ();
		if (locationNumber == 1) {

		}
	
	}
	
	// Update is called once per frame
	void Update () {


	
	}
}
