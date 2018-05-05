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
        foreach (HipsterController npc in m_hipster)
        {
            if (npc.isDead())
            {
                destroyNPC = npc;
                Destroy(npc.gameObject);
                break;
            }
        }
        if (destroyNPC)
        {
            m_hipster.Remove(destroyNPC);
        }
    }

    public void SpawnHipster(Vector3 position)
    {
        if (Random.Range(0.0f,1.0f) >= 0.5f)
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

    public bool allEnemiesDead()
    {
        if (m_hipster.Count == 0)
        {
            return true;
        }
        return false;
    }


}
