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

        HipsterController destroyNPC = null;
        Vector3 deathSpot = Vector3.zero;
        foreach (HipsterController npc in m_hipster)
        {
            if (npc.isDead())
            {
                deathSpot = npc.transform.position;
                destroyNPC = npc;
                Destroy(npc.gameObject);
                break;
            }
        }
        if (destroyNPC)
        {
            if (Random.Range(0, 10) < 1)
            {
                deathSpot.z = -10f;
                Managers.GetInstance().GetPowerupManager().SpawnPowerup(deathSpot);
            }
            if (destroyNPC.strength > 10) {
                Application.LoadLevel("WinScene");
            }
            m_hipster.Remove(destroyNPC);
        }
    }

    public void SpawnHipster(Vector3 position)
    {
        if (Random.Range(0.0f,3.0f) < 1f)
        {
            NPC = Managers.GetInstance().GetGameProperties().HipsterPrefab;
        }
        else
        {
            NPC = Managers.GetInstance().GetGameProperties().Hipster2Prefab;
        } 
        GameObject npc = GameObject.Instantiate(NPC);
        npc.transform.SetPositionAndRotation(position, npc.transform.rotation);
        m_hipster.Add(npc.GetComponent<HipsterController>());
        NPCCount++;
    }

    public void SpawnBoss(Vector3 position)
    {
        NPC = Managers.GetInstance().GetGameProperties().BossPrefab;
        GameObject npc = GameObject.Instantiate(NPC);
        npc.transform.SetPositionAndRotation(position, npc.transform.rotation);
        m_hipster.Add(npc.GetComponent<HipsterController>());
        NPCCount++;
    }

    public bool allEnemiesDead()
    {
        if (m_hipster.Count == 0)
        {
            return true;
        }
        return false;
    }


}
