using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour {
    private Texture2D tex;
    private Sprite sprite;
    private GameObject obj;

    Vector3 PixelToGame(float x, float y, float z) {
        Vector3 result = new Vector3(x / 100, y / 100, z / 100);
        return result;
    }

    Vector3 GameToPixel(float x, float y, float z)
    {
        Vector3 result = new Vector3(x * 100, y * 100, z * 100);
        return result;
    }

    // Use this for initialization
    void Start () {
        Camera camera = Camera.main;

        //sprite = Resources.Load("Tower", typeof(Sprite)) as Sprite;
        tex = Resources.Load("Tower") as Texture2D;

        sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0f, 0.5f), 100);

        obj = new GameObject("Background");

        //Attach a SpriteRenender to the newly created gameobject
        SpriteRenderer rend = obj.AddComponent(typeof(SpriteRenderer)) as SpriteRenderer;
        obj.transform.SetPositionAndRotation(PixelToGame(camera.transform.position.x-320, 0, 0), obj.transform.rotation);

        rend.sprite = sprite;
	}
	
	// Update is called once per frame
	void Update () {
		if (GameToPixel(Camera.main.transform.position.x, 0, 0).x > obj.GetComponent<SpriteRenderer>().sprite.rect.width) {
            GameObject.Destroy(obj);
        }
	}
}
