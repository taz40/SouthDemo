using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChoiceManager : MonoBehaviour {
	Choice choice;
	public GameObject choicePrefab;
	public GameObject optionPrefab;
	public GameObject choicePos;
    public GameObject optionPos;
    // Use this for initialization
    public static ChoiceManager instance;
	void Start () {
        instance = this;
	}

    public void init() {
        choice = new Choice();
        choice.Description = "Party or Homework?";
        Option option1 = new Option();
        option1.Description = "Party!";
        option1.Response = "YAY!";
        choice.Options.Add(option1);
        Option option2 = new Option();
        option2.Description = "Homework";
        option2.Response = "Looser...";
        choice.Options.Add(option2);
        showChoice(choice);
    }

    public void OptionPicked(string text)
    {
        if (text == "Done")
        {
            PlayerController.canMove = true;
            GameObject.Destroy(choicePos.transform.GetChild(0).gameObject);
        }
        else {
            Option o = null;
            foreach (Option o1 in this.choice.Options)
            {
                if (o1.Description == text)
                    o = o1;
            }
            Choice choice = new Choice();
            choice.Description = o.Response;
            Option option1 = new Option();
            option1.Description = "Done";
            choice.Options.Add(option1);
            showChoice(choice);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void showChoice(Choice choice){
        PlayerController.canMove = false;
        if(choicePos.transform.childCount > 0)
            GameObject.Destroy(choicePos.transform.GetChild(0).gameObject);
        GameObject choiceGO = (GameObject)GameObject.Instantiate (choicePrefab);
		choiceGO.transform.SetParent (choicePos.transform, false);
		choiceGO.transform.FindChild ("Desc").GetComponent<Text> ().text = choice.Description;
        int Offset = 0;
        foreach (Option o in choice.Options) {
            GameObject OptionGO = (GameObject)GameObject.Instantiate(optionPrefab);
            OptionGO.transform.SetParent(choiceGO.transform.FindChild("OptionPositioning"), false);
            OptionGO.transform.Translate(0, Offset, 0);
            Offset -= 30;
            OptionGO.transform.FindChild("Text").GetComponent<Text>().text = o.Description;
            string optionName = o.Description;
            OptionGO.GetComponent<Button>().onClick.AddListener(() => { OptionPicked(optionName); });
        }
	}
}
