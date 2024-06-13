using System.Collections.Generic;
using UnityEngine;

public class Building : DamageableObject
{
    public int maxWorkingUnitSize;
    public bool isAlly;
    public bool isCombat;
    public List<Unit> units;

    public Resources.Resource producedResource;
    public int maxResourceInc;

    public int producedResourceAmount;
    public float produceCooldown;
    public LayerMask necessaryMaterial;

    public Resources.Resource resourceForUpdate;
    public int rescourceCountForUpdate;
    public bool isUpdated;

    public GameObject startWorkingPoint;

    public float lastProduce;
    private bool canBuild = true;

    public Hex hex;
    private void Start()
    {
        units = new List<Unit>();

        InitStartHealth();

        GetMyHex();
    }

    private void GetMyHex()
    {
        var colls = Physics.OverlapSphere(gameObject.transform.position, 5);
        foreach (var coll in colls)
        {
            if (coll.gameObject.tag == "Ground")
                hex = coll.gameObject.GetComponent<Hex>();
        }
    }

    private void Update()
    {
        ProduceResource();
    }

    virtual public void ProduceResource()
    {
        if (Time.time - lastProduce > produceCooldown
            && Resources.Instance.max.GetValueOrDefault(producedResource) > Resources.Instance.current.GetValueOrDefault(producedResource)
            && units.Count != 0
            && (necessaryMaterial == 0 | Physics.CheckSphere(gameObject.transform.position, 1000, necessaryMaterial))
            && canBuild)
        {
            Resources.Instance.current[producedResource] += producedResourceAmount * units.Count;
            lastProduce = Time.time;
        }
    }

    virtual public void AssigneUnit(Unit unit)
    {
        if (units.Count < maxWorkingUnitSize)
        {
            units.Add(unit);
            unit.agent.SetDestination(startWorkingPoint.transform.position);
        }
    }

    virtual public void DetachUnit(Unit unit)
    {
        if (units.Contains(unit))
            units.Remove(unit);
    }

    public void ChangeCanBuildState(bool state)
    {
        canBuild = state;
    }

    public bool GetCanBuildState()
    {
        return canBuild;
    }

    public void IncreaseResourcesMax()
    {
        Resources.Instance.max[Resources.Resource.villagers] += maxResourceInc;
    }

    public void BaseWorkBeforeDie()
    {
        if (canBuild)
        {
            BuildingsButtonsManager.Instance.EnableButton(GetComponent<BuildingTag>().tagName);
            if (gameObject.transform.parent != null)
            {
                GetComponentInParent<Hex>().buildings.Remove(gameObject);
                GetComponentInParent<Hex>().ChangeColor();
            }
        }
    }

    override public void Die()
    {
        BaseWorkBeforeDie();
        Destroy(gameObject);
    }
}
