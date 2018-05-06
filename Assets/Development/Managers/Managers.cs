using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour {

	private static Managers m_instance;

	private GameStateManager m_gamestatemanager;
	private GameProperties m_properties;
	private PlayerManager m_playermanager;
	private NPCManager m_npcmanager;
    private StageManager m_stagemanager;
    private PowerupManager m_powerupmanager;

	//Accessors
	public static Managers GetInstance() {
		return m_instance;
	}

	public GameStateManager GetGameStateManager() {
		return m_gamestatemanager;
	}

	public GameProperties GetGameProperties(){
		return m_properties;
	}

	public PlayerManager GetPlayerManager() {
		return m_playermanager;
	}

	public NPCManager GetNPCManager() {
		return m_npcmanager;
	}

    public StageManager GetStageManager()
    {
        return m_stagemanager;
    }

    public PowerupManager GetPowerupManager()
    {
        return m_powerupmanager;
    }

    //Public Variables
    public void Awake()	{
		m_instance = this;
    }

	public void Start() {
		//Create managers here
		m_properties = gameObject.GetComponent<GameProperties>();
		m_gamestatemanager = gameObject.AddComponent<GameStateManager>();
		m_playermanager = gameObject.AddComponent<PlayerManager> ();
		m_npcmanager = gameObject.AddComponent<NPCManager> ();
        m_stagemanager = gameObject.AddComponent<StageManager> ();
        m_powerupmanager = gameObject.AddComponent<PowerupManager>();

        //preferably call init after
        m_gamestatemanager.Init();
	}

}
