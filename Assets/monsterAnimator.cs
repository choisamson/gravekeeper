using UnityEngine;
using System.Collections;

public class monsterAnimator : MonoBehaviour {


	private int invisCounter = 60;
	
	// Use this for initialization
	protected Animator animator;
	
	void Start () {
		
		animator = GetComponent<Animator> ();
	}
	// Update is called once per frame
	void Update () {
			
			
			
				if (Input.GetAxis("Horizontal") == 1 ||Input.GetAxis("Horizontal") == -1){
					animator.SetBool ("isWalking", true);	
					this.transform.localScale = new Vector3 (-5*Input.GetAxis("Horizontal"),5,1);
					invisCounter = 60;	
					animator.SetBool ("isCloak", false);
					animator.SetBool ("isUncloak", true);
				}
				else if(Input.GetAxis ("Vertical") == 1 || Input.GetAxis ("Vertical") == -1 ){
					animator.SetBool ("isWalking", true);	
					invisCounter = 60;	
					animator.SetBool ("isCloak", false);
					animator.SetBool ("isUncloak", true);
				}
				else{
					animator.SetBool ("isWalking", false);
					invisCounter--;
				}

			
			if(invisCounter < 0){
				animator.SetBool ("isCloak", true);
				animator.SetBool ("isUncloak", false);
			}
			
		
	}
}
