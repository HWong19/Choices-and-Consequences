using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[CreateAssetMenu]
[Serializable]
public class WorldEventsPairList : ScriptableObject {
	
	[HeaderAttribute("Probability weight compared to regular events (0-1)")]
	public float probabilityWeight;
	public List<WorldEventsPair> worldEventsPairList;

	public void Reset(){
		foreach (WorldEventsPair wep in worldEventsPairList) {
			wep.ResetEvent();
		}
	}
}


[Serializable]
public class WorldEventsPair{
	
	[HeaderAttribute("Events")]
	public WorldEvent causalEvent;
	public WorldEvent resultingEvent;

	private string causingCityState;
	private bool causalEventOccured;


	public bool HasCausalEventOccured(){
		return causalEventOccured;
	}
	public string GetCausingCityState(){
		return causingCityState;
	}
	public void ResetEvent(){
		causalEventOccured = false;
	}
}
	
[Serializable]
public class WorldEvent{

	[HeaderAttribute("Event")]
	public string eventMessage;

	[HeaderAttribute("Choices")]
	public List<WorldChoice> eventChoices;

}

[Serializable]
public class WorldChoice{


	[HeaderAttribute("Choice Message")]
	public string choiceMessage;

	[HeaderAttribute("Choice Effects")]
	public int effectOnPeasant;
	public int effectOnNoble;
	public int effectOnClergy;
	public int effectOnRoyalTreasury;

	public bool hasImpact;

}