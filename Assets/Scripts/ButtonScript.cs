using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour {

	public void StartGame(){
		SceneManager.LoadScene ("Game Scene");
	}

	public void MainMenu(){
		SceneManager.LoadScene ("Title Scene");
	}
}
