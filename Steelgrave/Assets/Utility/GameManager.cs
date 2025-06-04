using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private bool gameStarted = false;

    [SerializeField] private Player player;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private UIManager uiManager;

    private float overheat = 0;
    public float Overheat { get { return overheat; } set { overheat = Mathf.Clamp(value, 0, 1); } }

    [SerializeField] private float cooldownRate;
    [SerializeField] private float shutdownLength;

    private bool isCooling = false;

    public int Score = 0;
    public int HighScore = 0;

    private void Awake()    
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

    }
    private void Update()
    {
        if (Overheat >= 1 && !isCooling)
        {
            DisableTank();
            Debug.LogError("OVERHEAT");
        }
    }

    // Game state
    public void OnStartGame()
    {
        inputManager.Init(player);
        uiManager.SetHeatFill(Overheat);
        gameStarted = true;
    }
    /*public void OnResetGame()
    {
        *//*StopAllCoroutines();
         *
         * and more here
         *
        OnStartGame();*//*
    }*/

    public void OnOptions()
    {
        gameStarted = false;
    }

    public void OnQuitGame()
    {
        Application.Quit();
    }

   

    // Overheat
    public void HeatGenerate(float heat)
    {
        Overheat += heat / 100;
        uiManager.SetHeatFill(Overheat);
    }

    public void HeatCooldown()
    {
        Overheat -= cooldownRate;
        uiManager.SetHeatFill(Overheat);
    }

    private void DisableTank()
    {
        StartCoroutine(CooldownRoutine());
    }

    IEnumerator CooldownRoutine()
    {
        inputManager.SetDisabled(true);
        isCooling = true;

        float timer = 0f;

        while (timer < shutdownLength)
        {
            timer += Time.deltaTime;

            Overheat = Mathf.Lerp(1, 0, timer / shutdownLength);
            uiManager.SetHeatFill(Overheat);

            yield return null;
        }

        isCooling = false;
        inputManager.SetDisabled(false);
    }

     // Score

    public void IncreaseScore(int score)
    {
        Score += score;
        if (Score >= HighScore)
        {
            HighScore = Score; 
        }

        uiManager.SetScore(Score, HighScore);
    }

}
