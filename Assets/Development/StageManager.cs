using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour {
    private Texture2D tex;
    private Sprite sprite;

    private List<GameObject> backgroundObjects = new List<GameObject>();

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
        tex = Resources.Load("background/bg_0002_ground") as Texture2D;

        sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0f, 0.5f), 100);

        for (int i = 0; i < 3; i++) {
            GameObject obj = new GameObject();
            //Attach a SpriteRenender to the newly created gameobject
            SpriteRenderer rend = obj.AddComponent(typeof(SpriteRenderer)) as SpriteRenderer;
            obj.transform.SetPositionAndRotation(PixelToGame(camera.transform.position.x - camera.rect.width + (i * tex.width), 0, 0), obj.transform.rotation);

            rend.sprite = sprite;

            backgroundObjects.Add(obj);
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (backgroundObjects.Count > 0) {
            Camera camera = Camera.main;

            GameObject obj = backgroundObjects[0];
            if (GameToPixel(camera.transform.position.x, 0, 0).x - camera.rect.width > GameToPixel(obj.transform.position.x, 0, 0).x + obj.GetComponent<SpriteRenderer>().sprite.rect.width) {
                
                GameObject newObj = new GameObject();
                //Attach a SpriteRenender to the newly created gameobject
                SpriteRenderer rend = newObj.AddComponent(typeof(SpriteRenderer)) as SpriteRenderer;
                newObj.transform.SetPositionAndRotation(PixelToGame(GameToPixel(obj.transform.position.x, 0, 0).x + (3 * obj.GetComponent<SpriteRenderer>().sprite.rect.width), 0, 0), newObj.transform.rotation);

                rend.sprite = sprite;

                backgroundObjects.Add(newObj);

                GameObject.Destroy(obj);
                backgroundObjects.Remove(obj);
            }
        }
	}
}
