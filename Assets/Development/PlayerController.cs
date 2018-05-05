using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speed = 2f;
    public float ySpeedMult = 0.5f; // Multiplier for speed when moving up or down
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
            transform.rotation = Quaternion.identity;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position = transform.position + step * Vector3.right;
            transform.rotation = Quaternion.AngleAxis(180, Vector3.up);

            if (transform.position.x > camera.transform.position.x)
            {
                camera.transform.Translate(step * Vector3.right);
            }
        }
        if (Input.GetKey(KeyCode.W))
        {
            transform.position = transform.position + step * Vector3.up * ySpeedMult;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position = transform.position + step * Vector3.down * ySpeedMult;
        }
    }
}
