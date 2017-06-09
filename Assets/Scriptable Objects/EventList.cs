using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[CreateAssetMenu]
[Serializable]
public class EventList : ScriptableObject {

	public int probabilityWeight;
	public List<GameEvent> eventsList;


}
	
[Serializable]
public class GameEvent{
	public bool repeatable;
	private bool occurred = false;

	public void toggleOccured(){
		occurred = true;
	}

	public bool hasOccured(){
		return occurred;
	}

	[HeaderAttribute("Event")]
	public string eventMessage;

	[HeaderAttribute("Choices")]
	public List<Choice> eventOptions;
}


[Serializable]
public struct Choice{


	[HeaderAttribute("Choice Message")]
	public string choiceMessage;

	[HeaderAttribute("Choice Effects")]
	public int effectOnPeasant;
	public int effectOnNoble;
	public int effectOnClergy;
	public int effectOnRoyalTreasury;


}