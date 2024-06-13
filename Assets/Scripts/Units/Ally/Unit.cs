using UnityEngine;
using UnityEngine.AI;

public class Unit : DamageableObject
{
    public UnitStats stats;
    public UnitState state;
    public bool isCombat;

    Camera camera;
    public NavMeshAgent agent;
    public Canvas sleepCanv;

    private float lastHit;

    private void Start()
    {
        state = new UnitState();
        state.status = UnitState.State.idle;

        camera = Camera.main;
        agent = GetComponent<NavMeshAgent>();
        sleepCanv = transform.GetChild(2).GetComponent<Canvas>();

        UnitSelectionManager.Instance.allUnits.Add(gameObject);

        agent.SetDestination(transform.position + Vector3.forward * -1);

        ChangeSleepState(false);
    }

    public void Init(UnitStats newStats)
    {
        stats = newStats;
        health = stats.health;

        InitStartHealth();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit) && state.canMove)
            {
                switch (hit.transform.gameObject.tag)
                {
                    case "Ground":
                        agent.SetDestination(hit.point);
                        SwitchState(state.canMove, UnitState.State.running, hit.point);
                    break;
                    case "Enemy":
                        Debug.Log("пошел атаковать");
                        SwitchState(state.canMove, UnitState.State.attacking, new Vector3(), 
                            hit.transform.GetComponent<DamageableObject>());
                        break;
                    case "Building":
                        Debug.Log("пошел работать");
                        SwitchState(state.canMove, UnitState.State.working, new Vector3(), null, 
                            hit.transform.GetComponent<Building>());
                        break;
                }
            }
        }
        switch (state.status)
        {
            case UnitState.State.idle:
                ChangeSleepState(true);
                break;
            case UnitState.State.running:
                if (agent.remainingDistance == 0f)
                    SwitchState(state.canMove, UnitState.State.idle);
                ChangeSleepState(false);
                break;
            case UnitState.State.attacking:
                if (state.enemy == null) state.status = UnitState.State.idle;
                else if (Vector3.Distance(gameObject.transform.position, 
                    state.enemy.gameObject.transform.position) > stats.attackRange) 
                    FollowEnemy(state.enemy);
                else Attack(state.enemy);
                ChangeSleepState(false);
                break;
            case UnitState.State.working:
                ChangeSleepState(false);
                break;
            default:
                break;
        }
    }

    private void SwitchState(bool newCanMove, 
        UnitState.State newStatus = UnitState.State.idle, 
        Vector3 newFollowPoint = new Vector3(), 
        DamageableObject newEnemy = null, 
        Building newBuilding = null)
    {
        if (state.status == UnitState.State.working && newStatus != UnitState.State.working)
            state.building.DetachUnit(this);

        if (state.status != UnitState.State.working && newStatus == UnitState.State.working)
            newBuilding.AssigneUnit(this);

        state.Init(newCanMove, newStatus, newFollowPoint, newEnemy, newBuilding);
    }

    private void ChangeSleepState(bool state)
    {
        if(sleepCanv != null)
            sleepCanv.gameObject.SetActive(state);
    }

    private void Attack(DamageableObject enemy)
    {
        agent.SetDestination(gameObject.transform.position);
        if (Time.time - lastHit > stats.attackSpeed && enemy != null)
        {
            lastHit = Time.time;
            enemy.TakeDamage(stats.damage);
        }
    }

    public void FollowEnemy(DamageableObject enemy)
    {
        agent.SetDestination(Vector3.MoveTowards(gameObject.transform.position, 
            enemy.transform.position, 
            Vector3.Distance(gameObject.transform.position, 
            enemy.transform.position) - stats.attackRange));
    }

    private void OnDestroy()
    {
        UnitSelectionManager.Instance.allUnits.Remove(gameObject);
        UnitSelectionManager.Instance.selectedUnits.Remove(gameObject);
    }
}
