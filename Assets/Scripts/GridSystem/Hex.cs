using System.Collections.Generic;
using UnityEngine;


public class Hex : MonoBehaviour
{
    public bool isAlly;
    public List<Hex> neighborHexes;

    public GameObject townhall;
    public List<GameObject> peaceBuildingsPrefabs;
    public List<GameObject> combatBuildingsPrefabs;
    public List<GameObject> materialsPrefabs;

    public int maxCombatEnemies;
    public List<GameObject> buildings = new List<GameObject>();
    public List<GameObject> materials = new List<GameObject>();
    public List<GameObject> alliedUnits = new List<GameObject>();
    public List<GameObject> enemydUnits = new List<GameObject>();

    public HexRenderer hexRenderer;
    public Material neutralMaterial;
    public Material allyMaterial;
    public Material enemyMaterial;

    public List<Vector3> freeSectors;

    public GameObject fog;

    // первоначальная настройка
    public void Init()
    {
        hexRenderer = GetComponent<HexRenderer>();
        neighborHexes = new List<Hex>();
        freeSectors = new List<Vector3>() {
        //new Vector3(-1, 0, 0),
        new Vector3(-1, 0, 1),
        new Vector3(1, 0, 1),
        //new Vector3(1, 0, 0),
        new Vector3(1, 0, -1),
        new Vector3(-1, 0, -1),
        };

        if (isAlly)
            CreateInitialAllyContent();
        else
            CreateRandomEnemyContent();

        ChangeColor();
        GetNeighbors();


    }

    // полученние ссылок на соседние хексы
    private void GetNeighbors()
    {
        var colls = Physics.OverlapSphere(gameObject.transform.position, 
            gameObject.GetComponent<HexRenderer>().outerSize + 5);
        foreach (var coll in colls)
        {
            if (coll.gameObject.tag == "Ground" && 
                !neighborHexes.Contains(coll.gameObject.GetComponent<Hex>()) && 
                coll.gameObject.GetComponent<Hex>() != this)
            {
                neighborHexes.Add(coll.gameObject.GetComponent<Hex>());
                if (!coll.gameObject.GetComponent<Hex>().neighborHexes.Contains(this))
                    coll.gameObject.GetComponent<Hex>().neighborHexes.Add(this);
            }
        }
    }

    public void ChangeColor()
    {
        if (buildings.Count == 0)
            hexRenderer.SetMaterial(neutralMaterial);
        else if (isAlly)
            hexRenderer.SetMaterial(allyMaterial);
        else
            hexRenderer.SetMaterial(enemyMaterial);
    }

    // создание базовых зданий и ресурсов на хексе игрока
    private void CreateInitialAllyContent()
    {
        buildings.Add(Instantiate(townhall, 
            gameObject.transform.position + Vector3.forward * 12, 
            Quaternion.identity, gameObject.transform));
        Instantiate(materialsPrefabs.Find(x => x.name == "Tree"), 
            gameObject.transform.position + Vector3.right * 17, 
            Quaternion.identity);

        buildings[0].GetComponent<Building>().isAlly = true;
    }

    // создание случайных зданий и ресурсов на вражеских территориях
    public void CreateRandomEnemyContent()
    {
        //townhall
        int sectorInd = Random.Range(0, freeSectors.Count);
        buildings.Add(Instantiate(townhall, 
            gameObject.transform.position + freeSectors[sectorInd] * Random.Range(6f, 12f), 
            Quaternion.identity, gameObject.transform));
        freeSectors.RemoveAt(sectorInd);

        //combatBuilding
        sectorInd = Random.Range(0, freeSectors.Count);
        var buildingInd = Random.Range(0, combatBuildingsPrefabs.Count);
        buildings.Add(Instantiate(combatBuildingsPrefabs[buildingInd], 
            gameObject.transform.position + freeSectors[sectorInd] * Random.Range(6f, 12f), 
            Quaternion.identity, gameObject.transform));
        freeSectors.RemoveAt(sectorInd);

        //peaceBuilding
        sectorInd = Random.Range(0, freeSectors.Count);
        buildings.Add(Instantiate(peaceBuildingsPrefabs[Random.Range(0, peaceBuildingsPrefabs.Count)], 
            gameObject.transform.position + freeSectors[sectorInd] * Random.Range(6f, 12f), 
            Quaternion.identity, gameObject.transform));
        freeSectors.RemoveAt(sectorInd);

        //material
        sectorInd = Random.Range(0, freeSectors.Count);
        materials.Add(Instantiate(materialsPrefabs[Random.Range(0, materialsPrefabs.Count)], 
            gameObject.transform.position + freeSectors[sectorInd] * Random.Range(6f, 12f), 
            Quaternion.identity, gameObject.transform));
        freeSectors.RemoveAt(sectorInd);

        MakeBuildingsEnemy();
    }

    private void MakeBuildingsEnemy()
    {
        foreach (var building in buildings)
        {
            building.tag = "Enemy";
            building.GetComponent<Building>().isAlly = false;
            Debug.Log(building.GetComponent<Building>().isAlly);
        }
    }

    // реагирование на юнитов, входящих внутрь границы
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && !enemydUnits.Contains(other.gameObject))
        {
            enemydUnits.Add(other.gameObject);
            other.gameObject.GetComponent<Enemy>().currentHex = this;
        }

        if (other.gameObject.tag == "Ally" && !alliedUnits.Contains(other.gameObject))
            alliedUnits.Add(other.gameObject);
    }

    // реагирование на юнитов, выходящих за границы границы
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
            enemydUnits.Remove(other.gameObject);

        if (other.gameObject.tag == "Ally")
            alliedUnits.Remove(other.gameObject);
    }
}
