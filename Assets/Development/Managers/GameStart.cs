using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		gameObject.AddComponent<Managers>();
	}
}
