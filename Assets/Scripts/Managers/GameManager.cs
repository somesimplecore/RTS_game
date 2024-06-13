using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }

    public int townHallsCount;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    public void ChangeTownHallCount(int change)
    {
        townHallsCount += change;

        if (townHallsCount < 1)
            Loose();
        if (townHallsCount == HexGridLayout.Instance.gridSize.x * HexGridLayout.Instance.gridSize.y)
            Win();

    }

    private void Loose()
    {
        SceneManager.LoadScene(0);
    }

    private void Win()
    {
        SceneManager.LoadScene(0);
    }
}
