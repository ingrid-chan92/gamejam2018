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


    // Use this for initialization
    void Start () {
        speed = 10f;
        health = 30;
        state = "idle";
        Player = Managers.GetInstance().GetPlayerManager().GetPlayer();
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


    }

    private string getState(Vector3 playerPos)
    {

        if (Vector3.Distance(playerPos, transform.position) > 10)
        {
            return "idle";
        } else if (Vector3.Distance(playerPos, transform.position) > 0.7f)
        {   
            return "move";
        } else
        {
            return "attack";
        }
    }

    public void walkToPlayer()
    {


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
            return "left";
        } else
        {
            return "right";
        }
    }
}
