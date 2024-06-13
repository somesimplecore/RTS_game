using UnityEngine;
using Unity.AI.Navigation;

public class BuildingManager : MonoBehaviour
{
    public LayerMask ground;
    public NavMeshSurface navSurface;

    public Material goodBuildingMat;
    public Material badBuildingMat;

    private Material oldMaterial;

    private Camera camera;
    private GameObject GhostBuilding;

    private void Start()
    {
        camera = Camera.main;
    }

    private void Update()
    {
        if(GhostBuilding != null)
        {
            
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit) && hit.transform.tag == "Ground")
            {
                GhostBuilding.transform.position = hit.point;

                ChangeGhostBuildingState(hit.transform.gameObject.GetComponent<Hex>());
            }

            if (Input.GetMouseButtonDown(0) && hit.transform.tag == "Ground" 
                && IsCanBuildThere(hit.transform.gameObject.GetComponent<Hex>()))
            {
                Hex hex = hit.transform.gameObject.GetComponent<Hex>();

                GhostBuilding.transform.GetComponent<Building>().ChangeCanBuildState(true);
                GhostBuilding.GetComponentInChildren<MeshRenderer>().material = oldMaterial;
                hex.buildings.Add(GhostBuilding);
                ChangeGHostBuildingCollisionsStates(true);

                if (GhostBuilding.GetComponent<BuildingTag>().tagName == BuildingTag.buildingName.townHall)
                {
                    hex.isAlly = true;
                    hex.ChangeColor();
                }

                GhostBuilding.GetComponent<Building>().isAlly = true;

                GhostBuilding = null;
            }

            if (Input.GetMouseButtonDown(1))
            {
                Destroy(GhostBuilding);
            }
        }
    }

    public void CreateGhostBuilding(GameObject buildingPrefab)
    {
        if (GhostBuilding != null)
            Destroy(GhostBuilding);
        GhostBuilding = Instantiate(buildingPrefab);
        GhostBuilding.GetComponent<Building>().ChangeCanBuildState(false);
        ChangeGHostBuildingCollisionsStates(false);

        oldMaterial = GhostBuilding.GetComponentInChildren<MeshRenderer>().material;
        GhostBuilding.GetComponentInChildren<MeshRenderer>().material = goodBuildingMat;
    }

    private bool IsCanBuildThere(Hex hex)
    {
        if (hex.isAlly && hex.buildings.Find(x => x.gameObject.name == GhostBuilding.name) == null 
            || hex.buildings.Count == 0 
            && GhostBuilding.GetComponent<BuildingTag>().tagName == BuildingTag.buildingName.townHall)
            return true;
        return false;
    }

    private void ChangeGhostBuildingState(Hex hex)
    {
        GhostBuilding.GetComponentInChildren<MeshRenderer>().material = badBuildingMat;
        if (IsCanBuildThere(hex))
            GhostBuilding.GetComponentInChildren<MeshRenderer>().material = goodBuildingMat;
    }

    private void ChangeGHostBuildingCollisionsStates(bool state)
    {
        foreach (var colldider in GhostBuilding.GetComponents<Collider>())
            colldider.enabled = state;
    }
}
