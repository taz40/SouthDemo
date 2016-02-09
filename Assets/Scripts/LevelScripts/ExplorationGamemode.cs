using UnityEngine;
using System.Collections;

public class ExplorationGamemode : MonoBehaviour {

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
