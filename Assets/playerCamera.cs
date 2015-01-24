using UnityEngine;
using System.Collections;

public class playerCamera : MonoBehaviour {

	public string cameraName = "pcamera";

	// Use this for initialization
	void Start () {
		if(networkView.isMine){
			Debug.Log ("is human");
			this.camera.enabled = true;
		}
		else{
			Debug.Log ("not human");
			this.camera.enabled = false;
		}
	}
}
