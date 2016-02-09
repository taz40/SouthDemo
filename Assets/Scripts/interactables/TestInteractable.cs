using UnityEngine;
using System.Collections;

public class TestInteractable : MonoBehaviour, Interactable {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Interactable.use(GameObject user){
		Debug.Log("Oh No! I am being Used!");
	}
}
