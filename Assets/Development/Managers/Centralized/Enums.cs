using UnityEngine;
using System.Collections;

public class Enums : MonoBehaviour {

	public enum GameStateNames
	{
		GS_00_NULL = -1,
		GS_01_MENU = 0,
		GS_02_LOADING = 1, 
		GS_03_INPLAY,
		GS_04_LEAVING,
        GS_05_MENURELOAD

    };

	public enum BookTypes {
		Null = 0,
		NonFiction = 1,
		Horror = 2,
		Fantasy,
		SciFi,
		Romance,
		Children,
		Mystery,
		Classics,
		Art,
		Tragedy,
	};
}
