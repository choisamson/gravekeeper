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
		Transform myTransform = (Transform)Network.Instantiate (cubePrefab, transform.position, transform.rotation, 0);
	}
}
