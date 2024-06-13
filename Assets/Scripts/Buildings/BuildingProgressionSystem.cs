using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuildingProgressionSystem : MonoBehaviour
{
    public GameObject window;
    public TMP_Text buildName;
    public TMP_Text resourceNameAndCount;
    public TMP_Text updateText;
    private Building currentBuilding;

    private Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        window.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Input.anyKeyDown && !Input.GetMouseButtonDown(0))
        {
            window.SetActive(false);
        }
        else if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out hit) 
            && hit.transform.gameObject.tag == "Building")
        {
            window.SetActive(true);
            currentBuilding = hit.transform.GetComponent<Building>();
            SetText();
        }
    }

    public void SetText()
    {
        buildName.text = currentBuilding.GetComponent<BuildingTag>().tagName.ToString();
        resourceNameAndCount.text = currentBuilding.resourceForUpdate.ToString() 
            + " " 
            + currentBuilding.rescourceCountForUpdate.ToString();
        if (currentBuilding.isCombat)
            updateText.text = "Стоимость найма уменьшена на 20%";
        else
            updateText.text = "К зданию можно привязать дополнительного юнита";
    }

    public void ImproveBuilding()
    {
        if(!currentBuilding.isUpdated
            && Resources.Instance.current[currentBuilding.resourceForUpdate] >= currentBuilding.rescourceCountForUpdate)
        {
            currentBuilding.isUpdated = true;
            Resources.Instance.current[currentBuilding.resourceForUpdate] -= currentBuilding.rescourceCountForUpdate;
            if (currentBuilding.isCombat)
                currentBuilding.maxWorkingUnitSize += 1;
        }
    }
}
