using UnityEngine;
using System.Collections;

public class monitor : MonoBehaviour, Interactable {

	Renderer renderer;
	public Material on;
	public Material off;
	bool ison = false;

	// Use this for initialization
	void Start () {
		renderer = transform.FindChild ("screen").GetComponent<Renderer> ();
		renderer.material = off;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Interactable.use(GameObject user){
		ison = !ison;
		if (ison) {
			renderer.material = on;
		} else {
			renderer.material = off;
		}
	}
}
