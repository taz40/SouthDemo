using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class ExplorationGamemode : MonoBehaviour {

    protected List<Objective> objectives;
    Action<Objective> objectiveAdded;
    Action<Objective> objectiveRemoved;

    public void addObjective(Objective o) {
        objectives.Add(o);
        if (objectiveAdded != null)
            objectiveAdded(o);
		updateObjectiveList ();
    }

    public void completeObjective(string name) {
		Objective o = null;
        foreach (Objective obj in objectives) {
            if (obj.objectiveName == name) {
				o = obj;
				break;
            }
        }
		o.completed();
		objectives.Remove (o);
		if(objectiveRemoved != null)
			objectiveRemoved(o);
		updateObjectiveList ();
    }

	public void failedObjective(string name) {
		Objective o = null;
		foreach (Objective obj in objectives) {
			if (obj.objectiveName == name) {
				o = obj;
				break;
			}
		}
		o.failed();
		objectives.Remove (o);
		if(objectiveRemoved != null)
			objectiveRemoved(o);
		updateObjectiveList ();
	}

	public void RegisterObjectiveAddedCallback(Action<Objective> callback){
		objectiveAdded += callback;
	}

	public void UnregisterObjectiveAddedCallback(Action<Objective> callback){
		objectiveAdded -= callback;
	}

	public void RegisterObjectiveRemovedCallback(Action<Objective> callback){
		objectiveRemoved += callback;
	}
	
	public void UnregisterObjectiveRemovedCallback(Action<Objective> callback){
		objectiveRemoved -= callback;
	}

	public Transform objectivesTransform;

	void updateObjectiveList(){
		for (int i = 0; i < objectivesTransform.childCount; i++) {
			Transform objectiveListItem = objectivesTransform.GetChild(i);
			GameObject.Destroy(objectiveListItem.gameObject);
		}

		int offset = 0;
		foreach (Objective o in objectives) {
			Debug.Log("adding objective " + o.objectiveName);
			GameObject objectiveListItem = GameObject.Instantiate(objectivesTransform.gameObject);
			objectiveListItem.name = "OLI_"+o.objectiveName;
			offset += 10;
			objectiveListItem.transform.position = new Vector3(objectivesTransform.position.x,objectivesTransform.position.y-offset,objectivesTransform.position.z);
			objectiveListItem.GetComponent<Text>().text = o.objectiveName;
			objectiveListItem.transform.parent = objectivesTransform;
		}
	}

	string[,] schedule = {
		 {
			"English 9",
			"Alg 1",
			"Physical Education 3",
			"Discovering Pathways",
			"US History",
			"Spanish 1",
			"Physical Science",
			"Computer Applications Microsoft Office"
		}, {"Geometry", "Spanish 2", "Biology", "English 10", "Health 4", "World Geography", "Hardware Matinence", "Programming in Visual Basic"},
						{"Chemistry 1", "Webpage authering 1", "College and business apps", "Spanish 3", "English 11", "Alg 2", "US and Wyo Government", "Comuter graphics 1"},
		{"Computer Graphics 3", "Chemistry 2", "English 12", "off", "College alg. and trig.", "Spanish 4", "Webpage authering 3", "off"}};

	float timeSec;
	public Text time;
	public Text period;
	int abcday = 0; // 0 = A, 1 = B, 2 = C
	int schoolStarts = (7-1) * 60 * 60 + 40 * 60;
	int schoolEnds = (2+12-1) * 60 * 60 + 45 * 60;
	List<Period> schedualA;
	int lastPeriod = 0;
	public Image scheduleImage;

	// Use this for initialization
	void Start () {
		if (!PhotonNetwork.connected) {
			PhotonNetwork.ConnectUsingSettings ("SouthDemo v. " + MainMenu.version);
		} else {
			PhotonNetwork.JoinRandomRoom ();
		}
		schedualA = new List<Period> ();
		Period period = new Period (getSec(7,40,0), "1APassing");
		// TODO: add 1APassing objectives here
		period.objectives.Add (new Objective ("Go to Room A221"));
		schedualA.Add(period);
		period = new Period (getSec(7,45,0), "1A");
		// TODO: add 1A objectives here
		schedualA.Add(period);
		period = new Period (getSec(9,5,0), "ELOPassing");
		// TODO: add ELOPassing objectives here
		schedualA.Add(period);
		period = new Period (getSec(9,10,0), "ELO");
		// TODO: add ELO objectives here
		schedualA.Add(period);
		period = new Period (getSec(9,50,0), "2APassing");
		// TODO: add 2APassing objectives here
		schedualA.Add(period);
		period = new Period (getSec(9,55,0), "2A");
		// TODO: add 2A objectives here
		schedualA.Add(period);
		period = new Period (getSec(11,15,0), "Lunch");
		// TODO: add Lunch objectives here
		schedualA.Add(period);
		period = new Period (getSec(11,55,0), "3APassing");
		// TODO: add 3APassing objectives here
		schedualA.Add(period);
		period = new Period (getSec(12,0,0), "3A");
		// TODO: add 3A objectives here
		schedualA.Add(period);
		period = new Period (getSec(13,20,0), "4APassing");
		// TODO: add 4APassing objectives here
		schedualA.Add(period);
		period = new Period (getSec(13,25,0), "4A");
		// TODO: add 4A objectives here
		schedualA.Add(period);
	}

	public int getSec(int hour, int min, int sec){
		if (timeSec < 0) {
			return ((hour-1) * 60 * 60 + min * 60 + sec) + 24*60*60;
		}
		return (hour-1) * 60 * 60 + min * 60 + sec;
	}

	public void setTime(int hour, int min, int sec){
		timeSec = (hour-1) * 60 * 60 + min * 60 + sec;
		if (timeSec < 0) {
			timeSec += 24*60*60;
		}
	}

	public void OnJoinedLobby(){
		PhotonNetwork.JoinRandomRoom ();
	}

	public void OnJoinedRoom(){
		GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag ("Spawner");
		int spawnPointIndex = UnityEngine.Random.Range (0, spawnPoints.Length);
		Transform spawnPoint = spawnPoints [spawnPointIndex].transform;
		GameObject player = PhotonNetwork.Instantiate ("MyPlayer", spawnPoint.position, spawnPoint.rotation, 0);
		player.GetComponent<PlayerController> ().enabled = true;
		player.transform.FindChild ("CameraHinge").gameObject.SetActive (true);
		setTime (7, 39, 55);
		objectives = new List<Objective> ();
		updateObjectiveList ();
	}

	public void OnPhotonRandomJoinFailed(){
		PhotonNetwork.CreateRoom (null);
	}

	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey("j"))
			timeSec += Time.deltaTime*60;
		else if(Input.GetKey("k"))
			timeSec += Time.deltaTime*60*60;
		else
			timeSec += Time.deltaTime;
		if (timeSec > 24 * 60 * 60) {
			timeSec -= 24*60*60;
		}
		if (Input.GetButton ("Schedule")) {
			scheduleImage.enabled = true;
		} else {
			scheduleImage.enabled = false;
		}
		float currSec = timeSec;
		int hour = Mathf.FloorToInt ((float)currSec/(60f*60f));
		currSec -= hour * 60 * 60;
		hour += 1;
		int min = Mathf.FloorToInt ((float)currSec / 60f);
		currSec -= min * 60;
		int sec = (int)currSec;
		string ampm = "AM";
		if (hour >= 12 && hour != 24) {
			ampm = "PM";
		}
		if (hour > 12) {
			hour -= 12;
		}
		string minstr = ""+min;
		string secstr = ""+sec;
		if (min < 10) {
			minstr = "0" + min;
		}
		if (sec < 10) {
			secstr = "0" + sec;
		}
		if ((timeSec < schoolStarts) || timeSec > schoolEnds) {
			period.text = "School Not In Session.";
			if(lastPeriod != 0){
				lastPeriod = 0;
				Objective[] objectivesTemp = new Objective[objectives.Count];
				objectives.CopyTo(objectivesTemp);
				foreach(Objective o in objectivesTemp){
					if(o.periodBound)
						failedObjective(o.objectiveName);
				}
			}
		}else{
			if (abcday  == 0) {
				foreach(Period p in schedualA){
					int startTime = p.startTime;
					if(timeSec >= startTime && lastPeriod < startTime){
						period.text = p.name;
						lastPeriod = (int)timeSec;
						Objective[] objectivesTemp = new Objective[objectives.Count];
						objectives.CopyTo(objectivesTemp);
						foreach(Objective o in objectivesTemp){
							if(o.periodBound)
								failedObjective(o.objectiveName);
						}
						foreach(Objective o in p.objectives){
							addObjective(new Objective(o.objectiveName, o.periodBound));
						}
					}
				}
			}
		}
		time.text = hour + ":" + minstr + ":" + secstr + " " + ampm;
	}
}
