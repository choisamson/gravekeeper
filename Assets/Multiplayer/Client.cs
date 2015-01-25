using UnityEngine;
using System.Collections;

public class Client : MonoBehaviour {
	
	public Transform humanPrefab;
	public Transform monsterPrefab;

	private Transform human;
	private Transform monster;

	void OnServerInitialized(){ 
		human = (Transform)Network.Instantiate (humanPrefab, new Vector3(-36, 0, 0), transform.rotation, 0);
	}


	void OnConnectedToServer(){ 
		SpawnPlayer();
	}

	void SpawnPlayer(){ 
		monster = (Transform)Network.Instantiate (monsterPrefab, new Vector3(36, 0, 0), transform.rotation, 0);
		GameObject.Find("Human(Clone)").transform.position = new Vector3 (-36, 0, 0);
	}
}
