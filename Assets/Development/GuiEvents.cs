using UnityEngine;
using System.Collections;

public class GuiEvents : MonoBehaviour {


	public void StartGame() {
		Managers.GetInstance ().GetGameStateManager ().ChangeGameState (Enums.GameStateNames.GS_02_LOADING);
	}
}
