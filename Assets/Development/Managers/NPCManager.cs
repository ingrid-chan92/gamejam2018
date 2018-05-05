﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPCManager : MonoBehaviour {
    private GameObject NPC;
    private int NPCCount;
    private List<HipsterController> m_hipster = new List<HipsterController>();


    // Use this for initialization
    void Start () {
        NPCCount = 0;
        SpawnHipster();
    }
	
	// Update is called once per frame
	void Update () {

        HipsterController destroyNPC = null;
        foreach (HipsterController npc in m_hipster)
        {
            if (npc.isDead())
            {
                destroyNPC = npc;
              //  Destroy(npc.gameObject);
            }
        }
        if (destroyNPC)
        {
            //m_hipster.Remove(destroyNPC);
        }
    }

    public void SpawnHipster()
    {
        NPC = Managers.GetInstance().GetGameProperties().HipsterPrefab;
        GameObject npc = GameObject.Instantiate(NPC);
        m_hipster.Add(npc.GetComponent<HipsterController>());
        NPCCount++;
    }

    public void KillHipster()
    {

    }


}
