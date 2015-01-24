using UnityEngine;
using System.Collections;

public class MonsterCamera: MonoBehaviour {
	
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
