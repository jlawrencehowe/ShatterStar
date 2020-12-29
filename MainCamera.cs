using UnityEngine;
using System.Collections;

public class MainCamera : MonoBehaviour {

	private float leftRight = 1;
	private float upDown = 0.5f;
	GameController gc;

	// Use this for initialization
	void Start () {

		gc = GameObject.Find ("GameController").GetComponent<GameController> ();
		Vector3 temp = gc.pc.transform.position;
		temp.z -= 50;
		transform.position = temp;
		this.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {

		if (!gc.pc.isDead) {
			Vector3 playerPosition = gc.pc.transform.position;
			Vector3 camPos = this.transform.position;
			Vector3 tempPos = this.transform.position;
			if (playerPosition.x > camPos.x + leftRight) {
				tempPos.x = playerPosition.x - leftRight;
				this.transform.position = tempPos;
			}
			if (playerPosition.x < camPos.x - leftRight) {
				tempPos.x = playerPosition.x + leftRight;
				this.transform.position = tempPos;
			}
			if (playerPosition.y > camPos.y + upDown) {
				tempPos.y = playerPosition.y - upDown;
				this.transform.position = tempPos;
			}
			if (playerPosition.y < camPos.y - upDown) {
				tempPos.y = playerPosition.y + upDown;
				this.transform.position = tempPos;
			}
		}
	
	}
}
