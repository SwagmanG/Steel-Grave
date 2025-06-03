using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public Image HeatBar;

    public float cooldownRate;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

    }

    private void Start()
    {
        HeatBar.fillAmount = 0;
    }


    public void UpdateHeat(float heat)
    {
        HeatBar.fillAmount += heat/100;
    }
    public void UpdateHeat()
    {
        HeatBar.fillAmount -= cooldownRate;
    }
}
