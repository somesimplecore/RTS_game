using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    public static EnemiesManager Instance { get; set; }

    private List<Hex> hexes;
    private HexGridLayout hexGL;

    public bool isAttacking;
    private float lastTimeAttackCheck;
    private float TimeAttackCheckCooldown = 5f;
    private float lastAttack;
    private float attackCooldown = 60f;

    public List<Enemy> attackingEnemies;

    private void Start()
    {
        attackingEnemies = new List<Enemy>();
        hexes = new List<Hex>();
        hexGL = HexGridLayout.Instance;
        foreach (GameObject go in hexGL.hexes)
            hexes.Add(go.GetComponent<Hex>());

        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    private void Update()
    {

        if (CanAttack())
            AttackPlayer();

        UpdateAttackState();

    }

    private void UpdateAttackState()
    {
        if (attackingEnemies.Count == 0 || Time.time - lastAttack > attackCooldown)
            isAttacking = false;
    }

    private bool CanAttack()
    {
        if (Time.time - lastTimeAttackCheck > TimeAttackCheckCooldown)
            return false;
        lastTimeAttackCheck = Time.time;
        var cureUnits = Resources.Instance.current[Resources.Resource.villagers];
        var maxUnits = Resources.Instance.max[Resources.Resource.villagers];
        return !isAttacking
            && Random.Range(0f, 1f) * 100 < cureUnits / maxUnits;
    }

    private void AttackPlayer()
    {
        lastAttack = Time.time;
        isAttacking = true;
        Hex enemyHex = GetHexWithEnemies();

        if (enemyHex == null)
        {
            isAttacking = false;
            return;
        }

        Hex allyHex = GetAllyHex(enemyHex);
        foreach(var enemy in enemyHex.enemydUnits)
        {
            enemy.GetComponent<Enemy>().SetHexDestination(allyHex);
            attackingEnemies.Add(enemy.GetComponent<Enemy>());
        }
    }

    private Hex GetHexWithEnemies()
    {
        List<Hex> bestHexes = new List<Hex>();
        foreach(var go in hexGL.hexes)
        {
            Hex hex = go.GetComponent<Hex>();
            if (hex.enemydUnits.Count == 0)
                continue;
            foreach(var neighborHex in hex.neighborHexes)
            {
                if (neighborHex.isAlly)
                    bestHexes.Add(hex);
            }
        }

        if (bestHexes.Count == 0)
            return null;

        return bestHexes[Random.Range(0, bestHexes.Count - 1)];
    }

    private Hex GetAllyHex(Hex enemyHex)
    {
        List<Hex> bestHexes = new List<Hex>();
        foreach (var neighborHex in enemyHex.neighborHexes)
        {
            if (neighborHex.isAlly)
                bestHexes.Add(neighborHex);
        }

        if (bestHexes.Count == 0)
            return null;

        return bestHexes[Random.Range(0, bestHexes.Count - 1)];
    }
}
