using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public static GameController gameController;		//singleton
	public EventList[] eventLists;
	public WorldEventsPairList worldEventsPairList;
	public float timeToWin;
	public int startYear;
	public int yearsTillRetirement;

	public int minStartingAffinity;
	public int maxStartingAffinity;
	public int minStartingMoney;
	public int maxStartingMoney;
	public float maxTimerDecreaseRate;
	public float minTimerDecreaseRate;
	public float discontentionMultiplier;

	private int peasantAffinity;
	private int nobleAffinity;
	private int clergyAffinity;
	private int royalTreasury;

	private GameEvent currentGameEvent;

	void Awake () {
		if (gameController == null) { 
			gameController = this;
			peasantAffinity = Random.Range (minStartingAffinity, maxStartingAffinity);
			nobleAffinity = Random.Range (minStartingAffinity, maxStartingAffinity);
			clergyAffinity = Random.Range (minStartingAffinity, maxStartingAffinity);
			royalTreasury = Random.Range (minStartingMoney, maxStartingMoney);
		} else {
			Destroy (this);
		}
	}

	void Start() {
		foreach (EventList el in eventLists) {
			el.Reset ();
		}

	}
	//----------------------geters and seters and incrementers-------------------------

	public float GetTimeToWin() {return timeToWin;}
	public int GetStartYear() {return startYear;}
	public int GetYearsTillRetirement() {return yearsTillRetirement;}
	public int GetPeasantAffinity() {return peasantAffinity;}
	public int GetNobleAffinity() {return nobleAffinity;}
	public int GetClergyAffinity() {return clergyAffinity;}
	public int GetRoyalTreasury() {return royalTreasury;}

	public void SetPeasantAffinity (int amount) {peasantAffinity = amount;}
	public void SetNobleAffinity (int amount) {nobleAffinity = amount;}
	public void SetClergyAffinity (int amount) {clergyAffinity = amount;}
	public void SetRoyalTreasury (int amount) {royalTreasury = amount;}

	public void IncPeasantAffinity (int amount) {
		peasantAffinity += amount;
		if (peasantAffinity > 100) {
			peasantAffinity = 100;
		}
	}

	public void IncNobleAffinity (int amount) {
		nobleAffinity += amount;
		if (nobleAffinity > 100) {
			nobleAffinity = 100;
		}
	}

	public void IncClergyAffinity (int amount) {
		clergyAffinity += amount;
		if (clergyAffinity > 100) {
			clergyAffinity = 100;
		}
	}

	public void IncRoyalTreasury (int amount){
		royalTreasury += amount;
	}
		
	//-------------------------------------------------------------

	public bool[] AreAffinitiesAcceptable(){
		bool[] result = new bool[] { true, true, true, true };
		if (peasantAffinity <= 0) {
			result [0] = false;
		}
		if (nobleAffinity <= 0) {
			result [1] = false;
		}
		if (clergyAffinity <= 0) {
			result [2] = false;
		}
		if (royalTreasury <= 0) {
			result [3] = false;
		}
		return result;
	}

	public void ApplyOptionEffects(int option){
		IncPeasantAffinity (currentGameEvent.eventOptions [option].effectOnPeasant);
		IncNobleAffinity (currentGameEvent.eventOptions [option].effectOnNoble);
		IncClergyAffinity (currentGameEvent.eventOptions [option].effectOnClergy);
		IncRoyalTreasury (currentGameEvent.eventOptions [option].effectOnRoyalTreasury);
	}

	public int[] GetOptionEffects(int option){
		return new int[]{currentGameEvent.eventOptions [option].effectOnPeasant,currentGameEvent.eventOptions [option].effectOnNoble,
			currentGameEvent.eventOptions [option].effectOnClergy,currentGameEvent.eventOptions [option].effectOnRoyalTreasury};
	}

	public void DecrementAffinity(){
		peasantAffinity -= 1;
		nobleAffinity -= 1;
		clergyAffinity -= 1;
	}


	//-------------------------------------------------------------------------------------------------
	public GameEvent ChooseRandomGameEvent(){
		while (true) {
				EventList eventList = ChooseRandomEventList ();
				currentGameEvent = eventList.eventsList [Random.Range (0, eventList.eventsList.Count)];
				if (currentGameEvent.eventMessage == "" || (!currentGameEvent.repeatable && currentGameEvent.hasOccured ())) {
					continue;
				} else {
					currentGameEvent.toggleOccured ();
					return currentGameEvent;
				}
		}
	}
		

	private EventList ChooseRandomEventList(){
		float totalWeight = 0f;
		foreach (EventList el in eventLists) {
			totalWeight += el.probabilityWeight;
		}
		float chosenNumber = Random.Range (0f, totalWeight);

		foreach (EventList el in eventLists) {
			if (chosenNumber <= el.probabilityWeight) {
				return el;
			} else {
				chosenNumber -= el.probabilityWeight;
			}
		}
		print ("I done goofed");
		return new EventList ();
	}
		
	//------------------------------------------------------------------------------------------------------------
	public float GetTimerDecreaseRate(){
		float decreaseRate = minTimerDecreaseRate;
		decreaseRate = decreaseRate + ((300 - peasantAffinity - nobleAffinity - clergyAffinity) * discontentionMultiplier);
		if (decreaseRate > maxTimerDecreaseRate){
			decreaseRate = maxTimerDecreaseRate;
		}
		return decreaseRate;
	}

	public void StartDiscontention(){
		InvokeRepeating ("DecrementAffinity", 0f, 0.5f);
	}

	public void StopDiscontention(){
		CancelInvoke ();
	}
		
}
  