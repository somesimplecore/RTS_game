using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatBuilding : Building
{
    [Header("Combat Building Settings")]
    public GameObject unitPrefab;
    public UnitStats stats;
    public Resources.Resource necessaryResource;
    public int necessaryResourceCount;
    public GameObject spawnPoint;

    private float lastSpawnTime;
    private float spawnColldown = 20f;

    private void Update()
    {
        if (!isAlly && Time.time - lastSpawnTime > spawnColldown && hex.enemydUnits.Count < hex.maxCombatEnemies)
        {
            var enemyGO = Instantiate(unitPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
            enemyGO.gameObject.tag = "Enemy";
            var enemy = enemyGO.AddComponent<Enemy>();
            enemy.Init(stats);
            lastSpawnTime = Time.time;
        }
    }

    override public void AssigneUnit(Unit unit)
    {
        if (Resources.Instance.current[necessaryResource] >= necessaryResourceCount)
        {
            units.Add(unit);
            Resources.Instance.current[necessaryResource] -= necessaryResourceCount;
            unit.agent.SetDestination(gameObject.transform.position);
        }
    }

    override public void DetachUnit(Unit unit)
    {
        if (units.Contains(unit))
        {
            units.Remove(unit);
            Resources.Instance.current[necessaryResource] += necessaryResourceCount;;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Unit unit;
        if (other.transform.TryGetComponent(out unit) && units.Contains(unit))
        {
            units.Remove(unit);
            unit.Die();
            SpawnUnit();
        }
    }

    public void SpawnUnit()
    {
        if (Resources.Instance.current[necessaryResource] >= necessaryResourceCount
            || isUpdated && Resources.Instance.current[necessaryResource] >= necessaryResourceCount * 0.8)
        {
            var unit = Instantiate(unitPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
            unit.gameObject.tag = "Ally";
            unit.AddComponent<Unit>();
            unit.GetComponent<Unit>().stats = stats;
            Resources.Instance.current[necessaryResource] -= necessaryResourceCount;
        }
    }
}
