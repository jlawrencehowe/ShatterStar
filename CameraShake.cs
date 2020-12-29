using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CameraShake : MonoBehaviour {

	private Vector3 startPos, newLoc, prevLoc;
	public float shakeDecay = 0.05f;
	public float shakeIntensity;
	public float shakeSpeed;
	private float distance;
	private bool shaking = false;
	private RectTransform rectTransform;
	private Animator anim;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		distance = 0;
		rectTransform = GetComponent<RectTransform>();
		startPos = anim.rootPosition;
		prevLoc = startPos;
		newLoc = startPos;

	}
	
	// Update is called once per frame
	void Update () {
		if(shaking){
			//Debug.Log("test");
			shakeIntensity -= (Time.deltaTime * shakeDecay);
			distance += Time.deltaTime * shakeSpeed;
			anim.rootPosition = Vector2.Lerp(prevLoc, newLoc, distance);
			if(distance >= 1){
				distance = 0;
				NewLocation();
			}
			if(shakeIntensity <= 0){
				shakeIntensity = 0;
				shaking = false;
				distance = 0;
				prevLoc = anim.rootPosition;
			}
		}
		else{
			anim.rootPosition = Vector3.Lerp(prevLoc, startPos, distance);
			distance += Time.deltaTime * shakeSpeed;
		}
		
		anim.rootPosition = new Vector3(anim.rootPosition.x, anim.rootPosition.y, -10);

	}

	public void ShakeCamera(){
		shakeIntensity += 0.3f;
		if(shakeIntensity > 0.4f){
			shakeIntensity = 0.4f;
		}
		shaking = true;
	}

	public void MiniShake(){
		
		if(shakeIntensity < 0.05f){
			shakeIntensity = 0.05f;
		}
		shaking = true;
	}

	private void NewLocation(){
		prevLoc = newLoc;
		newLoc.x = Random.Range(startPos.x - shakeIntensity, startPos.x + shakeIntensity);
		newLoc.y =	Random.Range(startPos.y - shakeIntensity, startPos.y + shakeIntensity);
	}
}
