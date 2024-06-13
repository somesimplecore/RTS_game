using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;
using TMPro;

public class TownHall : Building
{
    public GameObject villagerPrefab;
    public GameObject spawnPoint;

    public UnitStats stats;

    private void Start()
    {
        InitStartHealth();

        if (gameObject.tag == "Ally")
            GameManager.Instance.ChangeTownHallCount(1);
    }

    override public void ProduceResource()
    {
        if (Time.time - lastProduce > produceCooldown
            && Resources.Instance.max.GetValueOrDefault(producedResource) > Resources.Instance.current.GetValueOrDefault(producedResource)
            && GetCanBuildState()
            && gameObject.tag != "Enemy")
        {
            Resources.Instance.current[producedResource] += producedResourceAmount;
            var unitGO = Instantiate(villagerPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
            var unit = unitGO.AddComponent<Unit>();
            unit.Init(stats);
            unitGO.tag = "Ally";
            lastProduce = Time.time;
        }
    }

    override public void Die()
    {
        if (GetCanBuildState())
        {
            BaseWorkBeforeDie();
            if (gameObject.tag == "Ally")
                GameManager.Instance.ChangeTownHallCount(-1);
        }

        BaseWorkBeforeDie();
        Destroy(gameObject);
    }
}
