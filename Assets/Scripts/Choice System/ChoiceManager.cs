using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Xml;
using System.IO;

public class ChoiceManager : MonoBehaviour {
	Dictionary<string, ChoiceGroup> choiceGroups = new Dictionary<string, ChoiceGroup>();
    Choice currentChoice;
	public GameObject choicePrefab;
	public GameObject optionPrefab;
	public GameObject choicePos;
    public GameObject optionPos;
    TextAsset[] xmlFiles;
    // Use this for initialization
    public static ChoiceManager instance;
	void Start () {
        instance = this;
	}

    public void init() {
        xmlFiles = Resources.LoadAll<TextAsset>("XML/");
        List<TextAsset> choicesAssets = new List<TextAsset>();
        foreach (TextAsset text in xmlFiles) {
            if (text.name.StartsWith("Choices"))
            {
                choicesAssets.Add(text);
            }
        }
        foreach (TextAsset text in choicesAssets)
        {
            XmlTextReader xmlReader = new XmlTextReader(new StringReader(text.text));
            Stack<Choice> choices = new Stack<Choice>();
            Choice currentChoice = null;
            Stack<Option> options = new Stack<Option>();
            Option currentOption = null;
            ChoiceGroup currentgroup = null;
            while (xmlReader.Read())
            {
                if (xmlReader.Name == "Choice")
                {
                    if (!xmlReader.IsStartElement())
                    {
                        
                        if (currentOption != null)
                        {
                            currentOption.resultingChoice = currentChoice;
                        }
                        else {
                            currentgroup.choices.Add(currentChoice);
                        }
                        if (choices.Count > 0)
                            currentChoice = choices.Pop();
                        else
                            currentChoice = null;
                    }
                    else if (xmlReader.IsEmptyElement) {
                        if (currentChoice != null)
                            choices.Push(currentChoice);
                        currentChoice = new Choice();
                        currentChoice.Description = xmlReader.GetAttribute("Desc");
                        currentChoice.name = xmlReader.GetAttribute("id");
                        currentChoice.tag = xmlReader.GetAttribute("tag");
                        if (currentOption != null)
                        {
                            currentOption.resultingChoice = currentChoice;
                        }
                        else {
                            currentgroup.choices.Add(currentChoice);
                        }
                        if (choices.Count > 0)
                            currentChoice = choices.Pop();
                        else
                            currentChoice = null;
                    }
                    else {
                        if (currentChoice != null)
                            choices.Push(currentChoice);
                        currentChoice = new Choice();
                        currentChoice.Description = xmlReader.GetAttribute("Desc");
                        currentChoice.name = xmlReader.GetAttribute("id");
                        currentChoice.tag = xmlReader.GetAttribute("tag");
                    }
                }
                else if (xmlReader.Name == "Option")
                {
                    if (!xmlReader.IsStartElement())
                    {
                        if (currentChoice != null)
                            currentChoice.Options.Add(currentOption);
                        if (options.Count > 0)
                            currentOption = options.Pop();
                        else
                            currentOption = null;
                    }
                    else if (xmlReader.IsEmptyElement) {
                        if (currentOption != null)
                            options.Push(currentOption);
                        currentOption = new Option();
                        currentOption.Description = xmlReader.GetAttribute("Desc");
                        currentOption.Response = xmlReader.GetAttribute("Response");
                        if (currentChoice != null)
                            currentChoice.Options.Add(currentOption);
                        if (options.Count > 0)
                            currentOption = options.Pop();
                        else
                            currentOption = null;
                    }
                    else
                    {
                        if (currentOption != null)
                            options.Push(currentOption);
                        currentOption = new Option();
                        currentOption.Description = xmlReader.GetAttribute("Desc");
                        currentOption.Response = xmlReader.GetAttribute("Response");
                    }
                }
                else if (xmlReader.Name == "ChoiceGroup") {
                    if (!xmlReader.IsStartElement())
                    {
                        choiceGroups.Add(currentgroup.name, currentgroup);
                        currentgroup = null;
                    }
                    else if (xmlReader.IsEmptyElement) {
                        if (choiceGroups.ContainsKey(xmlReader.GetAttribute("name")))
                        {
                            currentgroup = choiceGroups[xmlReader.GetAttribute("name")];
                            choiceGroups.Remove(xmlReader.GetAttribute("name"));
                        }
                        else {
                            currentgroup = new ChoiceGroup();
                            currentgroup.name = xmlReader.GetAttribute("name");
                        }
                        choiceGroups.Add(currentgroup.name, currentgroup);
                        currentgroup = null;
                    }
                    else {
                        if (choiceGroups.ContainsKey(xmlReader.GetAttribute("name")))
                        {
                            currentgroup = choiceGroups[xmlReader.GetAttribute("name")];
                            choiceGroups.Remove(xmlReader.GetAttribute("name"));
                        }
                        else {
                            currentgroup = new ChoiceGroup();
                            currentgroup.name = xmlReader.GetAttribute("name");
                        }
                    }
                }
            }
        }
        showChoice(choiceGroups["Startup"].choices[0]);
        /*
        choice = new Choice();
        choice.Description = "Party or Homework?";
        Option option1 = new Option();
        option1.Description = "Party!";
        option1.Response = "YAY!";
        choice.Options.Add(option1);
        Option option2 = new Option();
        option2.Description = "Homework";
        Choice choice2 = new Choice();
        choice2.Description = "Math or English?";
        Option option3 = new Option();
        option3.Description = "Math";
        option3.Response = "Nerd!";
        choice2.Options.Add(option3);
        Option option4 = new Option();
        option4.Description = "English";
        option4.Response = "Boring...";
        choice2.Options.Add(option4);
        option2.resultingChoice = choice2;
        choice.Options.Add(option2);
        showChoice(choice);*/
    }

    public void OptionPicked(string text)
    {
        if (text == "Done")
        {
            PlayerController.canMove = true;
            ExplorationGamemode.timeFrozen = false;
            GameObject.Destroy(choicePos.transform.GetChild(0).gameObject);
        }
        else {
            Option o = null;
            foreach (Option o1 in this.currentChoice.Options)
            {
                if (o1.Description == text)
                    o = o1;
            }
            if (o == null || o.resultingChoice == null)
            {
                Choice choice = new Choice();
                choice.Description = o.Response;
                Option option1 = new Option();
                option1.Description = "Done";
                choice.Options.Add(option1);
                showChoice(choice);
            }
            else {
                showChoice(o.resultingChoice);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void showChoice(Choice choice){
        currentChoice = choice;
        PlayerController.canMove = false;
        ExplorationGamemode.timeFrozen = true;
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