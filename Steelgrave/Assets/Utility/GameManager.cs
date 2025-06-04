using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    public static GameManager Instance;

    [SerializeField] private Player player;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private Spawner spawner;

    [Header("Game")]
    public int Score = 0;
    public int HighScore = 0;

    [Header("Tank")]
    public float Health = 100;

    private float overheat = 0;
    public float Overheat { get { return overheat; } set { overheat = Mathf.Clamp(value, 0, 1); } }

    [SerializeField] public float CooldownRate;
    [SerializeField] private float shutdownLength;

    private bool isCooling = false;
    bool isGameOver = false;

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
        if(Health <= 0 && !isGameOver)
        {
            isGameOver = true;
            inputManager.SetDisabled(true);
            uiManager.ActivateEnd();
            spawner.isSpawning = false;
        }

        if (Overheat >= 1 && !isCooling)
        {
            DisableTank();
            Debug.LogError("OVERHEAT");
        }

        HeatCooldown(GameManager.Instance.CooldownRate/100); // auto cooldown

        player.DamageBuff = Overheat;
        //Debug.Log(player.DamageBuff);
    }

    // Game state
    public void OnStartGame()
    {
        inputManager.Init(player);
        uiManager.SetHeatFill(0);
        uiManager.OverheatFlash.SetActive(false);
        spawner.isSpawning = true;

        Health = 100;
        Overheat = 0;
        Score = 0;
        isCooling = false;
        isGameOver = false;
    }

    public void OnResetGame()
    {
        GameObject[] enemiesToDestroy = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject obj in enemiesToDestroy)
        {
            Destroy(obj);
        }

        uiManager.ActivateGame(); 
        inputManager.SetDisabled(false);
        OnStartGame();
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

    public void HeatCooldown(float cool)
    {
        Overheat -= cool / 100;
        uiManager.SetHeatFill(Overheat);
    }

    private void DisableTank()
    {
        StartCoroutine(CooldownRoutine());
    }

    IEnumerator CooldownRoutine()
    {
        uiManager.OverheatFlash.SetActive(true);

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
        uiManager.OverheatFlash.SetActive(false);
    }

    public void IncreaseScore(int score)
    {
        Score += score;
        if (Score >= HighScore)
        {
            HighScore = Score; 
        }

        uiManager.SetScore(Score, HighScore);
    }
    public void TakeDamage(float dmg)
    {
        Health -= dmg;
        uiManager.SetHealthFill(Health/100);
    }

}
