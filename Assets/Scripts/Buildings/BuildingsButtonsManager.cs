using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingsButtonsManager : MonoBehaviour
{
    public static BuildingsButtonsManager Instance { get; set; }
    public List<GameObject> buttons;
    private Canvas canv;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;

        canv = GetComponent<Canvas>();
    }

    public void EnableButton(BuildingTag.buildingName name)
    {
        foreach(var button in buttons)
        {
            if (button != null && button.GetComponent<BuildingTag>().tagName == name)
                button.SetActive(true);
        }

        UpdateCanvas();
    }

    private void UpdateCanvas()
    {
        canv.gameObject.SetActive(false);
        canv.gameObject.SetActive(true);
    }
}
