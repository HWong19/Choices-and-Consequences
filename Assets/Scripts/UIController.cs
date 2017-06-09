using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

	public GameObject gameOverPanel;
	public Text peasantAffinityText;
	public Text nobleAffinityText;
	public Text clergyAffinityText;
	public Text royalTreasuryText;

	public Text dateText;
	public Slider timerSlider;
	public Text eventText;

	public GameObject[] buttonList;
	public GameObject[] arrowsList;

	private GameController GC;
	private bool gameOver;
	private float timerDecreaseRate;
	private bool discontent;

	void Start () {
		GC = GameController.gameController;
		RefreshUI ();
		gameOver = false;
		discontent = false;
		DisableArrows ();

	}

	void FixedUpdate(){
		if (!gameOver){
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
		}
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
		} else {
			GC.ApplyOptionEffects (0);
			RefreshArrows (0);
			RefreshUI ();

		}
	}

	public void Option2Chosen(){
		GC.ApplyOptionEffects (1);
		RefreshArrows (1);
		RefreshUI ();


	}

	public void Option3Chosen(){
		GC.ApplyOptionEffects (2);
		RefreshArrows (2);
		RefreshUI ();


	}

	public void Option4Chosen(){
		GC.ApplyOptionEffects (3);
		RefreshArrows (3);
		RefreshUI ();

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

	public void DisplayGameOverPanel(){
		gameOverPanel.SetActive (true);
	}
		
	private void RestartTimer(){
		timerSlider.value = 100;
		timerDecreaseRate = GC.GetTimerDecreaseRate ();
	}
}
