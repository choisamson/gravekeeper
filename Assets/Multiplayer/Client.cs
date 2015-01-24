using UnityEngine;
using System.Collections;

public class Client : MonoBehaviour {
	
	public Transform humanPrefab;
	public Transform monsterPrefab;

	void OnServerInitialized(){ 
		Transform myTransform = (Transform)Network.Instantiate (humanPrefab, transform.position, transform.rotation, 0);
	}


	void OnConnectedToServer(){ 
		SpawnPlayer();
	}

	void SpawnPlayer(){ 
		Transform myTransform = (Transform)Network.Instantiate (monsterPrefab, transform.position, transform.rotation, 0);
	}
}
