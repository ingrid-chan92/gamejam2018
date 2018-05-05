using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speed = 2f;
    public float ySpeedMult = 0.5f; // Multiplier for speed when moving up or down
    public float startHealth = 100f;
    private float currentHealth;
    private Camera camera;
    private PlayerStates state = PlayerStates.idle;

    private enum PlayerStates {
        stopped,
        walking,
        idle,
        attack,
        throwing,
        dying,
        dead
    }

    // Use this for initialization
    void Start() {
        camera = Camera.main;
        currentHealth = startHealth;
    }

    void die() {
        state = PlayerStates.dying;
        Debug.Log("I am slain");
    }

    public void damage(int amount) {
        this.currentHealth -= amount;
        if (this.currentHealth < 0) {
            die();
        }
    }

    // Update is called once per frame
    void Update() {
        Vector3 walkVector = Vector3.zero;

        if (Input.GetKey(KeyCode.A)) {
            walkVector += Vector3.left;
        }
        if (Input.GetKey(KeyCode.D)) {
            walkVector += Vector3.right;
        }
        if (Input.GetKey(KeyCode.W)) {
            walkVector += (Vector3.up * ySpeedMult);
        }
        if (Input.GetKey(KeyCode.S)) {
            walkVector += (Vector3.down * ySpeedMult);
        }

        if (walkVector != Vector3.zero) {
            this.walk(walkVector);
        }
    }

    void walk(Vector3 walkVector) {
        if (this.state == PlayerStates.dead || this.state == PlayerStates.dying) {
            return;
        }

        this.state = PlayerStates.walking;
        float step = speed * Time.deltaTime;

        transform.position += walkVector * step;
        if (walkVector.x > 0) {
            transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
        }
        if (walkVector.x < 0) {
            transform.rotation = Quaternion.identity;
        }

        if (transform.position.x > camera.transform.position.x) {
            camera.transform.Translate(step * Vector3.right);
        }
    }
}
