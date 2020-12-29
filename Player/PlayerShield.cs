using UnityEngine;
using System.Collections;

public class PlayerShield : MonoBehaviour {

	private SpriteRenderer sr;
	protected GameController gc;

	// Use this for initialization
	void Start () {
		
		gc = GameObject.Find ("GameController").GetComponent<GameController> ();
		sr = GetComponent<SpriteRenderer> ();
	
	}
	
	// Update is called once per frame
	void Update () {
	
		sr.color = new Color (1, 1, 1, gc.pc.GetShield () / gc.pc.GetMaxShield ());

	}
}
