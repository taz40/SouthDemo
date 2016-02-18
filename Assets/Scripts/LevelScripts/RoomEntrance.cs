using UnityEngine;
using System.Collections;

public class RoomEntrance : MonoBehaviour {

	public string roomNum;

	void OnTriggerEnter(Collider other){
		if(other.GetComponent<PlayerController>() != null)
			GameObject.FindObjectOfType<ExplorationGamemode> ().completeObjective ("Go to " + roomNum);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
