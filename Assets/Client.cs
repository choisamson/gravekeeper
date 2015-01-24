using UnityEngine;
using System.Collections;

public class Client : MonoBehaviour {

	public Transform cubePrefab;
	public Transform humanPrefab;

	void OnServerInitialized(){ 
		Transform myTransform = (Transform)Network.Instantiate (humanPrefab, transform.position, transform.rotation, 0);
	}


	void OnConnectedToServer(){ 
		SpawnPlayer();
	}

	void SpawnPlayer(){ 
		Transform myTransform = (Transform)Network.Instantiate (cubePrefab, transform.position, transform.rotation, 0);
	}
}
