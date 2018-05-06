﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaccoonController : MonoBehaviour {

    private Vector3 velocity = Vector3.zero;
    private LayerMask enemiesMask;
    private Collider2D hitbox;
    private enum racState
    {
        Launched,
        Latched
    }
    private racState curState;
    private HipsterController attachedHipster;
    private Vector3 headOffset = new Vector3(0, 0.5f, 5);
    private Animator animator;
    public int biteDamage = 1;
    public float biteIntervalSeconds = 1.0f;
    private float timeSinceBite;

    // Use this for initialization
    void Start () {
        //velocity = Vector3.zero;
        enemiesMask = ~((1 << 8) | (1 << 9));
        hitbox = this.GetComponentInChildren<CircleCollider2D>();
        curState = racState.Launched;
        animator = this.GetComponentInChildren<Animator>();
        this.timeSinceBite = biteIntervalSeconds;
    }

    public void SetVelocity(Vector3 newVelocity) {
        this.velocity = newVelocity;
    }
	
	// Update is called once per frame
	void Update () {
        // Accelerate due to gravity
        Vector3 deltaV = Vector3.down * Time.deltaTime * Constants.gravity;
        velocity += deltaV;

        // Move due to velocity
        transform.position += (velocity * Time.deltaTime);

        if (transform.position.y < -10)
        {
            Destroy(gameObject);
        }

        if (curState == racState.Launched)
        {
            checkIfHittingEnemy();
        }
        
        if (curState == racState.Latched)
        {
            bite();
        }
    }

    void bite()
    {
        if (attachedHipster == null)
        {
            return;
        }

        this.transform.position = attachedHipster.transform.position + headOffset;
 //       this.transform.Rotate(new Vector3(0, 0, 1), -360 * Time.deltaTime);

        if (timeSinceBite < biteIntervalSeconds)
        {
            timeSinceBite += Time.deltaTime;
            return;
        }

        timeSinceBite -= biteIntervalSeconds;

        Collider2D[] results = new Collider2D[20];

        ContactFilter2D filter = new ContactFilter2D();
        filter.layerMask = enemiesMask;
        filter.useLayerMask = true;
        int resultCount = hitbox.OverlapCollider(filter, results);
        Debug.Log("Attack contacting " + resultCount + " other things");

        for (int i = 0; i < resultCount; i++)
        {
            HipsterController enemy;
            enemy = results[i].GetComponentInParent<HipsterController>();
            enemy.Damage(biteDamage);
        }
    }

    void checkIfHittingEnemy()
    {
        Collider2D[] results = new Collider2D[20];

        ContactFilter2D filter = new ContactFilter2D();
        filter.layerMask = enemiesMask;
        filter.useLayerMask = true;
        int resultCount = hitbox.OverlapCollider(filter, results);
        if (resultCount == 0)
        {
            return;
        }
        Debug.Log("raccoon contacting " + resultCount + " other things");

        Collider2D hitEnemyCollider = results[0];
        HipsterController hitHipster = hitEnemyCollider.GetComponentInParent<HipsterController>();
        if (hitHipster == null)
        {
            // TODO: apply to boss as well
            return;
        }

        curState = racState.Latched;
        attachedHipster = hitHipster;
        animator.SetBool("biting", true);
    }
}