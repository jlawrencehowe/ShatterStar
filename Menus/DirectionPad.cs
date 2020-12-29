using UnityEngine;
using System.Collections;

public class DirectionPad : MonoBehaviour {

	public int direction;
	public PlayerController pc;
	private bool isTouched = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		//if(isTouched)
			//pc.MovePlayer(direction);

	}

	public void TouhcExit(){
		isTouched = false;
	}


	public void TouchEnter(){
		isTouched = true;
	}
}
