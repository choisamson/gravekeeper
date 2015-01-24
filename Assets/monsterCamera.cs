using UnityEngine;
using System.Collections;

public class monsterCamera : MonoBehaviour {
	
	public string cameraName = "mcamera";
	
	// Use this for initialization
	void Start () {
		if(networkView.isMine){
			this.camera.enabled = true;
		}
		else{
			this.camera.enabled = false;
		}
	}
}
