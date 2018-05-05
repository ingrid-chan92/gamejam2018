﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HipsterController : MonoBehaviour {
    public float speed;
    public int strength;
    public int health;
    private string state;
    private string direction;
    private GameObject Player;
    private int attacking;
    private float atkTimer;
    private Animator animator;
    private bool dead;
    private bool fullDead;
    private float deadTime;
    private float fullAttackTime;
    private float attackDist;


    // Use this for initialization
    void Start () {
        state = "walk";
        direction = "left";
        Player = Managers.GetInstance().GetPlayerManager().GetPlayer();
        animator = this.GetComponentInChildren<Animator>();
        attacking = 1;
        fullAttackTime = 1.2f;
        atkTimer = fullAttackTime;
        dead = false;
        fullDead = false;
        attackDist = 1f;
        deadTime = 3f;
    }

    // Update is called once per frame
    void Update () {
        if (fullDead)
        {
            return;
        }
        if (dead)
        {
            deadTime -= Time.deltaTime;
            if (deadTime <= 0)
            {
                fullDead = true;
            }
            return;
        }

        if (state == "attack" && attacking == 1 && atkTimer > 0)
        {
            atkTimer -= Time.deltaTime;
        }
        if (attacking == 1 && atkTimer <= 0)
        {
            atkTimer = fullAttackTime;
            attacking = 0;
        }
        Vector3 playPos = Player.transform.position;

        state = getState(playPos);
        direction = facePlayer(playPos);

        if (state == "move")
        {
            walkToPlayer(playPos);
        }
        if (state == "attack")
        {
            attack(playPos);
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            this.die();
        }
    }

    private string getState(Vector3 playerPos)
    {
        if (dead)
        {
            return "dead";
        }

        if (Vector3.Distance(playerPos, transform.position) > attackDist)
        {
            animator.SetInteger("State", 0);
            return "move";
        } else
        {
            animator.SetInteger("State", 1);
            return "attack";
        }
    }

    public void walkToPlayer(Vector3 playerPos)
    {
        float step = speed * Time.deltaTime;

        transform.position = Vector3.MoveTowards(transform.position, playerPos, step);

    }

    public void attack (Vector3 playerPos)
    {
        if (attacking == 1)
        {
            return;
        }
        attacking = 1;
        if (Vector3.Distance(playerPos, transform.position) < attackDist)
        {
            Player.GetComponent<PlayerController>().damage(strength);
        }
    }

    private string facePlayer(Vector3 playerPos)
    {
        if (playerPos.x < transform.position.x)
        {
            if (direction != "left")
            { 
                transform.Rotate(0, 180, 0);
            }
            return "left";
        } else
        {
            if (direction != "right")
            {
                transform.Rotate(0, -180, 0);
            }
            return "right";
        }
    }

    public void Damage(int damageVal)
    {
        health -= damageVal;
        if (health <= 0)
        {
            die();
        }
    }

    private void die()
    {
        animator.SetInteger("State", 2);
        dead = true;        
    }

    public bool isDead()
    {
        return fullDead;
    }
}
