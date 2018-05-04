using UnityEngine;
using System.Collections;

abstract public class GameStateBase {


	protected GameStateManager m_gameStateManager;

	public GameStateBase()
	{

	}

	public GameStateBase(GameStateManager p_gameStateManager)
	{
		m_gameStateManager = p_gameStateManager;
	}


	public virtual void EnterState(Enums.GameStateNames p_prevState)
	{

	}

	public virtual void UpdateState()
	{

	}

	public virtual void ExitState(Enums.GameStateNames p_nextState)
	{

	}
}
