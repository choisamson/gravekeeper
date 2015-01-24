using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SinMove_VLS : MonoBehaviour 
{
    public float magnitude = 5;
    public float speed = 5;

    private Vector3 origPos = Vector3.zero;

    void Start()
    {
        origPos = transform.position;
    }

    void Update()
    {
		if (Network.isServer) {
				Vector3 moveDir = new Vector3 (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical"), 0);
				float speed = 5; 
				transform.Translate (speed * moveDir * Time.deltaTime);
		}
    }
}
