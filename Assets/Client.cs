using UnityEngine;
using System.Collections;

public class Client : MonoBehaviour {

	public Transform cubePrefab;
	void OnServerInitialized(){ 
	}

	void OnConnectedToServer(){ 
		SpawnPlayer();
	}

	void SpawnPlayer(){ 

	}
}
