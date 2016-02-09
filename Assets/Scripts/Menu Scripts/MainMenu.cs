using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
	public GameObject mainMenu;
	public GameObject options;
	public string nameOfStartingLevel = "TestingEnvironment";
	// Use this for initialization
	void Start () {
		//PhotonNetworkingMessage.joine
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log("State: "+PhotonNetwork.connectionStateDetailed);
	}
	public void exit(){
		Application.Quit ();
	} 
	public void SinglePlayer(){
		PhotonNetwork.offlineMode = true;
		Application.LoadLevel (nameOfStartingLevel);
	}
	public void MultiPlayer(){
		PhotonNetwork.ConnectUsingSettings ("SouthDemo");
		//Application.LoadLevel ("TestingEnvironment");
	}
	public void OnJoinedLobby(){
		Application.LoadLevel (nameOfStartingLevel);
	}
	public void Options() {
		options.SetActive (true);
		mainMenu.SetActive (false);
	}
}
