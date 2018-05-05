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
   // private Animator animator;


    // Use this for initialization
    void Start () {
        speed = 1f;
        health = 30;
        state = "idle";
        direction = "left";
        Player = Managers.GetInstance().GetPlayerManager().GetPlayer();
//        animator = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update () {
        if (Managers.GetInstance().GetGameStateManager().CurrentState != Enums.GameStateNames.GS_03_INPLAY)
        {
    //        return;
        }

        Vector3 playPos = Player.transform.position;

        state = getState(playPos);
        direction = facePlayer(playPos);
        Debug.Log("direction " + direction);
        Debug.Log("state " + state);

        if (state == "move")
        {
            walkToPlayer(playPos);
        }


    }

    private string getState(Vector3 playerPos)
    {

        if (Vector3.Distance(playerPos, transform.position) > 10)
        {
         //   animator.SetInteger("State", 1);
            return "idle";
        } else if (Vector3.Distance(playerPos, transform.position) > .7f)
        {
        //    animator.SetInteger("State", 2);
            return "move";
        } else
        {
         //   animator.SetInteger("State", 3);
            return "attack";
        }
    }

    public void walkToPlayer(Vector3 playerPos)
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, playerPos, step);

    }

    public void idle ()
    {

    }

    public void attack ()
    {


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
    }
}
