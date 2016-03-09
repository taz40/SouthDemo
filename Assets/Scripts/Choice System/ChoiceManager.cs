using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChoiceManager : MonoBehaviour {
	Choice choice;
	public GameObject choicePrefab;
	public GameObject optionPrefab;
	public GameObject choicePos;
	// Use this for initialization
	void Start () {
		choice = new Choice ();
		choice.Description = "Party or Homework?";
		Option option1 = new Option ();
		option1.Description = "Party!";
		option1.Response = "YAY!";
		choice.Options.Add (option1);
		Option option2 = new Option ();
		option2.Description = "Homework";
		option2.Response = "Looser...";
		choice.Options.Add (option2);
		showChoice (choice);
	}

	public void OptionPicked(Text text){
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void showChoice(Choice choice){
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		GameObject choiceGO = (GameObject)GameObject.Instantiate (choicePrefab);
		choiceGO.transform.SetParent (choicePos.transform, false);
		choiceGO.transform.FindChild ("Desc").GetComponent<Text> ().text = choice.Description;
	}
}
