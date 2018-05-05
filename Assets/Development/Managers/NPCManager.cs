using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPCManager : MonoBehaviour {
    private GameObject NPC;
    private int NPCCount;
    private List<HipsterController> m_hipster = new List<HipsterController>();


    // Use this for initialization
    void Start () {
        NPCCount = 0;

	}
	
	// Update is called once per frame
	void Update () {
	}

    public void SpawnLineHipster()
    {
        //NPC = Managers.GetInstance().GetGameProperties().HipsterPrefab;
        GameObject npc = GameObject.Instantiate(NPC);
        m_hipster.Add(npc.GetComponent<HipsterController>());
        NPCCount++;
    }


}
