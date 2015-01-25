using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HumanMovement : MonoBehaviour 
{
	public Transform torchPrefab;
	public Transform trapPrefab;
	public GameObject manaBar;
	public float magnitude = 5;
	public float speed = 5;
	
	Vector3 lastPosition;
	float minimumMovement = .05f;
	
	private float mana = 0f;
	
	void Update()
	{
		if (networkView.isMine) {
			Vector3 moveDir = new Vector3 (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical"), 0);
			transform.Translate (speed * moveDir * Time.deltaTime);
			if (Vector3.Distance(transform.position, lastPosition) > minimumMovement) {
				lastPosition = transform.position;
				networkView.RPC("SetPosition", RPCMode.Others, transform.position);
			}
			
			/*if (Input.GetButtonUp("Torch")){
				Network.Instantiate (torchPrefab, transform.position, transform.rotation, 0);
			}*/
			
			if (Input.GetButtonUp ("Trap")){
				if (mana == 300) {
					Network.Instantiate (trapPrefab, transform.position, transform.rotation, 0);
					mana = 0;
				} else {
					//TODO: make a toast or some notification of oom
				}
			}
			
			if (mana < 300){
				mana ++;
			}
		}
		
		manaBar.transform.localScale = new Vector3 (mana / 300f, 1f, 1f);
	}
	
	[RPC]
	void SetPosition(Vector3 newPosition) {
		transform.position = newPosition;
	}
}
