using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupManager : MonoBehaviour
{
    public GameObject camRoot;
    private GameObject initialHex;
    void Start()
    {
        initialHex = GameObject.Find("Hex 0,0");
        SetCameraPosition();
        SetInitialResources();

    }

    private void SetInitialResources()
    {
        Resources.Instance.current[Resources.Resource.gold] = 50;
        Resources.Instance.current[Resources.Resource.food] = 20;
    }

    private void SetCameraPosition()
    {
        Vector3 camPos = new Vector3(initialHex.transform.position.x, camRoot.transform.position.y, initialHex.transform.position.z - 30);
        camRoot.transform.position = camPos;
        camRoot.GetComponent<CameraController>().SetTargetPosition(camPos);
    }
}
