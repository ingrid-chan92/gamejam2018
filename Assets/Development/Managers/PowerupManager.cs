using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupManager : MonoBehaviour {
    private GameObject PU;
    private List<PowerupController> m_powerup = new List<PowerupController>();

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        PowerupController destroyItem = null;

        foreach (PowerupController pu in m_powerup)
        {
            if (pu.used)
            {
                destroyItem = pu;
                Destroy(pu.gameObject);
                break;
            }
        }
        if (destroyItem)
        {
            m_powerup.Remove(destroyItem);
        }

    }

    public void SpawnPowerup(Vector3 position)
    {
        float randVal = Random.Range(0.0f, 10.0f);
        if (randVal < 7)
        {
            PU = Managers.GetInstance().GetGameProperties().SmallHealthPrefab;
        } else if (randVal < 9)
        {
            PU = Managers.GetInstance().GetGameProperties().BigHealthPrefab;
        } else
        {
            //
        }

        GameObject powerup = GameObject.Instantiate(PU);
        powerup.transform.SetPositionAndRotation(position, powerup.transform.rotation);
        m_powerup.Add(powerup.GetComponent<PowerupController>());
    }
    
}
