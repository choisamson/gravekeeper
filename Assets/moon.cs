using UnityEngine;
using System.Collections;

public class moon : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
		Client client = GameObject.Find ("client").GetComponent<Client> ();
		Server server = GameObject.Find ("server").GetComponent<Server> ();
		if (!client.GameOver && server.GameTime > 0 && server.Started) {
			Vector3 moveDir = new Vector3 (0, 1, 0);
			transform.Translate (0.5f * moveDir * Time.deltaTime);
		}
	}
}
