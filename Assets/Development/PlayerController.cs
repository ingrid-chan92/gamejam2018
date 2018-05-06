using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speed = 2f;
    public float ySpeedMult = 0.5f; // Multiplier for speed when moving up or down
    public int startHealth = 100;
    private int currentHealth;
    private Camera camera;
    private PlayerStates state = PlayerStates.idle;
    private float deadTime = 3f;

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
    }

    public void damage(int amount) {
        this.currentHealth -= amount;
        Debug.Log("Current Health = " + currentHealth);
        if (this.currentHealth < 0) {
            this.die();
        }
    }

    // Update is called once per frame
    void Update() {
        Vector3 walkVector = Vector3.zero;

        if (state == PlayerStates.dead || state == PlayerStates.dying)
        {
            deadTime -= Time.deltaTime;
            if (deadTime <= 0) {
                Application.LoadLevel("GameOver");
            }
        }

        if (Input.GetKeyDown(KeyCode.J)) {
            this.die();
        }

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

        if(transform.position.x > camera.transform.position.x && !Managers.GetInstance().GetStageManager().ActiveScene()) {
            camera.transform.Translate(step * Vector3.right);
        }
    }
}
