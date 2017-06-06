using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public static GameController gameController;		//singleton
	public EventList eventList;


	public int minStartingAffinity;
	public int maxStartingAffinity;
	public int minStartingMoney;
	public int maxStartingMoney;


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
	
	//----------------------geters and seters and incrementers-------------------------

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


	public GameEvent ChooseRandomGameEvent(){
		while (true) {
			currentGameEvent = eventList.eventsList [Random.Range (0, eventList.eventsList.Count)];
			if (!currentGameEvent.repeatable && currentGameEvent.hasOccured()) {
				continue;
			} else {
				currentGameEvent.toggleOccured ();
				return currentGameEvent;
			}
		}
	}


	public void ApplyOptionEffects(int option){
		IncPeasantAffinity (currentGameEvent.eventOptions [option].effectOnPeasant);
		IncNobleAffinity (currentGameEvent.eventOptions [option].effectOnNoble);
		IncClergyAffinity (currentGameEvent.eventOptions [option].effectOnClergy);
		IncRoyalTreasury (currentGameEvent.eventOptions [option].effectOnRoyalTreasury);
	}
}
  