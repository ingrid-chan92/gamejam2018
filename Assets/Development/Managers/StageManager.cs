using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour {
    private Texture2D pothole1Texture;
    private Texture2D pothole2Texture;
    private Texture2D groundTexture;

    private Texture2D building1Texture;
    private Texture2D backBuildingTexture;

    private Sprite pothole2Sprite;
    private Sprite pothole1Sprite;

    private Camera camera;

    public float spawnTime = 5.0f;
    private float spawnTimer = 0.0f;

    public float potholeChance = 0.2f;

    private bool complete = true;
    private int waves = 0;
    private int currentScene = 0;
    private int bossScene = 3;

    private GameObject groundPrefab;
    private GameObject buildingPrefab;
    private GameObject fencePrefab;
    private GameObject backBuildingPrefab;

    private List<GameObject> grounds = new List<GameObject>();
    private List<GameObject> potholes = new List<GameObject>();
    private List<GameObject> buildings = new List<GameObject>();
    private List<GameObject> backBuildings = new List<GameObject>();

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

    void addNewBackBuilding()
    {
        GameObject backBuilding = GameObject.Instantiate(backBuildingPrefab);

        if (backBuildings.Count == 0)
        {
            backBuilding.transform.SetPositionAndRotation(PixelToGame(camera.transform.position.x - camera.rect.width, 200, 1000), backBuilding.transform.rotation);
        }
        else
        {
            GameObject lastTile = backBuildings[backBuildings.Count - 1];
            backBuilding.transform.SetPositionAndRotation(PixelToGame(GameToPixel(lastTile.transform.position.x, 0, 0).x + (backBuildingTexture.width * backBuilding.transform.localScale.x), 200, 1000), backBuilding.transform.rotation);
        }

        backBuildings.Add(backBuilding);
    }

    void addNewBuilding()
    {
        int randPoor = Random.Range(0, 2);
        int randRich = Random.Range(0, 5);
        int randSetting = Random.Range(1, bossScene+1);

        if (randSetting <= currentScene)
        {
            buildingPrefab = Managers.GetInstance().GetGameProperties().GentrifiedBuildings[randRich];
            fencePrefab = Managers.GetInstance().GetGameProperties().RichFence[randPoor];
        } else
        {
            buildingPrefab = Managers.GetInstance().GetGameProperties().PoorBuildings[randPoor];
            fencePrefab = Managers.GetInstance().GetGameProperties().PoorFence;
        }



        GameObject building = GameObject.Instantiate(buildingPrefab);
        GameObject fence = GameObject.Instantiate(fencePrefab);


        if (buildings.Count == 0)
        {
            building.transform.SetPositionAndRotation(PixelToGame(camera.transform.position.x - camera.rect.width, 60, -1000), building.transform.rotation);
        }

        else
        {
            GameObject lastTile = buildings[buildings.Count - 1];
            building.transform.SetPositionAndRotation(PixelToGame(GameToPixel(lastTile.transform.position.x, 0, 0).x + (building1Texture.width * building.transform.localScale.x), 60, -1000), building.transform.rotation);
        }


        buildings.Add(building);

        fence.transform.SetPositionAndRotation(PixelToGame(GameToPixel(building.transform.position.x, 0, 0).x + (building1Texture.width * fence.transform.localScale.x), 60, -1000), fence.transform.rotation);
        buildings.Add(fence);
    }

    void addNewGround()
    {
        GameObject ground = GameObject.Instantiate(groundPrefab);

        if (grounds.Count == 0) {
            ground.transform.SetPositionAndRotation(PixelToGame(camera.transform.position.x - camera.rect.width, -125, ground.transform.position.y), ground.transform.rotation);
        } else {
            GameObject lastGround = grounds[grounds.Count - 1];
            ground.transform.SetPositionAndRotation(PixelToGame(GameToPixel(lastGround.transform.position.x, 0, 0).x + (groundTexture.width * ground.transform.localScale.x), -125, ground.transform.position.y), ground.transform.rotation);
        }

        grounds.Add(ground);

        if (Random.Range(0.0f, 1.0f) < potholeChance)
        {
            float potholePosition = Random.Range(0.0f, 5.0f);
            GameObject pothole = new GameObject();
            SpriteRenderer renderer = pothole.AddComponent(typeof(SpriteRenderer)) as SpriteRenderer;

            Vector3 pos = GameToPixel(ground.transform.position.x, ground.transform.position.y, 0);
            pothole.transform.SetPositionAndRotation(PixelToGame(pos.x + Random.Range(0.0f, (groundTexture.width * ground.transform.localScale.x) - pothole2Sprite.rect.width), pos.y + Random.Range(-100.0f, 80.0f), -0.01f), pothole.transform.rotation);

            renderer.sprite = pothole2Sprite;
            potholes.Add(pothole);
        }
    }

    void spawnNPCs()
    {
        for (int i = 0; i < 5; i++)
        {
            Vector3 spawnLocation = camera.transform.position + PixelToGame((camera.rect.width / 2 + 100) * -1, Random.Range(-320.0f, 0.0f), -1);
            Managers.GetInstance().GetNPCManager().SpawnHipster(spawnLocation);

            spawnLocation = camera.transform.position + PixelToGame((camera.rect.width / 2 + 100), Random.Range(-320.0f, 0.0f), -1);
            Managers.GetInstance().GetNPCManager().SpawnHipster(spawnLocation);
        }
    }

    void newScene(int waves)
    {
        complete = false;
        this.waves = waves;

        addNewGround();
    }

    // Use this for initialization
    void Start () {
        camera = Camera.main;

        groundPrefab = Managers.GetInstance().GetGameProperties().GroundPrefab;

        backBuildingPrefab = Managers.GetInstance().GetGameProperties().BackBuildingsPrefab;

        pothole2Texture = Resources.Load("background/bg_0000_pothole2") as Texture2D;
        pothole1Texture = Resources.Load("background/bg_0001_pothole1") as Texture2D;
        groundTexture = Resources.Load("background/bg_0002_ground") as Texture2D;
        building1Texture = Resources.Load("background/bg_0000_Daniel’s-Board-game-Emporium") as Texture2D;
        backBuildingTexture = Resources.Load("background/bg_0004_buildings_tower") as Texture2D;

        pothole2Sprite = Sprite.Create(pothole2Texture, new Rect(0, 0, pothole2Texture.width, pothole2Texture.height), new Vector2(0f, 0.5f), 100);
        pothole1Sprite = Sprite.Create(pothole1Texture, new Rect(0, 0, pothole1Texture.width, pothole1Texture.height), new Vector2(0f, 0.5f), 100);

        for (int i = 0; i < 6; i++) {
            addNewGround();
        }

        for (int i = 0; i < 10; i++)
        {
            addNewBuilding();
        }

        for (int i = 0; i < 3; i++)
        {
            addNewBackBuilding();
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

        if (grounds.Count > 0)
        {
            GameObject obj = grounds[0];
            if (isOutOfBounds(obj))
            {
                GameObject.Destroy(obj);
                grounds.Remove(obj);
                
                currentScene += 1;
                newScene(currentScene);
            }
        }
        if (buildings.Count > 0)
        {
            GameObject obj = buildings[0];
            if (isOutOfBounds(obj))
            {
                GameObject.Destroy(obj);
                buildings.Remove(obj);
                
                addNewBuilding();
            }
        }
        if (backBuildings.Count > 0)
        {
            GameObject obj = backBuildings[0];
            if (isOutOfBounds(obj))
            {
                GameObject.Destroy(obj);
                backBuildings.Remove(obj);

                addNewBackBuilding();
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
