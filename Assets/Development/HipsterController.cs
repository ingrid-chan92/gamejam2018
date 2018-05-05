using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HipsterController : MonoBehaviour {
    private float speed;
    private int strength;
    private int health;
    private string state;
    private string direction;
    private GameObject Player;
    private int attacking;
    private float atkTimer;
    private Animator animator;
    public bool dead;


    // Use this for initialization
    void Start () {
        speed = 1f;
        health = 30;
        state = "walk";
        direction = "left";
        strength = 8;
        Player = Managers.GetInstance().GetPlayerManager().GetPlayer();
        animator = this.GetComponentInChildren<Animator>();
        attacking = 0;
        atkTimer = 1f;
        dead = false;
    }

    // Update is called once per frame
    void Update () {
        if (dead)
        {
            return;
        }

        if (attacking == 1 && atkTimer > 0)
        {
            atkTimer -= Time.deltaTime;
        }
        if (attacking == 1 && atkTimer <= 0)
        {
            atkTimer = 1f;
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
            attack();
        }


    }

    private string getState(Vector3 playerPos)
    {
        if (dead)
        {
            return "dead";
        }

        if (Vector3.Distance(playerPos, transform.position) > .7f)
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

    public void attack ()
    {
        if (attacking == 1)
        {
            return;
        }
        attacking = 1;
      
        Player.GetComponent<PlayerController>().damage(8);


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
        return dead;
    }
}
