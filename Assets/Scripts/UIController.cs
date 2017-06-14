using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

	public static UIController UIC;

	public GameObject gameOverPanel;
	public GameObject winPanel;
	public GameObject namingPanel;
	public InputField cityNamingInputField;
	public GameObject cityNamingErrorText;


	public Text peasantAffinityText;
	public Text nobleAffinityText;
	public Text clergyAffinityText;
	public Text royalTreasuryText;

	public Text dateText;
	public Slider timerSlider;
	public Text eventText;
	public Text cityNameText;

	public GameObject[] buttonList;
	public GameObject[] arrowsList;

	private GameController GC;
	private bool gameOver;
	private bool gameStarted;
	private float timerDecreaseRate;
	private bool discontent;
	private int year;
	private int yearToWin;
	private float timeToWin;
	private float timeYearStarted;


	void Awake(){
		if (UIC == null) {
			UIC = this;
		} else {
			Destroy (this);
		}
	}

	void Start () {
		GC = GameController.gameController;
		RefreshUI ();
		gameOver = false;
		gameStarted = false;
		discontent = false;
		DisableArrows ();
		year = GC.GetStartYear ();
		yearToWin = year + GC.GetYearsTillRetirement ();
		dateText.text = "Year: " + year.ToString();
	}

	void FixedUpdate(){
		if (gameStarted && !gameOver){
			timerSlider.value -= timerDecreaseRate;
			if (timerSlider.value <= 0) {
				if (!discontent) {
					GC.StartDiscontention ();
				}
				discontent = true;
				RefreshAffinities ();
				if (!CheckAffinities ()) {
					gameOver = true;
					DisplayGameOverEvent ();
				}
			}

			if (Time.time - timeYearStarted >= timeToWin / 50) {
				IncrementYear ();
				timeYearStarted = Time.time;
				if (year == yearToWin) {
					DisplayWinEvent ();
				}
			}
		}
	}

	public void StartGame(){
		gameStarted = true;
		timeToWin = GC.GetTimeToWin ();
		timeYearStarted = Time.time;
	}

	public void RefreshUI(){
		RefreshAffinities ();
		if (CheckAffinities ()) {
			ReloadEventPanel ();
			RestartTimer ();
			GC.StopDiscontention ();
			discontent = false;
		} else {
			gameOver = true;
			DisplayGameOverEvent ();
		}
	}
		

	private void RefreshAffinities(){
		peasantAffinityText.text = "Peasant Mood: ";
		nobleAffinityText.text = "Noble Mood: ";
		clergyAffinityText.text = "Clergy Mood: ";
		royalTreasuryText.text = "Royal Treasury: " + GC.GetRoyalTreasury ();

		RefreshAffinity (peasantAffinityText, GC.GetPeasantAffinity());
		RefreshAffinity (nobleAffinityText, GC.GetNobleAffinity());
		RefreshAffinity (clergyAffinityText, GC.GetClergyAffinity());

	}

	private void RefreshAffinity(Text textGameObject, int affinity){
		if (affinity <= 0) {
			textGameObject.text += "Openly Hostile";
		} else if (affinity < 20) {
			textGameObject.text += "Extremely Angry";
		} else if (affinity < 40) {
			textGameObject.text += "Unhappy";
		} else if (affinity < 60) {
			textGameObject.text += "Neutral";
		} else if (affinity < 80) {
			textGameObject.text += "Happy";
		} else if (affinity < 100) {
			textGameObject.text += "Extremely Pleased";
		} else if (affinity == 100) {
			textGameObject.text += "Utterly Euphoric";
		}
	}
		

	private void ReloadEventPanel(){
		GameEvent gameEvent = GC.ChooseRandomGameEvent ();
		eventText.text = gameEvent.eventMessage;
		for (int i = 0; i < buttonList.Length; ++i) {
			buttonList [i].GetComponentInChildren<Text> ().text = "";
			buttonList [i].SetActive (false);
		}
		for (int i = 0; i < gameEvent.eventOptions.Count; ++i) {
			buttonList [i].SetActive (true);
			buttonList [i].GetComponentInChildren<Text> ().text = gameEvent.eventOptions [i].choiceMessage;

		}
	}

	private void RestartTimer(){
		timerSlider.value = 100;
		timerDecreaseRate = GC.GetTimerDecreaseRate ();
	}

	private bool CheckAffinities(){
		bool[] affinityArray =  GC.AreAffinitiesAcceptable ();

		foreach (bool affinity in affinityArray) {
			if (affinity == false) {
				return false;
			}
		}
		return true;
	}

	private void DisplayGameOverEvent(){
		eventText.text = "Revolution!";
		for (int i = 0; i < buttonList.Length; ++i) {
			buttonList [i].GetComponentInChildren<Text> ().text = "";
			buttonList [i].SetActive (false);
		}
		buttonList [0].SetActive (true);
		buttonList [0].GetComponentInChildren<Text> ().text = "Continue";
	}
		

	public void Option1Chosen(){
		if (gameOver) {
			DisplayGameOverPanel ();
		} else if (gameStarted){
			GC.ApplyOptionEffects (0);
			RefreshArrows (0);
			RefreshUI ();

		}
	}

	public void Option2Chosen(){
		if (gameOver) {
			DisplayWinPanel ();
		} else if (gameStarted){
			GC.ApplyOptionEffects (1);
			RefreshArrows (1);
			RefreshUI ();
		}

	}

	public void Option3Chosen(){
		if (gameStarted) {
			GC.ApplyOptionEffects (2);
			RefreshArrows (2);
			RefreshUI ();
		}


	}

	public void Option4Chosen(){
		if (gameStarted) {
			GC.ApplyOptionEffects (3);
			RefreshArrows (3);
			RefreshUI ();
		}

	}

	private void DisableArrows(){
		foreach (GameObject go in arrowsList) {
			go.SetActive (false);
		}
	}

	private void RefreshArrows(int option){
		DisableArrows ();

		int[] optionEffects = GC.GetOptionEffects (option);

		for (int i = 0; i < optionEffects.Length; ++i) {
			if (optionEffects [i] > 0) {
				arrowsList [2*i].SetActive (true);
			} else if (optionEffects [i] < 0) {
				arrowsList [(2*i) + 1].SetActive (true);
			}
		}
	}

	public void IncrementYear(){
		++year;
		dateText.text = "Year: " + year.ToString();
	}

	public void DisplayGameOverPanel(){
		gameOverPanel.SetActive (true);
	}
		
	public void DisplayWinPanel(){
		winPanel.SetActive (true);
	}

	public void DisplayWinEvent(){
		gameOver = true;
		eventText.text = "Congratulations, you have ruled for 50 years! You can now retire.";
		for (int i = 0; i < buttonList.Length; ++i) {
			buttonList [i].GetComponentInChildren<Text> ().text = "";
			buttonList [i].SetActive (false);
		}
			buttonList [1].SetActive (true);
		buttonList [1].GetComponentInChildren<Text> ().text = "Continue";
	}

	public void SubmitName(){
		if (cityNamingInputField.text.Equals ("~ResetPlayerPrefs")) {
			PlayerPrefs.DeleteAll ();
		} else if (PlayerPrefs.HasKey (cityNamingInputField.text)) {
			cityNamingErrorText.SetActive (true);
		} else {
			PlayerPrefs.SetInt (cityNamingInputField.text, 0);
			namingPanel.SetActive (false);
			cityNameText.text = cityNamingInputField.text;
			StartGame ();
		}
	}
}
