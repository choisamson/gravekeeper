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
	public bool alive = true;

	public Transform deadhumanPrefab;

	Vector3 lastPosition;
	float minimumMovement = .05f;
	
	private float mana = 0f;
	
	void Update()
	{
		if (networkView.isMine && alive) {
			Vector3 moveDir = new Vector3 (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical"), 0);
			transform.Translate (speed * moveDir * Time.deltaTime);
			if (Vector3.Distance(transform.position, lastPosition) > minimumMovement) {
				lastPosition = transform.position;
				networkView.RPC("SetPosition", RPCMode.Others, transform.position);
			}
			
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
	
	void OnTriggerEnter2D(Collider2D collider){
		if (collider.name == "Monster(Clone)" && alive == true) {
			//TODO: death animation
			/*GameObject[] sprites = GameObject.FindGameObjectsWithTag("human");
			foreach(GameObject sprite in sprites) {
				sprite.renderer.enabled = false;
			}*/


			Network.Instantiate(deadhumanPrefab, this.transform.position, this.transform.rotation, 0);
			GameObject[] sprites = GameObject.FindGameObjectsWithTag("human");
			foreach(GameObject sprite in sprites) {
				sprite.renderer.enabled = false;
			}

			Client client = GameObject.Find("client").GetComponent<Client>();
			alive = false;
			client.GameOver = true;
		}
	}

	
	public bool Alive {
		get {
			return alive;
		}
		set {
			alive = value;
		}
	}
	
	[RPC]
	void SetPosition(Vector3 newPosition) {
		transform.position = newPosition;
	}
}
