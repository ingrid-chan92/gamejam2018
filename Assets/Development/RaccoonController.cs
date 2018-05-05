using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaccoonController : MonoBehaviour {

    private Vector3 velocity = Vector3.zero;

	// Use this for initialization
	void Start () {
        //velocity = Vector3.zero;
	}

    public void SetVelocity(Vector3 newVelocity) {
        this.velocity = newVelocity;
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 deltaV = Vector3.down * Time.deltaTime;
        velocity += deltaV;
        transform.position += (velocity * Time.deltaTime);
	}
}
