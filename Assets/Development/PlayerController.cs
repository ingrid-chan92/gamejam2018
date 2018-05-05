using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speed = 10f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float step = speed * Time.deltaTime;

		if (Input.GetKey(KeyCode.A))
        {
            transform.position = transform.position + step * Vector3.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position = transform.position + step * Vector3.right;
        }
    }
}
