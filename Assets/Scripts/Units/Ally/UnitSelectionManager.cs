using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelectionManager : MonoBehaviour
{
    public static UnitSelectionManager Instance { get; set; }
    public List<GameObject> allUnits = new List<GameObject>();
    public List<GameObject> selectedUnits = new List<GameObject>();


    Camera camera;
    public LayerMask clickable;
    public LayerMask ground;
    public GameObject groundMarkerPrefab;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    private void Start()
    {
        camera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Unit unit;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, clickable) && hit.transform.gameObject.TryGetComponent<Unit>(out unit))
            {
                if(Input.GetKey(KeyCode.LeftShift))
                    MultiSelect(hit.collider.gameObject);
                else
                    SelectByClicking(hit.collider.gameObject);
            }
            else
                DeselectAll();
        }

        if (Input.GetMouseButtonDown(1) && selectedUnits.Count > 0)
        {
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, clickable))
            {
                GameObject groundMarker = Instantiate(groundMarkerPrefab);

                groundMarker.transform.position = hit.point;
                Destroy(groundMarker);
            }
        }
    }


    private void SelectByClicking(GameObject unit)
    {
        DeselectAll();

        selectedUnits.Add(unit);

        SelectUnit(unit, true);
    }

    private void SelectUnit(GameObject unit, bool isSelected)
    {
        EnableUnitMovement(unit, isSelected);
        SwitchSelectionMarkerState(unit, isSelected);
    }

    private void MultiSelect(GameObject unit)
    {
        if(selectedUnits.Contains(unit) == false)
        {
            selectedUnits.Add(unit);
            SelectUnit(unit, true);
        }
        else
        {
            SelectUnit(unit, false);
            selectedUnits.Remove(unit);
        }
    }

    private void EnableUnitMovement(GameObject unit, bool shouldMove)
    {
        unit.GetComponent<Unit>().state.canMove = shouldMove;
    }

    public void DeselectAll()
    {
        foreach (var unit in selectedUnits)
        {
            SelectUnit(unit, false);
        }

        selectedUnits.Clear();
    }

    private void SwitchSelectionMarkerState(GameObject unit, bool isVisible)
    {
        unit.transform.GetChild(0).gameObject.SetActive(isVisible);
    }

    public void DragSelect(GameObject unit)
    {
        if(selectedUnits.Contains(unit) == false)
        {
            selectedUnits.Add(unit);
            SelectUnit(unit, true);
        }
    }
}
