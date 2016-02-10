using UnityEngine;
using System.Collections;

public class Computer : MonoBehaviour, Interactable {

	Animator anim;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Interactable.use(GameObject user){
		anim.SetTrigger("Used");
	}
}
