using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Image[] hearts = new Image[3];
    public static GameManager Instance { get; private set; }
    void Awake()
    {
        // Ensure only one instance exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: persists across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void LoseLife()
    {
        for (int i = hearts.Length - 1; i >= 0; i--)
        {
            if (hearts[i].enabled)
            {
                hearts[i].enabled = false;
                break;
            }
        }
    }
    public void RestoreLives()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].enabled = true;
        }
    }
}
