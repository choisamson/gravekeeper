using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterMovement: MonoBehaviour
{
	public float magnitude = 5;
	public float speed = 5;
	public GameObject healthBar;

	private Vector3 origPos = Vector3.zero;
	private float health = 4;
	//DASH
	float dashDurationCounter;
	
	bool isDashing;

	private int dashTapCounter;
	private float forwardAccel;  // The current acceleration of the player

	private static int doubleTapTime = 60; // The amount of ticks that can occur between two key presses to double tap it

	private static float forwardReg = 5f;   // The ordinary acceleration
	private static float forwardDash = 30.0f;  // The acceleration while dashing
	private static float dashDuration = 15; //dash duration in frames
	//CON

	private bool alive = true;
	public Transform deadmonsterPrefab;

	void Start()
	{
		dashTapCounter = 0;
		dashDurationCounter = 0;
		forwardAccel = 0.0f;
		isDashing = false;
	}

	void Update()
	{

		if (!alive){
			Network.Instantiate(deadmonsterPrefab, this.transform.position, this.transform.rotation, 0);
			GameObject[] sprites = GameObject.FindGameObjectsWithTag("monster");
			foreach(GameObject sprite in sprites) {
				sprite.renderer.enabled = false;
			}
		}
		if (networkView.isMine && alive) {

			//DASH
			rigidbody2D.angularVelocity = 0;

			if (Input.GetButton ("Dash")) {
				if(isDashing) {
					rigidbody2D.velocity += new Vector2 (Input.GetAxis ("Horizontal") * forwardAccel, Input.GetAxis ("Vertical") * forwardAccel);
				} else {
					rigidbody2D.velocity = new Vector2(0,0);
				}

			}


			if (Input.GetButtonDown ("Dash") && isDashing == false) {

				if (dashTapCounter > 0) {
					isDashing = true;
					forwardAccel = forwardDash;
					dashDurationCounter = dashDuration;
				} else {
					forwardAccel = forwardReg;
				}
				dashTapCounter = doubleTapTime;
			}

			if(isDashing){
				dashDurationCounter--;
				if(dashDurationCounter == 0){
					isDashing = false;
					if (forwardAccel > forwardReg){
						forwardAccel = forwardAccel * 0.6f;
					}
				}
			}


			if (dashTapCounter > 0) {
				dashTapCounter--;
			}

			//Decelerate if going past max speed
			if(rigidbody2D.velocity.magnitude > 10){
				rigidbody2D.velocity = rigidbody2D.velocity * 0.7f;
			}
			//CON

			Vector3 moveDir = new Vector3 (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical"), 0);
			transform.Translate (speed * moveDir * Time.deltaTime);
		}

		healthBar.transform.localScale = new Vector3 (health / 4f, 1f, 1f);
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

	void OnTriggerEnter2D (Collider2D collider) {
		if (collider.name == "Trap(Clone)") {
			health --;
			if (health == 0 && alive == true){
				
				Network.Instantiate(deadmonsterPrefab, this.transform.position, this.transform.rotation, 0);
				GameObject[] sprites = GameObject.FindGameObjectsWithTag("monster");
				foreach(GameObject sprite in sprites) {
					sprite.renderer.enabled = false;
				}
				Client client = GameObject.Find("client").GetComponent<Client>();
				alive = false;
				client.GameOver = true;
			}
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
}
