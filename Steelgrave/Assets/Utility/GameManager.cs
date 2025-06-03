using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private Player player;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private UIManager uiManager;

    private float overheat;

    [SerializeField] private float cooldownRate;

    private void Awake()    
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        inputManager.Init(player);
    }

    private void Start()
    {
        overheat = 0;
    }

    public void HeatGenerate(float heat)
    {
        overheat += heat / 100;
        uiManager.SetHeatFill(overheat);
    }

    public void HeatCooldown()
    {
        overheat -= cooldownRate;
        uiManager.SetHeatFill(overheat);
    }


}
