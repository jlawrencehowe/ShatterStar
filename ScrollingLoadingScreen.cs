using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class ScrollingLoadingScreen : MonoBehaviour {

	public List<RectTransform> screens;
	private int order = 1;

	// Use this for initialization
	void Start () {
		GetComponent<ScrollingLoadingScreen>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {

		for (int i = 0; i < screens.Count; i++) {
			var tempPos = screens[i].localPosition;
			tempPos.x -= Time.deltaTime * 400;
			screens[i].localPosition = tempPos;
		}
		int prev = order - 1;
		if(order == 0){
			prev = 2;
		}
		int next = order + 1;
		if(order == 2){
			next = 0;
		}
		if(screens[prev].localPosition.x <= -screens[0].rect.width){
			var tempPos = screens[next].localPosition;
			tempPos.x += screens[0].rect.width;
			screens[prev].localPosition = tempPos;
			order = next;
		}
	}
}
