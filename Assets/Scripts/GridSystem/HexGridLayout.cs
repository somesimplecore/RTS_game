using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;


public class HexGridLayout : MonoBehaviour
{
    public static HexGridLayout Instance { get; set; }

    [Header("Grid Settings")] 
    public Vector2Int gridSize;

    [Header("Tile Settings")] 
    public float outerSize = 1f;
    public float innerSize = 0f;
    public float height = 1f;
    public bool isFlatTopped;
    public Material neutralMaterial;
    public Material allyMaterial;
    public Material enemyMaterial;

    public NavMeshSurface navSurface;

    public List<GameObject> hexes;

    public GameObject townhall;
    public List<GameObject> peaceBuildingsPrefabs;
    public List<GameObject> combatBuildingsPrefabs;
    public List<GameObject> materialsPrefabs;

    [Header("Fog Settings")]
    public float fogOuterSize = 1f;
    public float fogInnerSize = 0f;
    public float fogHeight = 1f;
    public Material fogMaterial;
    public GameObject particles;

    private void Awake()
    {
        hexes = new List<GameObject>();
        LayoutGrid();

        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    private void LayoutGrid()
    {
        for (int y = 0; y < gridSize.y; y++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                GameObject tile = new GameObject($"Hex {x},{y}", typeof(HexRenderer));
                hexes.Add(tile);
                Hex hex = tile.AddComponent<Hex>();
                if (x == 0 && y == 0)
                    tile.GetComponent<Hex>().isAlly = true;
                hex.townhall = townhall;
                hex.peaceBuildingsPrefabs = peaceBuildingsPrefabs;
                hex.combatBuildingsPrefabs = combatBuildingsPrefabs;
                hex.materialsPrefabs = materialsPrefabs;

                hex.neutralMaterial = neutralMaterial;
                hex.allyMaterial = allyMaterial;
                hex.enemyMaterial = enemyMaterial;

                tile.transform.position = GetPositionForHexFromCoordinate(new Vector2Int(x, y));

                HexRenderer hexRenderer = tile.GetComponent<HexRenderer>();
                hexRenderer.isFlatTopped = isFlatTopped;
                hexRenderer.outerSize = outerSize;
                hexRenderer.innerSize = innerSize;
                hexRenderer.height = height;
                hexRenderer.DrawMesh();
                tile.transform.SetParent(transform, true);

                tile.tag = "Ground";

                hex.Init();
                hex.maxCombatEnemies = Mathf.Max(2, x, y);
            }
        }
        //navSurface.BuildNavMesh();
    }

    private void AddFogToTile(GameObject tile)
    {
        GameObject fog = new GameObject($"Fog", typeof(FogRenderer));
        fog.transform.position = tile.transform.position;
        fog.transform.SetParent(tile.transform, true);
        

        FogRenderer fogRenderer = fog.GetComponent<FogRenderer>();
        fogRenderer.isFlatTopped = isFlatTopped;
        fogRenderer.outerSize = fogOuterSize;
        fogRenderer.innerSize = fogInnerSize;
        fogRenderer.height = Random.Range(fogHeight - 2f, fogHeight + 2f);
        fogRenderer.DrawMesh();

        fogRenderer.particles = particles;

        fogRenderer.SetMaterial(fogMaterial);

        tile.GetComponent<Hex>().fog = fog;
    }

    public Vector3 GetPositionForHexFromCoordinate(Vector2Int coordinate)
    {
        int column = coordinate.x; 
        int row = coordinate.y; 
        float width;
        float height;
        float xPosition;
        float yPosition;
        bool shouldOffset;
        float horizontalDistance; 
        float verticalDistance; 
        float offset;
        float size = outerSize;
        if(!isFlatTopped)
        {
            shouldOffset = (row % 2) == 0;
            width = Mathf.Sqrt(3) * size; 
            height = 2f * size;

            horizontalDistance = width; 
            verticalDistance = height * (3f / 4f);

            offset = (shouldOffset) ? width / 2 : 0;

            xPosition = (column * (horizontalDistance)) + offset; 
            yPosition = (row * verticalDistance);
        }
        else
        {
            shouldOffset = (column % 2) == 0; 
            width = 2f * size;
            height = Mathf.Sqrt(3f) * size;
            
            horizontalDistance = width * (3f / 4f); 
            verticalDistance = height;
            
            offset = (shouldOffset) ? height / 2 : 0; 
            xPosition = (column * (horizontalDistance)); 
            yPosition = (row * (row * verticalDistance) - offset);
        }

        return new Vector3(xPosition, 0, -yPosition);
    }
}
