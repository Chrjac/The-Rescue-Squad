using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {

	public Camera standByCamera;
	SpawnSpot[] spawnSpots;
	public bool offlineMode = false;



	void Start () {

		spawnSpots = GameObject.FindObjectsOfType<SpawnSpot> ();
		Connect ();

	}
	void Connect(){
		if (offlineMode) {
			PhotonNetwork.offlineMode = true;
			OnJoinedLobby();
		} 
		else {
		PhotonNetwork.ConnectUsingSettings("1.0.0");
		}
	}
	void OnGUI(){
		GUILayout.Label (PhotonNetwork.connectionStateDetailed.ToString ());
		GUILayout.Label("Ping: " + PhotonNetwork.GetPing());

	}
	void OnJoinedLobby(){
		PhotonNetwork.JoinRandomRoom ();

	}
	void OnPhotonRandomJoinFailed(){
		PhotonNetwork.CreateRoom (null);
	}
	void OnJoinedRoom(){
		SpawnMyPlayer ();
	}
	void SpawnMyPlayer(){
		if (spawnSpots == null) {
			Debug.Log("No spawnpoint found");
			return;
				}
		SpawnSpot mySpawnSpot = spawnSpots [Random.Range (0, spawnSpots.Length)];
		GameObject myPlayerGo = (GameObject)PhotonNetwork.Instantiate("PlayerController", mySpawnSpot.transform.position, mySpawnSpot.transform.rotation, 0);
		standByCamera.enabled = false;

		((MonoBehaviour)myPlayerGo.GetComponent ("PlayerController")).enabled = true;
		((MonoBehaviour)myPlayerGo.GetComponent ("Entity")).enabled = true;
	}
}