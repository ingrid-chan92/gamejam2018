using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour {
    private Texture2D pothole1Texture;
    private Texture2D pothole2Texture;
    private Texture2D groundTexture;
    
    private Texture2D backBuildingTexture;

    private Texture2D poorfenceTexture;

    private Sprite pothole2Sprite;
    private Sprite pothole1Sprite;

    private Camera camera;
    private Vector3 cameraPreviousPosition;

    public float spawnTime = 1.0f;
    private float spawnTime2 = 5.0f;
    private float spawnTimer = 0.0f;

    public float potholeChance = 0.2f;

    private bool arrowCanBeShown = true;

    private int buildingsPast = 0;

    private bool complete = true;
    private int waves = 0;
    private int stream = 0;
    private int currentScene = 0;
    private bool bossSpawned = false;

    private GameObject arrow;
    private GameObject AR;

    private int bossScene = 5;

    private GameObject groundPrefab;
    private GameObject buildingPrefab;
    private GameObject fencePrefab;
    private GameObject backBuildingPrefab;
    private GameObject musicPrefab;
    private GameObject music;

    private List<GameObject> grounds = new List<GameObject>();
    private List<GameObject> potholes = new List<GameObject>();
    private List<GameObject> buildings = new List<GameObject>();
    private List<int> buildingWidths = new List<int>();
    private List<GameObject> backBuildings = new List<GameObject>();

    private List<Texture2D> fenceTextures = new List<Texture2D>();
    private List<Texture2D> poorBuildingTextures = new List<Texture2D>();
    private List<Texture2D> richBuildingTextures = new List<Texture2D>();

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
            backBuilding.transform.SetPositionAndRotation(PixelToGame(camera.transform.position.x - camera.rect.width, 190, 1000), backBuilding.transform.rotation);
        }
        else
        {
            GameObject lastTile = backBuildings[backBuildings.Count - 1];
            backBuilding.transform.SetPositionAndRotation(PixelToGame(GameToPixel(lastTile.transform.position.x, 0, 0).x + (backBuildingTexture.width * backBuilding.transform.localScale.x), 190, 1000), backBuilding.transform.rotation);
        }

        backBuildings.Add(backBuilding);
    }

    void addNewBuilding()
    {
        int randPoor = Random.Range(0, 2);
        int randRich = Random.Range(0, 5);
        int randSetting = Random.Range(1, bossScene);

        Texture2D fenceTex;
        Texture2D buildingTex;
        if (randSetting <= currentScene)
        {
            buildingPrefab = Managers.GetInstance().GetGameProperties().GentrifiedBuildings[randRich];
            fencePrefab = Managers.GetInstance().GetGameProperties().RichFence[randPoor];
            fenceTex = fenceTextures[randPoor];
            buildingTex = richBuildingTextures[randRich];
        } else
        {
            buildingPrefab = Managers.GetInstance().GetGameProperties().PoorBuildings[randPoor];
            fencePrefab = Managers.GetInstance().GetGameProperties().PoorFence;
            fenceTex = poorfenceTexture;
            buildingTex = poorBuildingTextures[randPoor];
        }
        
        GameObject building = GameObject.Instantiate(buildingPrefab);
        GameObject fence = GameObject.Instantiate(fencePrefab);
        
        if (buildings.Count == 0)
        {
            building.transform.SetPositionAndRotation(PixelToGame(camera.transform.position.x - camera.rect.width + (buildingTex.width * building.transform.localScale.x / 2), 155, -1000), building.transform.rotation);
        }
        else
        {
            GameObject lastTile = buildings[buildings.Count - 1];
            int lastWidth = buildingWidths[buildingWidths.Count - 1];
            building.transform.SetPositionAndRotation(PixelToGame(GameToPixel(lastTile.transform.position.x, 0, 0).x + (lastWidth / 2 * building.transform.localScale.x) + (buildingTex.width / 2 * building.transform.localScale.x), 155, -1000), building.transform.rotation);
        }
        
        buildings.Add(building);
        buildingWidths.Add(buildingTex.width);


        Vector3 fencePos = PixelToGame(GameToPixel(building.transform.position.x, 0, 0).x + (buildingTex.width / 2 * building.transform.localScale.x) + (fenceTex.width / 2 * fence.transform.localScale.x), 85 + (fenceTex.height / 2 * fence.transform.localScale.y), fence.transform.position.z);

        fence.transform.SetPositionAndRotation(fencePos, fence.transform.rotation);
        buildingWidths.Add(fenceTex.width);

        int randGoat = Random.Range(0, 10);
        if (randGoat <= 1)
        {
            GameObject goat = GameObject.Instantiate(Managers.GetInstance().GetGameProperties().GoatPrefab);
            goat.transform.SetPositionAndRotation(PixelToGame(GameToPixel(fencePos.x, 0, 0).x, 163, 20), fence.transform.rotation);
        }

        buildings.Add(fence);
    }

    void addNewGround()
    {
        GameObject ground = GameObject.Instantiate(groundPrefab);

        if (grounds.Count == 0) {
            ground.transform.SetPositionAndRotation(PixelToGame(camera.transform.position.x - camera.rect.width, -75, ground.transform.position.y), ground.transform.rotation);
        } else {
            GameObject lastGround = grounds[grounds.Count - 1];
            ground.transform.SetPositionAndRotation(PixelToGame(GameToPixel(lastGround.transform.position.x, 0, 0).x + (groundTexture.width * ground.transform.localScale.x), -75, ground.transform.position.y), ground.transform.rotation);
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

    void spawnNPCs(int count)
    {
        for (int i = 0; i < count; i++)
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

        this.waves = (waves / 2) + 1;
        stream = (waves % this.waves) + 1;

        Debug.Log(stream);
        Debug.Log(this.waves);
    }

    // Use this for initialization
    void Start () {
        camera = Camera.main;

        AR = Managers.GetInstance().GetGameProperties().Arrow;
        arrow = GameObject.Instantiate(AR);

        musicPrefab = Managers.GetInstance().GetGameProperties().LevelMusic;
        music = GameObject.Instantiate(musicPrefab);

        cameraPreviousPosition = camera.transform.position;

        groundPrefab = Managers.GetInstance().GetGameProperties().GroundPrefab;

        backBuildingPrefab = Managers.GetInstance().GetGameProperties().BackBuildingsPrefab;

        pothole2Texture = Resources.Load("background/bg_0000_pothole2") as Texture2D;
        pothole1Texture = Resources.Load("background/bg_0001_pothole1") as Texture2D;
        groundTexture = Resources.Load("background/bg_0002_ground") as Texture2D;
        backBuildingTexture = Resources.Load("background/bg_0004_buildings_tower") as Texture2D;
        
        poorBuildingTextures.Add(Resources.Load("background/bg_0001_rundown") as Texture2D);
        poorBuildingTextures.Add(Resources.Load("background/bg_0000_randobuilding") as Texture2D);
        
        richBuildingTextures.Add(Resources.Load("background/bg_0003_Allie-James-Diner") as Texture2D);
        richBuildingTextures.Add(Resources.Load("background/bg_0002_bookstore") as Texture2D);
        richBuildingTextures.Add(Resources.Load("background/bg_0001_Coffee-with-Charles") as Texture2D);
        richBuildingTextures.Add(Resources.Load("background/bg_0000_Daniel’s-Board-game-Emporium") as Texture2D);
        richBuildingTextures.Add(Resources.Load("background/bg_0002_johns-cats") as Texture2D);

        fenceTextures.Add(Resources.Load("background/bg_0002_fence1") as Texture2D);
        fenceTextures.Add(Resources.Load("background/bg_0001_fence2") as Texture2D);
        poorfenceTexture = Resources.Load("background/bg_0000_fence3") as Texture2D;

        pothole2Sprite = Sprite.Create(pothole2Texture, new Rect(0, 0, pothole2Texture.width, pothole2Texture.height), new Vector2(0f, 0.5f), 100);
        pothole1Sprite = Sprite.Create(pothole1Texture, new Rect(0, 0, pothole1Texture.width, pothole1Texture.height), new Vector2(0f, 0.5f), 100);

        for (int i = 0; i < 5; i++) {
            addNewGround();
        }
        
        for (int i = 0; i < 5; i++)
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
        if (!complete && currentScene < bossScene && waves > 0)
        {
            arrowCanBeShown = true;
            spawnTimer -= Time.deltaTime;

            if (spawnTimer <= 0.0f && stream > 0)
            {
                spawnTimer = spawnTime;

                spawnNPCs(1);
                stream -= 1;
            }
            else if (spawnTimer <= 0.0f)
            {
                waves -= 1;
                if (waves > 0)
                {
                    spawnTimer = spawnTime2;
                    stream = (currentScene % ((currentScene / 2) + 1)) + 1;
                }
            }
        } else if (currentScene == bossScene && !bossSpawned)
        {
            bossSpawned = true;
            Vector3 spawnLocation = camera.transform.position + PixelToGame((camera.rect.width / 2 + 100), Random.Range(-320.0f, 0.0f), -1);
            Managers.GetInstance().GetNPCManager().SpawnBoss(spawnLocation);
            waves = 0;
        }
        else if (!complete && currentScene == bossScene)
        {
            spawnTimer -= Time.deltaTime;

            if (spawnTimer <= 0.0f)
            {
                spawnTimer = spawnTime * 8;
                spawnNPCs(1);
            }
        }

        if (Managers.GetInstance().GetNPCManager().allEnemiesDead() && waves <= 0 && stream <= 0)
        {
            complete = true;
            spawnTimer = 0.0f;

            waves = 0;
            stream = 0;

            if (arrowCanBeShown == true)
            {
                arrow.SetActive(true);
            }
        }

        Vector3 cameraDiff = camera.transform.position - cameraPreviousPosition;

        if (cameraDiff.x > 0)
        {
            arrow.SetActive(false);
            arrowCanBeShown = false;
        }

        foreach (GameObject backBuilding in backBuildings)
        {
            backBuilding.transform.Translate(cameraDiff / 2);
        }
        cameraPreviousPosition = camera.transform.position;

        if (grounds.Count > 0)
        {
            GameObject obj = grounds[0];
            if (isOutOfBounds(obj))
            {
                GameObject.Destroy(obj);
                grounds.Remove(obj);
                addNewGround();

                buildingsPast += 1;
            }
        }
        if (buildings.Count > 0)
        {
            GameObject obj = buildings[0];
            if (isOutOfBounds(obj))
            {
                GameObject obj2 = buildings[1];
                GameObject.Destroy(obj);
                buildings.Remove(obj);
                GameObject.Destroy(obj2);
                buildings.Remove(obj2);

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

        if (buildingsPast >= 1)
        {
            currentScene += 1;
            newScene(currentScene);
            buildingsPast = 0;
        }
    }

    public void bossMusic ()
    {
        Destroy(music);
        musicPrefab = Managers.GetInstance().GetGameProperties().BossMusic;
        music = GameObject.Instantiate(musicPrefab);

    }
}
