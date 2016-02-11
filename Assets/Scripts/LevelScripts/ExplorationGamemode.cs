using UnityEngine;
using System.Collections;

public class ExplorationGamemode : MonoBehaviour {

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



	// Use this for initialization
	void Start () {
		PhotonNetwork.JoinRandomRoom ();
	}

	public void OnJoinedRoom(){
		GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag ("Spawner");
		int spawnPointIndex = Random.Range (0, spawnPoints.Length);
		Transform spawnPoint = spawnPoints [spawnPointIndex].transform;
		GameObject player = PhotonNetwork.Instantiate ("MyPlayer", spawnPoint.position, spawnPoint.rotation, 0);
		player.GetComponent<PlayerController> ().enabled = true;
		player.transform.FindChild ("CameraHinge").gameObject.SetActive (true);
	}

	public void OnPhotonRandomJoinFailed(){
		PhotonNetwork.CreateRoom (null);
	}

	
	// Update is called once per frame
	void Update () {
	
	}
}
