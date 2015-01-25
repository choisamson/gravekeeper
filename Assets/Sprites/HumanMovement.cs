using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HumanMovement : MonoBehaviour 
{
	public Transform torchPrefab;
    public float magnitude = 5;
    public float speed = 5;
	
	Vector3 lastPosition;
	float minimumMovement = .05f;

	void Update()
	{
		if (networkView.isMine) {
			Vector3 moveDir = new Vector3 (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical"), 0);
			float speed = 5; 
			transform.Translate (speed * moveDir * Time.deltaTime);
			if (Vector3.Distance(transform.position, lastPosition) > minimumMovement) {
				lastPosition = transform.position;
				networkView.RPC("SetPosition", RPCMode.Others, transform.position);
			}

			if (Input.GetButtonUp("Right")){
				Network.Instantiate (torchPrefab, transform.position, transform.rotation, 0);
			}
		}
	}
	
	[RPC]
	void SetPosition(Vector3 newPosition) {
		transform.position = newPosition;
	}
}
