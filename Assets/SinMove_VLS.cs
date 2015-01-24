using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SinMove_VLS : MonoBehaviour 
{
    public Vector3 axis = new Vector3(0, 0, 1);
    public float magnitude = 5;
    public float speed = 5;

    private Vector3 origPos = Vector3.zero;

    void Start()
    {
        origPos = transform.position;
    }

    void Update()
    {
		Vector3 moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		float speed = 5; 
		transform.Translate(speed * moveDir * Time.deltaTime);
    }
}
