using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[CreateAssetMenu]
[Serializable]
public class EventList : ScriptableObject {

	public int probabilityWeight;
	public List<GameEvent> eventsList;

	public void Reset(){
		foreach (GameEvent ge in eventsList) {
			ge.Reset ();
		}
	}
}
	
[Serializable]
public class GameEvent{
	public bool repeatable;
	private bool occurred;

	public void toggleOccured(){
		occurred = true;
	}

	public bool hasOccured(){
		return occurred;
	}

	public void Reset(){
		occurred = false;
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