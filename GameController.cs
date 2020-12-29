using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class GameController : MonoBehaviour
{

	public PersistingGameData pgd;
	public PlayerController pc;
	public SkillUps skills;
	public TheAverage average;
	public TheTank tank;
	public TheSniper sniper;
	public TheSpeedster speedster;
	public List<Wave> Waves;
	public int currentWave;
	public int currentGroup;
    public bool debugBoss;
	private enum WaveState
	{
		waveComplete,
		waveStart,
		activeWave}
	;
	private WaveState waveState;
	private float stateTimer;

	//public bool activateWave;

	public List<EnemyBase> LiveEnemies;
	public List<EnemyBase> LiveShields;
	public Camera miniMapCam;
	public Text waveText;
	public Image waveBackground;
	public Canvas waveCanvas;
	public bool startGame;
	public bool finishedOp;
	public GameObject mainCam;

    private float opPCSpeed = 5;

	// Use this for initialization
	void Awake ()
	{
		//currentWave = -1;

		waveState = WaveState.waveStart;
		stateTimer = 5;
		waveCanvas.enabled = false;
		//skills = GameObject.Find ("SkillUpCanvas").GetComponent<SkillUps> ();
		pgd = GameObject.Find ("PersistingGameData").GetComponent<PersistingGameData> ();
		if (pgd.shipType == "Average") {
			pc = Instantiate (average) as PlayerController;
			skills.HomingShot ();

		} else if (pgd.shipType == "Tank") {
			pc = Instantiate (tank) as PlayerController;
			skills.Explosive ();
		} else if (pgd.shipType == "Sniper") {
			pc = Instantiate (sniper) as PlayerController;
			pc.IncreasePierce ();
		} else if (pgd.shipType == "Speedster") {
			pc = Instantiate (speedster) as PlayerController;
			pc.guns = 2;
		}
		var tempPos = Vector3.zero;
		tempPos.x = -12;
		pc.transform.position = tempPos;
		pc.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
		foreach (var booster in pc.shipBoosters) {
			booster.enabled = true;
		}
		AudioSource boosterAudio = pc.shipBoosters[0].GetComponent<AudioSource>();	
		boosterAudio.volume = pgd.sfxVol/300;
		boosterAudio.Play();
        if (debugBoss)
        {
            currentWave = 9;
        }
		//pc.enabled = false;
	
	}
	
	// Update is called once per frame

	void Update ()
	{

		if (!pc.isDead && finishedOp) {
			if (waveState == WaveState.activeWave) {
				Waves [currentWave].isActiveWave = true;
				//Debug.Log("currentWave" + currentWave);
				if (Waves [currentWave].WaveTick ()) {
					stateTimer = 5;
					waveState = WaveState.waveComplete;
				}
			} else if (waveState == WaveState.waveComplete) {
				WaveComplete ();
			} else if (waveState == WaveState.waveStart) {
				WaveStart ();
			}
			stateTimer -= Time.deltaTime;
		} else {
			waveText.enabled = false;
			waveBackground.enabled = false;
		}

		if(!finishedOp){
			mainCam.transform.position = new Vector3(0, 0, -20);
			var tempPos = pc.transform.position;
			tempPos += pc.transform.up * Time.deltaTime * opPCSpeed;
			pc.transform.position = tempPos;

            if (tempPos.x >= 0){
				tempPos.x = 0;
				finishedOp = true;
				pc.enabled = true;
				mainCam.GetComponent<MainCamera>().enabled = true;
				foreach (var booster in pc.shipBoosters) {
					booster.enabled = false;
				}
                
                pc.shipBoosters[0].GetComponent<AudioSource>().Stop();
			}
            if(tempPos.x >= -5 && opPCSpeed > 1.5f)
            {

                opPCSpeed -= Time.deltaTime * 3;
            }
		}

	}

	private void WaveComplete ()
	{
		waveText.text = "Wave " + (currentWave + 1) + " complete";
		waveCanvas.enabled = true;
		var tempScale = waveBackground.rectTransform.localScale;
		if (stateTimer > 0.5f) {
			tempScale.x = 5 - (stateTimer);
			if (tempScale.x > 1) {
				tempScale.x = 1;
			}
		} else {
			tempScale.x = stateTimer * 2;
		}
		waveBackground.rectTransform.localScale = tempScale;
		if (stateTimer <= 0 && currentWave < Waves.Count - 1) {
			waveState = WaveState.waveStart;
			currentWave++;
			stateTimer = 5;
		}

	}

	private void WaveStart ()
	{

		if (startGame) {
			waveText.text = "Wave " + (currentWave + 1);
			waveCanvas.enabled = true;
			var tempScale = waveBackground.rectTransform.localScale;
			if (stateTimer > 0.5f) {
				tempScale.x = 5 - (stateTimer);
				if (tempScale.x > 1) {
					tempScale.x = 1;
				}
			} else {
				tempScale.x = stateTimer * 2;
			}
			waveBackground.rectTransform.localScale = tempScale;
			if (stateTimer <= 0) {
				waveState = WaveState.activeWave;
				waveCanvas.enabled = false;
			}
		}
	}

	private void CountEnemy ()
	{
	

		int waveCounter = 0;
			foreach (var wave in Waves) {
			waveCounter++;
			int charger = 0, trooper = 0, triplicate = 0, bulk = 0, cluster = 0, artillery = 0, shield = 0, dragger = 0, dreadnaught = 0;

				foreach (var group in wave.Groups) {
					foreach (var test in group.enemies) {

					
						if (test == Group.EnemyType.chargerE) {
							charger++;
						} else if (test == Group.EnemyType.trooperE) {
							trooper++;
						} else if (test == Group.EnemyType.triplicateE) {
							triplicate++;
						} else if (test == Group.EnemyType.artilleryE) {
							artillery++;
						} else if (test == Group.EnemyType.bulkE) {
							bulk++;
						} else if (test == Group.EnemyType.clusterE) {
							cluster++;
						} else if (test == Group.EnemyType.draggerE) {
							dragger++;
						} else if (test == Group.EnemyType.shielderE) {
							shield++;
						} else {
							dreadnaught++;
						}

					}
				}
			
			Debug.Log ("Wave #: " + waveCounter);
			Debug.Log ("Charger: " + charger * 50);
			Debug.Log ("Trooper: " + trooper * 50);
			Debug.Log ("triplicate: " + triplicate * 25);
			Debug.Log ("artillery: " + artillery * 75);
			Debug.Log ("bulk: " + bulk * 75);
			Debug.Log ("cluster: " + cluster * 75);
			Debug.Log ("dragger: " + dragger * 75);
			Debug.Log ("shield: " + shield * 75);
			Debug.Log ("dreadnaught: " + dreadnaught * 200);
			}
		


	}

	
	private IEnumerator WaveText(){
		waveText.text = "Wave " + currentWave + "complete";
		waveText.enabled = true;
		Color tempColor = waveText.color;
		tempColor.a = 0;
		waveText.color = tempColor; 
		float i = 0;
		for (i = 0; i < 3; i += Time.deltaTime) {
			tempColor.a = i * 3;
			waveText.color = tempColor;
			if(i >= 2){
				//Debug.Log(i);
				waveText.text = "START";
			}
			yield return null;
		}
		waveState = WaveState.activeWave;
		waveText.enabled = false;
		currentWave++;
		
		yield return null;
	}
    

}
