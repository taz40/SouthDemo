using UnityEngine;
using System.Collections;

public class ObjectiveTrigger : MonoBehaviour {

	public string ObjectiveName;
	public bool periodBound = true;
	public bool MultiUse = false;

	bool used = false;

	void OnTriggerEnter(Collider other){
		if (other.GetComponent<PlayerController> () != null && (!used || MultiUse)) {
			used = true;
			GameObject.FindObjectOfType<ExplorationGamemode> ().addObjective(new Objective(ObjectiveName, periodBound));
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
