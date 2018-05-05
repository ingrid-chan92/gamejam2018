using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour {
    private Texture2D pothole1Texture;
    private Texture2D pothole2Texture;
    private Texture2D groundTexture;

    private Sprite pothole2Sprite;
    private Sprite pothole1Sprite;
    private Sprite groundSprite;

    private Camera camera;

    public float spawnTime = 5.0f;
    private float spawnTimer = 0.0f;

    public float potholeChance = 0.2f;

    private bool complete = true;
    private int waves = 0;
    private int currentScene = 0;

    private List<GameObject> backgroundObjects = new List<GameObject>();
    private List<GameObject> potholes = new List<GameObject>();

    Vector3 PixelToGame(float x, float y, float z) {
        Vector3 result = new Vector3(x / 100, y / 100, z / 100);
        return result;
    }

    Vector3 GameToPixel(float x, float y, float z)
    {
        Vector3 result = new Vector3(x * 100, y * 100, z * 100);
        return result;
    }

    public bool ActiveScene()
    {
        return !complete;
    }

    bool isOutOfBounds(GameObject obj) {
        float cameraLeftEdge = GameToPixel(camera.transform.position.x, 0, 0).x - camera.rect.width;
        float objRightEdge = GameToPixel(obj.transform.position.x, 0, 0).x + obj.GetComponent<SpriteRenderer>().sprite.rect.width;

        if (cameraLeftEdge > objRightEdge)
        {
            return true;
        }
        return false;
    }

    void addNewTile() {
        GameObject obj = new GameObject();

        if (backgroundObjects.Count == 0) {
            obj.transform.SetPositionAndRotation(PixelToGame(camera.transform.position.x - camera.rect.width * 2, 0, 0), obj.transform.rotation);
        } else {
            GameObject lastTile = backgroundObjects[backgroundObjects.Count - 1];
            obj.transform.SetPositionAndRotation(PixelToGame(GameToPixel(lastTile.transform.position.x, 0, 0).x + groundTexture.width, 0, 0), obj.transform.rotation);
        }

        SpriteRenderer rend = obj.AddComponent(typeof(SpriteRenderer)) as SpriteRenderer;
        rend.sprite = groundSprite;

        backgroundObjects.Add(obj);

        if (Random.Range(0.0f, 1.0f) < potholeChance)
        {
            float potholePosition = Random.Range(0.0f, 5.0f);
            GameObject pothole = new GameObject();
            SpriteRenderer renderer = pothole.AddComponent(typeof(SpriteRenderer)) as SpriteRenderer;

            Vector3 pos = GameToPixel(obj.transform.position.x, obj.transform.position.y, 0);
            pothole.transform.SetPositionAndRotation(PixelToGame(pos.x + Random.Range(0.0f, groundSprite.rect.width - pothole2Sprite.rect.width), pos.y + Random.Range(-100.0f, 80.0f), -0.01f), pothole.transform.rotation);

            renderer.sprite = pothole2Sprite;
            potholes.Add(pothole);
        }
    }

    void spawnNPCs()
    {
        for (int i = 0; i < 5; i++)
        {
            Vector3 spawnLocation = camera.transform.position + PixelToGame((camera.rect.width / 2 + 100) * -1, Random.Range(-320.0f, 320.0f), -1);
            Managers.GetInstance().GetNPCManager().SpawnHipster(spawnLocation);

            spawnLocation = camera.transform.position + PixelToGame((camera.rect.width / 2 + 100), Random.Range(-320.0f, 320.0f), -1);
            Managers.GetInstance().GetNPCManager().SpawnHipster(spawnLocation);
        }
    }

    void newScene(int waves)
    {
        complete = false;
        this.waves = waves;

        addNewTile();
    }

    // Use this for initialization
    void Start () {
        camera = Camera.main;
        
        pothole2Texture = Resources.Load("background/bg_0000_pothole2") as Texture2D;
        pothole1Texture = Resources.Load("background/bg_0001_pothole1") as Texture2D;
        groundTexture = Resources.Load("background/bg_0002_ground") as Texture2D;

        pothole2Sprite = Sprite.Create(pothole2Texture, new Rect(0, 0, pothole2Texture.width, pothole2Texture.height), new Vector2(0f, 0.5f), 100);
        pothole1Sprite = Sprite.Create(pothole1Texture, new Rect(0, 0, pothole1Texture.width, pothole1Texture.height), new Vector2(0f, 0.5f), 100);
        groundSprite = Sprite.Create(groundTexture, new Rect(0, 0, groundTexture.width, groundTexture.height), new Vector2(0f, 0.5f), 100);

        for (int i = 0; i < 3; i++) {
            addNewTile();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!complete && waves > 0)
        {
            spawnTimer -= Time.deltaTime;

            if (spawnTimer <= 0.0f)
            {
                spawnTimer = spawnTime;

                spawnNPCs();
                waves -= 1;

                if (waves <= 0)
                {
                    complete = true;
                    spawnTimer = 0.0f;
                }
            }
        }

        if (backgroundObjects.Count > 0)
        {
            GameObject obj = backgroundObjects[0];
            if (isOutOfBounds(obj))
            {
                GameObject.Destroy(obj);
                backgroundObjects.Remove(obj);
                
                currentScene += 1;
                newScene(currentScene);
            }
        }
        if (potholes.Count > 0)
        {
            GameObject obj = potholes[0];
            if (isOutOfBounds(obj))
            {
                GameObject.Destroy(obj);
                potholes.Remove(obj);
            }
        }
    }
}
