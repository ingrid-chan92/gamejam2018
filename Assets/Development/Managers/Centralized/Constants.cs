using UnityEngine;
using System.Collections;

public class Constants : MonoBehaviour {

    public enum GameStateNames
    {
        GS_00_NULL = -1,
        GS_01_MENU = 0,
        GS_02_LOADING = 1,
        GS_03_INPLAY,
        GS_04_LEAVING,
        GS_05_MENURELOAD
    }

    public const float gravity = 3.0f;
}
