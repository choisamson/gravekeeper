using UnityEngine;
using System.Collections;

public class trapAnimator : MonoBehaviour {

	private bool tripped = false;
	private float trapTimer = 180;
	private float expiryTimer = 60;

	protected Animator animator;
	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
	}
	

	void Update(){
		if (trapTimer > 0) {
			trapTimer --;
		}
	

	}
	
	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.name == "Monster(Clone)") {
			tripped = true;
			animator.SetBool("isTripped", true);
		}

	}
	
}
