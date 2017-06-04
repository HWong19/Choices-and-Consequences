using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public static GameController gameController;		//singleton

	private int peasantAffinity;
	private int nobleAffinity;
	private int clergyAffinity;
	private int royalTreasury;

	void Awake () {
		if (gameController == null) { 
			gameController = this;
			peasantAffinity = Random.Range (60, 80);
			nobleAffinity = Random.Range (60, 80);
			clergyAffinity = Random.Range (60, 80);
			royalTreasury = Random.Range (1000, 2000);
		} else {
			Destroy (this);
		}
	}
	
	//----------------------geters and seters and incrementers-------------------------

	public int getPeasantAffinity() {return peasantAffinity;}
	public int getNobleAffinity() {return nobleAffinity;}
	public int getClergyAffinity() {return clergyAffinity;}
	public int getRoyalTreasury() {return royalTreasury;}

	public void setPeasantAffinity (int amount) {peasantAffinity = amount;}
	public void setNobleAffinity (int amount) {nobleAffinity = amount;}
	public void setClergyAffinity (int amount) {clergyAffinity = amount;}
	public void setRoyalTreasury (int amount) {royalTreasury = amount;}

	public void incPeasantAffinity (int amount) {
		peasantAffinity += amount;
		if (peasantAffinity > 100) {
			peasantAffinity = 100;
		}
	}

	public void incNobleAffinity (int amount) {
		nobleAffinity += amount;
		if (nobleAffinity > 100) {
			nobleAffinity = 100;
		}
	}

	public void incClergyAffinity (int amount) {
		clergyAffinity += amount;
		if (clergyAffinity > 100) {
			clergyAffinity = 100;
		}
	}

	public void incRoyalTreasury (int amount){
		royalTreasury += amount;
	}
		
	//-------------------------------------------------------------

	public bool[] areAffinitiesAcceptable(){
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
}
