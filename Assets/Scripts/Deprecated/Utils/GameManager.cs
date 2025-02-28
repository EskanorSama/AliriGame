using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject LoseMenu;
    public static GameManager Instance;
    [HideInInspector] public bool CanRevive = false;

    private void Awake() => Instance = this;
    
    public void Lose()
    {
        if (CanRevive)
        {

        }
        else
        {
            Time.timeScale = 0f;
            LoseMenu.SetActive(true);
        }
    }
}
