using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourceUI : MonoBehaviour
{
    public Resources.Resource resource;
    private TMP_Text text; 
    private void Start()
    {
        text = GetComponent<TMP_Text>();
    }
    void Update()
    {
        text.text = Resources.Instance.current[resource].ToString();
    }
}
