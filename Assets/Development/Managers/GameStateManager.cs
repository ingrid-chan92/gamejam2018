using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameStateManager : MonoBehaviour {
	
	private Dictionary<Enums.GameStateNames, GameStateBase> m_gameStateDictionary = new Dictionary<Enums.GameStateNames, GameStateBase>();
	private GameStateBase m_currentGameState = null;
	private Enums.GameStateNames m_currentGameStateIndex = Enums.GameStateNames.GS_00_NULL;
	private Enums.GameStateNames m_nextGameStateIndex = Enums.GameStateNames.GS_00_NULL;
	private bool m_initialised = false;

	public Enums.GameStateNames CurrentState
	{
		get { return m_currentGameStateIndex; }
	}


	// Update is called once per frame
	void Update () {
        // State machine shenanigans 
        if (!m_initialised)
			return;

		if (m_currentGameState != null)
			m_currentGameState.UpdateState();

		if (m_nextGameStateIndex != Enums.GameStateNames.GS_00_NULL)
		{
			if (m_currentGameState != null)
				m_currentGameState.ExitState(m_nextGameStateIndex);
			m_currentGameState = m_gameStateDictionary[m_nextGameStateIndex];
			m_currentGameState.EnterState(m_currentGameStateIndex);
			m_currentGameStateIndex = m_nextGameStateIndex;
			m_nextGameStateIndex = Enums.GameStateNames.GS_00_NULL;
		}

        //Camera camera = Camera.main;
        //camera.transform.Translate(new Vector3(0.01f, 0, 0));
	}

	public void Init()
    {

        GameObject.Instantiate(Managers.GetInstance().GetGameProperties().CameraPrefab);

        // Initialise the bookstateDictionary
        m_gameStateDictionary.Add(Enums.GameStateNames.GS_01_MENU, new GameStateMenu(this));
		m_gameStateDictionary.Add(Enums.GameStateNames.GS_02_LOADING, new GameStateLoading(this));
		m_gameStateDictionary.Add(Enums.GameStateNames.GS_03_INPLAY, new GameStateInPlay(this));
		m_gameStateDictionary.Add(Enums.GameStateNames.GS_04_LEAVING, new GameStateLeaving(this));
        m_gameStateDictionary.Add(Enums.GameStateNames.GS_05_MENURELOAD, new GameStateMenuReload(this));

        //start the state machine
        ChangeGameState(Enums.GameStateNames.GS_01_MENU); //starts in the menu state

		m_initialised = true;

    }

	//Change the game state (occurs on next frame)
	public void ChangeGameState(Enums.GameStateNames nextState)
	{
		if (!m_gameStateDictionary.ContainsKey(nextState))
			return;

		m_nextGameStateIndex = nextState;
	}
}
