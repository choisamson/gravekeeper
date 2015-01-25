using UnityEngine;
using System.Collections;

public class Trap : MonoBehaviour {

	private bool tripped = false;
	private float trapTimer = 180;
	private float expiryTimer = 60;

	void Update(){
		if (trapTimer > 0) {
			trapTimer --;
		}

		if (tripped) {
			expiryTimer --;
			if (expiryTimer == 0) {
				GameObject.Destroy(this.gameObject);
			}
		}
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if (trapTimer == 0 && collider.name == "Monster(Clone)") {
			tripped = true;
		}
	}

}
