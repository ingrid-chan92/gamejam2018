﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speed = 2f;
    public float ySpeedMult = 0.5f; // Multiplier for speed when moving up or down
    public int startHealth = 100;
    public int punchDamage = 10;
    public float iFrameTime = 1.0f;
    private int currentHealth;
    private Camera camera;
    private PlayerStates state = PlayerStates.idle;
    private Animator animator;
    private Collider2D hurtbox;
    private Vector3 initScale;
    private LayerMask enemiesMask;
    private float remainingIFrameTime = 0f;
    public int numRaccoons = 0;

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
        animator = this.GetComponentInChildren<Animator>();
        hurtbox = this.GetComponentInChildren<CircleCollider2D>();
        this.initScale = transform.localScale;
        enemiesMask = ~((1 << 8) | (1 << 9));
    }

    void die() {
        Debug.Log("The player has been killed because they are bad at video games");
        animator.SetBool("dying", true);
        state = PlayerStates.dying;
        //Debug.Log("I am slain");
    }

    public void damage(int amount) {
        if (this.currentHealth < 0) {
            return;
        }
        if (this.remainingIFrameTime > 0 && amount > 0)
        {
            Debug.Log("player is invincible right now, for " + remainingIFrameTime + " more seconds");
            return;
        }
        if (amount > 0)
        {
            this.remainingIFrameTime = iFrameTime;
        }

        GameObject damageTextPrefab = Managers.GetInstance().GetGameProperties().FloatText;

        GameObject damageText = GameObject.Instantiate(damageTextPrefab);
        damageText.transform.position = transform.position + (Vector3.up * 0.4f);
        FloatTextController cntrl = damageText.GetComponent<FloatTextController>();
        if (amount > 0)
        {
            cntrl.setText(amount.ToString());
            cntrl.setColor(Color.red);
        }
        if (amount < 0)
        {
            cntrl.setText("++" + (amount * -1).ToString());
            cntrl.setColor(Color.cyan);
        }
        

        this.currentHealth -= amount;
        if (this.currentHealth > this.startHealth) {
            this.currentHealth = this.startHealth;
        }
        //Debug.Log("Damaging player by " + amount + " units");
        Debug.Log("Current Health = " + currentHealth);
        if (this.currentHealth < 0) {
            this.die();
        }
    }

    // Update is called once per frame
    void Update() {

        if (this.remainingIFrameTime > 0)
        {
            this.remainingIFrameTime -= Time.deltaTime;
        }

        Vector3 walkVector = Vector3.zero;

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

        if (Input.GetKeyDown(KeyCode.Space)) {
            attack();
        }

        if (Input.GetKeyDown(KeyCode.F)) {
            throwRaccoon();
        }

        if (walkVector != Vector3.zero) {
            this.walk(walkVector);
            animator.SetBool("walking", true);
        } else {
            animator.SetBool("walking", false);
        }

    }

    void attack() {
        animator.SetTrigger("punch");
        Collider2D[] results = new Collider2D[20];

        ContactFilter2D filter = new ContactFilter2D();
        filter.layerMask = enemiesMask;
        filter.useLayerMask = true;
        int resultCount = hurtbox.OverlapCollider(filter, results);
        Debug.Log("Attack contacting " + resultCount + " other things");

        for (int i = 0; i < resultCount; i++) {
            HipsterController enemy;
            enemy = results[i].GetComponentInParent<HipsterController>();
            enemy.Damage(punchDamage);
        }
    }

    void throwRaccoon() {
        if (this.numRaccoons <= 0)
        {
            return;
        }
        this.numRaccoons--;

        animator.SetTrigger("throwing");
        GameObject raccoonPrefab = Managers.GetInstance().GetGameProperties().RaccoonPrefab;

        GameObject raccoon = GameObject.Instantiate(raccoonPrefab);
        raccoon.transform.SetPositionAndRotation(transform.position, transform.rotation);


        Vector3 initialVelocity;

        if (transform.localScale.x < 0) {
            initialVelocity = new Vector3(-2, 2);
            Vector3 racScale = raccoon.transform.localScale;
            raccoon.transform.localScale = new Vector3(-1 * racScale.x, racScale.y, racScale.z);

        } else {
            initialVelocity = new Vector3(2, 2);
            
        }

        RaccoonController cntrl = raccoon.GetComponent<RaccoonController>();
        cntrl.SetVelocity(initialVelocity);
        cntrl.SetShadowY(transform.position.y - 0.25f);
    }

    void walk(Vector3 walkVector) {
        if (this.state == PlayerStates.dead || this.state == PlayerStates.dying) {
            return;
        }

        

        this.state = PlayerStates.walking;
        float step = speed * Time.deltaTime;

        transform.position += walkVector * step;
        if (walkVector.x > 0) {
            //transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
            Vector3 newthing = this.initScale;
            //newthing.x *= -1;
            transform.localScale = newthing;
        }
        if (walkVector.x < 0) {
            //transform.rotation = Quaternion.identity;
            Vector3 newthing = this.initScale;
            newthing.x *= -1;
            transform.localScale = newthing;
        }

        if(transform.position.x > camera.transform.position.x && !Managers.GetInstance().GetStageManager().ActiveScene()) {
            camera.transform.Translate(step * Vector3.right);
        }
    }
}
