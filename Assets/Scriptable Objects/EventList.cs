using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[CreateAssetMenu]
[Serializable]
public class EventList : ScriptableObject {

	public List<Event> eventsList;

}
	
[Serializable]
public struct Event{

	[HeaderAttribute("Event")]
	public string eventMessage;

	[HeaderAttribute("Choices")]
	public List<Choices> eventOptions;
}


[Serializable]
public struct Choices{
	[HeaderAttribute("Choice Message")]
	public string choiceMessage;

	[HeaderAttribute("Choice Effects")]
	public int effectOnPeasant;
	public int effectOnNoble;
	public int effectOnClergy;
	public int effectOnRoyalTreasury;

}