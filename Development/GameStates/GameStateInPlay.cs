using UnityEngine;
using System.Collections;

public class GameStateInPlay : GameStateBase {

	public GameStateInPlay(GameStateManager p_gameStateManager)
	{
		m_gameStateManager = p_gameStateManager;
	}


	public override void EnterState(Enums.GameStateNames p_prevState)
	{
		Debug.Log ("In Play");
		//spawn menu here
	}

	public override void UpdateState()
	{
		
	}

	public override void ExitState(Enums.GameStateNames p_nextState)
	{
		Application.LoadLevel ("GameOver");
	}
}
