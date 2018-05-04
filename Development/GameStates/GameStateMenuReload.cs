using UnityEngine;
using System.Collections;

public class GameStateMenuReload : GameStateBase {

	public GameStateMenuReload(GameStateManager p_gameStateManager)
	{
		m_gameStateManager = p_gameStateManager;
	}


	public override void EnterState(Enums.GameStateNames p_prevState)
	{
		Debug.Log ("In Menu Reload State");
		//spawn menu here
	}

	public override void UpdateState()
	{
		//add some loading screen shenanigans before this
		//m_gameStateManager.ChangeGameState(Enums.GameStateNames.GS_02_LOADING);
	}

	public override void ExitState(Enums.GameStateNames p_nextState)
	{
		Application.LoadLevel ("OPENSCENE");
	}

}
