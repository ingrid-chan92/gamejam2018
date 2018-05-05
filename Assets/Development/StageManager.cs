using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour {

    private Sprite sprite;
    public Texture2D tex;

    // Use this for initialization
    void Start () {

        tex = Resources.Load("Tower") as Texture2D;
        sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);

        Instantiate<Sprite>(sprite);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
