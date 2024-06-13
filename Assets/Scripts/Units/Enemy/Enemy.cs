using UnityEngine;
using UnityEngine.AI;

public class Enemy : DamageableObject
{
    public UnitStats stats;
    public Hex currentHex;
    private DamageableObject currentTarget;
    private NavMeshAgent agent;

    private float lastHit;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        GetMyHex();

        agent.SetDestination(transform.position + Vector3.forward * -1);
    }

    public void Init(UnitStats newStats)
    {
        stats = newStats;
        health = stats.health;

        InitStartHealth();
    }

    private void Update()
    {
        if (currentTarget == null) TryToFindTarget();
        else if (Vector3.Distance(gameObject.transform.position, 
            currentTarget.gameObject.transform.position) > stats.attackRange) 
            FollowTarget(currentTarget);
        else Attack(currentTarget);
    }

    public void SetHexDestination(Hex hex)
    {
        agent.SetDestination(hex.gameObject.transform.position);
    }

    public void FollowTarget(DamageableObject target)
    {
        agent.SetDestination(target.transform.position);
    }

    private void Attack(DamageableObject target)
    {
        agent.SetDestination(gameObject.transform.position);
        if (Time.time - lastHit > stats.attackSpeed && target != null)
        {
            lastHit = Time.time;
            Debug.Log("аттакую в энеми");
            target.TakeDamage(stats.damage);
        }
    }

    private void TryToFindTarget()
    {
        GameObject bestTarget = null;
        foreach (var target in currentHex.alliedUnits)
        {
            if (target != null 
                && (bestTarget == null 
                || Vector3.Distance(gameObject.transform.position, target.transform.position) 
                < Vector3.Distance(gameObject.transform.position, bestTarget.transform.position)))
                bestTarget = target;
        }
        if(bestTarget == null)
        {
            foreach (var target in currentHex.buildings)
            {
                if (target != null
                    && (bestTarget == null
                    || Vector3.Distance(gameObject.transform.position, target.transform.position) 
                    < Vector3.Distance(gameObject.transform.position, bestTarget.transform.position))
                    && target.tag != "Enemy")
                    bestTarget = target;
            }
        }
        if(bestTarget != null)
            currentTarget = bestTarget.GetComponent<DamageableObject>();
    }

    private void GetMyHex()
    {
        var colls = Physics.OverlapSphere(gameObject.transform.position, 5);
        foreach (var coll in colls)
        {
            if (coll.gameObject.tag == "Ground")
                currentHex = coll.gameObject.GetComponent<Hex>();
        }
    }

    private void OnDestroy()
    {
        currentHex.enemydUnits.Remove(gameObject);
        if(EnemiesManager.Instance.attackingEnemies.Contains(this))
            EnemiesManager.Instance.attackingEnemies.Remove(this);
    }
}
