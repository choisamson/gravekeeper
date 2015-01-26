using UnityEngine;
using System.Collections;

public class humanAnimator : MonoBehaviour {

	// Use this for initialization
	protected Animator animator;
	private bool dead = false;

	void Start () {
		animator = GetComponent<Animator> ();
		
	}
	// Update is called once per frame
	void Update () {

		if (!dead){
			if(Input.GetButtonDown ("Trap")){
				animator.SetBool ("isCasting", true);
			}
			else if (Input.GetButtonDown ("Trap") == false){
				animator.SetBool ("isCasting", false);

				if (Input.GetAxis("Horizontal") == 1 ||Input.GetAxis("Horizontal") == -1){
					animator.SetBool ("isWalking", true);	
					this.transform.localScale = new Vector3 (5*Input.GetAxis("Horizontal"),5,1);
				}
				else if(Input.GetAxis ("Vertical") == 1 || Input.GetAxis ("Vertical") == -1 ){
					animator.SetBool ("isWalking", true);	
				}
				else{
					animator.SetBool ("isWalking", false);
				}
			}

		}

		if(Input.GetButtonDown ("Kill")){
			dead = true;
			animator.SetBool ("isDead", true);
		}

	}
	
	void OnTriggerEnter2D(Collider2D collider){
		if (collider.name == "Monster(Clone)") {
			animator.SetBool("isDead", true);
			dead = true;
			
		}
	}

}
