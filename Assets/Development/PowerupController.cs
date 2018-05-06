using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupController : MonoBehaviour {
    public int heal;
    public bool used;
    private GameObject Player;

    // Use this for initialization
    void Start () {
        used = false;
        Player = Managers.GetInstance().GetPlayerManager().GetPlayer();

    }

    // Update is called once per frame
    void Update () {
        if (used)
        {
            return;
        }

        if (heal > 0 && Vector3.Distance(Player.transform.position, transform.position) < .5)
        {
            Player.GetComponent<PlayerController>().damage(heal * -1);
            used = true;
        }

        if (heal == 0 && Vector3.Distance(Player.transform.position, transform.position) < .5)
        {
            Player.GetComponent<PlayerController>().numRaccoons++;
            used = true;
        }

    }
}
