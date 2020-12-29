using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class WarpingScript : MonoBehaviour {

	public GameObject[,] backGroundImages;
	private GameController gc;
	public float imageSize;

	// Use this for initialization
	void Start () {
		backGroundImages = new GameObject[3,3];
		backGroundImages [0, 0] = GameObject.Find ("Background 0,0");
		
		backGroundImages [0, 1] = GameObject.Find ("Background 0,1");
		
		backGroundImages [0, 2] = GameObject.Find ("Background 0,2");
		
		backGroundImages [1, 0] = GameObject.Find ("Background 1,0");
		
		backGroundImages [1, 1] = GameObject.Find ("Background 1,1");
		
		backGroundImages [1, 2] = GameObject.Find ("Background 1,2");
		
		backGroundImages [2, 0] = GameObject.Find ("Background 2,0");
		
		backGroundImages [2, 1] = GameObject.Find ("Background 2,1");
		
		backGroundImages [2, 2] = GameObject.Find ("Background 2,2");
		gc = GameObject.Find ("GameController").GetComponent<GameController> ();
	
	}

	public void PrintImage(){

		for (int i = 0; i < 3; i++) {
			for (int x = 0; x < 3; x++) {
				Debug.Log("X: " + i + ", Y: " + x + ", " + backGroundImages[i, x]);
			}
		}

	}
	
	// Update is called once per frame
	void Update () {

		StartCoroutine (EnemyScan ());

		Vector2 playerPos = gc.pc.transform.position;
		Vector2 centerPos = backGroundImages [1, 1].transform.position;
		//move right
		if (playerPos.x > centerPos.x + imageSize/2) {
			Vector2 tempPos = backGroundImages[0, 0].transform.position;
			tempPos.x += (imageSize * 3);
			backGroundImages[0, 0].transform.position = tempPos;

			tempPos = backGroundImages[0, 1].transform.position;
			tempPos.x += (imageSize * 3);
			backGroundImages[0, 1].transform.position = tempPos;

			tempPos = backGroundImages[0, 2].transform.position;
			tempPos.x += (imageSize * 3);
			backGroundImages[0, 2].transform.position = tempPos;

			SwapImages(new Vector2(0,0), new Vector2(2,0));
			SwapImages(new Vector2(0,1), new Vector2(2,1));
			SwapImages(new Vector2(0,2), new Vector2(2,2));

			SwapImages(new Vector2(0,0), new Vector2(1,0));
			SwapImages(new Vector2(0,1), new Vector2(1,1));
			SwapImages(new Vector2(0,2), new Vector2(1,2));

		}

		//move left
		else if(playerPos.x < centerPos.x - imageSize/2){
			Vector2 tempPos = backGroundImages[2, 0].transform.position;
			tempPos.x -= (imageSize * 3);
			backGroundImages[2, 0].transform.position = tempPos;
			
			tempPos = backGroundImages[2, 1].transform.position;
			tempPos.x -= (imageSize * 3);
			backGroundImages[2, 1].transform.position = tempPos;
			
			tempPos = backGroundImages[2, 2].transform.position;
			tempPos.x -= (imageSize * 3);
			backGroundImages[2, 2].transform.position = tempPos;
			
			SwapImages(new Vector2(0,0), new Vector2(2,0));
			SwapImages(new Vector2(0,1), new Vector2(2,1));
			SwapImages(new Vector2(0,2), new Vector2(2,2));
			
			SwapImages(new Vector2(2,0), new Vector2(1,0));
			SwapImages(new Vector2(2,1), new Vector2(1,1));
			SwapImages(new Vector2(2,2), new Vector2(1,2));
		}

		//move up
		else if(playerPos.y > centerPos.y + imageSize/2){
			Vector2 tempPos = backGroundImages[0, 2].transform.position;
			tempPos.y += (imageSize * 3);
			backGroundImages[0, 2].transform.position = tempPos;
			
			tempPos = backGroundImages[1, 2].transform.position;
			tempPos.y += (imageSize * 3);
			backGroundImages[1, 2].transform.position = tempPos;
			
			tempPos = backGroundImages[2, 2].transform.position;
			tempPos.y += (imageSize * 3);
			backGroundImages[2, 2].transform.position = tempPos;
			
			SwapImages(new Vector2(0,2), new Vector2(0,0));
			SwapImages(new Vector2(1,2), new Vector2(1,0));
			SwapImages(new Vector2(2,2), new Vector2(2,0));
			
			SwapImages(new Vector2(0,2), new Vector2(0,1));
			SwapImages(new Vector2(1,2), new Vector2(1,1));
			SwapImages(new Vector2(2,2), new Vector2(2,1));
		}

		//move down
		else if(playerPos.y < centerPos.y - imageSize/2){
			Vector3 tempPos = backGroundImages[0, 0].transform.position;
			tempPos.y -= (imageSize * 3);
			backGroundImages[0, 0].transform.position = tempPos;
			
			tempPos = backGroundImages[1, 0].transform.position;
			tempPos.y -= (imageSize * 3);
			backGroundImages[1, 0].transform.position = tempPos;
			
			tempPos = backGroundImages[2, 0].transform.position;
			tempPos.y -= (imageSize * 3);
			backGroundImages[2, 0].transform.position = tempPos;
			
			SwapImages(new Vector2(0,0), new Vector2(0,2));
			SwapImages(new Vector2(1,0), new Vector2(1,2));
			SwapImages(new Vector2(2,0), new Vector2(2,2));
			
			SwapImages(new Vector2(0,0), new Vector2(0,1));
			SwapImages(new Vector2(1,0), new Vector2(1,1));
			SwapImages(new Vector2(2,0), new Vector2(2,1));
		}

		foreach (var background in backGroundImages) {
			Vector3 tempPos = background.transform.position;
			tempPos.z = 100;
			background.transform.position = tempPos;
		}

	}

	private void SwapImages(Vector2 a, Vector2 b){

		GameObject tempObject = backGroundImages [(int)a.x, (int)a.y];
		backGroundImages [(int)a.x, (int)a.y] = backGroundImages [(int)b.x, (int)b.y];
		backGroundImages [(int)b.x, (int)b.y] = tempObject;

	}

	private IEnumerator EnemyScan(){
		
		//Debug.Log("what");
		for (int i = 0; i < gc.LiveEnemies.Count; i++) {
			//Debug.Log(Vector2.Distance(gc.LiveEnemies[i].transform.position, gc.pc.transform.position));
			if(gc.LiveEnemies[i] != null && Vector2.Distance(gc.LiveEnemies[i].transform.position, gc.pc.transform.position) > 40){
				MoveEnemy(gc.LiveEnemies[i].gameObject);
			}

			if(i%20 == 0){
				yield return new WaitForSeconds(1f);
			}

		}

	}

	private void MoveEnemy(GameObject enemy){
		Vector3 forward = gc.pc.transform.up;
		Vector3 right = gc.pc.transform.right;
		Vector3 ppos = gc.pc.transform.position;
		Vector3 tempPos = ppos + (forward * 15) + (right * ((Random.value * 15) - 5));
		tempPos.z = enemy.transform.position.z;

		enemy.transform.position = tempPos;;
	}
}
