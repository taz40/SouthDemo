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
    }

    public void completeObjective(string name) {
        foreach (Objective obj in objectives) {
            if (obj.objectiveName == name) {
                objectives.Remove(obj);
                obj.completed();
                if(objectiveRemoved != null)
                    objectiveRemoved(obj);
            }
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
	Dictionary<int, string> schedualA;
	int lastPeriod = 0;
	public Image scheduleImage;

	// Use this for initialization
	void Start () {
		if (!PhotonNetwork.connected) {
			PhotonNetwork.ConnectUsingSettings ("SouthDemo v. " + MainMenu.version);
		} else {
			PhotonNetwork.JoinRandomRoom ();
		}
		schedualA = new Dictionary<int, string> ();
		schedualA.Add(getSec(7,40,0), "1APassing");
		schedualA.Add(getSec(7,45,0), "1A");
		schedualA.Add(getSec(9,5,0), "ELOPassing");
		schedualA.Add(getSec(9,10,0), "ELO");
		schedualA.Add(getSec(9,50,0), "2APassing");
		schedualA.Add(getSec(9,55,0), "2A");
		schedualA.Add(getSec(11,15,0), "Lunch");
		schedualA.Add(getSec(11,55,0), "3APassing");
		schedualA.Add(getSec(12,0,0), "3A");
		schedualA.Add(getSec(13,20,0), "4APassing");
		schedualA.Add(getSec(13,25,0), "4A");
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
			lastPeriod = 0;
		}else{
			if (abcday  == 0) {
				foreach(int startTime in schedualA.Keys){
					if(timeSec >= startTime && lastPeriod < startTime){
						period.text = schedualA[startTime];
						lastPeriod = (int)timeSec;
					}
				}
			}
		}
		time.text = hour + ":" + minstr + ":" + secstr + " " + ampm;
	}
}
