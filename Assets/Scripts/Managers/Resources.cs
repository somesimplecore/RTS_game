using System.Collections.Generic;
using UnityEngine;

public class Resources : MonoBehaviour
{
    public static Resources Instance { get; set; }

    public Dictionary<Resource, int> current = new Dictionary<Resource, int> {
    { Resource.villagers, 0 },
    { Resource.food, 0 },
    { Resource.wood, 0 },
    { Resource.stone, 0 },
    { Resource.iron, 0 },
    { Resource.gold, 0 },
    { Resource.faith, 0 }
    };
    public Dictionary<Resource, int> max = new Dictionary<Resource, int> {
    { Resource.villagers, 10 },
    { Resource.food, 1000 },
    { Resource.wood, 1000 },
    { Resource.stone, 1000 },
    { Resource.iron, 1000 },
    { Resource.gold, 1000 },
    { Resource.faith, 1000 }
    };

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    public enum Resource
    {
    villagers,
    food,
    wood,
    stone,
    iron,
    gold,
    faith
    }
}
