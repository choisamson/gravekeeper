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
			transform.Translate (speed * moveDir * Time.deltaTime);
			if (Vector3.Distance(transform.position, lastPosition) > minimumMovement) {
				lastPosition = transform.position;
				networkView.RPC("SetPosition", RPCMode.Others, transform.position);
			}

			if (Input.GetButtonUp("Torch")){
				Network.Instantiate (torchPrefab, transform.position, transform.rotation, 0);
			}
		}
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.name == "Monster(Clone)") {
			GameObject[] sprites = GameObject.FindGameObjectsWithTag("human");
			foreach(GameObject sprite in sprites) {
				sprite.renderer.enabled = false;
			}
		}
	}
	
	[RPC]
	void SetPosition(Vector3 newPosition) {
		transform.position = newPosition;
	}
}
