using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speed = 10f;
    private Camera camera;

	// Use this for initialization
	void Start () {
        camera = Camera.main;
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

            if(transform.position.x > camera.transform.position.x)
            {
                camera.transform.Translate(step * Vector3.right);
            }
        }
    }
}
