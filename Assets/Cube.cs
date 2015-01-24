using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cube : MonoBehaviour {
	public float magnitude = 5;
	public float speed = 5;
	
	private Vector3 origPos = Vector3.zero;

	void Start()
	{
		origPos = transform.position;
	}

	void Awake(){
		if (!Network.isServer) {
			enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Network.isServer) { 
			Vector3 moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
			float speed = 5; 
			transform.Translate(speed * moveDir * Time.deltaTime);
		}
	}

	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info) {
		if (stream.isWriting) { 
			Vector3 myPosition = transform.position; 
			stream.Serialize(ref myPosition); 
		} else { 
			Vector3 receivedPosition = Vector3.zero; 
			stream.Serialize(ref receivedPosition);
			transform.position = receivedPosition; 
		}
	}	
}
