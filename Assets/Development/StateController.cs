using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController : MonoBehaviour {

    public string state;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.Escape) && state == "Menu")
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape))
        {
            if (state == "Menu")
            {
                state = "InPlay";
                Application.LoadLevel("startScene");
            }
            else if (state != "InPlay")
            {
                state = "Menu";
                Application.LoadLevel("StartMenu");
            }
        }
    }

    public void Winner()
    {
        Application.LoadLevel("WinScene");

    }

    public void GameOver()
    {
        Application.LoadLevel("GameOver");

    }
}
