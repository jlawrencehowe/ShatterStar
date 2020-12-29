using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadingScreen : MonoBehaviour {

	private RectTransform selectedShip;
	
	private PersistingGameData pgd;
	public Camera mainCam;

	public enum LoadingState { idle, selected, loading, finished};

	public LoadingState loadingState = LoadingState.idle;

	public Canvas selectShipCanvas;
	private Canvas scrollingCanvas;
	private ScrollingLoadingScreen scrollingScreen;
	public Canvas mainCanvas;
	public GameObject average, tank, speedster, sniper;
	private RectTransform characterSelectShip;
	public Image loadingBar;
	private bool loadTest = false;


	// Use this for initialization
	void Start () {
		this.enabled = false;
		GetComponent<Canvas>().enabled = false;
		scrollingCanvas = GetComponent<Canvas>();
		scrollingScreen = GetComponent<ScrollingLoadingScreen>();
		pgd = GameObject.Find ("PersistingGameData").GetComponent<PersistingGameData> ();
	}
	
	// Update is called once per frame
	void Update () {
	
		if(loadingState == LoadingState.selected){
			characterSelectShip.position += (characterSelectShip.up * Time.deltaTime * 500);
			if(characterSelectShip.position.x > (mainCam.pixelWidth + 20)){
				GetComponent<Canvas>().enabled = true;
				loadingState = LoadingState.loading;
				selectShipCanvas.enabled = false;
				scrollingCanvas.enabled = true;
				scrollingScreen.enabled = true;
				mainCanvas.enabled = false;
				selectedShip.localPosition = Vector3.zero;
				StartCoroutine(LoadScene());

			}
		}
		else if(loadingState == LoadingState.loading){

		}
		else{
			//ship flies off
               			//load level
			selectedShip.position += (selectedShip.up * Time.deltaTime * 500);
			foreach (RectTransform child in selectedShip)
			{
				child.sizeDelta = new Vector2(5f, 7f);
			}

		}

	}

	IEnumerator LoadScene()
	{
		yield return null;
		
		AsyncOperation asyncOperation = Application.LoadLevelAsync("Test");

		asyncOperation.allowSceneActivation = false;
		while (!asyncOperation.isDone)
		{
			//Output the current progress
			loadingBar.fillAmount = (asyncOperation.progress + 0.1f);
			
			// Check if the load has finished
			if (asyncOperation.progress >= 0.9f)
			{
				loadingState = LoadingState.finished;
			}
			if(selectedShip.position.x > (mainCam.pixelWidth + 35)){
				asyncOperation.allowSceneActivation = true;
			}
			
			yield return null;
		}
	}

	public void StartLoading(RectTransform character){
		characterSelectShip = character;
		GameObject temp;
		if(pgd.shipType == "Average"){
			temp = Instantiate(average, Vector3.zero, Quaternion.Euler(new Vector3(0, 0, 270))) as GameObject;
		}
		else if(pgd.shipType == "Tank"){
			temp = Instantiate(tank, Vector3.zero, Quaternion.Euler(new Vector3(0, 0, 270))) as GameObject;
		}
		else if(pgd.shipType == "Sniper"){
			temp = Instantiate(sniper, Vector3.zero, Quaternion.Euler(new Vector3(0, 0, 270))) as GameObject;
		}
		else{
			temp = Instantiate(speedster, Vector3.zero, Quaternion.Euler(new Vector3(0, 0, 270))) as GameObject;
		}
		selectedShip = temp.GetComponent<RectTransform>();
		selectedShip.SetParent(this.transform);
		loadingState = LoadingState.selected;
        selectedShip.localScale = new Vector3(1, 1, 1);
		foreach (Transform child in characterSelectShip.transform)
		{
			child.GetComponent<Image>().enabled = true;
		}
	
	}
}
